using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SPA
{
    public class AuthOptions
    {
        public const string ISSUER = "Server";
        public const string AUDIENCE = "Client"; 
        const string KEY = "mysupersecret_secretkey!123";
        public const int LIFETIME = 10; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
