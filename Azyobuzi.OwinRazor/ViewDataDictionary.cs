using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Azyobuzi.OwinRazor
{
    public class ViewDataDictionary : DynamicObject, IDictionary<string, object>, IReadOnlyDictionary<string, object>
    {
        private IDictionary<string, object> innerDictionary = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            this.innerDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return this.innerDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.innerDictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                object value;
                this.innerDictionary.TryGetValue(key, out value);
                return value;
            }
            set
            {
                this.innerDictionary[key] = value;
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            this.innerDictionary.Add(item);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return this.innerDictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.innerDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return this.innerDictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.innerDictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerable<string> IReadOnlyDictionary<string, object>.Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        IEnumerable<object> IReadOnlyDictionary<string, object>.Values
        {
            get { return this.innerDictionary.Values; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }
    }
}
