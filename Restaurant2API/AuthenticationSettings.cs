namespace Restaurant2API
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpierDays { get; set; }
        public string JwtIssuer { get; set; }
    }
}
