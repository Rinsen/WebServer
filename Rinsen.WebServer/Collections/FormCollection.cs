using System;
using System.Collections;

namespace Rinsen.WebServer.Collections
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
            if (queryString == null)
            {
                throw new NullReferenceException("Query string can not be null");
            }

            if (queryString == string.Empty)
            {
                return;
            }
            
            var keyValuePairs = queryString.Split('&');
            foreach (var keyValuePair in keyValuePairs)
            {
                var keyValueParts = keyValuePair.Split('=');

                // If no equal sign is found, skip this part
                if (!(keyValueParts.Length > 1))
                {
                    continue;
                }

                AddValue(keyValueParts[0], keyValueParts[1]);
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
