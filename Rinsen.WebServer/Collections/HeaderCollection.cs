using System.Collections;
using System.Text;

namespace Rinsen.WebServer.Collections
{
    public class HeaderCollection
    {
        private Hashtable _valueTable;

        public HeaderCollection()
        {
            _valueTable = new Hashtable();
        }

        public string this[string key]
        {
            get
            {
                return (string)_valueTable[key];
            }
            set
            {
                _valueTable[key] = value;
            }

        }

        public bool ContainsKey(string key)
        {
            return _valueTable.Contains(key);
        }

        public string GetValue(string key)
        {
            return (string)_valueTable[key];
        }

        public void AddValue(string key, string value)
        {
            if (ContainsKey(key))
                _valueTable[key] = value;
            else
                _valueTable.Add(key, value);
        }

        public IEnumerable Keys
        {
            get
            {
                var keys = new ArrayList();
                foreach (var key in _valueTable.Keys)
                {
                    if (key != null)
                        keys.Add(key);
                }
                return keys;
            }
        }

        /// <summary>
        /// This is only for debug display of headers
        /// </summary>
        /// <returns>All headers with format key: value</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var key in Keys)
	        {
                sb.Append(key + ": " + _valueTable[key] + "\r\n");
            }
            return sb.ToString();
        }
    }
}
