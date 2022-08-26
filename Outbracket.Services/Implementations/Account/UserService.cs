using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Outbracket.Common.Extensions;
using Outbracket.Entities.Account;
using Outbracket.Enums.DbDictionaries;
using Outbracket.Globalization;
using Outbracket.Helpers;
using Outbracket.Repositories.Contracts.Interfaces.Account;
using Outbracket.Repositories.Contracts.Interfaces.Dictionaries;
using Outbracket.Services.Contracts.Exceptions;
using Outbracket.Services.Contracts.Interfaces.Account;
using Outbracket.Services.Contracts.Models.Account;

namespace Outbracket.Services.Implementations.Account
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; set; }
        private IRoleRepository RoleRepository { get; set; }

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            var user = await UserRepository.GetFirstOrDefaultAsync(x => x.Email == email, y => y.Roles, y => y.UserTokens);
            return ToUserModel(user);
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var user = await UserRepository.GetFirstOrDefaultAsync(id);
            return ToUserModel(user);
        }

        public async Task RemoveAsync(Guid id)
        {
            var user = await UserRepository.GetFirstOrDefaultAsync(id);
            UserRepository.Delete(user);
            await UserRepository.CommitAsync();
        }

        public async Task<UserModel> AddAsync(UserCreateModel user)
        {
            var id = Guid.NewGuid();
            user.Id = id;
            if (!(await UserRepository.IsEmailUniqAsync(user.Email)))
            {
                throw new ValidationException(new []{ ValidationErrors.UserEmailExist});
            }

            if (!(await UserRepository.IsUsernameUniqAsync(user.Username)))
            {
                throw new ValidationException(new []{ ValidationErrors.UsernameExist});
            }

            var hashSalt = PasswordHasher.GenerateSaltedHash(64, user.Password);
            var playerRole = await RoleRepository.GetFirstOrDefaultAsync((int) Roles.Player);
            var userForCreate = new User()
            {
                Email = user.Email, 
                Id = id, 
                Username = user.Username, 
                PasswordHash = hashSalt.Hash, 
                PasswordSalt = hashSalt.Salt, 
                EmailConfirmed = false, 
                Roles = new List<Role>{ playerRole }
            };
            UserRepository.AddAsync(userForCreate);
            await UserRepository.CommitAsync();
            return ToUserModel(userForCreate);
        }
        
        public async Task ActivateUserByIdAsync(Guid id)
        {
            var user = await UserRepository.GetFirstOrDefaultAsync(id);
            user.EmailConfirmed = true;
            UserRepository.Update(user);
            await UserRepository.CommitAsync();
        }
        
        public async Task ResetPasswordByIdAsync(Guid id, string password)
        {
            var user = await UserRepository.GetFirstOrDefaultAsync(id);
            var hashSalt = PasswordHasher.GenerateSaltedHash(64, password);
            user.PasswordHash = hashSalt.Hash;
            user.PasswordSalt = hashSalt.Salt;
            UserRepository.Update(user);
            await UserRepository.CommitAsync();
        }

        private static UserModel ToUserModel(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserModel()
            {
                Email = user.Email,
                Id = user.Id,
                Username = user.Username, 
                PasswordSalt = user.PasswordSalt, 
                PasswordHash = user.PasswordHash,
                Roles = user.Roles.ToEnumerable(x => x.Name), 
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}
