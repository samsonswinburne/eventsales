using System.Security.Cryptography;
using EventSalesBackend.Data;
using EventSalesBackend.Services.Interfaces;

namespace EventSalesBackend.Services.Implementation;

public class CryptoService : ICryptoService
{
    public string GenerateKey()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Excludes confusing chars
        var bytes = RandomNumberGenerator.GetBytes(Constants.EventKeyLength);
    
        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}