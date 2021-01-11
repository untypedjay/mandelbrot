using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace MandelbrotGenerator
{
    public class AsyncImageGeneratorBackgroundWorker : IImageGenerator
    {
        public AsyncImageGeneratorBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += Run;
            backgroundWorker.WorkerSupportsCancellation = true;

            resetEvent = new AutoResetEvent(false);
        }

        public void GenerateImage(Area area)
        {
            if (backgroundWorker.IsBusy == true)
            {
                backgroundWorker.CancelAsync();
                resetEvent.WaitOne();
            }
            else
            {
                backgroundWorker.RunWorkerAsync(argument: area);
            }
        }

        public event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
        private void OnImageGenerated(Area area, Bitmap bitmap, TimeSpan timespan)
        {
            ImageGenerated?.Invoke(this, new EventArgs<Tuple<Area, Bitmap, TimeSpan>>(new Tuple<Area, Bitmap, TimeSpan>(area, bitmap, timespan)));
        }

        private BackgroundWorker backgroundWorker;
        private AutoResetEvent resetEvent;

        private void Run(object sender, DoWorkEventArgs e)
        {
            Area area = (Area)e.Argument;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var bitmap = ImageGenerator.GenerateImageAsync(area, backgroundWorker);
            
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                resetEvent.Set();
            }
            else
            {
                stopwatch.Stop();
                OnImageGenerated(area, bitmap, stopwatch.Elapsed);
            }
        }
    }
}
