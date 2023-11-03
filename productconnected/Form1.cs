using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;


namespace productconnected
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Category> list = new List<Category>();

                string qry = "Select * from Category";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category cat = new Category();
                        cat.cid = Convert.ToInt32(reader["cid"]);
                        cat.name = reader["name"].ToString();
                        list.Add(cat);

                    }
                }
                // display dname & on selection of dname we need did
                cmb.DataSource = list;
                cmb.DisplayMember = "name";
                cmb.ValueMember = "cid";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into product values(@name,@price,@cid)";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtname.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtprice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmb.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllProducts();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Product set name=@name,price=@price,cid=@cid where pid=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtname.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtprice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmb.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllProducts();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Product where pid=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllProducts();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.name from Product p inner join category c on c.cid = p.cid where p.pid=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtname.Text = reader["name"].ToString();
                        txtprice.Text = reader["Price"].ToString();
                        cmb.Text = reader["name"].ToString();

                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetAllProducts()
        {
            string qry = "select p.*, c.name from Product p inner join category c on c.cid = p.cid";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();

        }
        private void ClearFields()
        {
            txtid.Clear();
            txtname.Clear();
            txtprice.Clear();
            cmb.ResetText();
        }
    }
}
