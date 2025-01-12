using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class Frm_SifremiUnuttum : Form
    {
        // Sabit kullanıcı bilgileri
        private readonly string dogruKullaniciAdi = "admin"; // Kullanıcı adı
        private readonly string dogruMail = "sametevlicetw@gmail.com"; // Mail adresi
        private readonly string sifre = "1234"; // Kullanıcı şifresi

        public Frm_SifremiUnuttum()
        {
            InitializeComponent();
        }

        private void btton_gonder_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı ve mail doğrulama
            if (txt_kad.Text == dogruKullaniciAdi && txt_mail.Text == dogruMail)
            {
                try
                {
                    SmtpClient smtpserver = new SmtpClient();
                    MailMessage mail = new MailMessage();
                    string tarih = DateTime.Now.ToLongDateString();
                    string mailadresin = "sametevlicetw@gmail.com"; // Gönderici mail adresi
                    string mailsifren = "krzj arba crpu mcku"; // Buraya aldığınız 16 haneli şifreyi yapıştırın
                    string smptsrvr = "smtp.gmail.com"; // SMTP sunucu
                    string kime = dogruMail; // Alıcı mail adresi
                    string konu = "Şifre Hatırlatma Talebi"; // Mail konusu
                    string yazi = $"Sayın Kullanıcı,\n\n{tarih} tarihinde şifre hatırlatma talebinde bulundunuz.\n\n" +
                                  $"Şifreniz: {sifre}\n\nİyi günler."; // Mail içeriği

                    smtpserver.Credentials = new NetworkCredential(mailadresin, mailsifren);
                    smtpserver.Port = 587;
                    smtpserver.Host = smptsrvr;
                    smtpserver.EnableSsl = true;

                    mail.From = new MailAddress(mailadresin);
                    mail.To.Add(kime);
                    mail.Subject = konu;
                    mail.Body = yazi;

                    smtpserver.Send(mail);
                    MessageBox.Show("Şifreniz, e-posta adresinize gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Mail gönderimi sırasında bir hata oluştu: " + hata.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya e-posta bilgileri hatalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoginForm lform = new LoginForm();
            lform.Show();
            this.Hide();
        }
    }
}
