using System.Text.Json;

namespace ShopApp.Web.Helpers;

public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value) 
    {
        session.SetString(key, JsonSerializer.Serialize(value));//JsonSerializer bu metod ile veriyi stringe çevireceğiz 
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        return sessionData == null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
    }
    
}