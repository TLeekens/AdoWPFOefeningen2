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
        public List<Plant> gewijzigdePlanten = new List<Plant>();
        public List<Plant> listBoxPLantenLijst = new List<Plant>();
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
                Int32 soortNr = 0;
                soortNr = Convert.ToInt32(comboBoxSoort.SelectedValue);
                listBoxPLantenLijst = plantManager.GetPlanten(soortNr);
                //foreach (var eenPlant in allePlanten)
                //{
                //    listBoxPlant.Items.Add(eenPlant);
                //}
                listBoxPlant.ItemsSource = listBoxPLantenLijst;
                listBoxPlant.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool WijzigingenOpslaan()
        {
            try
            {
                if (Validation.GetHasError(textBoxKleur) == true || Validation.GetHasError(textBoxVerkoopPrijs) == true)
                {
                    MessageBox.Show("Gelieve eerst de fouten op te lossen", "Opgelet", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                string vraag;
                if (comboBoxSoort.SelectedIndex == 0)
                    vraag = "Wilt u uw wijzigingen opslaan?";
                else
                    vraag = String.Format("Gewijzigde planten van soort '{0}' opslaan?", comboBoxSoort.SelectedItem.ToString());
                if (gewijzigdePlanten.Count > 0)
                {
                    if (MessageBox.Show(vraag, "Wijzigingen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        plantManager.SchrijfWijzigingen(gewijzigdePlanten);
                        gewijzigdePlanten.Clear();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void textBoxKleur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listBoxPlant.SelectedItem != null)
            {
                Plant plant = (Plant)listBoxPlant.SelectedItem;
                if (plant.Changed == true)
                    gewijzigdePlanten.Add(plant); 
            }
        }

        private void textBoxVerkoopPrijs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listBoxPlant.SelectedItem != null)
            {
                Plant plant = (Plant)listBoxPlant.SelectedItem;
                if (plant.Changed == true)
                    gewijzigdePlanten.Add(plant);
            }
        }

        private void comboBoxSoort_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!WijzigingenOpslaan())
            {
                e.Handled = true;
            }
        }

        private void listBoxPlant_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Validation.GetHasError(textBoxKleur) == true || Validation.GetHasError(textBoxVerkoopPrijs) == true)
            {
                MessageBox.Show("Gelieve eerst de fouten op te lossen", "Opgelet", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
        }

        private void buttonOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (gewijzigdePlanten.Count == 0)
                MessageBox.Show("Er zijn geen wijzigingen op te slaan");
            else if (WijzigingenOpslaan())
                MessageBox.Show("Wijzigingen succesvol opgeslagen");
            else
                MessageBox.Show("Geen wijzigingen opgeslagen");
        }
    }
}
