using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public static class ImageGenerator
    {
        public static Bitmap GenerateImageSync(Area area)
        {
            int maxIterations;
            double zBorder;
            double cReal, cImg, zReal, zImg, zNewReal, zNewImg;

            maxIterations = Settings.DefaultSettings.MaxIterations;
            zBorder = Settings.DefaultSettings.ZBorder * Settings.DefaultSettings.ZBorder; // so we don't have to take the square root in while loop

            Bitmap bitmap = new Bitmap(area.Width, area.Height);

            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    cReal = area.MinReal + i * area.PixelWidth;
                    cImg = area.MinImg + j * area.PixelHeight;
                    zReal = 0;
                    zImg = 0;

                    int k = 0;
                    while (((zReal * zReal + zImg * zImg) < zBorder) && (k < maxIterations))
                    {
                        zNewReal = zReal * zReal - zImg * zImg + cReal;
                        zNewImg = 2 * zReal * zImg + cImg;
                        zReal = zNewReal;
                        zImg = zNewImg;
                        k++;
                    }
                    bitmap.SetPixel(i, j, ColorSchema.GetColor(k));
                }
            }

            return bitmap;
        }

        private class ObjParams
        {
            public int x;
            public int y;
            public double cReal;
            public double cImg;
        }

        private static object lockObject = new object();
        private static Bitmap bitmap;

        public static Bitmap GenerateImageAsync(Area area, BackgroundWorker backgroundWorker)
        {
            bitmap = new Bitmap(area.Width, area.Height);

            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    int ii = i;
                    int jj = j;

                    var parameters = new ObjParams();

                    parameters.x = ii;
                    parameters.y = jj;
                    parameters.cReal = area.MinReal + ii * area.PixelWidth;
                    parameters.cImg = area.MinImg + jj * area.PixelHeight;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(calculatePixel), parameters);
                }

                if (backgroundWorker.CancellationPending)
                {
                    return null;
                }

            }

            return bitmap;
        }

        private static void calculatePixel(object threadContext)
        {
            ObjParams parameters = (ObjParams)threadContext;
            double zReal, zImg, zNewReal, zNewImg;

            int maxIterations;
            double zBorder;

            maxIterations = Settings.DefaultSettings.MaxIterations;
            zBorder = Settings.DefaultSettings.ZBorder * Settings.DefaultSettings.ZBorder;

            zReal = 0;
            zImg = 0;

            int k = 0;
            while (((zReal * zReal + zImg * zImg) < zBorder) && (k < maxIterations))
            {
                zNewReal = zReal * zReal - zImg * zImg + parameters.cReal;
                zNewImg = 2 * zReal * zImg + parameters.cImg;
                zReal = zNewReal;
                zImg = zNewImg;
                k++;
            }

            lock (lockObject)
            {
                bitmap.SetPixel(parameters.x, parameters.y, ColorSchema.GetColor(k));
            }
        }
    }
}
