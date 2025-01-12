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
    public partial class MusteriLogin : DevExpress.XtraEditors.XtraForm
    {
        public MusteriLogin()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string tckimlikNo = login_tckimlik.Text.Trim();
            string sifre = login_sifre.Text.Trim();

            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    string sorgu = "SELECT Ad_Soyad,Sifre=@Sifre FROM Musteriler WHERE Tc_No = @Tc_No";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@Tc_No", tckimlikNo);
                    komut.Parameters.AddWithValue("@Sifre", sifre);

                    object sonuc = komut.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string adSoyad = sonuc.ToString();
                        MessageBox.Show($"Hoşgeldin {adSoyad}!", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Yeni forma geçiş
                        MusteriSayfa musteriSayfa = new MusteriSayfa();
                        musteriSayfa.TcNo = tckimlikNo; // TC Kimlik Numarasını geçiriyoruz
                        musteriSayfa.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz TC Kimlik Numarası veya Sifre.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
            ilkgrs.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı "Evet" derse, uygulamayı kapat
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_sifre.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }
    }
}