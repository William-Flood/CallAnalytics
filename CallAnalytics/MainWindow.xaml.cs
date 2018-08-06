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
using DataObjects;

namespace CallAnalytics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsManager _settingsManager;
        List<AnalyticsResult> analysisResults;
        public MainWindow()
        {
            InitializeComponent();
            _settingsManager = new SettingsManager();
        }

        private void LoadFiles(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = _settingsManager.LastSearchedFolder,
                Multiselect = true
            };
            fileDialog.ShowDialog();
            if(0 == fileDialog.FileNames.Length)
            {
                return;
            }
            _settingsManager.LastSearchedFolder = Path.GetDirectoryName(fileDialog.FileName);
            analysisResults = RecordParser.Parse(fileDialog.FileNames);
            dgrdAnalyticResults.DataContext = analysisResults;
            dgrdAnalyticResults.Items.Refresh();
        }

        private void AssignGroup(object sender, RoutedEventArgs e)
        {
            if(0 == dgrdAnalyticResults.SelectedItems.Count)
            {
                MessageBox.Show("Please select one or more items first");
                return;
            }
            var groupNameDialog = new AddGroup();
            groupNameDialog.ShowDialog();
            if(null == groupNameDialog.GroupName || "".Equals(groupNameDialog.GroupName))
            {
                return;
            }
            var combinedGroupData = new AnalyticsResult() { Name = groupNameDialog.GroupName, ObservationCount = 0, AverageResponseTime = 0 };
            var itemsToRemove = new List<AnalyticsResult>();
            foreach(var rowSelected in dgrdAnalyticResults.SelectedItems)
            {
                var gridIndex = 0;
                foreach(var dataGridRow in dgrdAnalyticResults.Items)
                {
                    if(rowSelected.Equals(dataGridRow))
                    {
                        break;
                    }
                    gridIndex++;
                }
                var itemInGroup = analysisResults[gridIndex];
                combinedGroupData.AverageResponseTime = 
                    (combinedGroupData.AverageResponseTime * combinedGroupData.ObservationCount + itemInGroup.AverageResponseTime * itemInGroup.ObservationCount) / 
                    (combinedGroupData.ObservationCount + itemInGroup.ObservationCount);
                combinedGroupData.ObservationCount += itemInGroup.ObservationCount;
                itemsToRemove.Add(itemInGroup);
            }
            foreach(var itemToRemove in itemsToRemove)
            {
                analysisResults.Remove(itemToRemove);
            }
            analysisResults.Add(combinedGroupData);
            dgrdAnalyticResults.DataContext = analysisResults;
            dgrdAnalyticResults.Items.Refresh();
        }
    }
}
