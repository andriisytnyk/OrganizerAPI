using System.ComponentModel.DataAnnotations;

namespace OrganizerAPI.Models.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
