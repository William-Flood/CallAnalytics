using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Control;
using System.IO;
using Path = System.IO.Path;

namespace CallAnalytics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsManager _settingsManager;
        public MainWindow()
        {
            InitializeComponent();
            _settingsManager = new SettingsManager();
        }

        private void LoadFiles(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = _settingsManager.LastSearchedFolder
            };
            fileDialog.ShowDialog();
            _settingsManager.LastSearchedFolder = Path.GetDirectoryName(fileDialog.FileName);

        }
    }
}
