using JobPortal.Models;

namespace JobPortal.Services.Abstraction
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<UserProfile?> GetProfileAsync(int userId);
        Task SaveProfileAsync(UserProfile profile);
    }
}
