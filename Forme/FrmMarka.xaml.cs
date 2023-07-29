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
    /// Interaction logic for FrmMarka.xaml
    /// </summary>
    public partial class FrmMarka : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmMarka()
        {
            InitializeComponent();
            txtMarka.Focus();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmMarka(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtMarka.Focus();
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
                cmd.Parameters.Add("@marka", SqlDbType.NVarChar).Value = txtMarka.Text;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblMarka Set nazivMarke=@marka where idMarke=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblMarka(nazivMarke) values (@marka);";
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
