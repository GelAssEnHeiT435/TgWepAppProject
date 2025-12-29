using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class InitDataValidator
{
    public static bool Validate(SortedDictionary<string, string> dataDict, string botToken, ILogger logger, TimeSpan maxAge = default)
    {
        string constantKey = "WebAppData";

        var dataCheckString = string.Join(
            '\n',
            dataDict.Where(x => x.Key != "hash")
                    .Select(x => $"{x.Key}={x.Value}")
        );

        logger.LogDebug("Data-check-string: {DataCheckString}", dataCheckString);

        var secretKey = ComputeHMACSHA256(
            Encoding.UTF8.GetBytes(constantKey),
            Encoding.UTF8.GetBytes(botToken)
        );

        var generatedHash = ComputeHMACSHA256(
            secretKey,
            Encoding.UTF8.GetBytes(dataCheckString)
        );

        // Convert received hash from telegram to a byte array.
        byte[] actualHash;
        try
        {
            actualHash = Convert.FromHexString(dataDict["hash"]);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to parse 'hash' as hex: {HashValue}", dataDict["hash"]);
            return false;
        }

        // Compare our hash with the one from telegram.
        if (actualHash.SequenceEqual(generatedHash))
        {
            logger.LogInformation("InitData hash validation succeeded.");

            // Optionally, check the auth_date to prevent outdated data
            if (dataDict.TryGetValue("auth_date", out var authDateStr) && long.TryParse(authDateStr, out var authDate))
            {
                var authDateTime = DateTimeOffset.FromUnixTimeSeconds(authDate);
                var now = DateTimeOffset.UtcNow;
                var age = now - authDateTime;

                logger.LogDebug("Auth date: {AuthDateTime}, age: {Age}", authDateTime, age);

                if (authDateTime < now.AddMinutes(-5))
                {
                    logger.LogWarning("InitData is too old (>5 minutes): {AuthDateTime}", authDateTime);
                    return false;
                }
            }

            return true;
        }

        logger.LogWarning("InitData hash validation failed. Expected hash doesn't match.");
        return false;
    }

    public static long? GetUserId(SortedDictionary<string, string> dataDict)
    {
        if (!dataDict.TryGetValue("user", out var userJson) || string.IsNullOrEmpty(userJson))
            return null;

        try
        {
            using var doc = JsonDocument.Parse(userJson);
            if (doc.RootElement.TryGetProperty("id", out var idElement) &&
                idElement.ValueKind == JsonValueKind.Number)
            {
                return idElement.GetInt64();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        return null;
    }

    private static byte[] ComputeHMACSHA256(byte[] key, byte[] data)
    {
        using var hmac = new HMACSHA256(key);
        return hmac.ComputeHash(data);
    }
}