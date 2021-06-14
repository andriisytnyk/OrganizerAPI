using FluentValidation;
using OrganizerAPI.Models.Common;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Validators
{
    public class UserTaskValidator : AbstractValidator<UserTaskDTO>
    {
        public UserTaskValidator()
        {
            RuleFor(g => g.Title).NotEmpty().Length(1, 50);
            RuleFor(g => g.Description).Length(1, 200);
            RuleFor(g => g.Status).IsInEnum();
            RuleFor(g => g.Date).NotNull();
        }
    }
}
