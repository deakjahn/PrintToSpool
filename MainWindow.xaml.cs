using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace PrintToSpool {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    public MainWindow(string PrinterName, string file) {
      InitializeComponent();

      try {
        var DocInfo = new DOCINFO {
          pDocName = "RAW",
          pDataType = "RAW"
        };

        bool success = false;
        if (OpenPrinter(PrinterName, out nint hPrinter, IntPtr.Zero)) {
          if (StartDocPrinter(hPrinter, 1, DocInfo)) {
            if (StartPagePrinter(hPrinter)) {
              ReadOnlySpan<byte> bytes = File.ReadAllBytes(file);
              ref readonly byte pBytes = ref MemoryMarshal.AsRef<byte>(bytes);
              success = WritePrinter(hPrinter, in pBytes, bytes.Length, out int _);
              EndPagePrinter(hPrinter);
            }
            EndDocPrinter(hPrinter);
          }
          ClosePrinter(hPrinter);
        }
        if (!success)
          Error(new Win32Exception(Marshal.GetLastWin32Error()).Message);
      }
      catch (Exception e) {
        Error(e.Message);
      }
      finally {
        Application.Current.Shutdown();
      }
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void Error(string message) {
      MessageBox.Show(message, Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private class DOCINFO {
      [MarshalAs(UnmanagedType.LPStr)] public string? pDocName;
      [MarshalAs(UnmanagedType.LPStr)] public string? pOutputFile;
      [MarshalAs(UnmanagedType.LPStr)] public string? pDataType;
    }

    [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Ansi)]
    [SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments")]
    private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out nint hPrinter, nint pd);

    [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Ansi)]
    private static extern bool StartDocPrinter(nint hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFO di);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndDocPrinter(nint hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool StartPagePrinter(nint hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndPagePrinter(nint hPrinter);

    [DllImport("winspool.Drv", SetLastError = true)]
    private static extern bool WritePrinter(nint hPrinter, in byte pBytes, int dwCount, out int dwWritten);

    [DllImport("winspool.Drv", SetLastError = true)]
    private static extern bool ClosePrinter(nint hPrinter);
  }
}
