namespace OrganizerAPI.Models.Common
{
    public class TokensPair
    {
        public string JwtToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public TokensPair(string jwtToken, RefreshToken refreshToken)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
