using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

   
namespace AracKiralama
{
    public partial class Anasayfa : DevExpress.XtraEditors.XtraForm
    {
        public Anasayfa()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";
       

        private void btnPersonelEkle1_Click(object sender, EventArgs e)
        {
            PersonelEkle personeleklefrm = new PersonelEkle();
            personeleklefrm.Show();
            
        }

        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            

            MusteriEkle musterieklefrm = new MusteriEkle();
            musterieklefrm.Show();
            musterieklefrm.GonderenForm = "Anasayfa";
            
        }

        private void btnMusteriListele_Click(object sender, EventArgs e)
        {

            MusteriListele müsterilistefrm = new MusteriListele();
            müsterilistefrm.Show();
            müsterilistefrm.GonderenForm = "Anasayfa";
            
        }

       

        private void btnAracEkle_Click(object sender, EventArgs e)
        {
            AracEkle araceklefrm = new AracEkle();
            araceklefrm.Show();
            araceklefrm.GonderenForm = "Anasayfa";

           
        }

        private void btnAracListele_Click(object sender, EventArgs e)
        {
            AracListele araclistelefrm = new AracListele();
            araclistelefrm.Show();
            araclistelefrm.GonderenForm = "Anasayfa";
            
        }

        private void btnSozlesme_Click(object sender, EventArgs e)
        {
            Sözlesme sozlesmefrm = new Sözlesme();
            sozlesmefrm.Show();
            sozlesmefrm.GonderenForm = "Anasayfa";
            
        }

        private void btnSatislar_Click(object sender, EventArgs e)
        {
            Satis satisfrm = new Satis();
            satisfrm.Show();
            satisfrm.GonderenForm = "Anasayfa";
            
        }

        private void btnPersonelListele1_Click(object sender, EventArgs e)
        {
            PersonelListele personellistelefrm = new PersonelListele();
            personellistelefrm.Show();
            personellistelefrm.GonderenForm = "Anasayfa";

            
        }


        

