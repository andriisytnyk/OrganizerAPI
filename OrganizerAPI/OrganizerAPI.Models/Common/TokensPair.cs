using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Models.Common
{
    public class TokensPair
    {
        public string JwtToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public TokensPair(string jwtToken, RefreshToken refreshToken)
        {
            this.JwtToken = jwtToken;
            this.RefreshToken = refreshToken;
        }
    }
}
