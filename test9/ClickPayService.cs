using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace InstagramDMs.API
{
    public class ClickPayService(IOptions<ClickPaySettings> clickPaySettings)
    {


        public string ComputeHmacSha256(string jsonPayload)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clickPaySettings.Value.ServerKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(jsonPayload));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


    }
}
