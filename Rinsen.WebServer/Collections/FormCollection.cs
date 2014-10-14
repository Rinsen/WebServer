using System.Collections;

namespace Fredde.Web.MicroFramework
{
    public class FormCollection
    {
        private Hashtable _valueTable;

        public FormCollection()
        {
            _valueTable = new Hashtable();
        }

        public FormCollection(string queryString)
        {
            _valueTable = new Hashtable();
            AddQueryString(queryString);
        }

        internal void AddQueryString(string queryString)
        {
            var keyValuePairs = queryString.Split('&');
            if (keyValuePairs.Length > 0)
            {
                foreach (var keyValuePair in keyValuePairs)
                {
                    var keyValueParts = keyValuePair.Split('=');

                    AddValue(keyValueParts[0], keyValueParts[1]);
                }
            }
        }

        public string this[string key]
        {
            get
            {
                return (string)_valueTable[key];
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

    }
}
