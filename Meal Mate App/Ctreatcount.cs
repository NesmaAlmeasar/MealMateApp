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


namespace Meal_Mate_App
{
    public partial class Ctreatcount : Form
    {
        public Ctreatcount()
        {
            InitializeComponent();
        }
        //   SqlConnection conn = new SqlConnection(@"Data Source=ABDULMAJEEDSPC\SQLSERVER;Initial Catalog=mealmate;Integrated Security=True");
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EKGE4HQ;Initial Catalog=mealmate;Integrated Security=True");
        private void Ctreatcount_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (textBoxphone.Text == "")
            {
                MessageBox.Show("يرجى ادخال جميع البيانات ");

            }
            else if (password.Text != confPassword.Text)
            {
                MessageBox.Show("كلمة المرور غيرمطابقة  ");

            }
            else
            {

                int UserID = int.Parse(textBoxphone.Text);
                string firstName = FirstName.Text;
                string lastName = LastName.Text;
                string Email = email.Text;
                string Password = password.Text;


                conn.Open();

                string query = "INSERT INTO Users (UserID, FirstName, LastName, Email, Password) VALUES (@UserID,@FirstName, @LastName, @Email, @Password)";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("تم إنشاء الحساب بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                        textBoxphone.Clear();
                        FirstName.Clear();
                        LastName.Clear();
                        email.Clear();
                        password.Clear();
                        confPassword.Clear();
                        loginuser log = new loginuser();
                        log.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ أثناء إنشاء الحساب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            loginuser log = new loginuser();
            log.Show();
            this.Hide();
        }
    }
}