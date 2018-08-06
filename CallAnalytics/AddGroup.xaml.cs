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
using System.Windows.Shapes;

namespace CallAnalytics
{
    /// <summary>
    /// Interaction logic for AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window
    {
        public String GroupName { get; set; }
        public AddGroup()
        {
            InitializeComponent();
        }

        private void AddGroupClick(object sender, RoutedEventArgs e)
        {
            GroupName = txtGroupName.Text;
            this.Close();
        }
    }
}
