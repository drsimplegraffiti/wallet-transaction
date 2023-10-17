using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Utils.Interface
{
    public interface IWalletService
    {
          (string walletAddress, string publicKey, string privateKey) GenerateWallet();
    }
}