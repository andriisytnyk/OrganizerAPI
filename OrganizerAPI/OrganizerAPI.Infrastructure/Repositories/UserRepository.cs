using System.Threading.Tasks;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Models;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(OrganizerContext oContext) : base(oContext)
        {

        }
    }
}
