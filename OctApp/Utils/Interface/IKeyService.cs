using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Utils.Interface
{
    public interface IKeyService
    {
    string GeneratePrivateKey();
    string GeneratePublicKey();
    }
}