using System.Reflection;
using System.Windows;

namespace PrintToSpool {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private void Application_Startup(object sender, StartupEventArgs e) {
      if (e.Args.Length == 2) {
        var window = new MainWindow(e.Args[0], e.Args[1]);
        window.Show();
      }
      else
        MessageBox.Show("Syntax: \"printername\"", Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
}
