using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BluePayLibrary.Interfaces.BluePay20Post;
using BluePayLibrary.Interfaces.BluePay20Post.Fluent;

namespace BluePayLibrary.Interfaces
{
    public class BluePayMessage : DynamicObject
    {
        private readonly Dictionary<string, object> _fields;

        public BluePayMessage()
        {
            _fields = new Dictionary<string, object>();
        }

        public BluePayMessage(Dictionary<string, object> fields)
        {
            _fields = fields;
        }

        public static BluePayMessage Parse(TextReader tr)
        {
            using (var parser = new FormEncodedResponseParser(tr))
            {
                return new BluePayMessage(parser.ReadAll().ToDictionary(kvp => kvp.Item1, kvp => (object)kvp.Item2));
            }
        }

        public object this[string index]
        {
            get
            {
                object result;
                _fields.TryGetValue(index, out result);
                return result;
            }
            set { _fields[index] = value; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            //any member "exists" but would just be null to make life easy
            return true;
        }
        
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _fields.Keys;
        }

        public IEnumerable<KeyValuePair<string, object>> GetFields()
        {
            return _fields;
        }

        public static IBluePay20PostRequestBuilder Build(string accountId, Mode mode)
        {
            return new BluePay20PostRequestBuilder()
                .WithFields(f => f.AccountId(accountId).Mode(mode));
        }
    }
}