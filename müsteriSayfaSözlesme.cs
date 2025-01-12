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
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace AracKiralama
{
    public partial class müsteriSayfaSözlesme : DevExpress.XtraEditors.XtraForm
    {
        public string TcNo { get; set; }

        public müsteriSayfaSözlesme()
        {
            InitializeComponent();
        }

        public string baglantiCumlesi = @"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True";

        public void müsteriSayfaSözlesme_Load_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TcNo))
            {
                SözlesmeBilgileriniGoster(TcNo);
                MusteriBilgileriniGoster(TcNo);
            }
        }

        public void MusteriBilgileriniGoster(string tcNo)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    string musteriSorgu = "SELECT Telefon_Numarasi, Mail, Ehliyet_No, Ehliyet_Tarihi, Adres,resim FROM Musteriler WHERE Tc_No = @TcNo";
                    SqlCommand musteriKomut = new SqlCommand(musteriSorgu, baglanti);
                    musteriKomut.Parameters.AddWithValue("@TcNo", tcNo);

                    SqlDataReader musteriReader = musteriKomut.ExecuteReader();
                    if (musteriReader.Read())
                    {
                        txtTelNo.Text = musteriReader["Telefon_Numarasi"].ToString();
                        txtMail.Text = musteriReader["Mail"].ToString();
                        txtEhliyetNo.Text = musteriReader["Ehliyet_No"].ToString();
                        txtEhliyetTarihi.Text = Convert.ToDateTime(musteriReader["Ehliyet_Tarihi"]).ToString("yyyy-MM-dd");
                        txtAdres.Text = musteriReader["Adres"].ToString();
                        if (musteriReader["resim"] != DBNull.Value && !string.IsNullOrEmpty(musteriReader["resim"].ToString()))
                        {
                            pictureBox3.Image = System.Drawing.Image.FromFile(musteriReader["resim"].ToString());
                        }
                        else
                        {
                            pictureBox3.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                        }
                    }

                    musteriReader.Close();
                    ClearTextBoxSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SözlesmeBilgileriniGoster(string tcNo)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    string sozlesmeSorgu = @"
                        SELECT [Tc_No], [Ad_Soyad], [Plaka], [Gun], [Kira_Sekli], [kira_Ucret], 
                               [Tutar], [Cikis_Tarih], [Dönüs_Tarih] 
                        FROM [AracKiralama].[dbo].[Satislar] 
                        WHERE Tc_No = @TcNo";
                    SqlCommand sozlesmeKomut = new SqlCommand(sozlesmeSorgu, baglanti);
                    sozlesmeKomut.Parameters.AddWithValue("@TcNo", tcNo);

                    SqlDataReader sozlesmeReader = sozlesmeKomut.ExecuteReader();
                    if (sozlesmeReader.Read())
                    {
                        txtTcNo.Text = sozlesmeReader["Tc_No"].ToString();
                        txtAdSoyad.Text = sozlesmeReader["Ad_Soyad"].ToString();
                        txtPlaka.Text = sozlesmeReader["Plaka"].ToString();
                        txtGün.Text = sozlesmeReader["Gun"].ToString();
                        txtKiraSekil.Text = sozlesmeReader["Kira_Sekli"].ToString();
                        txtKiraUcret.Text = sozlesmeReader["kira_Ucret"].ToString();
                        txtTutar.Text = sozlesmeReader["Tutar"].ToString();

                        if (sozlesmeReader["Cikis_Tarih"] != DBNull.Value)
                        {
                            DateTimeCikis.Value = Convert.ToDateTime(sozlesmeReader["Cikis_Tarih"]);
                        }

                        if (sozlesmeReader["Dönüs_Tarih"] != DBNull.Value)
                        {
                            DateTimeDönüs.Value = Convert.ToDateTime(sozlesmeReader["Dönüs_Tarih"]);
                        }

                        // Plakayı al ve araç bilgilerini çek
                        string plaka = sozlesmeReader["Plaka"].ToString();
                        AracBilgileriniGetir(plaka);
                    }

                    sozlesmeReader.Close();
                    ClearTextBoxSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AracBilgileriniGetir(string plaka)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                try
                {
                    baglanti.Open();

                    string aracSorgu = "SELECT Marka, Seri, Model, Renk, Kilometre,resim FROM Araclar WHERE Plaka = @Plaka";
                    SqlCommand aracKomut = new SqlCommand(aracSorgu, baglanti);
                    aracKomut.Parameters.AddWithValue("@Plaka", plaka);

                    SqlDataReader aracReader = aracKomut.ExecuteReader();
                    if (aracReader.Read())
                    {
                        txtMarka.Text = aracReader["Marka"].ToString();
                        txtSeri.Text = aracReader["Seri"].ToString();
                        txtModel.Text = aracReader["Model"].ToString();
                        txtRenk.Text = aracReader["Renk"].ToString();
                        txtKm.Text = aracReader["Kilometre"].ToString();
                        if (aracReader["resim"] != DBNull.Value && !string.IsNullOrEmpty(aracReader["resim"].ToString()))
                        {
                            pictureBox2.Image = System.Drawing.Image.FromFile(aracReader["resim"].ToString());
                        }
                        else
                        {
                            pictureBox2.Image = null; // Eğer resim yoksa PictureBox'ı temizle
                        }
                    }

                    aracReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearTextBoxSelection()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                }
            }

            this.ActiveControl = null;
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void btnPdf_Click(object sender, EventArgs e)
        {
            pdfMetot();

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
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

        public void pdfMetot()
        {
            iTextSharp.text.Document raporum = new iTextSharp.text.Document();
            PdfWriter.GetInstance(raporum, new FileStream("C:Deneme.Pdf", FileMode.Create));
            raporum.AddAuthor("Admin");
            raporum.AddCreationDate();
            raporum.AddTitle("AraC Sozlesmesi");
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
            infoCell.AddElement(new Paragraph($"TC : {txtTcNo.Text}"));
            infoCell.AddElement(new Paragraph($"Ad Soyad : {txtAdSoyad.Text}"));
            infoCell.AddElement(new Paragraph($"Telefon Numarasi : {txtTelNo.Text}"));
            infoCell.AddElement(new Paragraph($"E-Posta : {txtMail.Text}"));
            infoCell.AddElement(new Paragraph($"Ehliyet No : {txtEhliyetNo.Text}"));
            infoCell.AddElement(new Paragraph($"Ehliyet Tarihi : {txtEhliyetTarihi.Text}"));
            infoCell.AddElement(new Paragraph($"Adres : {txtAdres.Text}"));

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
            carInfoCell.AddElement(new Paragraph($"Plaka : {txtPlaka.Text}"));
            carInfoCell.AddElement(new Paragraph($"Marka : {txtMarka.Text}"));
            carInfoCell.AddElement(new Paragraph($"Seri : {txtSeri.Text}"));
            carInfoCell.AddElement(new Paragraph($"Model : {txtModel.Text}"));
            carInfoCell.AddElement(new Paragraph($"Renk : {txtRenk.Text}"));
            carInfoCell.AddElement(new Paragraph($"Kilometre : {txtKm.Text}"));

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
            raporum.Add(new Paragraph($"Kira Sekli : {txtKiraSekil.Text}"));
            raporum.Add(new Paragraph($"Gun Sayisi : {txtGün.Text}"));
            raporum.Add(new Paragraph($"Kira Ucreti : {txtKiraUcret.Text}"));
            raporum.Add(new Paragraph($"Tutar : {txtTutar.Text}"));
            raporum.Add(new Paragraph($"Cikis Tarihi : {DateTimeCikis.Value:yyyy-MM-dd}"));
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

        
    }
}

