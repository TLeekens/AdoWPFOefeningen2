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
using AdoConnections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace AdoWPFOefeningen2
{
    /// <summary>
    /// Interaction logic for LeverancierWindow.xaml
    /// </summary>
    public partial class LeverancierWindow : Window
    {
        CollectionViewSource leverancierViewSource;
        LeverancierManager manager = new LeverancierManager();
        ObservableCollection<Leverancier> leveranciers = new ObservableCollection<Leverancier>();
        List<Leverancier> oudeLeveranciers = new List<Leverancier>();
        public LeverancierWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                leverancierViewSource = ((CollectionViewSource)(this.FindResource("leverancierViewSource")));
                leveranciers = manager.GetLeveranciersVolgensNaam("<alles>");
                leverancierViewSource.Source = leveranciers;

                leveranciers.CollectionChanged += this.OnCollectionChanged;

                comboBoxPostNr.ItemsSource = manager.GetPostNrsMetAllesVoorop();
                comboBoxPostNr.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxPostNr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            leverancierViewSource.Source = manager.GetLeveranciersVolgensNaam((String)comboBoxPostNr.SelectedValue);
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Leverancier lev in e.OldItems)
                {
                    oudeLeveranciers.Add(lev);
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (oudeLeveranciers.Count != 0)
            {
                manager.SchrijfVerwijderingen(oudeLeveranciers);
            }
            oudeLeveranciers.Clear();

            MessageBox.Show("Alle wijzigingen zijn opgeslagen");
        }
    }
}
