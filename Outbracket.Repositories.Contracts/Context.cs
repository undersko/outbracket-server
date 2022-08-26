using Microsoft.EntityFrameworkCore;
using System.Linq;
using Outbracket.Entities.Account;
using Outbracket.Entities.Dictionaries;

namespace Outbracket.Repositories.Contracts
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            ConfigureModelBuilderForUser(modelBuilder);
        }

        void ConfigureModelBuilderForUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserToken>().ToTable("UserTokens");
            modelBuilder.Entity<UserTokenType>().ToTable("UserTokenTypes");
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<UserInfo>().ToTable("UsersInfo");
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(user => user.Roles)
                .WithMany(role => role.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));
            modelBuilder.Entity<UserToken>()
                .HasOne(token => token.User)
                .WithMany(user => user.UserTokens);
            modelBuilder.Entity<UserToken>()
                .HasOne(token => token.Type)
                .WithMany(tokenType => tokenType.Tokens);
            modelBuilder.Entity<User>()
                .HasOne(user => user.UserInfo)
                .WithOne(userInfo => userInfo.User)
                .HasForeignKey<UserInfo>(user => user.Id);
        }
    }
}
