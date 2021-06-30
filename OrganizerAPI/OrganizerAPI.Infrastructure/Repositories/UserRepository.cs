using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Models;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(OrganizerContext oContext) : base(oContext)
        {

        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            try
            {
                return await OrganizerContext.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password);
            }
            catch (Exception)
            {
                throw;
            }       
        }

        public async Task<User> GetUserByRefreshToken(string token)
        {
            try
            {
                return await OrganizerContext.Users.Include(u => u.UserRefreshTokens).SingleOrDefaultAsync(u => u.UserRefreshTokens.Any(rt => rt.Token == token));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
