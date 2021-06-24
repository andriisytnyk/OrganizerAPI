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
    public class UserValidator : AbstractValidator<UserRequestDTO>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).Length(1, 30);
            RuleFor(u => u.LastName).Length(1, 30);
            RuleFor(u => u.Username).NotEmpty().Length(4,20);
            RuleFor(u => u.Password).NotEmpty().Length(8, 30);//.Must(HasValidPassword);
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.UserTasks).NotNull();
            RuleFor(u => u.UserRefreshTokens).NotNull();
        }

        private bool HasValidPassword(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return (lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw));
        }
    }
}
