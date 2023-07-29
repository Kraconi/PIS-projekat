using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

namespace WPFSportskaRadnja.Forme
{
    /// <summary>
    /// Interaction logic for FrmGarancija.xaml
    /// </summary>
    public partial class FrmGarancija : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmGarancija()
        {
            InitializeComponent();
            txtGarancija.Focus();
            PopuniListe();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmGarancija(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtGarancija.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            PopuniListe();
            konekcija = kon.KreirajKonekciju();
        }
        private void PopuniListe() 
        {
            try
            {
                konekcija = kon.KreirajKonekciju();
                konekcija.Open();

                string proizvod = @"select idProizvoda from tblProizvod";
                DataTable dtProizvod = new DataTable();
                SqlDataAdapter daProizvod = new SqlDataAdapter(proizvod, konekcija);
                daProizvod.Fill(dtProizvod);
                cbProizvod.ItemsSource = dtProizvod.DefaultView;
                dtProizvod.Dispose();
                daProizvod.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuće liste nisu popunjene!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
            }
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@broj", SqlDbType.Int).Value = txtGarancija.Text;
                cmd.Parameters.Add("@datum", SqlDbType.Date).Value = dpDatum.SelectedDate;
                cmd.Parameters.Add("@trajanje", SqlDbType.NVarChar).Value = txtTrajanje.Text;
                cmd.Parameters.Add("@proizvod", SqlDbType.Int).Value = cbProizvod.SelectedValue;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblGarancija Set brojGarancije=@broj,datumIzdavanja=@datum,trajanjeGarancije=@trajanje,idProizvoda=@proizvod where idGarancije=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblGarancija(brojGarancije,datumIzdavanja,trajanjeGarancije,idProizvoda) values (@broj,@datum,@trajanje,@proizvod);";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Greska u konekciji sa bazom podataka!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Unos podataka nije validan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
