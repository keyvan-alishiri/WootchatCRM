using WootchatCRM.Core.Entities;

namespace WootchatCRM.Core.Interfaces.Services;

public interface IUserService
{
   Task<IEnumerable<User>> GetAllAsync();
   Task<User?> GetByIdAsync(int id);
   Task<User?> GetByChatwootIdAsync(long chatwootUserId);
   Task<IEnumerable<User>> GetByTeamIdAsync(int teamId);
   Task<User> CreateAsync(User user);
   Task<User> UpdateAsync(User user);
   Task DeleteAsync(int id);
}
