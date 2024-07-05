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

namespace DoAn_CNNet
{
    public partial class fmAccount : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_account = new DataSet();
        public fmAccount()
        {
            InitializeComponent();
            cnn = new SqlConnection("Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }
        void LoadDuLieu()
        {
            string strSelect = "Select * from accounts";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_account, "accounts");
            dataGridView1.DataSource = ds_account.Tables["accounts"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_account.Tables["accounts"].Columns[0];
            ds_account.Tables["accounts"].PrimaryKey = key;
        }
        private void fmAccount_Load(object sender, EventArgs e)
        {
            LoadDuLieu();
            txtID.Enabled = txtUser.Enabled = txtPass.Enabled = txtStatus.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtID.Enabled = txtUser.Enabled = txtPass.Enabled = txtStatus.Enabled = true;
            btnLuu.Enabled = true;
            if (txtID.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap ID");
                txtID.Focus();
                return;
            }
            if (txtID.Enabled == true)
            {
                DataRow insert_New = ds_account.Tables["accounts"].NewRow();
                insert_New["ID"] = txtID.Text;
                insert_New["USERNAME"] = txtUser.Text;
                insert_New["PASSWORD"] = txtPass.Text;
                insert_New["STATUS"] = txtStatus.Text;
                ds_account.Tables["accounts"].Rows.Add(insert_New);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            txtStatus.Enabled = true;
            txtUser.Enabled = true;
            txtPass.Enabled = true;
            txtID.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtID.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap ID");
                txtID.Focus();
                return;
            }
            if (txtID.Enabled == true)
            {
                DataRow insert_New = ds_account.Tables["accounts"].NewRow();
                insert_New["ID"] = txtID.Text;
                insert_New["USERNAME"] = txtUser.Text;
                insert_New["PASSWORD"] = txtPass.Text;
                insert_New["STATUS"] = txtStatus.Text;
                ds_account.Tables["accounts"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_account.Tables["accounts"].Rows.Find(txtID.Text);
                if (update_New != null)
                {
                    update_New["USERNAME"] = txtUser.Text;
                    update_New["PASSWORD"] = txtPass.Text;
                    update_New["STATUS"] = txtStatus.Text;

                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_account, "accounts");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataRow update_New = ds_account.Tables["accounts"].Rows.Find(txtID.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_account, "accounts");
                MessageBox.Show("Thanh Cong");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSua.Enabled = btnXoa.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            string searchTerm = txtSearch.Text.Trim();

            DataRow foundRow = ds_account.Tables["accounts"].Rows.Find(searchTerm);

            if (foundRow == null)
            {
                DataRow[] foundRows = ds_account.Tables["accounts"].Select("TENKHACHHANG LIKE '%" + searchTerm + "%'");

                if (foundRows.Length == 0)
                {
                    MessageBox.Show("No matching records found.");
                    return;
                }

                foundRow = foundRows[0];
            }

            txtID.Text = foundRow["ID"].ToString();
            txtUser.Text = foundRow["USERNAME"].ToString();
            txtPass.Text = foundRow["PASSWORD"].ToString();
            txtStatus.Text = foundRow["STATUS"].ToString();

            dataGridView1.DataSource = ds_account.Tables["accounts"];
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "id = '" + txtID.Text + "' OR username LIKE '%" + txtUser.Text + "%'";
        }

    }
}