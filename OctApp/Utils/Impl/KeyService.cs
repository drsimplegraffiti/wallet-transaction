using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using OctApp.Utils.Interface;
using System.Security.Cryptography;


namespace OctApp.Utils.Impl
{
    public class KeyService : IKeyService
    {
        public string GeneratePrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            }
        }

        public string GeneratePublicKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                return Convert.ToBase64String(rsa.ExportRSAPublicKey());
            }
        }
    }
}