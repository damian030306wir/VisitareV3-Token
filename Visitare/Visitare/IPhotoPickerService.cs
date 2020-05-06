using System.IO;
using System.Threading.Tasks;

namespace Visitare
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
