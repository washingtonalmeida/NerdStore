
namespace NerdStore.Payments.AntiCorruption.Interfaces
{
    public interface IPayPalConfigurationManager
    {
        string GetApiKey();
        string GetEncriptionKey();
    }
}
