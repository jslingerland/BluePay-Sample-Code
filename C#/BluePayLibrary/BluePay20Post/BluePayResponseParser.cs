using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePayResponseParser<T> : IBluePayResponseParser<T> where T:new()
    {
        private readonly IBluePayResponseObjectConverter<T> _converter;

        public BluePayResponseParser()
        {
            _converter = new DefaultBluePayResponseObjectConverter<T>();
        }

        public BluePayResponseParser(IBluePayResponseObjectConverter<T> converter)
        {
            _converter = converter;
        }

        public T Parse(TextReader tr)
        {
            var ret = _converter.Create();

            using (var parser = new FormEncodedResponseParser(tr))
            {
                foreach (var kvp in parser.ReadAll())
                {
                    _converter.SetValue(ret, kvp.Item1, kvp.Item2);
                }
            }

            return ret;
        }

        public async Task<T> ParseAsync(TextReader tr)
        {
            var ret = _converter.Create();

            using (var parser = new FormEncodedResponseParser(tr))
            {
                Tuple<string, string> kvp;
                while ((kvp = await parser.ReadAsync()) != null)
                {
                    _converter.SetValue(ret, kvp.Item1, kvp.Item2);
                }
            }

            return ret;
        }
    }
}
