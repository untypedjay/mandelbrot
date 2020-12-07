using System;
using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public static class SyncImageGenerator
    {
        public static Bitmap GenerateImage(Area area)
        {
            int maxIterations;
            double zBorder;
            double cReal, cImg, zReal, zImg, zNewReal, zNewImg;

            maxIterations = Settings.DefaultSettings.MaxIterations;
            zBorder = Settings.DefaultSettings.ZBorder * Settings.DefaultSettings.ZBorder;

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
                    while (((zReal * zReal + zImg * zImg) < zBorder) && (k < maxIterations)) // may be zImg + zImg
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
    }
}
