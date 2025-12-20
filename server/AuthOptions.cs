using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FlowerBot
{
    public static class AuthOptions
    {
        public static readonly string ISSUER = "FlowerBot_Server"; 
        public static readonly string AUDIENCE = "FlowerBot_Client";
        private const string KEY = "mlaAMs?tx.0{szON^g_g,kkab7CjacsEhPnhQJ@KMV0V3TlP1gr)*Rv7^/&+&51H";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
