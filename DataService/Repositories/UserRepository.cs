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

        //public async Task<UserResponse> UpdateUser(UserUpdateRequest user)
        //{
        //    var userInDb = await _context.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId);
        //    if (userInDb != null) 
        //    {
        //        _mapper.Map(user, userInDb);
        //        await _context.SaveChangesAsync();
        //        var updatedUserInDb = await _context.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId);
        //        return _mapper.Map<UserResponse>(updatedUserInDb);
        //    }
        //    return _mapper.Map<UserResponse>(userInDb);
        //}

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
        //public async Task<IEnumerable<UserDetailsResponse>> GetAllUsersDetails()
        //{
        //    var users = await _context.Users
        //        .Include(user => user.Expenses)
        //        .Include(user => user.Groups)
        //        .ToListAsync();
        //    return _mapper.Map<List<UserDetailsResponse>>(users);
        //}

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

        //public async Task<List<GroupResponse>> GetUserGroups(int id)
        //{
        //    var userGroups = await _context.Groups
        //                           .Where(g => g.Users.Any(u => u.UserId == id))
        //                           .Select(group => new Group
        //                           {
        //                               GroupId = group.GroupId,
        //                               GroupName = group.GroupName,
        //                               CreatorId = group.CreatorId,
        //                               TotalExpenses = group.TotalExpenses,
        //                               Creator = new User
        //                               {
        //                                   UserId = group.Creator.UserId,
        //                                   Username = group.Creator.Username
        //                               },
        //                               // Other properties as needed
        //                           })
        //                           .ToListAsync();
        //    return _mapper.Map<List<GroupResponse>>(userGroups);
        //}

        //public async Task<UserDetailsResponse> GetUserDetailsById(int id)
        //{
        //    var user = await _context.Users
        //        //.Include(user => user.Approvals)
        //        //.Include(user => user.ExpenseApprovals)
        //        .Include(user => user.Expenses)
        //        .Include(user => user.Groups)
        //        //.Include(u => u.Groups)
        //        .SingleOrDefaultAsync(x => x.UserId == id);
        //    return _mapper.Map<UserDetailsResponse>(user);
        //}

        //public async Task<List<ExpenseResponse>> GetUserExpenses(int id)
        //{
        //    _logger.LogDebug("Getting User Expenses");
        //    var userExpenses = await _context.Expenses
        //        .Include(e => e.ExpenseApprovals)
        //        .Include(e => e.Users)
        //        .Include(e => e.Group)
        //        .Where(e => e.Users
        //            .Any(u => u.UserId == id))
        //        .ToListAsync();
        //    return _mapper.Map<List<ExpenseResponse>>(userExpenses);
        //}
    }
}
