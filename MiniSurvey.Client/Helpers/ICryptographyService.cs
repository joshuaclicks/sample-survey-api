using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public interface ICryptographyService
    {
        HashDetail GenerateHash(string input);
        bool ValidateHash(string input, string salt, string hashedValue);
        string Base64Encode(string plainText);
        string Base64Decode(string base64EncodedData);
        string ComputeHmac256(string key, string model);
    }
}
