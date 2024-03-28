﻿using System.Text.Json;

namespace ShopQuanAo.Infrastructure
{
    public static class SessionExtension
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T): JsonSerializer.Deserialize<T>(value);
        }
    }
}
