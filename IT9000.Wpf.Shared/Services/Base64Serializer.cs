using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Services
{
    public static class Base64Serializer
    {
        public static TItem? FromBase64String<TItem>(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return JsonSerializer.Deserialize<TItem>(bytes.AsSpan());
        }
        public static async ValueTask<TItem?> FromBase64StringAsync<TItem>(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using MemoryStream stream = new(bytes);
            return await JsonSerializer.DeserializeAsync<TItem>(stream);
        }
        public static string ToBase64String<TItem>(TItem item)
        {
            using MemoryStream stream = new();
            using Utf8JsonWriter utf8JsonWriter = new(stream);
            JsonSerializer.Serialize<TItem>(utf8JsonWriter, item);
            stream.Seek(0, SeekOrigin.Begin);
            return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
        }
        public static async ValueTask<string> ToBase64StringAsync<TItem>(TItem item)
        {
            using MemoryStream stream = new();
            await JsonSerializer.SerializeAsync<TItem>(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            return Convert.ToBase64String(stream.GetBuffer(), 0,(int)stream.Length);
        }
    }
}
