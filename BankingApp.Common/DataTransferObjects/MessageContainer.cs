using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BankingApp.Common.Interfaces;

namespace BankingApp.Common.DataTransferObjects
{
    public class MessageContainer()
    {
        public MessageContainer(IUnitOfWork unitOfWork) : this()
        {
            UnitOfWork = unitOfWork;
        }
        public Dictionary<string, object> Contents { get; set; } = new Dictionary<string, object>();

        public IUnitOfWork? UnitOfWork { get; set; }

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

        public T? Get<T>()
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
            var jsonElement = msg.Get<JsonElement>(key);
            var jsonString = jsonElement.GetRawText();

            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                var itemType = typeof(T).GetGenericArguments().FirstOrDefault();

                var listType = typeof(List<>).MakeGenericType(itemType);
                return (T)JsonSerializer.Deserialize(jsonString, listType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public T? TryToObject<T>(MessageContainer msg, string key)
        {
            try
            {
                var jsonElement = msg.Get<JsonElement>(key);
                var jsonString = jsonElement.GetRawText();

                if (jsonElement.ValueKind == JsonValueKind.Array)
                {
                    var itemType = typeof(T).GetGenericArguments().FirstOrDefault();

                    var listType = typeof(List<>).MakeGenericType(itemType);
                    return (T)JsonSerializer.Deserialize(jsonString, listType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (KeyNotFoundException e)
            {
                return default;
            }
        }
    }
}
