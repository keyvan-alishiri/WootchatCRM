using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Infrastructure.Services;

public class UserService : IUserService
{
   private readonly IRepository<User> _userRepository;
   private readonly IRepository<Team> _teamRepository;

   public UserService(
       IRepository<User> userRepository,
       IRepository<Team> teamRepository)
   {
      _userRepository = userRepository;
      _teamRepository = teamRepository;
   }

   #region ═══════════════════ Query Methods ═══════════════════

   public async Task<IEnumerable<User>> GetAllAsync()
   {
      return await _userRepository.Query()
          .Include(u => u.Team)
          .OrderBy(u => u.Name)
          .ToListAsync();
   }

   public async Task<User?> GetByIdAsync(int id)
   {
      return await _userRepository.Query()
          .Include(u => u.Team)
          .Include(u => u.AssignedConversations)
         
          .FirstOrDefaultAsync(u => u.Id == id);
   }

   public async Task<User?> GetByChatwootIdAsync(long chatwootUserId)
   {
      return await _userRepository.Query()
          .Include(u => u.Team)
          .FirstOrDefaultAsync(u => u.ChatwootUserId == chatwootUserId);
   }

   public async Task<IEnumerable<User>> GetByTeamIdAsync(int teamId)
   {
      return await _userRepository.Query()
          .Where(u => u.TeamId == teamId)
          .Include(u => u.Team)
          .OrderBy(u => u.Name)
          .ToListAsync();
   }

   #endregion

   #region ═══════════════════ Command Methods ═══════════════════

   public async Task<User> CreateAsync(User user)
   {
      // بررسی تکراری نبودن ChatwootUserId
      if (user.ChatwootUserId.HasValue)
      {
         var exists = await _userRepository
             .AnyAsync(u => u.ChatwootUserId == user.ChatwootUserId);

         if (exists)
            throw new InvalidOperationException($"User with Chatwoot ID {user.ChatwootUserId} already exists.");
      }

      // بررسی تکراری نبودن Email
      if (!string.IsNullOrWhiteSpace(user.Email))
      {
         var emailExists = await _userRepository
             .AnyAsync(u => u.Email != null && u.Email.ToLower() == user.Email.ToLower());

         if (emailExists)
            throw new InvalidOperationException($"User with email {user.Email} already exists.");
      }

      // بررسی وجود Team
      if (user.TeamId.HasValue)
      {
         var teamExists = await _teamRepository.AnyAsync(t => t.Id == user.TeamId);
         if (!teamExists)
            throw new InvalidOperationException($"Team with ID {user.TeamId} not found.");
      }

      user.CreatedAt = DateTime.UtcNow;

      return await _userRepository.AddAsync(user);
   }

   public async Task<User> UpdateAsync(User user)
   {
      var existing = await _userRepository.GetByIdAsync(user.Id);

      if (existing == null)
         throw new InvalidOperationException($"User with ID {user.Id} not found.");

      // بررسی تکراری نبودن Email (اگر تغییر کرده)
      if (!string.IsNullOrWhiteSpace(user.Email) &&
          existing.Email?.ToLower() != user.Email.ToLower())
      {
         var emailExists = await _userRepository
             .AnyAsync(u => u.Id != user.Id &&
                           u.Email != null &&
                           u.Email.ToLower() == user.Email.ToLower());

         if (emailExists)
            throw new InvalidOperationException($"User with email {user.Email} already exists.");
      }

      // بررسی وجود Team جدید
      if (user.TeamId.HasValue && user.TeamId != existing.TeamId)
      {
         var teamExists = await _teamRepository.AnyAsync(t => t.Id == user.TeamId);
         if (!teamExists)
            throw new InvalidOperationException($"Team with ID {user.TeamId} not found.");
      }

      // به‌روزرسانی فیلدها
      existing.Name = user.Name;
      existing.Email = user.Email;
    
      existing.Role = user.Role;
      existing.TeamId = user.TeamId;
      
      existing.UpdatedAt = DateTime.UtcNow;

      await _userRepository.UpdateAsync(existing);

      return existing;
   }

   public async Task DeleteAsync(int id)
   {
      var user = await _userRepository.GetByIdAsync(id);

      if (user != null)
      {
         await _userRepository.DeleteAsync(user);
      }
   }

   #endregion
}
