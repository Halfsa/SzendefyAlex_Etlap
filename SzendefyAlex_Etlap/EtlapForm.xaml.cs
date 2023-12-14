using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SzendefyAlex_Etlap
{
	/// <summary>
	/// Interaction logic for EtlapForm.xaml
	/// </summary>
	public partial class EtlapForm : Window
	{
		EtlapService service;
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		public EtlapForm(EtlapService etlapService)
		{
			InitializeComponent();
			this.service = etlapService;
			kategoriaOptions();
			formKategoria.SelectedIndex = 0;

		}

		private void kategoriaOptions()
		{
			List<string> kategoriak = new List<string>();
			List<Etlap> list = service.GetAllASC();
			foreach (var kaja in list)
			{
				if (!kategoriak.Contains(kaja.kategoria))
				{
					kategoriak.Add(kaja.kategoria);
				}
			}
			formKategoria.ItemsSource = kategoriak;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string leiras = formLeiras.Text.Trim();
				Etlap etlap = createKaja();
				if (service.Create(etlap,leiras))
				{
					MessageBox.Show("Sikeres hozzáadás");
					formNev.Text = "";
					formKategoria.SelectedIndex = 0;
					formLeiras.Text = "";
					formAr.Text = "";
				}
				else
				{
					MessageBox.Show("Hiba történt a hozzáadás során");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private Etlap createKaja()
		{
			string name = formNev.Text.Trim();
			string kategoria = formKategoria.SelectedItem.ToString();
			string arText = formAr.Text.Trim();
			if (string.IsNullOrEmpty(name))
			{
				throw new Exception("Név megadása kötelező");
			}
			if (string.IsNullOrEmpty(arText))
			{
				throw new Exception("Ár megadása kötelező");
			}
			if (!int.TryParse(arText, out int ar))
			{
				throw new Exception("Az ár csak szám lehet");
			}
			if (string.IsNullOrEmpty(kategoria))
			{
				throw new Exception("Válasszon kategóriát");
			}
			Etlap etlap = new Etlap();
			etlap.nev = name;
			etlap.kategoria = kategoria;
			etlap.ar = ar;
			return etlap;
		}
	}
}