        private void Anasayfa_Load(object sender, EventArgs e)
        {

            VerileriGuncelle();


            panelControl1.Appearance.BorderColor = Color.FromArgb(255, 128, 0); // RGB(255, 128, 0)
            panelControl1.Appearance.Options.UseBorderColor = true; // Çerçeve rengini aktif et

            timerSaat.Start();
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss");
            lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy dddd");

            // Formun orijinal boyutunu kaydet
            formOrijinalBoyut = this.Size;

            // Tüm kontrollerin orijinal boyut ve konumlarını kaydet
            foreach (Control kontrol in this.Controls)
            {
                kontrolOrijinalBoyutlar[kontrol] = new Rectangle(kontrol.Location, kontrol.Size);
            }

            // Veritabanı bağlantısı ve veri çekme işlemi
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            try
            {
                baglanti.Open();

                // Toplam araç sayısı
                SqlCommand toplamAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar", baglanti);
                int toplamAracSayisi = Convert.ToInt32(toplamAracSayisiCmd.ExecuteScalar() ?? 0);
                ToplamAracSayisiTextBox.Text = toplamAracSayisi.ToString();

                // Dolu araç sayısı
                SqlCommand doluAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Dolu'", baglanti);
                int doluAracSayisi = Convert.ToInt32(doluAracSayisiCmd.ExecuteScalar() ?? 0);
                DoluAracSayisiTextBox.Text = doluAracSayisi.ToString();

                // Boş araç sayısı
                SqlCommand bosAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Boş'", baglanti);
                int bosAracSayisi = Convert.ToInt32(bosAracSayisiCmd.ExecuteScalar() ?? 0);
                BosAracSayisiTextBox.Text = bosAracSayisi.ToString();

                // Bakım araç sayısı
                SqlCommand bakimAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Bakım'", baglanti);
                int bakimAracSayisi = Convert.ToInt32(bakimAracSayisiCmd.ExecuteScalar() ?? 0);
                txtBakım.Text = bakimAracSayisi.ToString();

                // Müşteri sayısı
                SqlCommand musteriSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Musteriler", baglanti);
                int musteriSayisi = Convert.ToInt32(musteriSayisiCmd.ExecuteScalar() ?? 0);
                MusteriSayisiTextBox.Text = musteriSayisi.ToString();

                // Personel sayısı
                SqlCommand personelSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Personeller", baglanti);
                int personelSayisi = Convert.ToInt32(personelSayisiCmd.ExecuteScalar() ?? 0);
                PersonelSayisiTextBox.Text = personelSayisi.ToString();
                // Sözleşme sayısı
                SqlCommand SözlesmeSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Sözlesme", baglanti);
                int SözlesmeSayisi = Convert.ToInt32(SözlesmeSayisiCmd.ExecuteScalar() ?? 0);
                txtSözlesme.Text = SözlesmeSayisi.ToString();

                // Satış sayısı
                SqlCommand SatisSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Satislar", baglanti);
                int SatisSayisi = Convert.ToInt32(SatisSayisiCmd.ExecuteScalar() ?? 0);
                txtSatis.Text = SatisSayisi.ToString();

                // Randevu sayısı
                SqlCommand RandevuSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Randevular", baglanti);
                int RandevuSayisi = Convert.ToInt32(RandevuSayisiCmd.ExecuteScalar() ?? 0);
                txtRandevu.Text = RandevuSayisi.ToString();

                //Memnuniyet
                OrtalamaDerecelendirme();
                // Nakit durumu
                SqlCommand nakitDurumuCmd = new SqlCommand("SELECT SUM(CAST(Tutar AS DECIMAL(10,2))) FROM AracKiralama.dbo.Satislar", baglanti);
                object nakitDurumuResult = nakitDurumuCmd.ExecuteScalar();
                decimal nakitDurumu = (nakitDurumuResult != DBNull.Value) ? (decimal)nakitDurumuResult : 0;
                NakitDurumuTextBox.Text = nakitDurumu.ToString("C2");

                // Gider
                SqlCommand GiderDurumuCmd = new SqlCommand("SELECT SUM(CAST(Maas AS DECIMAL(10,2))) FROM AracKiralama.dbo.Personeller", baglanti);
                object giderDurumuResult = GiderDurumuCmd.ExecuteScalar();
                decimal GiderDurumu = (giderDurumuResult != DBNull.Value) ? (decimal)giderDurumuResult : 0;
                txtGider.Text = GiderDurumu.ToString("C2");




                ToplamAracSayisiTextBox.ReadOnly = true;
                DoluAracSayisiTextBox.ReadOnly = true;
                BosAracSayisiTextBox.ReadOnly = true;
                MusteriSayisiTextBox.ReadOnly = true;
                PersonelSayisiTextBox.ReadOnly = true;
                NakitDurumuTextBox.ReadOnly = true;
                txtGider.ReadOnly = true;
                txtBakım.ReadOnly = true;
                txtSözlesme.ReadOnly = true;
                txtSatis.ReadOnly = true;
                txtMemnuniyet.ReadOnly = true;
                txtRandevu.ReadOnly = true;


                baglanti.Close();

  // Timer ve veritabanı işlemleri burada devam eder.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekilirken bir hata oluştu: " + ex.Message);
            }







            // Formun orijinal boyutunu kaydet
            formOrijinalBoyut = this.Size;

            // Tüm kontrollerin orijinal boyut ve konumlarını kaydet
            foreach (Control kontrol in this.Controls)
            {
                kontrolOrijinalBoyutlar[kontrol] = new Rectangle(kontrol.Location, kontrol.Size);
            }
        }

        private Dictionary<Control, Rectangle> kontrolOrijinalBoyutlar = new Dictionary<Control, Rectangle>();
        private Size formOrijinalBoyut;
        

