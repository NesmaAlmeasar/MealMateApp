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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          if(textBox_UserID.Text == ""&& labelFirstName.Text=="")
            {
                button2.Visible = true;
            }
        }

        private void HideOtherForms()
        {
            List<Form> formsToHide = new List<Form>();

            foreach (Form form in Application.OpenForms)
            {
                if (form != this) 
                {
                    formsToHide.Add(form);
                }
            }

            foreach (Form form in formsToHide)
            {
                form.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HideOtherForms();
            loginuser login = new loginuser();
            login.Show();
            this.Hide();    
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            HideOtherForms();

            The_cart cart = new The_cart(this);
            cart.MdiParent = this;
            cart.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            HideOtherForms();

            About_us abutus = new About_us();
            abutus.MdiParent = this;
            abutus.Show();


        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            HideOtherForms();

            ShowMeals showMeals = new ShowMeals(this);
            showMeals.MdiParent = this;
            showMeals.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
