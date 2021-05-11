using NerdStore.Payments.AntiCorruption.Interfaces;

namespace NerdStore.Payments.AntiCorruption.Implementations
{
    public class PayPalConfigurationManager : IPayPalConfigurationManager
    {
        private readonly IConfigurationManager _configurationManager;

        public PayPalConfigurationManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        
        public string GetApiKey()
        {
            return _configurationManager.GetValue("apikey");
        }

        public string GetEncriptionKey()
        {
            return _configurationManager.GetValue("encriptionKey");
        }
    }
}
