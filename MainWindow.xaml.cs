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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFSportskaRadnja.Forme;

namespace WPFSportskaRadnja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        string ucitanaTabela;
        bool azuriraj;
        #region Select upiti
        static string selectVrste = @"Select idVrste as ID,nazivVrste as Vrsta from tblVrsta";
        static string selectMarke = @"Select idMarke as ID,nazivMarke as Marka from tblMarka";
        static string selectKategorije = @"Select idKategorije as ID,nazivPola as 'Pol'
from tblKategorija;";
        static string selectKorisnika = @"Select idKorisnika as ID,imeKorisnika + ' ' + prezimeKorisnika as 'Korisnik',adresaKorisnika + ' ' + gradKorisnika as 'Adresa',
        kontaktKorisnika as 'Kontakt' from tblKorisnik";
        static string selectKupca = @"Select idKupca as ID,imeKupca + ' ' + prezimeKupca as 'Kupac',adresaKupca + ' ' + gradKupca as 'Adresa',
        kontaktKupca as 'Kontakt' from tblKupac";
        static string selectStavke = @"Select idStavke as ID,količina as 'Količina',tblProizvod.idProizvoda as 'ID proizvoda',namena as 'Namena',brojPorudžbine as 'Broj porudžbine',datum as 'Datum',cena as 'Cena'
        from tblStavkaPorudžbine join tblProizvod on tblStavkaPorudžbine.idProizvoda=tblProizvod.idProizvoda
                                 join tblPorudžbina on tblStavkaPorudžbine.idPorudžbine=tblPorudžbina.idPorudžbine";
        static string selectProizvoda = @"Select idProizvoda as ID,veličina as 'Veličina',boja as 'Boja',namena as 'Namena',nazivVrste as 'Vrsta',nazivMarke as 'Marka',nazivPola as 'Pol'
        from tblProizvod join tblVrsta on tblProizvod.idVrste=tblVrsta.idVrste
                         join tblMarka on tblProizvod.idMarke=tblMarka.idMarke
                         join tblKategorija on tblProizvod.idKategorije=tblKategorija.idKategorije";
        static string selectPorudzbine = @"Select idPorudžbine as ID,brojPorudžbine as 'Broj',datum as 'Datum',cena as 'Cena',imeKorisnika + ' ' + prezimeKorisnika as 'Korisnik',imeKupca + ' ' + prezimeKupca as 'Kupac'
        from tblPorudžbina join tblKorisnik on tblPorudžbina.idKorisnika=tblKorisnik.idKorisnika
						   join tblKupac on tblPorudžbina.idKupca=tblKupac.idKupca";
        static string selectGarancije = @"Select idGarancije as ID,brojGarancije as 'Broj garancije',datumIzdavanja as 'Datum',trajanjeGarancije as 'Trajanje',
        veličina as 'Veličina proizvoda' from tblGarancija join tblProizvod on tblGarancija.idProizvoda=tblProizvod.idProizvoda ";
        #endregion
        #region Select sa uslovom
        string selectUslovVrste = @"select * from tblVrsta where idVrste=";
        string selectUslovMarke = @"select * from tblMarka where idMarke=";
        string selectUslovKategorije = @"select * from tblKategorija where idKategorije=";
        string selectUslovKorisnika = @"select * from tblKorisnik where idKorisnika=";
        string selectUslovStavke = @"select * from tblStavkaPorudžbine where idStavke=";
        string selectUslovProizvoda = @"select * from tblProizvod where idProizvoda=";
        string selectUslovPorudzbine = @"select * from tblPorudžbina where idPorudžbine=";
        string selectUslovGarancije = @"select * from tblGarancija where idGarancije=";
        string selectUslovKupca = @"select * from tblKupac where idKupca=";
        #endregion
        #region Delete
        string deleteVrste = @"delete from tblVrsta where idVrste=";
        string deleteMarke = @"delete from tblMarka where idMarke=";
        string deleteKategorije = @"delete from tblKategorija where idKategorije=";
        string deleteKorisnika = @"delete from tblKorisnik where idKorisnika=";
        string deleteStavke = @"delete from tblStavkaPorudžbine where idVStavke=";
        string deleteProizvoda = @"delete from tblProizvod where idProizvoda=";
        string deletePorudzbine = @"delete from tblPorudžbina where idPorudžbine=";
        string deleteGarancije = @"delete from tblGarancija where idGarancije=";
        string deleteKupca = @"delete from tblKupac where idKupca=";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(DataGridCentralni, selectProizvoda);
        }
        private void UcitajPodatke(DataGrid grid,string selectUpit) 
        {
            try 
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (grid != null) 
                {
                    grid.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                dataAdapter.Dispose();
            }
            catch
            {
                MessageBox.Show("Neuspesno ucitani podaci.", "Greska", MessageBoxButton.OK,MessageBoxImage.Error);

            }
            finally
            {
                konekcija.Close();
            }
        }

        private void btnGarancije_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni,selectGarancije);
        }

        private void btnKategorije_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectKategorije);

        }

        private void btnKorisnici_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectKorisnika);

        }

        private void btnKupci_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectKupca);

        }

        private void btnMarke_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectMarke);

        }

        private void btnPorudžbine_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectPorudzbine);

        }

        private void btnProizvodi_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectProizvoda);

        }

        private void btnStavkePorudžbine_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectStavke);

        }

        private void btnVrste_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DataGridCentralni, selectVrste);

        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela == selectGarancije)
            {
                prozor = new FrmGarancija();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni,selectGarancije);
            }
            else if (ucitanaTabela == selectKategorije)
            {
                prozor = new FrmKategorija();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectKategorije);
            }
            else if (ucitanaTabela == selectKorisnika)
            {
                prozor = new FrmKorisnik();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectKorisnika);
            }
            else if (ucitanaTabela == selectKupca)
            {
                prozor = new FrmKupac();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectKupca);
            }
            else if (ucitanaTabela == selectMarke)
            {
                prozor = new FrmMarka();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectMarke);
            }
            else if (ucitanaTabela == selectPorudzbine)
            {
                prozor = new FrmPorudžbina();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectPorudzbine);
            }
            else if (ucitanaTabela == selectProizvoda)
            {
                prozor = new FrmProizvod();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectProizvoda);
            }
            else if (ucitanaTabela == selectStavke)
            {
                prozor = new FrmStavkaPorudžbine();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectStavke);
            }
            else if (ucitanaTabela == selectVrste)
            {
                prozor = new FrmVrsta();
                prozor.ShowDialog();
                UcitajPodatke(DataGridCentralni, selectVrste);
            }

        }

        void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela == selectGarancije)
                    {
                        FrmGarancija prozorGarancija = new FrmGarancija(azuriraj, red);
                        prozorGarancija.txtGarancija.Text = citac["brojGarancije"].ToString();
                        prozorGarancija.txtTrajanje.Text = citac["trajanjeGarancije"].ToString();
                        prozorGarancija.dpDatum.SelectedDate = (DateTime)citac["datumIzdavanja"];
                        prozorGarancija.cbProizvod.SelectedValue = citac["idProizvoda"].ToString();
                        prozorGarancija.ShowDialog();
                    }
                    else if (ucitanaTabela == selectKategorije)
                    {
                        FrmKategorija prozorKategorija = new FrmKategorija(azuriraj, red);
                        prozorKategorija.txtKategorija.Text = citac["nazivPola"].ToString();
                        prozorKategorija.ShowDialog();
                    }
                    else if (ucitanaTabela == selectKorisnika)
                    {
                        FrmKorisnik prozorKorisnik = new FrmKorisnik(azuriraj,red);
                        prozorKorisnik.txtIme.Text = citac["imeKorisnika"].ToString();
                        prozorKorisnik.txtPrezime.Text = citac["prezimeKorisnika"].ToString();
                        prozorKorisnik.txtAdresa.Text = citac["adresaKorisnika"].ToString();
                        prozorKorisnik.txtGrad.Text = citac["gradKorisnika"].ToString();
                        prozorKorisnik.txtKontakt.Text = citac["kontaktKorisnika"].ToString();
                        prozorKorisnik.ShowDialog();
                    }
                    else if (ucitanaTabela == selectKupca)
                    {
                        FrmKupac prozorKupac = new FrmKupac(azuriraj, red);
                        prozorKupac.txtIme.Text = citac["imeKupca"].ToString();
                        prozorKupac.txtPrezime.Text = citac["prezimeKupca"].ToString();
                        prozorKupac.txtAdresa.Text = citac["adresaKupca"].ToString();
                        prozorKupac.txtGrad.Text = citac["gradKupca"].ToString();
                        prozorKupac.txtKontakt.Text = citac["kontaktKupca"].ToString();
                        prozorKupac.ShowDialog();
                    }
                    else if (ucitanaTabela == selectMarke)
                    {
                        FrmMarka prozorMarka = new FrmMarka(azuriraj, red);
                        prozorMarka.txtMarka.Text = citac["nazivMarke"].ToString();
                        prozorMarka.ShowDialog();
                    }
                    else if (ucitanaTabela == selectPorudzbine)
                    {
                        FrmPorudžbina prozorPorudzbina = new FrmPorudžbina(azuriraj, red);
                        prozorPorudzbina.txtBroj.Text = citac["brojPorudžbine"].ToString();
                        prozorPorudzbina.txtCena.Text = citac["cena"].ToString();
                        prozorPorudzbina.cbKorisnik.SelectedValue = citac["idKorisnika"].ToString();
                        prozorPorudzbina.cbKupac.SelectedValue = citac["idKupca"].ToString();
                        prozorPorudzbina.dpDatum.SelectedDate = (DateTime)citac["datum"];
                        prozorPorudzbina.ShowDialog();
                    }
                    else if (ucitanaTabela == selectProizvoda)
                    {
                        FrmProizvod prozorProizvod = new FrmProizvod(azuriraj, red);
                        prozorProizvod.txtBoja.Text = citac["boja"].ToString();
                        prozorProizvod.txtVeličina.Text = citac["veličina"].ToString();
                        prozorProizvod.txtNamena.Text = citac["namena"].ToString();
                        prozorProizvod.cbKategorija.SelectedValue = citac["idKategorije"].ToString();
                        prozorProizvod.cbVrsta.SelectedValue = citac["idVrste"].ToString();
                        prozorProizvod.cbMarka.SelectedValue = citac["idMarke"].ToString();
                        prozorProizvod.ShowDialog();
                    }
                    else if (ucitanaTabela == selectStavke)
                    {
                        FrmStavkaPorudžbine prozorStavka = new FrmStavkaPorudžbine(azuriraj, red);
                        prozorStavka.txtKolicina.Text = citac["količina"].ToString();
                        prozorStavka.cbPorudžbine.SelectedValue = citac["idPorudžbine"].ToString();
                        prozorStavka.cbProizvod.SelectedValue = citac["idProizvoda"].ToString();
                        prozorStavka.ShowDialog();
                    }
                    else if (ucitanaTabela == selectVrste)
                    {
                        FrmVrsta prozorVrsta = new FrmVrsta(azuriraj, red);
                        prozorVrsta.txtVrsta.Text = citac["nazivVrste"].ToString();
                        prozorVrsta.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste izabrali red!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                konekcija.Close(); 
            }

        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela == selectGarancije)
            {
                PopuniFormu(DataGridCentralni, selectUslovGarancije);
                UcitajPodatke(DataGridCentralni, selectGarancije);
            }
            else if (ucitanaTabela == selectKategorije)
            {
                PopuniFormu(DataGridCentralni, selectUslovKategorije);
                UcitajPodatke(DataGridCentralni, selectKategorije);
            }
            else if (ucitanaTabela == selectKorisnika)
            {
                PopuniFormu(DataGridCentralni, selectUslovKorisnika);
                UcitajPodatke(DataGridCentralni, selectKorisnika);
            }
            else if (ucitanaTabela == selectKupca)
            {
                PopuniFormu(DataGridCentralni, selectUslovKupca);
                UcitajPodatke(DataGridCentralni, selectKupca);
            }
            else if (ucitanaTabela == selectMarke)
            {
                PopuniFormu(DataGridCentralni, selectUslovMarke);
                UcitajPodatke(DataGridCentralni, selectMarke);
            }
            else if (ucitanaTabela == selectPorudzbine)
            {
                PopuniFormu(DataGridCentralni, selectUslovPorudzbine);
                UcitajPodatke(DataGridCentralni, selectPorudzbine);
            }
            else if (ucitanaTabela == selectProizvoda)
            {
                PopuniFormu(DataGridCentralni, selectUslovProizvoda);
                UcitajPodatke(DataGridCentralni, selectProizvoda);
            }
            else if (ucitanaTabela == selectStavke)
            {
                PopuniFormu(DataGridCentralni, selectUslovStavke);
                UcitajPodatke(DataGridCentralni, selectStavke);
            }
            else if (ucitanaTabela == selectVrste)
            {
                PopuniFormu(DataGridCentralni, selectUslovVrste);
                UcitajPodatke(DataGridCentralni, selectVrste);
            }
        }
        void ObrisiZapis(DataGrid grid,string deleteUpit) 
        {
            try 
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                MessageBoxResult rez = MessageBox.Show("Da li ste sigurni da želite da obrišete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rez == MessageBoxResult.Yes) 
                {
                    SqlCommand cmd = new SqlCommand { Connection = konekcija };
                    cmd.Parameters.Add(@"id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste izabrali red!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Podaci iz tabele su povezani sa drugim tabelama!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { konekcija.Close(); }

        }

        private void btnBrisanje_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela == selectGarancije)
            {
                ObrisiZapis(DataGridCentralni, deleteGarancije);
                UcitajPodatke(DataGridCentralni, selectGarancije);
            }
            else if (ucitanaTabela == selectKategorije)
            {
                ObrisiZapis(DataGridCentralni, deleteKategorije);
                UcitajPodatke(DataGridCentralni, selectKategorije);
            }
            else if (ucitanaTabela == selectKorisnika)
            {
                ObrisiZapis(DataGridCentralni, deleteKorisnika);
                UcitajPodatke(DataGridCentralni, selectKorisnika);
            }
            else if (ucitanaTabela == selectKupca)
            {
                ObrisiZapis(DataGridCentralni, deleteKupca);
                UcitajPodatke(DataGridCentralni, selectKupca);
            }
            else if (ucitanaTabela == selectMarke)
            {
                ObrisiZapis(DataGridCentralni, deleteMarke);
                UcitajPodatke(DataGridCentralni, selectMarke);
            }
            else if (ucitanaTabela == selectPorudzbine)
            {
                ObrisiZapis(DataGridCentralni, deletePorudzbine);
                UcitajPodatke(DataGridCentralni, selectPorudzbine);
            }
            else if (ucitanaTabela == selectProizvoda)
            {
                ObrisiZapis(DataGridCentralni, deleteProizvoda);
                UcitajPodatke(DataGridCentralni, selectProizvoda);
            }
            else if (ucitanaTabela == selectStavke)
            {
                ObrisiZapis(DataGridCentralni, deleteStavke);
                UcitajPodatke(DataGridCentralni, selectStavke);
            }
            else if (ucitanaTabela == selectVrste)
            {
                ObrisiZapis(DataGridCentralni, deleteVrste);
                UcitajPodatke(DataGridCentralni, selectVrste);
            }
        }
    }
}