        private void OrtalamaDerecelendirme()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
                {
                    baglanti.Open();

                    // Derecelendirmeleri toplama ve toplam yorum sayısını alma
                    SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Derecelendirme), 0), COUNT(*) FROM Yorumlar", baglanti);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int toplamDerecelendirme = reader.GetInt32(0); // SUM(Derecelendirme)
                        int toplamYorumSayisi = reader.GetInt32(1);   // COUNT(*)

                        // Ortalama hesaplama
                        double ortalama = toplamYorumSayisi > 0 ? (double)toplamDerecelendirme / toplamYorumSayisi : 0;

                        // Ortalama sonucu TextBox'a yazdırma
                        txtMemnuniyet.Text = ortalama.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan onay al
            DialogResult result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı 'Yes' seçerse uygulamadan çık
            if (result == DialogResult.Yes)
            {
                Frm_ilkgiris frm_İlkgiris = new Frm_ilkgiris();
                frm_İlkgiris.Show();
                this.Hide();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı "Evet" derse, uygulamayı kapat
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss"); // Saat güncellemesi
            lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy dddd"); // Tarih güncellemesi
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz?", "Kapat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                Application.Exit();  // Uygulamayı kapat
            }
        }

        private Rectangle previousBounds; // Önceki boyut ve konum
        private bool isMaximized = false; // Tam ekran durumu

        private void btnMax_Click(object sender, EventArgs e)
        {
        

            this.WindowState = FormWindowState.Maximized;
            FormResize();
            btnNormal.Visible = true;
            btnMax.Visible = false;
            panelControl1.Visible = false;


        }



        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            RandevuListesi randevuListesi = new RandevuListesi();
            randevuListesi.Show();
        }

        private void labelControl7_Click(object sender, EventArgs e)
        {

        }

        private void btnNormal_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            FormResize();
            btnNormal.Visible = false;
            btnMax.Visible = true;
            panelControl1.Visible = true;

        }

        private void FormResize()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                foreach (Control kontrol in this.Controls)
                {
                    if (kontrol is DevExpress.XtraEditors.SimpleButton)
                    {
                        if (kontrolOrijinalBoyutlar.TryGetValue(kontrol, out Rectangle orijinal))
                        {
                            var btn = (DevExpress.XtraEditors.SimpleButton)kontrol;

                            // Boyutu %20 büyüt
                            int yeniGenislik = (int)(orijinal.Width * 1.2);
                            int yeniYukseklik = (int)(orijinal.Height * 1.2);

                            btn.Location = new Point(orijinal.X, orijinal.Y);
                            btn.Size = new Size(yeniGenislik, yeniYukseklik);

                            // Font boyutunu da %20 büyüt
                            float yeniFontBoyutu = btn.Font.Size * 1.2f;
                            btn.Font = new Font(btn.Font.FontFamily, yeniFontBoyutu, btn.Font.Style);
                        }
                    }
                    else
                    {
                        // Diğer kontroller için form boyutuna göre ölçekle
                        if (kontrolOrijinalBoyutlar.TryGetValue(kontrol, out Rectangle orijinal))
                        {
                            float oranGenislik = (float)this.Width / formOrijinalBoyut.Width;
                            float oranYukseklik = (float)this.Height / formOrijinalBoyut.Height;

                            kontrol.Bounds = new Rectangle(
                                new Point((int)(orijinal.X * oranGenislik), (int)(orijinal.Y * oranYukseklik)),
                                new Size((int)(orijinal.Width * oranGenislik), (int)(orijinal.Height * oranYukseklik))
                            );
                        }
                    }
                }
            }
            else // Normal boyuta dönerken
            {
                foreach (Control kontrol in this.Controls)
                {
                    if (kontrolOrijinalBoyutlar.TryGetValue(kontrol, out Rectangle orijinal))
                    {
                        kontrol.Bounds = orijinal;
                        if (kontrol is DevExpress.XtraEditors.SimpleButton)
                        {
                            kontrol.Font = new Font(kontrol.Font.FontFamily, orijinal.Height / 5.0f, kontrol.Font.Style);
                        }
                    }
                }
            }
        }

       

        private void VerileriGuncelle()
        {
            using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True"))
            {
                try
                {
                    baglanti.Open();

                    // Toplam araç sayısı
                    SqlCommand toplamAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar", baglanti);
                    int toplamAracSayisi = Convert.ToInt32(toplamAracSayisiCmd.ExecuteScalar() ?? 0);
                    ToplamAracSayisiTextBox.Text = toplamAracSayisi.ToString();

                    // Dolu araç sayısı
                    SqlCommand doluAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Dolu'", baglanti);
                    int doluAracSayisi = Convert.ToInt32(doluAracSayisiCmd.ExecuteScalar() ?? 0);
                    DoluAracSayisiTextBox.Text = doluAracSayisi.ToString();

                    // Boş araç sayısı
                    SqlCommand bosAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Boş'", baglanti);
                    int bosAracSayisi = Convert.ToInt32(bosAracSayisiCmd.ExecuteScalar() ?? 0);
                    BosAracSayisiTextBox.Text = bosAracSayisi.ToString();

                    // Bakım araç sayısı
                    SqlCommand bakimAracSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Araclar WHERE Durumu = 'Bakım'", baglanti);
                    int bakimAracSayisi = Convert.ToInt32(bakimAracSayisiCmd.ExecuteScalar() ?? 0);
                    txtBakım.Text = bakimAracSayisi.ToString();

                    // Müşteri sayısı
                    SqlCommand musteriSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Musteriler", baglanti);
                    int musteriSayisi = Convert.ToInt32(musteriSayisiCmd.ExecuteScalar() ?? 0);
                    MusteriSayisiTextBox.Text = musteriSayisi.ToString();

                    // Personel sayısı
                    SqlCommand personelSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Personeller", baglanti);
                    int personelSayisi = Convert.ToInt32(personelSayisiCmd.ExecuteScalar() ?? 0);
                    PersonelSayisiTextBox.Text = personelSayisi.ToString();
                    // Sözleşme sayısı
                    SqlCommand SözlesmeSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Sözlesme", baglanti);
                    int SözlesmeSayisi = Convert.ToInt32(SözlesmeSayisiCmd.ExecuteScalar() ?? 0);
                    txtSözlesme.Text = SözlesmeSayisi.ToString();

                    // Satış sayısı
                    SqlCommand SatisSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Satislar", baglanti);
                    int SatisSayisi = Convert.ToInt32(SatisSayisiCmd.ExecuteScalar() ?? 0);
                    txtSatis.Text = SatisSayisi.ToString();

                    // Randevu sayısı
                    SqlCommand RandevuSayisiCmd = new SqlCommand("SELECT COUNT(*) FROM AracKiralama.dbo.Randevular", baglanti);
                    int RandevuSayisi = Convert.ToInt32(RandevuSayisiCmd.ExecuteScalar() ?? 0);
                    txtRandevu.Text = RandevuSayisi.ToString();



                    // Nakit durumu
                    SqlCommand nakitDurumuCmd = new SqlCommand("SELECT SUM(CAST(Tutar AS DECIMAL(10,2))) FROM AracKiralama.dbo.Satislar", baglanti);
                    object nakitDurumuResult = nakitDurumuCmd.ExecuteScalar();
                    decimal nakitDurumu = (nakitDurumuResult != DBNull.Value) ? (decimal)nakitDurumuResult : 0;
                    NakitDurumuTextBox.Text = nakitDurumu.ToString("C2");

                    // Gider
                    SqlCommand GiderDurumuCmd = new SqlCommand("SELECT SUM(CAST(Maas AS DECIMAL(10,2))) FROM AracKiralama.dbo.Personeller", baglanti);
                    object giderDurumuResult = GiderDurumuCmd.ExecuteScalar();
                    decimal GiderDurumu = (giderDurumuResult != DBNull.Value) ? (decimal)giderDurumuResult : 0;
                    txtGider.Text = GiderDurumu.ToString("C2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veriler güncellenirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void pnlToplamAracSayisi_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void btnRapor1_Click(object sender, EventArgs e)
        {
            YapilanYorumlar yorumlar = new YapilanYorumlar();
            yorumlar.Show();
        }

        private void newYenilebtn_Click(object sender, EventArgs e)
        {
            VerileriGuncelle();
        }
    }
}
