using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Outbracket.Services.Contracts.Models.Account;

namespace Outbracket.Services.Contracts.Interfaces.Account
{
    public interface IUserService
    {
        Task<UserModel> GetByEmailAsync(string email);
        Task<UserModel> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task<UserModel> AddAsync(UserCreateModel user);
        Task ActivateUserByIdAsync(Guid id);
        Task ResetPasswordByIdAsync(Guid id, string password);
    }
}
