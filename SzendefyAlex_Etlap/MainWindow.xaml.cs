using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SzendefyAlex_Etlap
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		EtlapService service = new EtlapService();
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		public MainWindow()
		{
			InitializeComponent();
			etlap.ItemsSource = service.GetAllASC();
			

		}

		private void radioASC_Checked(object sender, RoutedEventArgs e)
		{
			etlap.ItemsSource = service.GetAllASC();
		}

		private void radioDESC_Checked(object sender, RoutedEventArgs e)
		{
			etlap.ItemsSource = service.GetAllDESC();
		}

		private void etlap_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Etlap selected = etlap.SelectedItem as Etlap;
			if (selected != null)
			{
				lblLeiras.Content = service.GetDescription(selected.id);
			}
		}

		private void btnUjEtel_Click(object sender, RoutedEventArgs e)
		{
			EtlapForm etlapform = new EtlapForm(service);
			etlapform.Closed += (_, _) =>
			{
					etlap.ItemsSource = service.GetAllASC();
            };
			etlapform.ShowDialog();
		}

		private void btnTorles_Click(object sender, RoutedEventArgs e)
		{
			Etlap selected = etlap.SelectedItem as Etlap;
			if (selected == null)
			{
				MessageBox.Show("Nincs kijelölve elem");
				return;
			}
			MessageBoxResult selectedButton = MessageBox.Show($"Biztos, hogy törölni szeretné az alábbi tételt az étlapról: {selected.nev}?", "Törlés", MessageBoxButton.YesNo);
			if (selectedButton == MessageBoxResult.Yes)
			{
				if (service.Delete(selected.id))
				{
					MessageBox.Show("Sikeres törlés!");
				}
				else
				{
					MessageBox.Show("Hiba történt a törlés során");
				}
				etlap.ItemsSource = service.GetAllASC();
			}
		}

		private void btnPercent_Click(object sender, RoutedEventArgs e)
		{
			Etlap selected = (Etlap) etlap.SelectedItem;
			string szazalekText = tbPercent.Text.Trim();
			if (szazalekText == "")
			{
				MessageBox.Show("Írjon be egy valós értéket!");
				return;
			}
			int szazalek = int.Parse(szazalekText);
			if (szazalek < 5 || szazalek >50)
			{
				MessageBox.Show("Írjon be egy valós értéket! (5 és 50% közötti valós szám)");
				return;
			}
			if (selected == null)
            {
                if (service.ModositMindSzazalek(szazalek))
                {
					MessageBox.Show("Sikeres módosítás");
                }
                else
                {
					MessageBox.Show("Sikertelen módosítás");
                }
                etlap.ItemsSource = service.GetAllASC();
				tbPercent.Text = "";
			}
            else
            {
				if (service.ModositEgySzazalek(szazalek,selected.id))
				{
					MessageBox.Show("Sikeres módosítás");
				}
				else
				{
					MessageBox.Show("Sikertelen módosítás");
				}
				etlap.ItemsSource = service.GetAllASC();
				tbPercent.Text = "";
			}
        }

		private void btnForint_Click(object sender, RoutedEventArgs e)
		{
			Etlap selected = (Etlap)etlap.SelectedItem;
			string noveloText = tbForint.Text.Trim();
			if (noveloText == "")
			{
				MessageBox.Show("Írjon be egy értéket!");
				return;
			}
			int novelo = int.Parse(noveloText);
			if (novelo < 50 && novelo>3000)
			{
				MessageBox.Show("Írjon be egy valós értéket! (50 és 3000 közötti egész szám)");
				return;
			}
			if (selected == null)
			{
				if (service.ModositMindForint(novelo))
				{
					MessageBox.Show("Sikeres módosítás");
				}
				else
				{
					MessageBox.Show("Sikertelen módosítás");
				}
				etlap.ItemsSource = service.GetAllASC();
				tbForint.Text = "";
			}
			else
			{
				if (service.ModositEgyForint(novelo, selected.id))
				{
					MessageBox.Show("Sikeres módosítás");
				}
				else
				{
					MessageBox.Show("Sikertelen módosítás");
				}
				etlap.ItemsSource = service.GetAllASC();
				tbForint.Text = "";
			}
		}
	}
}
