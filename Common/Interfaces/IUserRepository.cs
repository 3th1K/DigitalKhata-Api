using Common.DTOs.UserDTOs;
using Common.Models;

namespace Common.Interfaces;

public interface IUserRepository
{
    public Task<List<UserResponse>> GetAllUsers();
    public Task<UserResponse> GetUserById(int id);
    public Task<User?> GetUserByUsernameOrEmail(string username, string email);
    public Task<UserResponse> CreateUser(User user);
    public Task<User?> DeleteUser(int id);
    public Task<List<UserResponse>> SearchUserByUsername(string searchString);
}
