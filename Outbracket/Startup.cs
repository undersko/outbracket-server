using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Outbracket.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Outbracket.Api.Contracts.Requests.Account;
using Outbracket.Common.Helpers;
using Outbracket.Common.Services.Blob.Models;
using Outbracket.Common.Services.Blob.S3;
using Outbracket.Common.Services.Blob.StorageAccount;
using Outbracket.Common.Services.Notifier.Email;
using Outbracket.Controllers;
using Outbracket.Controllers.Validators;
using Outbracket.Mongo.Repositories;
using Outbracket.Mongo.Repositories.Contracts;
using Outbracket.Mongo.Repositories.Contracts.Interfaces.UserSettings;
using Outbracket.Mongo.Repositories.Implementations.UserSettings;
using Outbracket.Repositories.Implementations.Account;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Implementations.Account;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Contracts.Interfaces.Dictionaries;
using Outbracket.Repositories.Implementations.Dictionaries;
using Outbracket.Services.Contracts.Interfaces.Dictionary;
using Outbracket.Services.Implementations.Dictionary;
using WebApi.Binders;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddFluentValidation();
            
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Outbracket")));
            services.Configure<MongoConnectionStrings>(
                Configuration.GetSection(nameof(MongoConnectionStrings)));
            services.AddSingleton<IMongoConnectionStrings>(sp =>
                sp.GetRequiredService<IOptions<MongoConnectionStrings>>().Value);
            
            var assembly = typeof(ApiControllerBase).Assembly;
            services.AddControllersWithViews().AddApplicationPart(assembly);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Auth:JWTSecretKey"))
                        )
                    };
                });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();

            services.AddSingleton<IAuthService>(
                new AuthService(
                    Configuration.GetValue<string>("Auth:JWTSecretKey"),
                    Configuration.GetValue<int>("Auth:JWTLifespan"),
                    Configuration.GetValue<int>("Auth:RefreshTokenLifespan"),
                    Configuration.GetValue<int>("Auth:EmailConfirmationTokenLifespan"),
                    Configuration.GetValue<int>("Auth:ResetPasswordTokenLifespan")
                )
            );

            services.AddTransient<IStorageAccountBlobUtility, StorageAccountBlobUtility>(x => new StorageAccountBlobUtility(
                new StorageAccountOptions
                {
                    StorageAccountKeyOption = Configuration["StorageAccount:StorageAccountKeyOption"], 
                    StorageAccountNameOption = Configuration["StorageAccount:StorageAccountNameOption"]
                },
                new BlobContainers
                {
                    FullImagesContainerNameOption = Configuration["StorageAccount:FullImagesContainerNameOption"], 
                    ScaledImagesContainerNameOption = Configuration["StorageAccount:ScaledImagesContainerNameOption"]
                }
            ));
            services.AddTransient<IS3BlobUtility, S3BlobUtility>(x => new S3BlobUtility(
                new S3AccountOptions()
                {
                    AwsKey = Configuration["S3:AwsKey"],
                    AwsSecretKey = Configuration["S3:AwsSecretKey"],
                    BucketRegion = Configuration["S3:BucketRegion"]
                },
                new BlobContainers
                {
                    FullImagesContainerNameOption = Configuration["StorageAccount:FullImagesContainerNameOption"], 
                    ScaledImagesContainerNameOption = Configuration["StorageAccount:ScaledImagesContainerNameOption"]
                }, Configuration["S3:Buckets:Blob"]
            ));
            
            services.AddTransient<IValidator<UpdateUserInfoApiRequest>, UserInfoValidator>();
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddTransient<IEmailSender, EmailSender>(x => new EmailSender(
                new EmailSenderOptions
                {
                    ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"],
                    SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"],
                    SenderName = Configuration["ExternalProviders:SendGrid:SenderName"]
                }, x.GetRequiredService<IRazorViewToStringRenderer>())
            );
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
            }).AddJsonOptions(option =>
                option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
