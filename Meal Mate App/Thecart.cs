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
    public partial class The_cart : Form
    {
        private Form1 _form1;

        public The_cart(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
      //  SqlConnection conn = new SqlConnection(@" Server=DESKTOP-UABENI7\SQLSERVER;DataBase=mealmate ; Integrated Security=True");
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EKGE4HQ;Initial Catalog=mealmate;Integrated Security=True");
         void display()
        {
            try
            {
                if (_form1.textBox_UserID.Text == "")
                {
                    MessageBox.Show("عليك تسجيل الدخول ");


                 }
                else
                {

                    int UserID = Convert.ToInt32(_form1.textBox_UserID.Text);
                    conn.Open();
                    string query = @"SELECT 
                                    m.MealID, 
                                    m.MealName, 
                                    ci.Quantity, 
                                    m.MealPce, 
                                    (ci.Quantity * m.MealPce) AS TotalPrice
                                FROM 
                                    CartItems ci
                                JOIN 
                                    Carts c ON ci.CartID = c.CartID
                                JOIN 
                                    Meals m ON ci.MealID = m.MealID
                                WHERE 
                                    c.UserID = " + UserID + " AND c.state='قيد الانتظار'";

                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
            }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }   
        }

        int returncartid()
        {
            int cartId=-1;

            try
            {
                int UserID = Convert.ToInt32(_form1.textBox_UserID.Text);

                string checkCartQuery = "SELECT CartID FROM Carts WHERE UserID = @UserID AND state = 'قيد الانتظار'";
                SqlCommand checkCartCommand = new SqlCommand(checkCartQuery, conn);
                checkCartCommand.Parameters.AddWithValue("@UserID", UserID);

                SqlDataAdapter adapter = new SqlDataAdapter(checkCartCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    cartId = (int)dataTable.Rows[0]["CartID"];
                }

                }
            catch (SqlException ex)

            {
                MessageBox.Show(ex.Message);

            }
            return cartId;

        }
        private void The_cart_Load(object sender, EventArgs e)
        {
            display();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_form1.textBox_UserID.Text == "")
                {
                    MessageBox.Show("عليك تسجيل الدخول ");
                    return;
                }

                int UserID = Convert.ToInt32(_form1.textBox_UserID.Text);

                // تأكد من وجود سلة للمستخدم
                conn.Open();
                string getCartIdQuery = @"SELECT CartID FROM Carts WHERE UserID = @UserID AND state = 'قيد الانتظار'";
                SqlCommand getCartIdCommand = new SqlCommand(getCartIdQuery, conn);
                getCartIdCommand.Parameters.AddWithValue("@UserID", UserID);
                object result = getCartIdCommand.ExecuteScalar();
                if (result != null)
                {
                    int cartId = Convert.ToInt32(result);
                    
                    // تحديث حالة السلة إلى "قيد التنفيذ"
                    string updateCartStateQuery = @"UPDATE Carts SET state = 'قيد التنفيذ' WHERE CartID = @CartID";
                    SqlCommand updateCartStateCommand = new SqlCommand(updateCartStateQuery, conn);
                    updateCartStateCommand.Parameters.AddWithValue("@CartID", cartId);
                    updateCartStateCommand.ExecuteNonQuery();

                    MessageBox.Show("تم تأكيد إرسال السلة.");
                }
                else
                {
                    MessageBox.Show("لا توجد سلة قيد الانتظار.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            decimal total = 0;

            if (dataGridView1.Rows.Count > 1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["TotalPrice"].Value != null)
                    {
                        total += Convert.ToDecimal(row.Cells["TotalPrice"].Value);
                    }
                }

                label2.Text = "الإجمالي: " + total.ToString("C");
            }
            else
            {
                MessageBox.Show("لا توجد عناصر في السلة .");
            }
        




    }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int newQuantity = Convert.ToInt32(numericUpDown1.Value);
                    if (returncartid() == -1)
                    {
                        MessageBox.Show("لا توجد سله");
                        return;
                    }
                    else
                    {
                        int cartId = returncartid();
                        string mealName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        conn.Open();
                        string updateQuantityQuery = "UPDATE CartItems SET Quantity = @Quantity WHERE CartID = @CartID AND MealID = (SELECT MealID FROM Meals WHERE MealName = @MealName)";
                        SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, conn);
                        updateQuantityCommand.Parameters.AddWithValue("@Quantity", newQuantity);
                        updateQuantityCommand.Parameters.AddWithValue("@CartID", cartId);
                        updateQuantityCommand.Parameters.AddWithValue("@MealName", mealName);
                        updateQuantityCommand.ExecuteNonQuery();
                        MessageBox.Show("تم التعديل ");
                        display();
                        conn.Close();
                    }
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("يرجى اختيار وجبة ");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    if (returncartid() == -1)
                    {
                        MessageBox.Show("لا توجد سله");
                        return;
                    }
                    else
                    {
                        int cartId = returncartid();
                        string mealName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        conn.Open();
                        string updateQuantityQuery = "DELETE FROM CartItems  WHERE CartID = @CartID AND MealID = (SELECT MealID FROM Meals WHERE MealName = @MealName)";
                        SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, conn);
                        updateQuantityCommand.Parameters.AddWithValue("@CartID", cartId);
                        updateQuantityCommand.Parameters.AddWithValue("@MealName", mealName);
                        updateQuantityCommand.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("تم الحذف ");
                        display();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("يرجى اختيار وجبة ");

            }
        }
    }
}
