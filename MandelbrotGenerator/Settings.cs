using System.ComponentModel;

namespace MandelbrotGenerator {
  [DefaultPropertyAttribute("MinReal")]
  public class Settings {
    public static Settings defaultSettings = new Settings();
    public static Settings DefaultSettings {
      get { return defaultSettings; }
    }

    #region Initial Area
    [CategoryAttribute("Initial Area"),
     DescriptionAttribute("Minimum real value of the area")]
    public double MinReal { get; set; }
    [CategoryAttribute("Initial Area"),
     DescriptionAttribute("Minimum imaginary value of the area")]
    public double MinImg { get; set; }
    [CategoryAttribute("Initial Area"),
     DescriptionAttribute("Maximum real value of the area")]
    public double MaxReal { get; set; }
    [CategoryAttribute("Initial Area"),
     DescriptionAttribute("Maximum imaginary value of the area")]
    public double MaxImg { get; set; }
    #endregion

    #region Generator Settings
    private int maxIterations;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Maximum number of iterations")]
    public int MaxIterations {
      get { return maxIterations; }
      set { if (value > 0) maxIterations = value; }
    }
    private double zBorder;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Border value for z")]
    public double ZBorder {
      get { return zBorder; }
      set { if (value > 0) zBorder = value; }
    }
    #endregion

    #region Parallelization Settings
    private int workers;
    [CategoryAttribute("Parallelization Settings"),
     DescriptionAttribute("Number of worker threads")]
    public int Workers {
      get { return workers; }
      set { if (value > 0) workers = value; }
    }
    #endregion

    public Settings() {
      MinReal = -2; MinImg = -1;
      MaxReal = 1; MaxImg = 1;
      maxIterations = 10000;
      zBorder = 4.0;
      workers = 1;
    }
  }
}
