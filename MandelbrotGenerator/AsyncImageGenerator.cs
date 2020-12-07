using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public class AsyncImageGenerator : IImageGenerator
    {
        public void GenerateImage(Area area)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(Run));
            thread.Start(area);
        }

        public event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
        private void OnImageGenerated(Area area, Bitmap bitmap, TimeSpan timespan)
        {
            ImageGenerated?.Invoke(this, new EventArgs<Tuple<Area, Bitmap, TimeSpan>>(new Tuple<Area, Bitmap, TimeSpan>(area, bitmap, timespan)));
        }

        private void Run(object a)
        {
            Area area = (Area)a;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var bitmap = SyncImageGenerator.GenerateImage(area);
            stopwatch.Stop();
            OnImageGenerated(area, bitmap, stopwatch.Elapsed);
        }
    }
}
