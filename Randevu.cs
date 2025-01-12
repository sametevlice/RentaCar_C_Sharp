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
    public partial class Randevu : Form
    {
        // Seçilen saat ve kullanıcı bilgilerini tutacak değişkenler
        private string secilenSaat;
        private string tcNo;
        private string ad;
        private string soyad;
        private string telefonNo;
        private string tarih;

        public Randevu()
        {
            InitializeComponent();
        }

        // Form açıldığında saat butonlarını devre dışı bırakıyoruz
        private void Randevu_Load_1(object sender, EventArgs e)
        {
            // Saatler dizisi
            string[] saatler = new string[]
            {
                "08:00", "08:20", "08:40", "09:00", "09:20", "09:40", "10:00", "10:20", "10:40", "11:00",
                "11:00", "11:40", "13:00", "13:20", "13:40", "14:00", "14:20", "14:40", "15:00", "15:20",
                "15:40", "16:00", "16:20", "16:40", "17:00", "17:20", "17:40", "18:00"
            };

            // Button1'den Button28'e kadar olan butonların Text'ini saatlerle dolduruyoruz
            Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                button9, button10, button11, button12, button13, button14, button15, button16,
                                                button17, button18, button19, button20, button21, button22, button23, button24,
                                                button25, button26, button27, button28 };

            // Butonları saatlerle doldur
            for (int i = 0; i < butonlar.Length; i++)
            {
                butonlar[i].Text = saatler[i];
                butonlar[i].Click += SaatButonu_Click; // Her buton tıklandığında aynı olay çalışacak
                butonlar[i].Enabled = false; // Başlangıçta butonları devre dışı bırakıyoruz
            }
        }

        private void btnAra_Click_1(object sender, EventArgs e)
        {
            // Tarih, ad, soyad, telefon no girilmediyse kullanıcıyı uyar
            if (string.IsNullOrEmpty(txtTcNo.Text) || string.IsNullOrEmpty(txtAd.Text) || string.IsNullOrEmpty(txtSoyad.Text) || string.IsNullOrEmpty(txtTelNo.Text) || dtpTarih.Value == null)
            {
                MessageBox.Show("Lütfen tüm bilgileri doldurun.");
                return;
            }

            // TcNo'nun 11 haneli ve rakamlardan oluştuğunu kontrol et
            if (txtTcNo.Text.Length != 11 || !txtTcNo.Text.All(char.IsDigit))
            {
                MessageBox.Show("TC No 11 haneli olmalı ve sadece rakam içermelidir.");
                return;
            }

            // "Ara" butonuna tıklanınca butonlar aktif hale gelir
            Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                button9, button10, button11, button12, button13, button14, button15, button16,
                                                button17, button18, button19, button20, button21, button22, button23, button24,
                                                button25, button26, button27, button28 };

            foreach (Button btn in butonlar)
            {
                btn.Enabled = true; // Butonları aktif yap
            }

            // Veritabanından dolu saatleri al
            VeritabanindanDoluSaatleriGetir();
        }

        // Tarih değiştiğinde dolu saatleri yenile
        private void dtpTarih_ValueChanged(object sender, EventArgs e)
        {
            // Eğer kullanıcı "Ara" butonuna tıklamadan tarih değiştirirse dolu saatler boşalsın
            Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                button9, button10, button11, button12, button13, button14, button15, button16,
                                                button17, button18, button19, button20, button21, button22, button23, button24,
                                                button25, button26, button27, button28 };

            foreach (Button btn in butonlar)
            {
                btn.BackColor = SystemColors.Control; // Butonları varsayılan renklerine getir
                btn.Enabled = false; // Butonları devre dışı bırak
            }

            // Veritabanından dolu saatleri al
            VeritabanindanDoluSaatleriGetir();
        }

        // Veritabanından dolu olan saatleri alıyoruz
        private void VeritabanindanDoluSaatleriGetir()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True");
            try
            {
                baglanti.Open();
                string sorgu = "SELECT DISTINCT Saat FROM Randevular WHERE Tarih = @Tarih";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@Tarih", dtpTarih.Value.ToShortDateString());

                SqlDataReader reader = komut.ExecuteReader();

                // Butonları varsayılan duruma getir
                Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                    button9, button10, button11, button12, button13, button14, button15, button16,
                                                    button17, button18, button19, button20, button21, button22, button23, button24,
                                                    button25, button26, button27, button28 };

                // Butonları varsayılan renklerine döndür
                foreach (Button btn in butonlar)
                {
                    btn.BackColor = SystemColors.Control; // Varsayılan renk
                    btn.Enabled = true; // Butonları aktif yap
                }

                // Dolu olan saatleri bulup butonlarının rengini siyah yapıyoruz
                while (reader.Read())
                {
                    string doluSaat = reader["Saat"].ToString();

                    foreach (Button btn in butonlar)
                    {
                        if (btn.Text == doluSaat)
                        {
                            btn.BackColor = Color.Black;  // Dolu olan saatin butonunun rengini siyah yapıyoruz
                            btn.Enabled = false;  // Dolu saatin butonunu pasif hale getiriyoruz
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        // Saat butonlarına tıklanıldığında seçilen saat bilgisini alıyoruz
        private void SaatButonu_Click(object sender, EventArgs e)
        {
            // Eğer kullanıcı tarih, ad, soyad, telefon no bilgilerini girmediyse uyarı göster
            if (string.IsNullOrEmpty(txtTcNo.Text) || string.IsNullOrEmpty(txtAd.Text) || string.IsNullOrEmpty(txtSoyad.Text) || string.IsNullOrEmpty(txtTelNo.Text) || dtpTarih.Value == null)
            {
                MessageBox.Show("Lütfen önce tarih, ad, soyad ve telefon numarasını girin.");
                return;
            }

            Button secilenButon = sender as Button;

            // Eğer tıklanan saat dolu ise, uyarı mesajı göster
            if (secilenButon.BackColor == Color.Black)
            {
                MessageBox.Show("Bu saat dolu. Lütfen başka bir saat seçiniz.");
                return;
            }

            secilenSaat = secilenButon.Text;  // Seçilen saat

            // Saat butonunun etrafını renklendiriyoruz
            secilenButon.BackColor = Color.LightBlue; // Seçilen saat butonunun arka plan rengini mavi yapıyoruz

            // Diğer bilgileri alıyoruz
            tcNo = txtTcNo.Text;
            ad = txtAd.Text;
            soyad = txtSoyad.Text;
            telefonNo = txtTelNo.Text;
            tarih = dtpTarih.Value.ToShortDateString();

            // Diğer saat butonlarının rengini sıfırlıyoruz
            Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                button9, button10, button11, button12, button13, button14, button15, button16,
                                                button17, button18, button19, button20, button21, button22, button23, button24,
                                                button25, button26, button27, button28 };

            foreach (Button btn in butonlar)
            {
                if (btn != secilenButon && btn.BackColor != Color.Black)
                {
                    btn.BackColor = SystemColors.Control; // Diğer butonların rengini varsayılan hale getiriyoruz
                }
            }
        }

        // Kaydet butonuna tıklandığında bilgileri veritabanına kaydediyoruz
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Eğer saat seçilmemişse kullanıcıyı bilgilendir
            if (string.IsNullOrEmpty(secilenSaat))
            {
                MessageBox.Show("Lütfen bir saat seçin.");
                return;
            }

            // Veritabanı bağlantısı ve komut
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9FINMFA\\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True");
            try
            {
                baglanti.Open();
                string komutCumlesi = "INSERT INTO Randevular (TcNo, Ad, Soyad, TelefonNo, Tarih, Saat) " +
                    "VALUES (@TcNo, @Ad, @Soyad, @TelefonNo, @Tarih, @Saat)";
                SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);

                // Parametreleri ekleyin
                komut.Parameters.AddWithValue("@TcNo", tcNo);
                komut.Parameters.AddWithValue("@Ad", ad);
                komut.Parameters.AddWithValue("@Soyad", soyad);
                komut.Parameters.AddWithValue("@TelefonNo", telefonNo);
                komut.Parameters.AddWithValue("@Tarih", tarih);
                komut.Parameters.AddWithValue("@Saat", secilenSaat);

                // Veritabanına kaydet
                komut.ExecuteNonQuery();
                MessageBox.Show("Randevunuz kaydedildi.");

                // Formu temizle
                txtTcNo.Text = string.Empty;
                txtAd.Text = string.Empty;
                txtSoyad.Text = string.Empty;
                txtTelNo.Text = string.Empty;
                dtpTarih.Value = DateTime.Now;
                

                // Saat butonlarının rengini sıfırlıyoruz
                Button[] butonlar = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8,
                                                    button9, button10, button11, button12, button13, button14, button15, button16,
                                                    button17, button18, button19, button20, button21, button22, button23, button24,
                                                    button25, button26, button27, button28 };

                foreach (Button btn in butonlar)
                {
                    btn.BackColor = SystemColors.Control; // Varsayılan renklere dön
                    btn.Enabled = false; // Butonları devre dışı bırak
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgiris = new Frm_ilkgiris();
            ilkgiris.Show();
            this.Hide();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgiris = new Frm_ilkgiris();
            ilkgiris.Show();
            this.Hide();
        }


        private void btnGeri_Click(object sender, EventArgs e)
        {

            Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
            ilkgrs.Show();
            this.Hide();
        }
    }
}
