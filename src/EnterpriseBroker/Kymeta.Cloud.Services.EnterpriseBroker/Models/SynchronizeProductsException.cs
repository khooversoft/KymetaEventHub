namespace Kymeta.Cloud.Services.EnterpriseBroker.Models
{
    [Serializable]
    public class SynchronizeProductsException : Exception
    {
        public SynchronizeProductsException()
        { }

        public SynchronizeProductsException(string message)
            : base(message)
        { }

        public SynchronizeProductsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
