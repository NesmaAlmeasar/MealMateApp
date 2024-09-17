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
    public partial class loginuser : Form
    {
        public loginuser()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Ctreatcount newacount = new Ctreatcount();  
            newacount.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //   SqlConnection conn = new SqlConnection(@"Data Source=ABDULMAJEEDSPC\SQLSERVER;Initial Catalog=mealmate;Integrated Security=True");
            //SqlConnection conn = new SqlConnection(@" Server=DESKTOP-UABENI7\SQLSERVER;DataBase=mealmate ; Integrated Security=True");
            SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EKGE4HQ;Initial Catalog=mealmate;Integrated Security=True");
            conn.Open();

            string Email = email.Text;
            string Password = password.Text;
            string query = "SELECT  UserID ,FirstName FROM Users WHERE Email = @Email AND Password = @Password";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", Password);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0) 
                {
                    int userId = (int)dataTable.Rows[0]["UserID"]; 

                    Form1 form = new Form1();
                    form.labelFirstName.Text = (string)dataTable.Rows[0]["FirstName"];
                    form.textBox_UserID.Text = Convert.ToString( userId);
                    MessageBox.Show("تسجيل الدخول بنجاح "+ userId, "تم", MessageBoxButtons.OK);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("البريد الإلكتروني أو كلمة المرور غير صحيحة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginuser_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

            Form1 form = new Form1();
            form.labelFirstName.Text = "";
            form.textBox_UserID.Text = "";
            form.Show();
            this.Hide();

        }
    }
}
