using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BluePayLibrary.Interfaces
{
    public sealed class FormEncodedResponseParser : IDisposable
    {
        private readonly TextReader _tr;
        private int _bufferPosition = 0;
        private int _bufferLength = 0;
        private readonly char[] _buffer = new char[4096];

        public FormEncodedResponseParser(TextReader tr)
        {
            _tr = tr;
        }
        
        public IEnumerable<Tuple<string, string>> ReadAll()
        {
            Tuple<string, string> ret;
            while ((ret = Read()) != null)
            {
                yield return ret;
            }
        }
        
        public Tuple<string, string> Read()
        {
            if (_bufferPosition == _bufferLength && _tr.Peek() == -1)
            {
                return null;
            }

            var key = ReadToken('=');
            var value = ReadToken('&');
            return new Tuple<string, string>(key, value);
        }

        public async Task<Tuple<string, string>> ReadAsync()
        {
            if (_bufferPosition == _bufferLength && _tr.Peek() == -1)
            {
                return null;
            }

            var key = await ReadTokenAsync('=');
            var value = await ReadTokenAsync('&');
            return new Tuple<string, string>(key, value);
        }

        private int HexDigit(char c)
        {
            if (c < '0')
                return -1; //invalid

            if (c <= '9')
                return (int)(c - '0');

            if (c < 'A')
                return -1;

            if (c <= 'F')
                return (int)(c - 'A') + 10;

            if (c < 'f')
                return -1;

            if (c <= 'f')
                return (int)(c - 'a') + 10;

            return -1;
        }
        
        private string ReadToken(char delimeter)
        {
            var token = new StringBuilder();
            
            var start = _bufferPosition;
            var end = start;

            var bytesRead = _bufferLength - _bufferPosition;
            do
            {
                if (_bufferPosition == _bufferLength)
                {
                    if (end - start > 0)
                    {
                        token.Append(_buffer, start, end - start);
                    }

                    _bufferPosition = 0;
                    _bufferLength = 0;
                    start = 0;
                    end = 0;

                    bytesRead = _tr.Read(_buffer, 0, _buffer.Length);
                    _bufferLength = bytesRead;
                    if (bytesRead == 0)
                        break;
                }

                var c = _buffer[_bufferPosition];
                _bufferPosition++;
                if (c == delimeter)
                    break;
                else if (c == '%')
                {
                    if (end - start > 0)
                    {
                        token.Append(_buffer, start, end - start);
                    }

                    int left = 0;
                    int right = 0;

                    if (_bufferPosition == _bufferLength)
                    {
                        _bufferPosition = 0;
                        _bufferLength = 0;
                        start = 0;
                        end = 0;

                        bytesRead = _tr.ReadBlock(_buffer, 0, _buffer.Length);
                        _bufferLength = bytesRead;
                        if (bytesRead == 0)
                            break;

                        left = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }
                    else if (_bufferPosition + 1 == _bufferLength)
                    {
                        //read up to two more characters
                        left = HexDigit(_buffer[_bufferPosition]);

                        _bufferPosition = 0;
                        _bufferLength = 0;
                        start = 0;
                        end = 0;

                        bytesRead = _tr.ReadBlock(_buffer, 0, _buffer.Length);
                        _bufferLength = bytesRead;
                        if (bytesRead == 0)
                            break;

                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }
                    else
                    {
                        left = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }

                    token.Append((char) (left << 4 | right));

                    start = _bufferPosition;
                    end = start;
                }
                else
                {
                    end++;
                }

            } while (bytesRead > 0);

            if (end - start > 0)
            {
                token.Append(_buffer, start, end - start);
            }
            
            return token.ToString();
        }

        private async Task<string> ReadTokenAsync(char delimeter)
        {
            var token = new StringBuilder();

            var start = _bufferPosition;
            var end = start;

            var bytesRead = _bufferLength - _bufferPosition;
            do
            {
                if (_bufferPosition == _bufferLength)
                {
                    if (end - start > 0)
                    {
                        token.Append(_buffer, start, end - start);
                    }

                    _bufferPosition = 0;
                    _bufferLength = 0;
                    start = 0;
                    end = 0;

                    bytesRead = await _tr.ReadAsync(_buffer, 0, _buffer.Length);
                    _bufferLength = bytesRead;
                    if (bytesRead == 0)
                        break;
                }

                var c = _buffer[_bufferPosition];
                _bufferPosition++;
                if (c == delimeter)
                    break;
                else if (c == '%')
                {
                    if (end - start > 0)
                    {
                        token.Append(_buffer, start, end - start);
                    }

                    int left = 0;
                    int right = 0;

                    if (_bufferPosition == _bufferLength)
                    {
                        _bufferPosition = 0;
                        _bufferLength = 0;
                        start = 0;
                        end = 0;

                        bytesRead = await _tr.ReadBlockAsync(_buffer, 0, _buffer.Length);
                        _bufferLength = bytesRead;
                        if (bytesRead == 0)
                            break;

                        left = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }
                    else if (_bufferPosition + 1 == _bufferLength)
                    {
                        //read up to two more characters
                        left = HexDigit(_buffer[_bufferPosition]);

                        _bufferPosition = 0;
                        _bufferLength = 0;
                        start = 0;
                        end = 0;

                        bytesRead = await _tr.ReadBlockAsync(_buffer, 0, _buffer.Length);
                        _bufferLength = bytesRead;
                        if (bytesRead == 0)
                            break;

                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }
                    else
                    {
                        left = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                        right = HexDigit(_buffer[_bufferPosition]);
                        _bufferPosition++;
                    }

                    token.Append((char)(left << 4 | right));

                    start = _bufferPosition;
                    end = start;
                }
                else
                {
                    end++;
                }

            } while (bytesRead > 0);

            if (end - start > 0)
            {
                token.Append(_buffer, start, end - start);
            }

            return token.ToString();
        }

        /**     .
         *      +        
         * abcd%20efghi
         * 
         * **/

        public void Dispose()
        {
            _tr.Dispose();
        }
    }
}