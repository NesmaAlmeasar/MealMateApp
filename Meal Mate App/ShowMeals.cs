using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meal_Mate_App
{
    public partial class ShowMeals : Form
    {
        private Form1 _form1;

        public ShowMeals(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
        }


        //  SqlConnection conn = new SqlConnection(@" Server=DESKTOP-UABENI7\SQLSERVER;DataBase=mealmate ; Integrated Security=True");
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-EKGE4HQ;Initial Catalog=mealmate;Integrated Security=True");
        private string selectedCategory = string.Empty;
        private string selectedRestriction = string.Empty;

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ShowMeals_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT RestrictionName FROM DietaryRestrictions";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    menuStrip1.Items.Add(
                    
                        Text = reader["RestrictionName"].ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
            finally
            {
                conn.Close(); // تأكد من إغلاق الاتصال
            }

            try
            {
                conn.Open();
                string query = "SELECT CategoryName FROM FoodCategories";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    menuStrip2.Items.Add(

                        Text = reader["CategoryName"].ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }
            finally
            {
                conn.Close(); // تأكد من إغلاق الاتصال
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LoadMeals()
        {
            string query;

            if (string.IsNullOrEmpty(selectedCategory) || string.IsNullOrEmpty(selectedRestriction))
            {
                 query = "SELECT m.MealName, m.Description, m.MealPce FROM Meals m " +
                                  "INNER JOIN FoodCategories fc ON m.CategoryID = fc.CategoryID " +
                                  "INNER JOIN MealRestrictions mr ON m.MealID = mr.MealID " +
                                  "INNER JOIN DietaryRestrictions dr ON mr.RestrictionID = dr.RestrictionID ";
            }
            else if (selectedCategory == "الكل" && selectedRestriction == "مناسب للجميع")
            {
                query = "SELECT m.MealName, m.Description, m.MealPce FROM Meals m " +
                                  "INNER JOIN FoodCategories fc ON m.CategoryID = fc.CategoryID " +
                                  "INNER JOIN MealRestrictions mr ON m.MealID = mr.MealID " +
                                  "INNER JOIN DietaryRestrictions dr ON mr.RestrictionID = dr.RestrictionID " ;

            }
            else if (selectedCategory == "الكل")
            {
                 query = "SELECT m.MealName, m.Description, m.MealPce FROM Meals m " +
                                  "INNER JOIN FoodCategories fc ON m.CategoryID = fc.CategoryID " +
                                  "INNER JOIN MealRestrictions mr ON m.MealID = mr.MealID " +
                                  "INNER JOIN DietaryRestrictions dr ON mr.RestrictionID = dr.RestrictionID " +
                                  "WHERE  dr.RestrictionName = @RestrictionName";
            }
            else if (selectedRestriction == "مناسب للجميع")
            {
                 query = "SELECT m.MealName, m.Description, m.MealPce FROM Meals m " +
                                  "INNER JOIN FoodCategories fc ON m.CategoryID = fc.CategoryID " +
                                  "INNER JOIN MealRestrictions mr ON m.MealID = mr.MealID " +
                                  "INNER JOIN DietaryRestrictions dr ON mr.RestrictionID = dr.RestrictionID " +
                                  "WHERE fc.CategoryName = @CategoryName ";
            }
            else
            {
                 query = "SELECT m.MealName, m.Description, m.MealPce FROM Meals m " +
                                  "INNER JOIN FoodCategories fc ON m.CategoryID = fc.CategoryID " +
                                  "INNER JOIN MealRestrictions mr ON m.MealID = mr.MealID " +
                                  "INNER JOIN DietaryRestrictions dr ON mr.RestrictionID = dr.RestrictionID " +
                                  "WHERE fc.CategoryName = @CategoryName AND dr.RestrictionName = @RestrictionName";
            }
                try
                {
                    conn.Open();
                   
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryName", selectedCategory);
                    cmd.Parameters.AddWithValue("@RestrictionName", selectedRestriction);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in menuStrip2.Items)
            {
                item.BackColor = SystemColors.Menu; // يمكنك استخدام أي لون تفضله
            }

            selectedCategory = e.ClickedItem.Text;
            e.ClickedItem.BackColor = Color.Blue;
                LoadMeals();
            
          
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.BackColor = SystemColors.Menu; 
            }
            selectedRestriction = e.ClickedItem.Text;
            e.ClickedItem.BackColor = Color.Yellow;
            LoadMeals();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    if (_form1.textBox_UserID.Text == "")
                    {
                        MessageBox.Show("عليك تسجيل الدخول ");
                        return;
                    }

                    int UserID = Convert.ToInt32(_form1.textBox_UserID.Text);
                    string mealName = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    int quantity = Convert.ToInt32(numericUpDown1.Value); 

                    conn.Open();
                    int cartId;
                    //////////////////////////////////////////////////////////////////////////
                    // تحقق  إذا كانت هناك سلة قيد الانتظار للمستخدماو انشاء سلة 

                    // تحقق  إذا كانت هناك سلة قيد الانتظار للمستخدم
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
                    else
                    {
                        // إذا لم تكن هناك سلة قيد الانتظارننشئ ساه جديدة 
                        
                            int CartID = (int)(DateTime.Now.Ticks % int.MaxValue);
                            // إنشاء سلة جديدة
                            string createCartQuery = "INSERT INTO Carts (CartID, UserID, state, CreatedAt) OUTPUT INSERTED.CartID VALUES (@CartID ,@UserID, 'قيد الانتظار', @CreatedAt)";
                            SqlCommand createCartCommand = new SqlCommand(createCartQuery, conn);
                            createCartCommand.Parameters.AddWithValue("@UserID", UserID);
                            createCartCommand.Parameters.AddWithValue("@CartID", CartID);
                            createCartCommand.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                            int insertedCartId = (int)createCartCommand.ExecuteScalar();
                            cartId = CartID;
                        
                    }

                    //////////////////////////////////////////////////////////////////////////
                    ///                    // إضافة العنصر إلى السلة

                    string checkItemQuery = "SELECT Quantity FROM CartItems WHERE CartID = @CartID AND MealID = (SELECT MealID FROM Meals WHERE MealName = @MealName)";
                    SqlCommand checkItemCommand = new SqlCommand(checkItemQuery, conn);
                    checkItemCommand.Parameters.AddWithValue("@CartID", cartId);
                    checkItemCommand.Parameters.AddWithValue("@MealName", mealName);

                    ///                   اذا العنصر موجود في السيه نزيد الكمية فقط 

                    SqlDataAdapter adaptercheckItemCommand = new SqlDataAdapter(checkItemCommand);
                    DataTable dataTablecheckItemCommand = new DataTable();
                    adaptercheckItemCommand.Fill(dataTablecheckItemCommand);
                    if (dataTablecheckItemCommand.Rows.Count > 0)
                    {
                        int existingQuantity = (int)dataTablecheckItemCommand.Rows[0]["Quantity"]; ;
                        quantity = Convert.ToInt32(numericUpDown1.Value);
                        int newQuantity = existingQuantity + quantity;

                        string updateQuantityQuery = "UPDATE CartItems SET Quantity = @Quantity WHERE CartID = @CartID AND MealID = (SELECT MealID FROM Meals WHERE MealName = @MealName)";
                        SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, conn);
                        updateQuantityCommand.Parameters.AddWithValue("@Quantity", newQuantity);
                        updateQuantityCommand.Parameters.AddWithValue("@CartID", cartId);
                        updateQuantityCommand.Parameters.AddWithValue("@MealName", mealName);
                        updateQuantityCommand.ExecuteNonQuery();

                        MessageBox.Show("تم زيادة كمية العنصر في السلة.");

                    }
                    else
                    {
                        int CartItemID = (int)(DateTime.Now.Ticks % int.MaxValue);

                        string addItemQuery = "INSERT INTO CartItems (CartItemID, CartID, MealID, Quantity) VALUES (@CartItemID,@CartID, (SELECT MealID FROM Meals WHERE MealName = @MealName), @Quantity)";
                        SqlCommand addItemCommand = new SqlCommand(addItemQuery, conn);
                        addItemCommand.Parameters.AddWithValue("@CartItemID", CartItemID);
                        addItemCommand.Parameters.AddWithValue("@CartID", cartId);
                        addItemCommand.Parameters.AddWithValue("@MealName", mealName);
                        addItemCommand.Parameters.AddWithValue("@Quantity", quantity);
                        addItemCommand.ExecuteNonQuery();

                        MessageBox.Show("تم إضافة العنصر إلى السلة.");
                    }

                      
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ: " + ex.Message);
                }
                finally
                {
                    conn.Close(); // تأكد من إغلاق الاتصال
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string mealname = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                label1.Text = mealname;
                label2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

                try
                {
                    conn.Open();
                    string query = "SELECT photo FROM Meals WHERE MealName='"+mealname+"' ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    try
                    {
                        string potopath = (string)dataTable.Rows[0]["photo"];

                        pictureBox1.Image = Image.FromFile(potopath);
                    }
                    catch 
                    {
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Dell\Pictures\Screenshots\mealmate.png");
                    }   

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("خطأ: " + ex.Message);
                }
                finally
                {
                    conn.Close(); // تأكد من إغلاق الاتصال
                }

            }
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
