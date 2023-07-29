using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for FrmPorudžbina.xaml
    /// </summary>
    public partial class FrmPorudžbina : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmPorudžbina()
        {
            InitializeComponent();
            txtBroj.Focus();
            PopuniListe();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmPorudžbina(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtBroj.Focus();
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

                string korisnik = @"select idKorisnika,prezimeKorisnika from tblKorisnik";
                DataTable dtKorisink = new DataTable();
                SqlDataAdapter daKorisnik = new SqlDataAdapter(korisnik, konekcija);
                daKorisnik.Fill(dtKorisink);
                cbKorisnik.ItemsSource = dtKorisink.DefaultView;
                dtKorisink.Dispose();
                daKorisnik.Dispose();

                string kupac = @"select idKupca,prezimeKupca from tblKupac";
                DataTable dtKupac = new DataTable();
                SqlDataAdapter daKupac = new SqlDataAdapter(kupac, konekcija);
                daKupac.Fill(dtKupac);
                cbKupac.ItemsSource = dtKupac.DefaultView;
                dtKupac.Dispose();
                daKupac.Dispose();
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
                cmd.Parameters.Add("@broj", SqlDbType.Int).Value = txtBroj.Text;
                cmd.Parameters.Add("@cena", SqlDbType.Decimal).Value = txtCena.Text;
                cmd.Parameters.Add("@korisnik", SqlDbType.Int).Value = cbKorisnik.SelectedValue;
                cmd.Parameters.Add("@kupac", SqlDbType.Int).Value = cbKupac.SelectedValue;
                cmd.Parameters.Add("@datum", SqlDbType.Date).Value = dpDatum.SelectedDate;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblPorudžbina Set brojPorudžbine=@broj,cena=@cena,datum=@datum,idKorisnika=@korisnik,
                    idKupca=@kupac where idPorudžbine=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblPorudžbina(brojPorudžbine,cena,datum,idKorisnika,idKupca) 
                    values(@broj,@cena,@datum,@korisnik,@kupac);";
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
