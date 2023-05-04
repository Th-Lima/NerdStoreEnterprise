namespace NSE.Pagamentos.NerdsPag
{
    public class NerdsPayService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public NerdsPayService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}