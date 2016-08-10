using System.Threading.Tasks;
using BluePayLibrary.Interfaces.BluePay20Post.Fluent;

namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public interface IBluePay20PostClient
    {
        BluePayPost20ResponseV3 Process(BluePayMessage msg);
        Task<BluePayPost20ResponseV3> ProcessAsync(BluePayMessage msg);
    }
}
