using Newtonsoft.Json;

namespace Ecommerce.Utility
{
    public static class SessionExtenstion
    {
        public static void Set<T>(this ISession session, string Key,T value)
        {
            session.SetString(Key,JsonConvert.SerializeObject(value));
        }
        public static T Get<T>(this ISession session, string Key)
        {
            var value = session.GetString(Key);
            return value==null? default(T): JsonConvert.DeserializeObject<T>(value);
        }
    }
}
