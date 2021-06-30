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
            return await OrganizerContext.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
