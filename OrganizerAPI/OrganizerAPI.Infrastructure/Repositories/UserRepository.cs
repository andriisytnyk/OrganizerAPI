using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(OrganizerContext oContext) : base(oContext)
        {

        }
    }
}
