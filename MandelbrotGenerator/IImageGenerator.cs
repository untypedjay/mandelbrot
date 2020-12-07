using System;
using System.Drawing;

namespace MandelbrotGenerator
{
    public interface IImageGenerator
    {
        void GenerateImage(Area area);

        event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
    }
}
