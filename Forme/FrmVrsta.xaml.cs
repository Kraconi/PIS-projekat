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
    /// Interaction logic for FrmVrsta.xaml
    /// </summary>
    public partial class FrmVrsta : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmVrsta()
        {
            InitializeComponent();
            txtVrsta.Focus();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmVrsta(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtVrsta.Focus();
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
                cmd.Parameters.Add("@vrsta", SqlDbType.NVarChar).Value = txtVrsta.Text;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblVrsta Set nazivVrste=@vrsta where idVrste=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblVrsta(nazivVrste) values (@vrsta);";
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
