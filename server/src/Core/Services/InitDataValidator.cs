using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class InitDataValidator
{
    public static bool Validate(string initDataRaw, string botToken, ILogger logger, TimeSpan maxAge = default)
    {
        logger.LogInformation("Начало валидации InitData. Длина: {Length}", initDataRaw?.Length ?? 0);
        logger.LogDebug("Длина botToken: {TokenLength}", botToken.Length);

        if (string.IsNullOrEmpty(initDataRaw) || string.IsNullOrEmpty(botToken))
        {
            logger.LogWarning("initDataRaw или botToken пусты");
            return false;
        }

        // Парсим БЕЗ декодирования
        var pairs = initDataRaw.Split('&')
            .Select(part => part.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .Select(parts => new { Key = parts[0], Value = parts[1] })
            .ToList();

        logger.LogDebug("Распарсено {Count} пар", pairs.Count);

        if (!pairs.Any(p => p.Key == "hash"))
        {
            logger.LogWarning("Хеш отсутствует в данных");
            return false;
        }

        var receivedHash = pairs.First(p => p.Key == "hash").Value;
        logger.LogDebug("Получен хеш: {Hash}", receivedHash);

        // Собираем dataCheckString
        var dataCheckString = string.Join("\n",
            pairs.Where(p => p.Key != "hash" && p.Key != "signature")
                 .OrderBy(p => p.Key)
                 .Select(p => $"{p.Key}={p.Value}"));

        logger.LogDebug("dataCheckString:\n{DataCheckString}", dataCheckString);

        // Вычисляем secret_key
        byte[] secretKey;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(botToken)))
        {
            secretKey = hmac.ComputeHash(Encoding.UTF8.GetBytes("WebAppData"));
        }

        // Вычисляем хеш
        byte[] computedHashBytes;
        using (var hmac = new HMACSHA256(secretKey))
        {
            computedHashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        }
        var computedHash = Convert.ToHexString(computedHashBytes).ToLowerInvariant();

        logger.LogDebug("Вычисленный хеш: {ComputedHash}", computedHash);

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedHash),
                Encoding.UTF8.GetBytes(receivedHash)))
        {
            logger.LogWarning("Хеши НЕ совпадают!");
            return false;
        }

        // Проверка срока действия
        if (pairs.FirstOrDefault(p => p.Key == "auth_date")?.Value is string authDateStr &&
            long.TryParse(authDateStr, out long authDate))
        {
            var authTime = DateTimeOffset.FromUnixTimeSeconds(authDate);
            var age = DateTimeOffset.UtcNow - authTime;
            logger.LogDebug("Возраст данных: {Age}", age);
            if (age > (maxAge == default ? TimeSpan.FromHours(1) : maxAge))
            {
                logger.LogWarning("Данные устарели");
                return false;
            }
        }

        logger.LogInformation("Валидация успешна");
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