using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace AracKiralama
{
    public partial class Satis : DevExpress.XtraEditors.XtraForm
    {
        public Satis()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }

        private void Satis_Load(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            String komutCumlesi = "Select* From Satislar";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            if (GonderenForm == "Anasayfa")
            {
                Anasayfa anasayfa = new Anasayfa();
                anasayfa.Show();
                this.Hide();
            }
            // Eğer form Personel Anasayfa'dan açıldıysa, Personel Anasayfa'ya yönlendir
            else if (GonderenForm == "PersonelAnasayfa")
            {
                PersonelAnasayfa personelAnasayfa = new PersonelAnasayfa();
                personelAnasayfa.Show();
                this.Hide();
            }
        }

        private void btnteslimAl_Click(object sender, EventArgs e)
        {
            // DataGridView'den seçilen satır var mı kontrolü
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçilen satırdan plaka bilgisi alınır
                string plaka = dataGridView1.SelectedRows[0].Cells["Plaka"].Value.ToString();

                using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
                {
                    try
                    {
                        baglanti.Open();

                        // 1. Adım: Aracın durumunu "Boş" olarak güncelle
                        string aracDurumGuncelleSorgu = "UPDATE Araclar SET Durumu = 'Boş' WHERE Plaka = @Plaka";
                        SqlCommand aracDurumGuncelleKomut = new SqlCommand(aracDurumGuncelleSorgu, baglanti);
                        aracDurumGuncelleKomut.Parameters.AddWithValue("@Plaka", plaka);
                        aracDurumGuncelleKomut.ExecuteNonQuery();

                        // 2. Adım: Seçilen aracı Satislar tablosundan sil
                        string satisSilSorgu = "DELETE FROM Satislar WHERE Plaka = @Plaka";
                        SqlCommand satisSilKomut = new SqlCommand(satisSilSorgu, baglanti);
                        satisSilKomut.Parameters.AddWithValue("@Plaka", plaka);
                        satisSilKomut.ExecuteNonQuery();

                        // DataGridView'i tekrar güncelle
                        RefreshDataGridView();

                        MessageBox.Show("Araç teslim alındı ve durumu 'Boş' olarak güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen teslim almak istediğiniz aracı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // DataGridView'i tekrar yükleyen metot
        private void RefreshDataGridView()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            string komutCumlesi = "SELECT * FROM Satislar"; // Satislar tablosunu yeniden sorgula
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}