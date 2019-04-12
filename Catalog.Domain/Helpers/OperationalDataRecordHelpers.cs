using System.Security.Cryptography;
using System.Text;
using Catalog.Domain.Entity;
using Newtonsoft.Json;

namespace Catalog.Domain.Helpers
{
    public static class OperationalDataRecordHelpers
    {
        public static string CalculateDigest(this OperationalDataRecord record)
        {
            var json          = JsonConvert.SerializeObject(record);
            var stringBuilder = new StringBuilder();

            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
                foreach (var _byte in hash)
                    stringBuilder.Append(_byte.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}