using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class InitDataValidator
{
    public static bool Validate(string initDataRaw, string botToken, TimeSpan maxAge = default)
    {
        if (string.IsNullOrEmpty(initDataRaw) || string.IsNullOrEmpty(botToken))
            return false;

        var pairs = initDataRaw.Split('&')
            .Select(part => part.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .Select(parts => new { Key = parts[0], Value = Uri.UnescapeDataString(parts[1]) })
            .ToList();

        if (!pairs.Any(p => p.Key == "hash"))
            return false;

        var receivedHash = pairs.First(p => p.Key == "hash").Value;
        var dataCheckString = string.Join("\n",
            pairs.Where(p => p.Key != "hash" && p.Key != "signature")
                 .OrderBy(p => p.Key)
                 .Select(p => $"{p.Key}={p.Value}"));

        byte[] secretKey;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(botToken)))
        {
            secretKey = hmac.ComputeHash(Encoding.UTF8.GetBytes("WebAppData"));
        }

        byte[] computedHashBytes;
        using (var hmac = new HMACSHA256(secretKey))
        {
            computedHashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        }
        var computedHash = Convert.ToHexString(computedHashBytes).ToLowerInvariant();

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedHash),
                Encoding.UTF8.GetBytes(receivedHash)))
            return false;

        if (pairs.FirstOrDefault(p => p.Key == "auth_date")?.Value is string authDateStr &&
            long.TryParse(authDateStr, out long authDate))
        {
            var authTime = DateTimeOffset.FromUnixTimeSeconds(authDate);
            var age = DateTimeOffset.UtcNow - authTime;
            if (age > (maxAge == default ? TimeSpan.FromHours(1) : maxAge))
                return false;
        }

        return true;
    }

    public static long? GetUserId(string initDataRaw)
    {
        var userPart = initDataRaw.Split('&')
            .FirstOrDefault(part => part.StartsWith("user="));
        if (userPart == null) return null;

        try
        {
            var json = Uri.UnescapeDataString(userPart[5..]);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("id").GetInt64();
        }
        catch
        {
            return null;
        }
    }
}