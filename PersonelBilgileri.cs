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
using System.IO;

namespace AracKiralama
{
    public partial class PersonelBilgileri : DevExpress.XtraEditors.XtraForm
    {
        // Veritabanı bağlantı cümlesi
        private readonly string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string TcNo { get; set; }

        public PersonelBilgileri()
        {
            InitializeComponent();
        }

        private void PersonelBilgileri_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TcNo))
            {
                PersonelBilgileriniGoster(TcNo); // TcNo'ya göre personel bilgilerini göster
            }
            this.ActiveControl = btnExit;
        }

        private void PersonelBilgileriniGoster(string tcNo)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // Personel bilgilerini çekme
                    string sorgu = "Select TcKimlikNo, Ad, Soyad, Telefon, Email, Departman, Pozisyon, Adres, resim, Maas, Baslama_Tarihi, Calisma_Durumu, Dogum_Tarihi, Cinsiyet,Sifre from Personeller";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@TcNo", tcNo);

                    SqlDataReader reader = komut.ExecuteReader();

                    if (reader.Read())
                    {
                        // Personel bilgilerini form elemanlarına atama
                        txtTc.Text = reader["TCKimlikNo"].ToString();
                        txtAd.Text = reader["Ad"].ToString();
                        txtSoyad.Text = reader["Soyad"].ToString();
                        txtPozisyon.Text = reader["Pozisyon"].ToString();
                        txtTelNo.Text = reader["Telefon"].ToString();
                        txtMail.Text = reader["Email"].ToString();
                        cbxDepartman.Text = reader["Departman"].ToString();
                        txtMaas.Text = reader["Maas"].ToString();
                        txtAdres.Text = reader["Adres"].ToString();
                        cbxCalismaDurumu.Text = reader["Calisma_Durumu"].ToString();
                        cbxCinsiyet.Text = reader["Cinsiyet"].ToString();
                        txtDogumTarihi.Text = reader["Dogum_Tarihi"].ToString();
                        txtBaslamaTarihi.Text = reader["Baslama_Tarihi"].ToString();
                        txtSifre.Text = reader["Sifre"].ToString();


                        // Resim verisini kontrol et ve yükle

                        if (reader["resim"] != DBNull.Value && !string.IsNullOrEmpty(reader["resim"].ToString()))
                        {
                            pictureBox3.Image = Image.FromFile(reader["resim"].ToString());
                        }
                        else
                        {
                            pictureBox3.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                        }
                    }

                    reader.Close();

                    ClearTextBoxSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearTextBoxSelection()
        {
            // Tüm TextBox'ların seçimini kaldır
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.SelectionStart = textBox.Text.Length; // İmleci sona getir
                    textBox.SelectionLength = 0; // Seçimi sıfırla
                }
            }

            // Odağı kaldır veya başka bir kontrol üzerine ayarla
            this.ActiveControl = null;
        }
        private void btnback_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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