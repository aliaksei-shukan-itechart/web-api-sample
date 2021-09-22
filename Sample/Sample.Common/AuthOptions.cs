using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Sample.Common
{
    public class AuthOptions
    {
        private const string KEY = "secret_key_12345";

        public const string ISSUER = "TestAuthServer";
        public const string AUDIENCE = "TestAuthClient";
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey SecurityKey
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }
    }
}
