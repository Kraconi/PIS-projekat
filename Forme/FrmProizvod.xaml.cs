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
    /// Interaction logic for FrmProizvod.xaml
    /// </summary>
    public partial class FrmProizvod : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmProizvod()
        {
            InitializeComponent();
            txtVeličina.Focus();
            PopuniListe();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmProizvod(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtVeličina.Focus();
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

                string vrsta = @"select idVrste,nazivVrste from tblVrsta";
                DataTable dtVrsta = new DataTable();
                SqlDataAdapter daVrsta = new SqlDataAdapter(vrsta, konekcija);
                daVrsta.Fill(dtVrsta);
                cbVrsta.ItemsSource = dtVrsta.DefaultView;
                dtVrsta.Dispose();
                daVrsta.Dispose();

                string marka = @"select idMarke,nazivMarke from tblMarka";
                DataTable dtMarka = new DataTable();
                SqlDataAdapter daMarka = new SqlDataAdapter(marka, konekcija);
                daMarka.Fill(dtMarka);
                cbMarka.ItemsSource = dtMarka.DefaultView;
                dtMarka.Dispose();
                daMarka.Dispose();

                string kategorija = @"select idKategorije,nazivPola from tblKategorija";
                DataTable dtKategorija = new DataTable();
                SqlDataAdapter daKategorija = new SqlDataAdapter(kategorija, konekcija);
                daKategorija.Fill(dtKategorija);
                cbKategorija.ItemsSource = dtKategorija.DefaultView;
                dtKategorija.Dispose();
                daKategorija.Dispose();
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
                cmd.Parameters.Add("@velicina", SqlDbType.NVarChar).Value = txtVeličina.Text;
                cmd.Parameters.Add("@boja", SqlDbType.NVarChar).Value = txtBoja.Text;
                cmd.Parameters.Add("@namena", SqlDbType.NVarChar).Value = txtNamena.Text;
                cmd.Parameters.Add("@vrsta", SqlDbType.Int).Value = cbVrsta.SelectedValue;
                cmd.Parameters.Add("@kategorija", SqlDbType.Int).Value = cbKategorija.SelectedValue;
                cmd.Parameters.Add("@marka", SqlDbType.Int).Value = cbMarka.SelectedValue;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblProizvod Set veličina=@velicina,boja=@boja,namena=@namena,idVrste=@vrsta,
                    idKategorije=@kategorija,idMarke=@marka where idProizvoda=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblProizvod(veličina,boja,namena,idVrste,idKategorije,idMarke) 
                    values(@velicina,@boja,@namena,@vrsta,@kategorija,@marka);";
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
