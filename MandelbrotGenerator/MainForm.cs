using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MandelbrotGenerator
{
    public partial class MainForm : Form
    {
        private Area currentArea;
        private Point mouseDownPoint;
        private IImageGenerator generator;

        public MainForm()
        {
            InitializeComponent();

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(pictureBox.Image);
            graphics.FillRectangle(Brushes.Azure, 0, 0, pictureBox.Width, pictureBox.Height);
            graphics.Dispose();

            string path = Application.StartupPath;

            currentArea = new Area();
            currentArea.MinReal = Settings.DefaultSettings.MinReal;
            currentArea.MinImg = Settings.DefaultSettings.MinImg;
            currentArea.MaxReal = Settings.DefaultSettings.MaxReal;
            currentArea.MaxImg = Settings.DefaultSettings.MaxImg;
            currentArea.Width = pictureBox.Width;
            currentArea.Height = pictureBox.Height;

            generator = new AsyncImageGeneratorBackgroundWorker();
            generator.ImageGenerated += Generator_ImageGenerated;
        }

        private void Generator_ImageGenerated(object sender, EventArgs<Tuple<Area, Bitmap, TimeSpan>> e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs<Tuple<Area, Bitmap, TimeSpan>>>((s, args) => Generator_ImageGenerated(s, args)), sender, e);
            } else
            {
                currentArea = e.Value.Item1;
                pictureBox.Image = e.Value.Item2;
                toolStripStatusLabel.Text = "Done (Runtime: " + e.Value.Item3.ToString() + ")";
            }
        }

        private void UpdateImage(Area area)
        {
            toolStripStatusLabel.Text = "Calculating ...";
            generator.GenerateImage(area);
        }

        #region Menu events
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;

                ImageFormat format = null;
                if (filename.EndsWith("jpb")) format = ImageFormat.Jpeg;
                else if (filename.EndsWith("gif")) format = ImageFormat.Gif;
                else if (filename.EndsWith("png")) format = ImageFormat.Png;
                else format = ImageFormat.Bmp;

                pictureBox.Image.Save(filename, format);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                dialog.ShowDialog();
            }
        }
        #endregion

        #region Mouse events
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = e.Location;
            }
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = Math.Min(e.X, mouseDownPoint.X);
                int y = Math.Min(e.Y, mouseDownPoint.Y);
                int width = Math.Abs(e.X - mouseDownPoint.X);
                int height = Math.Abs(e.Y - mouseDownPoint.Y);

                pictureBox.Refresh();
                Graphics graphics = pictureBox.CreateGraphics();
                graphics.DrawRectangle(Pens.Yellow, x, y, width, height);
                graphics.Dispose();
            }
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Area area = new Area();
            area.Width = pictureBox.Width;
            area.Height = pictureBox.Height;

            if (e.Button == MouseButtons.Left)
            {
                area.MinReal = currentArea.MinReal + currentArea.PixelWidth * Math.Min(e.X, mouseDownPoint.X);
                area.MinImg = currentArea.MinImg + currentArea.PixelHeight * Math.Min(e.Y, mouseDownPoint.Y);
                area.MaxReal = currentArea.MinReal + currentArea.PixelWidth * Math.Max(e.X, mouseDownPoint.X);
                area.MaxImg = currentArea.MinImg + currentArea.PixelHeight * Math.Max(e.Y, mouseDownPoint.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                area.MinReal = Settings.DefaultSettings.MinReal;
                area.MinImg = Settings.DefaultSettings.MinImg;
                area.MaxReal = Settings.DefaultSettings.MaxReal;
                area.MaxImg = Settings.DefaultSettings.MaxImg;
            }

            UpdateImage(area);
        }
        #endregion
    }
}
