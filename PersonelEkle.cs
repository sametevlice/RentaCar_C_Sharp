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
    public partial class PersonelEkle : DevExpress.XtraEditors.XtraForm
    {
        // Veritabanı bağlantı cümlesi
        private readonly string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public PersonelEkle()
        {
            InitializeComponent();

            // txtTc için KeyPress olayını bağlama
            txtTc.KeyPress += txtTc_KeyPress;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // TCKimlikNo uzunluk kontrolü
            if (txtTc.Text.Length != 11)
            {
                MessageBox.Show("T.C. Kimlik Numarası 11 haneli olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Giriş verilerinin kontrolü
            if (string.IsNullOrWhiteSpace(txtTc.Text) ||
                string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtTelNo.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text) ||
                string.IsNullOrWhiteSpace(txtAdres.Text) ||
                string.IsNullOrWhiteSpace(txtMaas.Text) ||
                string.IsNullOrWhiteSpace(txtBaslamaTarihi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text) ||
                cbxCinsiyet.SelectedIndex == -1 ||
                cbxCalismaDurumu.SelectedIndex == -1 ||
                cbxDepartman.SelectedIndex == -1 ||
                txtPozisyon.SelectedIndex == -1
                )
            {
                MessageBox.Show("Lütfen gerekli tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TC Kimlik Numarası kontrolü
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // TCKimlikNo'nun daha önce eklenip eklenmediğini kontrol et
                    string tcKontrolSorgu = "SELECT COUNT(*) FROM Personeller WHERE TCKimlikNo = @TCKimlikNo";
                    SqlCommand tcKontrolKomut = new SqlCommand(tcKontrolSorgu, baglanti);
                    tcKontrolKomut.Parameters.AddWithValue("@TCKimlikNo", txtTc.Text.Trim());

                    int mevcutPersonelSayisi = (int)tcKontrolKomut.ExecuteScalar();

                    if (mevcutPersonelSayisi > 0)
                    {
                        MessageBox.Show("Bu TC Kimlik numarasına sahip bir personel zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Aynı TC Kimlik numarasına sahip bir personel varsa kaydetme
                    }

                    // Veritabanına ekleme sorgusu
                    string sorgu = "INSERT INTO Personeller (TCKimlikNo, Ad, Soyad, Telefon, Adres, Departman, Pozisyon, Email, resim,Maas,Baslama_Tarihi,Calisma_Durumu,Dogum_Tarihi,Cinsiyet,Sifre) " +
                                   "VALUES (@TCKimlikNo, @Ad, @Soyad, @Telefon, @Adres, @Departman, @Pozisyon, @Email, @resim,@Maas,@Baslama_Tarihi,@Calisma_Durumu,@Dogum_Tarihi,@Cinsiyet,@Sifre)";

                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        // Parametre ekleme
                        komut.Parameters.AddWithValue("@TCKimlikNo", txtTc.Text.Trim());
                        komut.Parameters.AddWithValue("@Ad", txtAd.Text.Trim());
                        komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text.Trim());
                        komut.Parameters.AddWithValue("@Telefon", string.IsNullOrWhiteSpace(txtTelNo.Text) ? (object)DBNull.Value : txtTelNo.Text.Trim());
                        komut.Parameters.AddWithValue("@Adres", string.IsNullOrWhiteSpace(txtAdres.Text) ? (object)DBNull.Value : txtAdres.Text.Trim());
                        komut.Parameters.AddWithValue("@Departman", cbxDepartman.Text.Trim());
                        komut.Parameters.AddWithValue("@Pozisyon", txtPozisyon.Text.Trim());
                        komut.Parameters.AddWithValue("@Maas", txtMaas.Text.Trim());
                        komut.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtMail.Text) ? (object)DBNull.Value : txtMail.Text.Trim());
                        komut.Parameters.AddWithValue("@resim", pictureBox3.ImageLocation);
                        komut.Parameters.AddWithValue("@Baslama_Tarihi", txtBaslamaTarihi.Text.Trim());
                        komut.Parameters.AddWithValue("@Calisma_Durumu", cbxCalismaDurumu.Text.Trim());
                        komut.Parameters.AddWithValue("@Dogum_Tarihi", txtDogumTarihi.Text.Trim());
                        komut.Parameters.AddWithValue("@Cinsiyet", cbxCinsiyet.Text.Trim());
                        komut.Parameters.AddWithValue("@Sifre", txtSifre.Text.Trim());

                        // Sorguyu çalıştır
                        komut.ExecuteNonQuery();
                    }

                    MessageBox.Show("Yeni personel başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Alanları temizle
                    Temizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakam girişine izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Geçersiz karakter girişini engelle
            }
        }



        private void Temizle()
        {
            // Tüm alanları temizle
            txtTc.Text = string.Empty;
            txtAd.Text = string.Empty;
            txtSoyad.Text = string.Empty;
            txtTelNo.Text = string.Empty;
            txtAdres.Text = string.Empty;
            cbxDepartman.SelectedIndex = -1;
            cbxCalismaDurumu.SelectedIndex = -1;
            cbxCinsiyet.SelectedIndex = -1;
            txtPozisyon.Text = string.Empty;
            txtMaas.Text = string.Empty;
            txtMail.Text = string.Empty;
            pictureBox3.Image = null;
            txtSifre.Text = string.Empty;
            txtDogumTarihi.Text=string.Empty;
            txtBaslamaTarihi.Text= string.Empty;
            txtSifre.Text= string.Empty;
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            Anasayfa anasayfaform = new Anasayfa();
            anasayfaform.Show();
            this.Hide();
        }

        private void cbxDepartman_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPozisyon.Items.Clear();
            txtPozisyon.Text = string.Empty;

            if (cbxDepartman.SelectedIndex == 0) 
            {
                txtPozisyon.Items.AddRange(new string[] {"Satış Danışmanı", "Pazarlama Uzmanı", "Müşteri İlişkileri Yöneticisi", "Reklam ve Tanıtım Uzmanı"});
            }
            else if (cbxDepartman.SelectedIndex == 1) 
            {
                txtPozisyon.Items.AddRange(new string[] {"Operasyon Sorumlusu","Filo Yönetim Uzmanı","Araç Teslimat ve Teslim Alma Görevlisi","Lojistik Planlama Uzmanı"  });
            }
            else if (cbxDepartman.SelectedIndex == 2) 
            {
                txtPozisyon.Items.AddRange(new string[] { "Teknik Servis Uzmanı", "Araç Bakım Teknisyeni","Lastik ve Yedek Parça Uzmanı","Hasar Değerlendirme Uzmanı"});
            }
            else if (cbxDepartman.SelectedIndex == 3)
            {
                txtPozisyon.Items.AddRange(new string[] { "Muhasebe Uzmanı", "Finans Analisti","Faturalama Sorumlusu","Ödeme Takip Uzmanı"});
            }
            else if (cbxDepartman.SelectedIndex == 4) 
            {
                txtPozisyon.Items.AddRange(new string[] {"İnsan Kaynakları Uzmanı","Eğitim ve Gelişim Sorumlusu","İşe Alım Uzmanı","Performans Değerlendirme Uzmanı" });
            }
        }

        
        

        private void btnResim_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox3.ImageLocation = openFileDialog1.FileName;
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

