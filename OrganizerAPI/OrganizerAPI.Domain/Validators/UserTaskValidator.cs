using FluentValidation;
using OrganizerAPI.Shared.ModelsDTO;

namespace OrganizerAPI.Domain.Validators
{
    public class UserTaskValidator : AbstractValidator<UserTaskDto>
    {
        public UserTaskValidator()
        {
            RuleFor(ut => ut.Title).NotEmpty().Length(1, 50);
            RuleFor(ut => ut.Description).Length(1, 200);
            RuleFor(ut => ut.Status).NotEmpty().IsInEnum();
            RuleFor(ut => ut.Date).NotEmpty();
        }
    }
}
