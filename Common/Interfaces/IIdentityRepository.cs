

using Common.Models;

namespace Common.Interfaces;

public interface IIdentityRepository
{
    public Task<User?> ValidateUser(string username, string password);
    public Task<string> GetToken(string username, string password);
}
