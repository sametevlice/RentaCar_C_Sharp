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
    public partial class PersonelAnasayfa : Form
    {
        public string TcNo { get; set; }

        public PersonelAnasayfa()
        {
            InitializeComponent();
           
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";


        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            MusteriEkle personelmusterieklefrm = new MusteriEkle();
            personelmusterieklefrm.Show();
            personelmusterieklefrm.GonderenForm = "PersonelAnasayfa";
            
        }

        private void btnMusteriListele_Click(object sender, EventArgs e)
        {
            MusteriListele musteriListeleform = new MusteriListele();
            musteriListeleform.Show();
            musteriListeleform.GonderenForm = "PersonelAnasayfa";
            
            //personelmusterilistelefrm.Show();
            //this.Hide();
        }

        private void btnAracEkle_Click(object sender, EventArgs e)
        {
            AracEkle personelsayfaaraceklefrm = new AracEkle();
            personelsayfaaraceklefrm.Show();
            personelsayfaaraceklefrm.GonderenForm = "PersonelAnasayfa";
            
        }

        private void btnAracListele_Click(object sender, EventArgs e)
        {
            AracListele personelsayfaaraclistelefrm =new AracListele();
            personelsayfaaraclistelefrm.Show();
            personelsayfaaraclistelefrm.GonderenForm = "PersonelAnasayfa";
            
        }

        private void btnSozlesme_Click(object sender, EventArgs e)
        {
            Sözlesme personelsayfasözlesmefrm = new Sözlesme();
            personelsayfasözlesmefrm.Show();
            personelsayfasözlesmefrm.GonderenForm = "PersonelAnasayfa";
            
        }

        private void btnSatislar_Click(object sender, EventArgs e)
        {
            Satis personelsayfasatisfrm = new Satis();
            personelsayfasatisfrm.Show();
            personelsayfasatisfrm.GonderenForm = "PersonelAnasayfa";
            
        }


        //çikiş butonu
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss"); // Saat güncellemesi
            lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy dddd"); // Tarih güncellemesi
        }

        private Dictionary<Control, Rectangle> kontrolOrijinalBoyutlar = new Dictionary<Control, Rectangle>();
        private Size formOrijinalBoyut;

        private void PersonelAnasayfa_Load(object sender, EventArgs e)
        {
            VerileriGuncelle();

            panelControl1.Appearance.BorderColor = Color.DarkCyan; // DarkCyan rengi
            panelControl1.Appearance.Options.UseBorderColor = true; // Çerçeve rengini aktif et

            timer1.Start();
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss");
            lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy dddd");

            // Formun orijinal boyutunu kaydet
            formOrijinalBoyut = this.Size;

            // Tüm kontrollerin orijinal boyut ve konumlarını kaydet
            foreach (Control kontrol in this.Controls)
            {
                kontrolOrijinalBoyutlar[kontrol] = new Rectangle(kontrol.Location, kontrol.Size);
            }

           



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

                
                //Memnuniyet
                OrtalamaDerecelendirme();
               
                ToplamAracSayisiTextBox.ReadOnly = true;
                DoluAracSayisiTextBox.ReadOnly = true;
                BosAracSayisiTextBox.ReadOnly = true;
                MusteriSayisiTextBox.ReadOnly = true;
                PersonelSayisiTextBox.ReadOnly = true;
                
                txtBakım.ReadOnly = true;
                txtSözlesme.ReadOnly = true;
                txtSatis.ReadOnly = true;
                txtMemnuniyet.ReadOnly = true;
               


                //Memnuniyet
                OrtalamaDerecelendirme();



                ToplamAracSayisiTextBox.ReadOnly = true;
                DoluAracSayisiTextBox.ReadOnly = true;
                BosAracSayisiTextBox.ReadOnly = true;
                MusteriSayisiTextBox.ReadOnly = true;
                PersonelSayisiTextBox.ReadOnly = true;
                txtBakım.ReadOnly = true;
                txtSözlesme.ReadOnly = true;
                txtSatis.ReadOnly = true;
                txtMemnuniyet.ReadOnly = true;
               


                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekilirken bir hata oluştu: " + ex.Message);
            }



        }

       

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

        private void btnPersonelListele1_Click(object sender, EventArgs e)
        {
            PersonelListele personellistelefrm = new PersonelListele();
            personellistelefrm.Show();
            personellistelefrm.GonderenForm = "PersonelAnasayfa";
            this.Hide();
        }

       

        private void btnBilgilerim_Click(object sender, EventArgs e)
        {
            PersonelBilgileri bilgi = new PersonelBilgileri();
            bilgi.TcNo = this.TcNo;
            bilgi.Show();
            
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

        
        

        private void FormResize()
        {
            float oranGenislik = (float)this.Width / formOrijinalBoyut.Width;
            float oranYukseklik = (float)this.Height / formOrijinalBoyut.Height;

            foreach (Control kontrol in this.Controls)
            {
                if (kontrolOrijinalBoyutlar.TryGetValue(kontrol, out Rectangle orijinal))
                {
                    int yeniGenislik = (int)(orijinal.Width * oranGenislik);
                    int yeniYukseklik = (int)(orijinal.Height * oranYukseklik);
                    int yeniX = (int)(orijinal.X * oranGenislik);
                    int yeniY = (int)(orijinal.Y * oranYukseklik);

                    kontrol.Bounds = new Rectangle(new Point(yeniX, yeniY), new Size(yeniGenislik, yeniYukseklik));
                }
            }
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            FormResize();
            btnNormal.Visible = false;
            btnMax.Visible = true;
            panelControl1.Visible = true;

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veriler güncellenirken bir hata oluştu: " + ex.Message);
                }
            }
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
