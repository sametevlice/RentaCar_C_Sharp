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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-9FINMFA\SQLEXPRESS;Initial Catalog=AracKiralama;Integrated Security=True");


        //Geri tuşu
        private void button1_Click(object sender, EventArgs e)
        {
            Frm_ilkgiris ilkgrs = new Frm_ilkgiris();
            ilkgrs.Show();
            this.Hide();

        }

        private void signupBtn_Click(object sender, EventArgs e)
        {
            RegisterForm registerfrm = new RegisterForm();
            registerfrm.Show();
            this.Hide();
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';

        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (login_username.Text == "" || login_password.Text == "")
            {
                MessageBox.Show("Lütfen Tüm bilgileri doldurunuz!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        String selectData = "SELECT * FROM users WHERE username =@username AND password =@password";

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {

                            cmd.Parameters.AddWithValue("@username", login_username.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", login_password.Text.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 0.5)
                            {
                                MessageBox.Show("Giriş Başarılı! Lütfen Bekleyiniz..", "Information Message",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                Anasayfa anasayfaform = new Anasayfa();
                                anasayfaform.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı adı ya da Şifre Yanlış ", "Error Message",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }



                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Connection Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        connect.Close();
                    }


                }

            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Frm_SifremiUnuttum sfreunuttum = new Frm_SifremiUnuttum();
            sfreunuttum.Show();
            this.Hide();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
    }
}
