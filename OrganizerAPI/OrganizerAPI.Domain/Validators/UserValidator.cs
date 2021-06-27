using FluentValidation;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Validators
{
    public class UserValidator : AbstractValidator<UserRequestDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).Length(1, 30);
            RuleFor(u => u.LastName).Length(1, 30);
            RuleFor(u => u.Username).NotEmpty().Length(4,20);
            RuleFor(u => u.Password).NotEmpty().Length(6, 30);//.Must(HasValidPassword);
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.UserTasks).NotNull();
            RuleFor(u => u.UserRefreshTokens).NotNull();
        }

        private bool HasValidPassword(string pw)
        {
            var digit = new Regex("(?=.*[0-9])");
            var uppercase = new Regex("(?=.*[A-Z])");
            var lowercase = new Regex("(?=.*[a-z])");
            var symbol = new Regex("(\\W)+");
            var more6 = new Regex("[0 - 9a - zA - Z!@#$%^&*]{6,30}");

            return lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw) && more6.IsMatch(pw);
        }
    }
}
