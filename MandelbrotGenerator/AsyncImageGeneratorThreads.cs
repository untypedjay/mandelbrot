using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public class AsyncImageGeneratorThreads : IImageGenerator
    {
        public void GenerateImage(Area area)
        {
            if (isRunning)
            {
                thread.Abort();
            }

            isRunning = true;
            thread = new Thread(new ParameterizedThreadStart(Run));
            thread.Start(area);
        }

        public event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
        private void OnImageGenerated(Area area, Bitmap bitmap, TimeSpan timespan)
        {
            ImageGenerated?.Invoke(this, new EventArgs<Tuple<Area, Bitmap, TimeSpan>>(new Tuple<Area, Bitmap, TimeSpan>(area, bitmap, timespan)));
        }

        private bool isRunning = false;
        private Thread thread;

        private void Run(object a)
        {
            Area area = (Area)a;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var bitmap = ImageGenerator.GenerateImageSync(area);
            stopwatch.Stop();
            OnImageGenerated(area, bitmap, stopwatch.Elapsed);
            isRunning = false;
        }
    }
}
