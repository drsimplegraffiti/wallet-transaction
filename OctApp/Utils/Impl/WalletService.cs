using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NBitcoin;
using OctApp.Utils.Interface;
using Key = NBitcoin.Key;

namespace OctApp.Utils.Impl
{
    public class WalletService : IWalletService
    {
        public (string walletAddress, string publicKey, string privateKey) GenerateWallet()
        {
            var privateKey = new Key();
            var publicKey = privateKey.PubKey;
            var address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main);

            // Extract just the private key value without the "NBitcoin.key(" prefix
            var privateKeyValue = privateKey.ToString(Network.Main);

            return (address.ToString(), publicKey.ToString(), privateKeyValue);
        }

    }
}