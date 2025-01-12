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
    public partial class MusteriEkle : DevExpress.XtraEditors.XtraForm
    {
        public MusteriEkle()
        {
            InitializeComponent();
            
            txtTcNo.KeyPress += TxtTcNo_KeyPress; 
        }
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }


        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Alanlar boş olamaz
            if (string.IsNullOrWhiteSpace(txtTcNo.Text) || txtTcNo.Text.Length != 11)
            {
                MessageBox.Show("TC Kimlik numarası 11 haneli olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTelNo.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMail.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMeslek.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDogumTarihi.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEhliyetTürü.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEhliyetTürü.Text))
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(string.IsNullOrEmpty(pictureBox3.ImageLocation))
            {
                MessageBox.Show(
                    "Lütfen tüm bilgileri doldurunuz.",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }


            // TC numarasının veritabanında var olup olmadığını kontrol et
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // TC numarasını kontrol et
                    string tcKontrolSorgu = "SELECT COUNT(*) FROM Musteriler WHERE Tc_No = @TcNo";
                    SqlCommand tcKontrolKomut = new SqlCommand(tcKontrolSorgu, baglanti);
                    tcKontrolKomut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);

                    int mevcutMusteriSayisi = (int)tcKontrolKomut.ExecuteScalar();

                    if (mevcutMusteriSayisi > 0)
                    {
                        MessageBox.Show("Bu TC Kimlik numarasına sahip bir müşteri zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Aynı TC numarasına sahip bir müşteri varsa kaydetme
                    }

                    // Eğer TC numarası benzersizse, müşteri kaydını yap
                    SqlCommand komut = new SqlCommand(
                        "INSERT INTO Musteriler (Tc_No, Ad_Soyad, Telefon_Numarasi, Mail, Ehliyet_No, Ehliyet_Tarihi, Adres,resim,Dogum_Tarihi,Meslek,Ehliyet_Türü,Sifre) " +
                        "VALUES (@Tc_No, @Ad_Soyad, @Telefon_Numarasi, @Mail, @Ehliyet_No, @Ehliyet_Tarihi, @Adres,@resim,@Dogum_Tarihi,@Meslek,@Ehliyet_Türü,@Sifre)", baglanti);

                    komut.Parameters.AddWithValue("@Tc_No", txtTcNo.Text);
                    komut.Parameters.AddWithValue("@Ad_Soyad", txtAdSoyad.Text);
                    komut.Parameters.AddWithValue("@Telefon_Numarasi", txtTelNo.Text);
                    komut.Parameters.AddWithValue("@Mail", txtMail.Text);
                    komut.Parameters.AddWithValue("@Ehliyet_No", txtEhliyetNo.Text);
                    komut.Parameters.AddWithValue("@Ehliyet_Tarihi", txtEhliyetTarihi.Text);
                    komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
                    komut.Parameters.AddWithValue("@resim", pictureBox3.ImageLocation);
                    komut.Parameters.AddWithValue("@Dogum_Tarihi", txtDogumTarihi.Text);
                    komut.Parameters.AddWithValue("@Meslek", txtMeslek.Text);
                    komut.Parameters.AddWithValue("@Ehliyet_Türü", txtEhliyetTürü.Text);
                    komut.Parameters.AddWithValue("@Sifre", txtSifre.Text);

                    komut.ExecuteNonQuery();

                    MessageBox.Show("Yeni müşteri başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Alanları temizle
                    txtTcNo.Text = string.Empty;
                    txtAdSoyad.Text = string.Empty;
                    txtTelNo.Text = string.Empty;
                    txtEhliyetNo.Text = string.Empty;
                    txtEhliyetTarihi.Text = string.Empty;
                    txtMail.Text = string.Empty;
                    txtAdres.Text = string.Empty;
                    pictureBox3.Image = null;
                    txtDogumTarihi.Text = string.Empty;
                    txtMeslek.Text = string.Empty;
                    txtEhliyetTürü.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void TxtTcNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

      
        private void btnback_Click(object sender, EventArgs e)
        {
            if (GonderenForm == "Anasayfa")
            {
                Anasayfa anasayfa = new Anasayfa();
                anasayfa.Show();
                this.Hide();
            }
            
            else if (GonderenForm == "PersonelAnasayfa")
            {
                PersonelAnasayfa personelAnasayfa = new PersonelAnasayfa();
                personelAnasayfa.Show();
                this.Hide();
            }
        }

        private void btnResim_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox3.ImageLocation = openFileDialog1.FileName;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        
        }
    }
}
