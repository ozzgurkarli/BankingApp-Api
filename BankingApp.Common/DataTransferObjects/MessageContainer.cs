using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Common.DataTransferObjects
{
    public class MessageContainer
    {
        public Dictionary<string, object> Contents { get; set; } = new Dictionary<string, object>();

        public void Add(object content)
        {
            Contents.Add(string.Empty, content);
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
    }
}
