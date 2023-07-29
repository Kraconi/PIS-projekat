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
    /// Interaction logic for FrmKupac.xaml
    /// </summary>
    public partial class FrmKupac : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmKupac()
        {
            InitializeComponent();
            txtIme.Focus();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmKupac(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                cmd.Parameters.Add("@grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblKupac Set imeKupca=@ime,prezimeKupca=@prezime,adresaKupca=@adresa,gradKupca=@grad,
                    kontaktKupca=@kontakt where idKupca=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblKupac(imeKupca,prezimeKupca,adresaKupca,gradKupca,kontaktKupca) 
                    values(@ime,@prezime,@adresa,@grad,@kontakt);";
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
