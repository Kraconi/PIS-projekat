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
    /// Interaction logic for FrmStavkaPorudžbine.xaml
    /// </summary>
    public partial class FrmStavkaPorudžbine : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmStavkaPorudžbine()
        {
            InitializeComponent();
            txtKolicina.Focus();
            PopuniListe();
            konekcija = kon.KreirajKonekciju();
        }
        public FrmStavkaPorudžbine(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtKolicina.Focus();
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

                string porudžbina = @"select idPorudžbine,brojPorudžbine from tblPorudžbina";
                DataTable dtPorudžbina = new DataTable();
                SqlDataAdapter daPorudžbina = new SqlDataAdapter(porudžbina, konekcija);
                daPorudžbina.Fill(dtPorudžbina);
                cbPorudžbine.ItemsSource = dtPorudžbina.DefaultView;
                dtPorudžbina.Dispose();
                daPorudžbina.Dispose();
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
                cmd.Parameters.Add("@kolicina", SqlDbType.Int).Value = txtKolicina.Text;
                cmd.Parameters.Add("@proizvod", SqlDbType.Int).Value = cbProizvod.SelectedValue;
                cmd.Parameters.Add("@porudzbina", SqlDbType.Int).Value = cbPorudžbine.SelectedValue;
                if (azuriraj)
                {
                    DataRowView red = pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblStavkaPorudžbine Set količina=@kolicina,idProizvoda=@proizvod,idPorudžbine=@porudzbina where idStavke=@id";
                    pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblStavkaPorudžbine(količina,idProizvoda,idPorudžbine) values (@kolicina,@proizvod,@porudzbina);";
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
