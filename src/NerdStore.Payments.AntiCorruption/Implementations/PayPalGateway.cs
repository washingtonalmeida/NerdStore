using NerdStore.Payments.AntiCorruption.Interfaces;
using System;
using System.Linq;

namespace NerdStore.Payments.AntiCorruption.Implementations
{
    public class PayPalGateway : IPayPalGateway
    {
        public string GetPayPalServiceKey(string apiKey, string encriptionKey)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public string GetCardHashKey(string serviceKey, string cardNumber)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public bool CommitTransaction(string cardHashKey, string orderId, decimal totalPrice)
        {
            return new Random().Next(2) == 0;
        }
    }
}
