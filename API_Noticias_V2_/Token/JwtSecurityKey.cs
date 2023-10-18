using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_Noticias_V2_.Token
{
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
