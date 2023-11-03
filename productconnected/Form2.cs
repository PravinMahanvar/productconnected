using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;


namespace productconnected
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;
        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string qry = "select * from category";
                da = new SqlDataAdapter(qry, con);
                ds = new DataSet();
                da.Fill(ds, "Catg");
                cmb.DataSource = ds.Tables["Catg"];
                cmb.DisplayMember = "cname";
                cmb.ValueMember = "cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet GetProducts()
        {
            string qry = "select * from product";
            // assign the query
            da = new SqlDataAdapter(qry, con);
            // when app load the in DataSet, we need to manage the PK also
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // SCB will track the DataSet & update quries to the DataAdapter
            builder = new SqlCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds, "Product");// this name given to the DataSet table
            return ds;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // create new row to add recrod
                DataRow row = ds.Tables["Product"].NewRow();
                // assign value to the row
                row["pname"] = txtname.Text;
                row["price"] = txtprice.Text;
                row["cid"] = cmb.SelectedValue;
                // attach this row in DataSet table
                ds.Tables["Product"].Rows.Add(row);
                // update the changes from DataSet to DB
                int result = da.Update(ds.Tables["Product"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {

            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    row["ename"] = txtname.Text;
                    row["price"] = txtprice.Text;
                    row["cid"] = cmb.SelectedValue;
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    // delete the current row from DataSet table
                    row.Delete();
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record deleted");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from product p inner join category c on c.cid=p.cid ";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "prod");
                dataGridView1.DataSource = ds.Tables["prod"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             txtid.Text= dataGridView1.CurrentRow.Cells["Pid"].Value.ToString();
            txtname.Text = dataGridView1.CurrentRow.Cells["name"].Value.ToString();
            txtprice.Text= dataGridView1.CurrentRow.Cells["price"].Value.ToString();
            cmb.Text= dataGridView1.CurrentRow.Cells["name"].Value.ToString() ;


        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from product p inner join category c on c.cid=p.cid ";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "prod");
                //find method can only seach the data if PK is applied in the DataSet table
                DataRow row = ds.Tables["prod"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    txtname.Text = row["pname"].ToString();
                    txtprice.Text = row["price"].ToString();
                    cmb.Text = row["cname"].ToString();
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
        }
    }
    
}


