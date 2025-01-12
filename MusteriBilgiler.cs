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
    public partial class MusteriBilgiler : DevExpress.XtraEditors.XtraForm
    {
        public string TcNo { get; set; }

        public MusteriBilgiler()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        private void MusteriBilgiler_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TcNo))
            {
                MusteriBilgileriniGoster(TcNo);
            }

            // Odaklanmayı kaldır veya başka bir kontrol seç
            this.ActiveControl = btnExit; // Odak 'Geri' butonuna ayarlanabilir
        }

        private void MusteriBilgileriniGoster(string tcNo)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // Müşteri bilgilerini çekme
                    string musteriSorgu = "SELECT Tc_No, Ad_Soyad, Telefon_Numarasi, Mail, Ehliyet_No, Ehliyet_Tarihi, Adres, resim,Dogum_Tarihi,Meslek,Ehliyet_Türü,Sifre FROM Musteriler WHERE Tc_No = @TcNo";
                    SqlCommand musteriKomut = new SqlCommand(musteriSorgu, baglanti);
                    musteriKomut.Parameters.AddWithValue("@TcNo", tcNo);

                    SqlDataReader musteriReader = musteriKomut.ExecuteReader();
                    if (musteriReader.Read())
                    {
                        txtTcNo.Text = musteriReader["Tc_No"].ToString();
                        txtAdSoyad.Text = musteriReader["Ad_Soyad"].ToString();
                        txtTelNo.Text = musteriReader["Telefon_Numarasi"].ToString();
                        txtMail.Text = musteriReader["Mail"].ToString();
                        txtEhliyetNo.Text = musteriReader["Ehliyet_No"].ToString();
                        txtEhliyetTarihi.Text = Convert.ToDateTime(musteriReader["Ehliyet_Tarihi"]).ToString("yyyy-MM-dd");
                        txtAdres.Text = musteriReader["Adres"].ToString();
                        txtDogumTarihi.Text = musteriReader["Dogum_Tarihi"].ToString();
                        txtMeslek.Text = musteriReader["Meslek"].ToString();
                        txtEhliyetTürü.Text = musteriReader["Ehliyet_Türü"].ToString();
                        txtSifre.Text = musteriReader["Sifre"].ToString();


                        if (musteriReader["resim"] != DBNull.Value && !string.IsNullOrEmpty(musteriReader["resim"].ToString()))
                        {
                            pictureBox3.Image = Image.FromFile(musteriReader["resim"].ToString());
                        }
                        else
                        {
                            pictureBox3.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                        }
                    }

                    musteriReader.Close();

                    // TextBox'ların seçimini kaldır
                    ClearTextBoxSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Seçimi kaldıran yardımcı metot
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
