using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Models;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository<UserTask>
    {
        public TaskRepository(OrganizerContext oContext) : base(oContext)
        {

        }
    }
}
