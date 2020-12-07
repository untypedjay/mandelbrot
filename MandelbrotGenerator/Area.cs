namespace MandelbrotGenerator {
  public class Area {
    public double MinReal { get; set; }
    public double MinImg { get; set; }
    public double MaxReal { get; set; }
    public double MaxImg { get; set; }
    private int width;
    public int Width { get { return width; } set { if (value > 0) width = value; } }
    private int height;
    public int Height { get { return height; } set { if (value > 0) height = value; } }
    public double PixelWidth { get { return (MaxReal - MinReal) / Width; } }
    public double PixelHeight { get { return (MaxImg - MinImg) / Height; } }

    public Area() {
      MinReal = -2;
      MinImg = -1;
      MaxReal = 1;
      MaxImg = 1;
      Width = 640;
      Height = 480;
    }

    public Area(double minReal, double minImg, double maxReal, double maxImg, int width, int height) {
      MinReal = minReal;
      MinImg = minImg;
      MaxReal = maxReal;
      MaxImg = maxImg;
      Width = width;
      Height = height;
    }
  }
}
