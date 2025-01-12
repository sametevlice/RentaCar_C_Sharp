using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AracKiralama
{
    public partial class PersonelLogin : Form
    {
        public PersonelLogin()
        {
            InitializeComponent();
        }
        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";


        private void btnGeri_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
            ilkgrs.Show();
            this.Hide();

        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string tckimlikNo = login_tckimlik.Text.Trim();
            string sifre = login_sifre.Text.Trim();



            // Kullanıcının TC Kimlik Numarasını kontrol etme
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    // SQL sorgusu
                    string sorgu = "SELECT Ad,Sifre=@Sifre FROM Personeller WHERE TCKimlikNo = @TCKimlikNo";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@TCKimlikNo", tckimlikNo);
                    komut.Parameters.AddWithValue("@Sifre", sifre);


                    // Veriyi oku
                    object sonuc = komut.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string ad = sonuc.ToString();
                        MessageBox.Show($"Hoşgeldin {ad}!", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Yeni forma geçiş
                        PersonelAnasayfa anasayfa = new PersonelAnasayfa();
                        anasayfa.TcNo = tckimlikNo;
                        anasayfa.Show();
                        this.Hide();
                        
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz TC Kimlik Numarası.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PersonelLogin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
