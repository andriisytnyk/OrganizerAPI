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
            RuleFor(ut => ut.Title).NotEmpty().Length(1, 50);
            RuleFor(ut => ut.Description).Length(1, 200);
            RuleFor(ut => ut.Status).IsInEnum();
            RuleFor(ut => ut.Date).NotNull();
            RuleFor(ut => ut.UserId).NotEmpty();
        }
    }
}
