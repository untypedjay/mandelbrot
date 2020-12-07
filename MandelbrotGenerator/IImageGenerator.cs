using System.Drawing;

namespace MandelbrotGenerator
{
    public interface IImageGenerator
    {
        Bitmap GenerateImage(Area area);
    }
}
