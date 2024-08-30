using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class MessageContainer
    {
        public Dictionary<string, object> Contents { get; set; } = new Dictionary<string, object>();

        public void Add(object content)
        {
            Contents.Add(content.GetType().FullName!, content);
        }

        public void Add(string key, object content)
        {
            Contents.Add(key, content);
        }
        public void Clear()
        {
            Contents = new Dictionary<string, object>();
        }

        public T Get<T>()
        {
            foreach (var content in Contents.Values)
            {
                if (content is T)
                {
                    return (T)content;
                }
            }
            return default;
        }

        public T Get<T>(string key)
        {
            return (T)Contents[key];
        }

        public T ToObject<T>(MessageContainer msg, string key)
        {
            return JsonSerializer.Deserialize<T>(msg.Get<JsonElement>(key).GetRawText(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

    }
}
