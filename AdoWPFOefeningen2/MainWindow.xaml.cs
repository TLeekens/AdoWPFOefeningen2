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
using AdoConnections;

namespace AdoWPFOefeningen2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoortManager soortManager = new SoortManager();
        PlantManager plantManager = new PlantManager();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                listBoxPlant.DisplayMemberPath = "Naam";

                comboBoxSoort.DisplayMemberPath = "SoortNaam";
                comboBoxSoort.SelectedValuePath = "SoortNr";
                comboBoxSoort.ItemsSource = soortManager.GetSoortenMetOptieAllemaalVoorop();
                comboBoxSoort.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxSoort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                listBoxPlant.Items.Clear();
                int soortNr = Convert.ToInt32(comboBoxSoort.SelectedValue);
                var allePlanten = plantManager.GetPlanten(soortNr);
                foreach (var eenPlant in allePlanten)
                {
                    listBoxPlant.Items.Add(eenPlant);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
