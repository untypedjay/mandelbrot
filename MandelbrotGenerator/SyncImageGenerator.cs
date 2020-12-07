using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public class SyncImageGenerator : IImageGenerator
    {
        public Bitmap GenerateImage(Area area)
        {
            int maxIterations;
            double zBorder;
            double cReal, cImg, zReal, zImg, zNewReal, zNewImg;

            maxIterations = Settings.DefaultSettings.MaxIterations;
            zBorder = Settings.DefaultSettings.ZBorder * Settings.DefaultSettings.ZBorder;

            Bitmap bitmap = new Bitmap(area.Width, area.Height);

            //insert code

            Thread.Sleep(1000);

            //end insert

            return bitmap;
        }
    }
}
