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
    public partial class SirketDegerlendirme : DevExpress.XtraEditors.XtraForm
    {
        // Veritabanı bağlantı cümlesi
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        // Seçilen derecelendirmeyi tutan değişken
        private int selectedRating = 0;

        public SirketDegerlendirme()
        {
            InitializeComponent();
        }

        private void btnYorumKaydet_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string eposta = txtEposta.Text.Trim();
            string yorum = txtYorum.Text.Trim();

            // Gerekli alanların doldurulup doldurulmadığını kontrol ediyoruz
            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(yorum) || selectedRating == 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurup bir derecelendirme seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Veritabanına ekleme işlemi
            try
            {
                using (SqlConnection conn = new SqlConnection(baglantiCumlesi))
                {
                    conn.Open();
                    string query = "INSERT INTO Yorumlar (Ad, Soyad, Mail, Derecelendirme, Yorum) VALUES (@Ad, @Soyad, @Mail, @Derecelendirme, @Yorum)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", ad);
                        cmd.Parameters.AddWithValue("@Soyad", soyad);
                        cmd.Parameters.AddWithValue("@Mail", eposta);
                        cmd.Parameters.AddWithValue("@Derecelendirme", selectedRating);
                        cmd.Parameters.AddWithValue("@Yorum", yorum);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Yorum başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Derecelendirme RadioButton olayları
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                selectedRating = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                selectedRating = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                selectedRating = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                selectedRating = 4;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
                selectedRating = 5;
        }

        // Formdaki alanları temizlemek için bir metod
        private void Temizle()
        {
            txtAd.Text = "";
            txtSoyad.Text = "";
            txtEposta.Text = "";
            txtYorum.Text = "";
            selectedRating = 0;

            // Tüm RadioButtonları temizle
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
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