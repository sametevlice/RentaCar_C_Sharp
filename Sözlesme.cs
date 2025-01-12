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
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AracKiralama
{
    public partial class Sözlesme : DevExpress.XtraEditors.XtraForm
    {
        public Sözlesme()
        {
            InitializeComponent();
        }

        private string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public string GonderenForm { get; internal set; }

        public void Arac_Listele()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            string komutCumlesi = "Select * From Araclar where Durumu = 'Boş' OR Durumu = 'Bos'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                cbxArac.Items.Add(read["Plaka"]);
            }

        }

        
        private void Sözlesme_Load(object sender, EventArgs e)
        {
            Arac_Listele();
            Sözlesme_Listele();
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            
        }

        public void Sözlesme_Listele()
        {

            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            String komutCumlesi = "Select * From Sözlesme";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        private void cbxArac_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            string komutCumlesi = "Select * From Araclar where Plaka like '" + cbxArac.SelectedItem + "'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtMarka.Text = read["Marka"].ToString();
                txtSeri.Text = read["Seri"].ToString();
                txtModel.Text = read["Model"].ToString();
                txtRenk.Text = read["Renk"].ToString();
                try
                {
                    if (read["resim"] != DBNull.Value && !string.IsNullOrEmpty(read["resim"].ToString()))
                    {
                        pictureBox2.Image = System.Drawing.Image.FromFile(read["resim"].ToString());
                    }
                    else
                    {
                        pictureBox2.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                    }
                }
                catch (Exception ex)
                {
                    pictureBox2.Image = null; // Resim yüklenemezse sadece resim kısmını temizle
                    MessageBox.Show("Aracın Resmi Sistemde yok.: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
           
        }

        private void cbxKiraSekil_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            string komutCumlesi = "Select Kira_Ücreti From Araclar";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (cbxKiraSekil.SelectedIndex == 0)
                {
                    txtKiraUcret.Text = (int.Parse(read["Kira_Ücreti"].ToString()) * 1).ToString();
                }
                else if (cbxKiraSekil.SelectedIndex == 1)
                {
                    txtKiraUcret.Text = (int.Parse(read["Kira_Ücreti"].ToString()) * 0.80).ToString();
                }
                else if (cbxKiraSekil.SelectedIndex == 2)
                {
                    txtKiraUcret.Text = (int.Parse(read["Kira_Ücreti"].ToString()) * 0.70).ToString();
                }
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            
        }

        private void tctTcAra_EditValueChanged(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();
            string komutCumlesi = "Select * From Musteriler where Tc_No like '" + txtTcAra.Text + "'";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtTc.Text = read["Tc_No"].ToString();
                txtAdSoyad.Text = read["Ad_Soyad"].ToString();
                txtTel.Text = read["Telefon_Numarasi"].ToString();
                txtEhliyetNo.Text = read["Ehliyet_No"].ToString();
                txtEhliyetTarihi.Text = read["Ehliyet_Tarihi"].ToString();

                try
                {
                    if (read["resim"] != DBNull.Value && !string.IsNullOrEmpty(read["resim"].ToString()))
                    {
                        pictureBox3.Image = System.Drawing.Image.FromFile(read["resim"].ToString());
                    }
                    else
                    {
                        pictureBox3.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                    }
                }
                catch (Exception ex)
                {
                    pictureBox3.Image = null; // Resim yüklenemezse sadece resim kısmını temizle
                    MessageBox.Show("Müşterinin Resmi Sistemde yok.: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }

        private void btnAracTeslim_Click(object sender, EventArgs e)
        {
            DataGridViewRow satir = dataGridView1.CurrentRow;

            // Seçilen satırın geçerli olup olmadığını kontrol et
            if (satir == null)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime cikis, donus;
            if (!DateTime.TryParse(satir.Cells["Cikis_Tarih"].Value.ToString(), out cikis) ||
                !DateTime.TryParse(satir.Cells["Dönüs_Tarih"].Value.ToString(), out donus))
            {
                MessageBox.Show("Geçersiz tarih formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Dönüş tarihinin çıkış tarihinden önce olup olmadığını kontrol et
            if (donus < cikis)
            {
                MessageBox.Show("Dönüş tarihi çıkış tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kira ücreti ve tutar kontrolü (decimal kullanıyoruz)
            decimal ucret = 0, tutar = 0;
            if (!decimal.TryParse(satir.Cells["Kira_Ücreti"].Value.ToString(), out ucret) ||
                !decimal.TryParse(satir.Cells["Tutar"].Value.ToString(), out tutar))
            {
                MessageBox.Show("Geçersiz kira ücreti veya tutar.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gün farkını hesapla
            TimeSpan gunFarki = donus - cikis;
            int gunSayisi = gunFarki.Days;

            if (gunSayisi < 0)
            {
                MessageBox.Show("Dönüş tarihi çıkış tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tutar hesaplaması
            decimal toplamTutar = gunSayisi * ucret;

            // Veritabanı bağlantısı ve işlemleri
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            baglanti.Open();

            // Sözleşmeyi silme işlemi
            string komutCumlesi = "DELETE FROM Sözlesme WHERE Plaka = @Plaka";
            SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
            komut.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
            komut.ExecuteNonQuery();

            // Aracın durumunu 'Dolu' olarak güncelleme
            string komutCumlesiUp = "UPDATE Araclar SET Durumu = 'Dolu' WHERE Plaka = @Plaka";
            SqlCommand komutUp = new SqlCommand(komutCumlesiUp, baglanti);
            komutUp.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
            komutUp.ExecuteNonQuery();

            // Satış verilerini ekleme
            string komutCumlesiSatis = "INSERT INTO Satislar (Tc_No, Ad_Soyad, Plaka, Gun, Kira_Sekli, Kira_Ucret, Tutar, Cikis_Tarih, Dönüs_Tarih) " +
                                       "VALUES (@Tc_No, @Ad_Soyad, @Plaka, @Gun, @Kira_Sekli, @Kira_Ucret, @Tutar, @Cikis_Tarih, @Dönüs_Tarih)";
            SqlCommand komutSatis = new SqlCommand(komutCumlesiSatis, baglanti);
            komutSatis.Parameters.AddWithValue("@Tc_No", satir.Cells["Tc_No"].Value.ToString());
            komutSatis.Parameters.AddWithValue("@Ad_Soyad", satir.Cells["Ad_Soyad"].Value.ToString());
            komutSatis.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
            komutSatis.Parameters.AddWithValue("@Gun", gunSayisi);
            komutSatis.Parameters.AddWithValue("@Kira_Sekli", satir.Cells["Kira_Sekli"].Value.ToString());
            komutSatis.Parameters.AddWithValue("@Kira_Ucret", ucret);
            komutSatis.Parameters.AddWithValue("@Tutar", toplamTutar);
            komutSatis.Parameters.AddWithValue("@Cikis_Tarih", cikis);
            komutSatis.Parameters.AddWithValue("@Dönüs_Tarih", donus);
            komutSatis.ExecuteNonQuery();

            // DataGridView'deki satırı sil
            dataGridView1.Rows.Remove(satir);

            // Başarı mesajı
            MessageBox.Show("Araç Teslim Edildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtTc.Text = "";
            txtAdSoyad.Text = "";
            txtTel.Text = "";
            txtEhliyetNo.Text = "";
            txtEhliyetTarihi.Text = "";
            txtMarka.Text = "";
            txtSeri.Text = "";
            txtModel.Text = "";
            txtRenk.Text = "";
            txtKiraUcret.Text = "";
            txtGün.Text = "";
            txtTutar.Text = "";
            cbxArac.Text = "";
            txtTcAra.Text = "";
            pictureBox3.Image = null;
            pictureBox2.Image = null;

            // ComboBox'ları temizleyin
            
            cbxArac.SelectedIndex = -1;  // Aracın plakasını temizler
            cbxKiraSekil.SelectedIndex = -1;  // Kira şekli seçeneğini temizler

            // DateTimePicker'ları sıfırlayın
            DateTimeCikis.Value = DateTime.Now;
            DateTimeDönüs.Value = DateTime.Now;

        }



        private void btnTemizle_Click(object sender, EventArgs e)
        {
           
        }

        private void Temizle()
        {
            DateTimeCikis.Text = DateTime.Now.ToShortDateString();
            DateTimeDönüs.Text = DateTime.Now.ToShortDateString();
            cbxKiraSekil.Text = "";
            txtKiraUcret.Text = "";
            txtGün.Text = "";
            txtTutar.Text = "";
        }




        private void btnback_Click(object sender, EventArgs e)
        {
            if (GonderenForm == "Anasayfa")
            {
                Anasayfa anasayfa = new Anasayfa();
                anasayfa.Show();
                this.Hide();
            }
            // Eğer form Personel Anasayfa'dan açıldıysa, Personel Anasayfa'ya yönlendir
            else if (GonderenForm == "PersonelAnasayfa")
            {
                PersonelAnasayfa personelAnasayfa = new PersonelAnasayfa();
                personelAnasayfa.Show();
                this.Hide();
            }
        }

        private void btnTumuTemizle_Click(object sender, EventArgs e)
        {
            txtTc.Text = "";
            txtAdSoyad.Text = "";
            txtTel.Text = "";
            txtEhliyetNo.Text = "";
            txtEhliyetTarihi.Text = "";
            txtMarka.Text = "";
            txtSeri.Text = "";
            txtModel.Text = "";
            txtRenk.Text = "";
            txtKiraUcret.Text = "";
            txtGün.Text = "";
            txtTutar.Text = "";
            cbxArac.Text = "";
            txtTcAra.Text = "";
            pictureBox3.Image = null;
            pictureBox2.Image = null;

            // ComboBox'ları temizleyin
            cbxArac.SelectedIndex = -1;  // Aracın plakasını temizler
            cbxKiraSekil.SelectedIndex = -1;  // Kira şekli seçeneğini temizler

            // DateTimePicker'ları sıfırlayın
            DateTimeCikis.Value = DateTime.Now;
            DateTimeDönüs.Value = DateTime.Now;
        }

      


        private void btnPdf_Click(object sender, EventArgs e)
        {
            DataGridViewRow satir = dataGridView1.CurrentRow;

            // Seçilen satırın geçerli olup olmadığını kontrol et
            if (satir == null)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iTextSharp.text.Document raporum = new iTextSharp.text.Document();
            PdfWriter.GetInstance(raporum, new FileStream("C:Deneme.Pdf", FileMode.Create));
            raporum.AddAuthor("Admin");
            raporum.AddCreationDate();
            raporum.AddTitle("Araç Sözleşmesi");
            if (raporum.IsOpen() == false)
            { raporum.Open(); }

            Paragraph title = new Paragraph("ARAÇ SÖZLEŞMESI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            raporum.Add(title);
            // Altına çizgi ekleyelim
            iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -2);
            raporum.Add(new Chunk(line));
            raporum.Add(new Paragraph("\n"));

            raporum.Add(new Paragraph());

            Paragraph customerTitle = new Paragraph("MUSTERI BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(customerTitle);
            raporum.Add(new Paragraph("\n"));

            // Bilgiler ve fotoğraf düzenlemesi
            PdfPTable customerTable = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            customerTable.SetWidths(new float[] { 2, 1 });

            // Bilgi kısmı
            PdfPCell infoCell = new PdfPCell
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER
            };
            infoCell.AddElement(new Paragraph($"TC : {txtTc.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Ad Soyad : {txtAdSoyad.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Telefon Numarası : {txtTel.Text}"));
            raporum.Add(new Paragraph("\n"));
            //infoCell.AddElement(new Paragraph($"E-Posta : {txtMail.Text}"));
            infoCell.AddElement(new Paragraph($"Ehliyet No : {txtEhliyetNo.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Ehliyet Tarihi : {txtEhliyetTarihi.Text}"));
            //infoCell.AddElement(new Paragraph($"Adres : {txtAdres.Text}"));

            customerTable.AddCell(infoCell);

            // Fotoğraf kısmı
            if (pictureBox3.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox3.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image customerImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    customerImage.ScaleToFit(150f, 100f);
                    customerImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                    PdfPCell imageCell = new PdfPCell(customerImage)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    customerTable.AddCell(imageCell);
                }
            }
            else
            {
                customerTable.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });
            }

            raporum.Add(customerTable);
            raporum.Add(new Paragraph("\n"));

            // Araç Bilgileri Başlığı
            Paragraph carTitle = new Paragraph("ARAÇ BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(carTitle);
            raporum.Add(new Paragraph("\n"));

            // Bilgiler ve fotoğraf düzenlemesi
            PdfPTable carTable = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            carTable.SetWidths(new float[] { 2, 1 });

            // Bilgi kısmı
            PdfPCell carInfoCell = new PdfPCell
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER
            };
            carInfoCell.AddElement(new Paragraph($"Plaka : {cbxArac.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Marka : {txtMarka.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Seri : {txtSeri.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Model : {txtModel.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Renk : {txtRenk.Text}"));
            //carInfoCell.AddElement(new Paragraph($"Kilometre : {txtKm.Text}"));

            carTable.AddCell(carInfoCell);

            // Fotoğraf kısmı
            if (pictureBox2.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image carImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    carImage.ScaleToFit(150f, 100f);
                    carImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                    PdfPCell imageCell = new PdfPCell(carImage)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    carTable.AddCell(imageCell);
                }
            }
            else
            {
                carTable.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });
            }

            raporum.Add(carTable);
            raporum.Add(new Paragraph("\n"));

            Paragraph contractTitle = new Paragraph("SOZLESME BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(contractTitle);
            raporum.Add(new Paragraph("\n"));

            // Sözleşme detayları
            raporum.Add(new Paragraph($"Kira Şekli : {cbxKiraSekil.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Gun Sayısı : {txtGün.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Kira Ücreti : {txtKiraUcret.Text}"));
            raporum.Add(new Paragraph($"Tutar : {txtTutar.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Çikis Tarihi : {DateTimeCikis.Value:yyyy-MM-dd}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Donus Tarihi : {DateTimeDönüs.Value:yyyy-MM-dd}"));
           

            raporum.Add(new Paragraph("\n\n"));

            Paragraph footer = new Paragraph("Evlice Araç Kiralama", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_RIGHT
            };
            raporum.Add(footer);

            // Kullanıcıya bilgilendirme mesajı gösteriyoruz
            raporum.Close();
            MessageBox.Show("PDF başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                txtTc.Text = satir.Cells["Tc_No"].Value?.ToString();
                txtAdSoyad.Text = satir.Cells["Ad_Soyad"].Value?.ToString();
                txtTel.Text = satir.Cells["Telefon_Numarasi"].Value?.ToString();
                txtEhliyetNo.Text = satir.Cells["Ehliyet_No"].Value?.ToString();
                txtEhliyetTarihi.Text = satir.Cells["Ehliyet_Tarihi"].Value?.ToString();
                cbxArac.Text = satir.Cells["Plaka"].Value?.ToString();
                txtMarka.Text = satir.Cells["Marka"].Value?.ToString();
                txtSeri.Text = satir.Cells["Seri"].Value?.ToString();
                txtModel.Text = satir.Cells["Model"].Value?.ToString();
                txtRenk.Text = satir.Cells["Renk"].Value?.ToString();
                cbxKiraSekil.SelectedItem = satir.Cells["Kira_Sekli"].Value?.ToString();
                txtKiraUcret.Text = satir.Cells["Kira_Ücreti"].Value?.ToString();
                txtGün.Text = satir.Cells["Kiralandigi_Gün_Sayisi"].Value?.ToString();
                txtTutar.Text = satir.Cells["Tutar"].Value?.ToString();

                // Araç resmi yükleme
                try
                {
                    if (satir.Cells["resim_araba"].Value != null && !string.IsNullOrEmpty(satir.Cells["resim_araba"].Value.ToString()))
                    {
                        string aracResimYolu = satir.Cells["resim_araba"].Value.ToString();
                        if (File.Exists(aracResimYolu))
                        {
                            pictureBox2.Image = System.Drawing.Image.FromFile(aracResimYolu);
                            pictureBox2.ImageLocation = aracResimYolu;
                        }
                        else
                        {
                            pictureBox2.Image = null;
                            MessageBox.Show("Araç resmi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        pictureBox2.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    pictureBox2.Image = null;
                    MessageBox.Show("Araç resmi yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Müşteri resmi yükleme
                try
                {
                    if (satir.Cells["resim_musteri"].Value != null && !string.IsNullOrEmpty(satir.Cells["resim_musteri"].Value.ToString()))
                    {
                        string musteriResimYolu = satir.Cells["resim_musteri"].Value.ToString();
                        if (File.Exists(musteriResimYolu))
                        {
                            pictureBox3.Image = System.Drawing.Image.FromFile(musteriResimYolu);
                            pictureBox3.ImageLocation = musteriResimYolu;
                        }
                        else
                        {
                            pictureBox3.Image = null;
                            MessageBox.Show("Müşteri resmi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        pictureBox3.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    pictureBox3.Image = null;
                    MessageBox.Show("Müşteri resmi yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Tarih değerlerini ayarlama
                if (DateTime.TryParse(satir.Cells["Cikis_Tarih"].Value?.ToString(), out DateTime cikisTarih))
                {
                    DateTimeCikis.Value = cikisTarih;
                }
                else
                {
                    DateTimeCikis.Value = DateTime.Now;
                }

                if (DateTime.TryParse(satir.Cells["Dönüs_Tarih"].Value?.ToString(), out DateTime donusTarih))
                {
                    DateTimeDönüs.Value = donusTarih;
                }
                else
                {
                    DateTimeDönüs.Value = DateTime.Now;
                }
            }
        }

        private void btnSozlesmeiptal_Click(object sender, EventArgs e)
        {

           // Seçilen satırı al
            DataGridViewRow satir = dataGridView1.CurrentRow;

            // Seçilen satırın geçerli olup olmadığını kontrol et
            if (satir == null)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kullanıcıdan onay almak için MessageBox
            DialogResult onay = MessageBox.Show("Sözleşmeyi iptal etmek ister misiniz?", "Sözleşme İptali", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Eğer kullanıcı "Evet" derse işlemi yap
            if (onay == DialogResult.Yes)
            {
                // Veritabanı bağlantısı
                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                baglanti.Open();

                // Sözleşmeyi silme işlemi
                string komutCumlesi = "DELETE FROM Sözlesme WHERE Plaka = @Plaka";
                SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);
                komut.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
                komut.ExecuteNonQuery();

                // Araç durumunu 'Boş' olarak güncelleme
                string komutCumlesiUp = "UPDATE Araclar SET Durumu = 'Boş' WHERE Plaka = @Plaka";
                SqlCommand komutUp = new SqlCommand(komutCumlesiUp, baglanti);
                komutUp.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
                komutUp.ExecuteNonQuery();

                // Satırı DataGridView'den sil
                dataGridView1.Rows.Remove(satir);

                // Başarı mesajı
                MessageBox.Show("Sözleşme iptal edildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gerekirse formdaki diğer alanları temizleyebilirsiniz
                txtTc.Text = "";
                txtAdSoyad.Text = "";
                txtTel.Text = "";
                txtEhliyetNo.Text = "";
                txtEhliyetTarihi.Text = "";
                txtMarka.Text = "";
                txtSeri.Text = "";
                txtModel.Text = "";
                txtRenk.Text = "";
                txtKiraUcret.Text = "";
                txtGün.Text = "";
                txtTutar.Text = "";
                cbxArac.Text = "";
                txtTcAra.Text = "";
                pictureBox3.Image = null;
                pictureBox2.Image = null;
                // ComboBox'ları temizleyin
                cbxArac.Items.Clear();
                cbxArac.SelectedIndex = -1;  // Aracın plakasını temizler
                cbxKiraSekil.SelectedIndex = -1;  // Kira şekli seçeneğini temizler

                // DateTimePicker'ları sıfırlayın
                DateTimeCikis.Value = DateTime.Now;
                DateTimeDönüs.Value = DateTime.Now;
            }
        }

        private void btnPdfCikar_Click(object sender, EventArgs e)
        {

            DataGridViewRow satir = dataGridView1.CurrentRow;

            // Seçilen satırın geçerli olup olmadığını kontrol et
            if (satir == null)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iTextSharp.text.Document raporum = new iTextSharp.text.Document();
            PdfWriter.GetInstance(raporum, new FileStream("C:Sozlesme.Pdf", FileMode.Create));
            raporum.AddAuthor("Admin");
            raporum.AddCreationDate();
            raporum.AddTitle("Arac Sozlesmesi");
            if (raporum.IsOpen() == false)
            { raporum.Open(); }

            Paragraph title = new Paragraph("ARAC SOZLESMESI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            raporum.Add(title);
            // Altına çizgi ekleyelim
            iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -2);
            raporum.Add(new Chunk(line));
            raporum.Add(new Paragraph("\n"));

            raporum.Add(new Paragraph());

            Paragraph customerTitle = new Paragraph("MUSTERI BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(customerTitle);
            raporum.Add(new Paragraph("\n"));

            // Bilgiler ve fotoğraf düzenlemesi
            PdfPTable customerTable = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            customerTable.SetWidths(new float[] { 2, 1 });

            // Bilgi kısmı
            PdfPCell infoCell = new PdfPCell
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER
            };
            infoCell.AddElement(new Paragraph($"TC : {txtTc.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Ad Soyad : {txtAdSoyad.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Telefon Numarasi : {txtTel.Text}"));
            raporum.Add(new Paragraph("\n"));
            //infoCell.AddElement(new Paragraph($"E-Posta : {txtMail.Text}"));
            infoCell.AddElement(new Paragraph($"Ehliyet No : {txtEhliyetNo.Text}"));
            raporum.Add(new Paragraph("\n"));
            infoCell.AddElement(new Paragraph($"Ehliyet Tarihi : {txtEhliyetTarihi.Text}"));
            //infoCell.AddElement(new Paragraph($"Adres : {txtAdres.Text}"));

            customerTable.AddCell(infoCell);

            // Fotoğraf kısmı
            if (pictureBox3.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox3.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image customerImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    customerImage.ScaleToFit(150f, 100f);
                    customerImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                    PdfPCell imageCell = new PdfPCell(customerImage)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    customerTable.AddCell(imageCell);
                }
            }
            else
            {
                customerTable.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });
            }

            raporum.Add(customerTable);
            raporum.Add(new Paragraph("\n"));

            // Araç Bilgileri Başlığı
            Paragraph carTitle = new Paragraph("ARAC BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(carTitle);
            raporum.Add(new Paragraph("\n"));

            // Bilgiler ve fotoğraf düzenlemesi
            PdfPTable carTable = new PdfPTable(2)
            {
                WidthPercentage = 100
            };
            carTable.SetWidths(new float[] { 2, 1 });

            // Bilgi kısmı
            PdfPCell carInfoCell = new PdfPCell
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER
            };
            carInfoCell.AddElement(new Paragraph($"Plaka : {cbxArac.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Marka : {txtMarka.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Seri : {txtSeri.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Model : {txtModel.Text}"));
            raporum.Add(new Paragraph("\n"));
            carInfoCell.AddElement(new Paragraph($"Renk : {txtRenk.Text}"));
            //carInfoCell.AddElement(new Paragraph($"Kilometre : {txtKm.Text}"));

            carTable.AddCell(carInfoCell);

            // Fotoğraf kısmı
            if (pictureBox2.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image carImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    carImage.ScaleToFit(150f, 100f);
                    carImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                    PdfPCell imageCell = new PdfPCell(carImage)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    carTable.AddCell(imageCell);
                }
            }
            else
            {
                carTable.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });
            }

            raporum.Add(carTable);
            raporum.Add(new Paragraph("\n"));

            Paragraph contractTitle = new Paragraph("SOZLESME BILGILERI\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_LEFT
            };
            raporum.Add(contractTitle);
            raporum.Add(new Paragraph("\n"));

            // Sözleşme detayları
            raporum.Add(new Paragraph($"Kira Sekli : {cbxKiraSekil.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Gun Sayisi : {txtGün.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Kira Ucreti : {txtKiraUcret.Text}"));
            raporum.Add(new Paragraph($"Tutar : {txtTutar.Text}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Cikis Tarihi : {DateTimeCikis.Value:yyyy-MM-dd}"));
            raporum.Add(new Paragraph("\n"));
            raporum.Add(new Paragraph($"Donus Tarihi : {DateTimeDönüs.Value:yyyy-MM-dd}"));


            raporum.Add(new Paragraph("\n\n"));

            Paragraph footer = new Paragraph("Evlice Arac Kiralama", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD))
            {
                Alignment = Element.ALIGN_RIGHT
            };
            raporum.Add(footer);

            // Kullanıcıya bilgilendirme mesajı gösteriyoruz
            raporum.Close();
            MessageBox.Show("PDF başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTc.Text))
            {
                MessageBox.Show("Henüz seçim yapmadınız. Lütfen bir sözleşme seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Fonksiyonu sonlandır
            }
            try
            {
                // Boş alan kontrolü
                if (string.IsNullOrEmpty(txtTc.Text) || string.IsNullOrEmpty(txtAdSoyad.Text) ||
                    string.IsNullOrEmpty(txtTel.Text) || string.IsNullOrEmpty(txtEhliyetNo.Text) ||
                    string.IsNullOrEmpty(cbxArac.Text) || string.IsNullOrEmpty(cbxKiraSekil.Text) ||
                    string.IsNullOrEmpty(txtKiraUcret.Text) || string.IsNullOrEmpty(txtGün.Text) ||
                    string.IsNullOrEmpty(txtTutar.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurunuz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tarih kontrolü
                if (DateTimeDönüs.Value < DateTimeCikis.Value)
                {
                    MessageBox.Show("Dönüş tarihi çıkış tarihinden önce olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Resim yollarını kaydet
                string aracResimYolu = "";
                string musteriResimYolu = "";

                // Araç resmi için
                if (pictureBox2.Image != null)
                {
                    // Resmi geçici bir klasöre kaydet
                    string aracResimAdi = "Arac_" + cbxArac.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    string aracResimKlasoru = Path.Combine(Application.StartupPath, "AracResimleri");

                    if (!Directory.Exists(aracResimKlasoru))
                        Directory.CreateDirectory(aracResimKlasoru);

                    aracResimYolu = Path.Combine(aracResimKlasoru, aracResimAdi);
                    pictureBox2.Image.Save(aracResimYolu, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                // Müşteri resmi için
                if (pictureBox3.Image != null)
                {
                    // Resmi geçici bir klasöre kaydet
                    string musteriResimAdi = "Musteri_" + txtTc.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    string musteriResimKlasoru = Path.Combine(Application.StartupPath, "MusteriResimleri");

                    if (!Directory.Exists(musteriResimKlasoru))
                        Directory.CreateDirectory(musteriResimKlasoru);

                    musteriResimYolu = Path.Combine(musteriResimKlasoru, musteriResimAdi);
                    pictureBox3.Image.Save(musteriResimYolu, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
                baglanti.Open();

                string komutCumlesi = "Insert Into Sözlesme Values (@Tc_No, @Ad_Soyad, @Telefon_Numarasi, @Ehliyet_No, @Ehliyet_Tarih, @plaka, @Marka, @Seri, @Model, @Renk, @Kira_Sekli, @Kira_Ücreti, @Kiralandigi_Gün_Sayisi, @Tutar, @Cikis_Tarih, @Dönüs_Tarih, @resim_araba, @resim_musteri)";
                SqlCommand komut = new SqlCommand(komutCumlesi, baglanti);

                komut.Parameters.AddWithValue("@Tc_No", txtTc.Text);
                komut.Parameters.AddWithValue("@Ad_Soyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@Telefon_Numarasi", txtTel.Text);
                komut.Parameters.AddWithValue("@Ehliyet_No", txtEhliyetNo.Text);
                komut.Parameters.AddWithValue("@Ehliyet_Tarih", txtEhliyetTarihi.Text);
                komut.Parameters.AddWithValue("@plaka", cbxArac.Text);
                komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
                komut.Parameters.AddWithValue("@Seri", txtSeri.Text);
                komut.Parameters.AddWithValue("@Model", txtModel.Text);
                komut.Parameters.AddWithValue("@Renk", txtRenk.Text);
                komut.Parameters.AddWithValue("@Kira_Sekli", cbxKiraSekil.SelectedItem);
                komut.Parameters.AddWithValue("@Kira_Ücreti", txtKiraUcret.Text);
                komut.Parameters.AddWithValue("@Kiralandigi_Gün_Sayisi", txtGün.Text);
                komut.Parameters.AddWithValue("@Tutar", txtTutar.Text);
                komut.Parameters.AddWithValue("@Cikis_Tarih", DateTimeCikis.Value);
                komut.Parameters.AddWithValue("@Dönüs_Tarih", DateTimeDönüs.Value);

                // Resim yollarını veritabanına kaydet
                komut.Parameters.AddWithValue("@resim_araba", string.IsNullOrEmpty(aracResimYolu) ? DBNull.Value : (object)aracResimYolu);
                komut.Parameters.AddWithValue("@resim_musteri", string.IsNullOrEmpty(musteriResimYolu) ? DBNull.Value : (object)musteriResimYolu);

                // Aracın durumunu güncelle
                string komutCumlesiUp = "update Araclar set Durumu = 'Dolu' where Plaka = @Plaka";
                SqlCommand komutUp = new SqlCommand(komutCumlesiUp, baglanti);
                komutUp.Parameters.AddWithValue("@Plaka", cbxArac.Text);

                komutUp.ExecuteNonQuery();
                komut.ExecuteNonQuery();
                baglanti.Close();

                Sözlesme_Listele();
                Arac_Listele();
                MessageBox.Show("Sözleşme başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHesaplaNew_Click(object sender, EventArgs e)
        { // Tarih girişlerini kontrol et
            if (!DateTime.TryParse(DateTimeDönüs.Text, out DateTime dönüsTarihi))
            {
                MessageBox.Show("Dönüş tarihi geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!DateTime.TryParse(DateTimeCikis.Text, out DateTime cikisTarihi))
            {
                MessageBox.Show("Çıkış tarihi geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tarihler arası fark hesapla
            TimeSpan gunFarki = dönüsTarihi - cikisTarihi;
            int gunSayisi = gunFarki.Days;

            // Gün farkını kontrol et
            if (gunSayisi < 0)
            {
                MessageBox.Show("Dönüş tarihi çıkış tarihinden önce olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtGün.Text = gunSayisi.ToString();

            // Kira ücretini kontrol et
            if (!decimal.TryParse(txtKiraUcret.Text, out decimal kiraUcreti))
            {
                MessageBox.Show("Kira ücreti geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tutarı hesapla ve göster
            decimal toplamTutar = gunSayisi * kiraUcreti;
            txtTutar.Text = toplamTutar.ToString("0.00");

        }

        private void btnTemizleNew_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }

}
