﻿
namespace NerdStore.Payments.AntiCorruption.Interfaces
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);
        string GetCardHashKey(string serviceKey, string cardNumber);
        bool CommitTransaction(string cardHashKey, string orderId, decimal totalPrice);
    }
}
