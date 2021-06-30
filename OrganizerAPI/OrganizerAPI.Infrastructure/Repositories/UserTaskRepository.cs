using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Models;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class UserTaskRepository : BaseRepository<UserTask>
    {
        public UserTaskRepository(OrganizerContext oContext) : base(oContext)
        {

        }
    }
}
