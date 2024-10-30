using System.Security.Cryptography;
using System.Text;

namespace VehicleMaintenance.Middleware
{
    public class GuidCreate
    {

        public static Guid GenerateGuidFromUsername(string username)
        {
            using (var md5 = MD5.Create())
            {
                if (!string.IsNullOrEmpty(username))
                {
                    byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(username));
                    return new Guid(hashBytes);
                }
                return Guid.Empty;
            }
        }
    }
}
