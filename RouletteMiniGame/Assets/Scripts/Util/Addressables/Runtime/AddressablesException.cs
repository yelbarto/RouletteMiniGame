using System;

namespace Util.Addressables.Runtime
{
    public class AddressablesException : Exception
    {
        protected AddressablesException(string message) : base(message)
        {
        }
    }
    
    public class AddressablesAssetLoadException : AddressablesException
    {
        public AddressablesAssetLoadException(string message) : base(message)
        {
        }
    }
    
    public class AddressablesAssetDownloadException : AddressablesException
    {
        public AddressablesAssetDownloadException(string message) : base(message)
        {
        }
    }
}