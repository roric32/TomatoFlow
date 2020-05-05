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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TomatoFlow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WorkTime = Int32.Parse(cboWorkTime.Text);
            Properties.Settings.Default.BreakTime = Int32.Parse(cboBreakTime.Text);
            Properties.Settings.Default.PartyTime = Int32.Parse(cboPartyTime.Text);
            Properties.Settings.Default.Cycles = Int32.Parse(cboCycles.Text);
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings Saved!", "Success!");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WorkTime = 25;
            Properties.Settings.Default.BreakTime = 5;
            Properties.Settings.Default.PartyTime = 30;
            Properties.Settings.Default.Cycles = 4;
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings Reset!", "Success!");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public OptionsWindow()
        {
            InitializeComponent();
            for(int x=1; x<61; x++)
            {
                cboWorkTime.Items.Add(new ComboBoxItem().Content = x);
                cboBreakTime.Items.Add(new ComboBoxItem().Content = x);
                cboPartyTime.Items.Add(new ComboBoxItem().Content = x);
                if(x < 11)
                {
                    cboCycles.Items.Add(new ComboBoxItem().Content = x);
                }
            }

            cboWorkTime.SelectedIndex = Properties.Settings.Default.WorkTime - 1;
            cboBreakTime.SelectedIndex = Properties.Settings.Default.BreakTime - 1;
            cboPartyTime.SelectedIndex = Properties.Settings.Default.PartyTime - 1;
            cboCycles.SelectedIndex = Properties.Settings.Default.Cycles - 1;

        }

    }
}
