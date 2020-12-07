using System.Windows.Forms;

namespace MandelbrotGenerator {
  public partial class SettingsDialog : Form {
    public SettingsDialog() {
      InitializeComponent();

      propertyGrid.SelectedObject = Settings.DefaultSettings;
    }
  }
}