using System.IO;
using System.Threading.Tasks;

namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public interface IBluePayResponseParser<T>
    {
        T Parse(TextReader tr);
        Task<T> ParseAsync(TextReader tr);
    }
}
