using FluentValidation;
using OrganizerAPI.Shared.ModelsDTO;

namespace OrganizerAPI.Domain.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.FirstName).Length(1, 30);
            RuleFor(u => u.LastName).Length(1, 30);
            RuleFor(u => u.Username).NotEmpty().Length(4, 20);
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.UserTasks).NotNull();
        }
    }
}
