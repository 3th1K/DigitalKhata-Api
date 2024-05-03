using AutoMapper;
using Common;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DigitalKhataDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IMapper mapper, DigitalKhataDbContext context, ILogger<UserRepository> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }
        public async Task<UserResponse> CreateUser(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var addedUser = await _context.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId);
            return _mapper.Map<UserResponse>(addedUser);
        }

        public async Task<User?> DeleteUser(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserId == id);
            if (user != null) 
            { 
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userResponses = _mapper.Map<List<UserResponse>>(users);
            return userResponses;
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserId == id);
            var userResponse = _mapper.Map<UserResponse>(user);
            return userResponse;
        }
        public async Task<User?> GetUserByUsernameOrEmail(string username, string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
            return user;
        }

        public async Task<List<UserResponse>> SearchUserByUsername(string searchString)
        {
            List<User> users = await _context.Users.Where(u => u.Username.Contains(searchString)).ToListAsync();
            return _mapper.Map<List<UserResponse>>(users);
        }
    }
}
