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
    public partial class fmHangSX : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_hangsx = new DataSet();
        public fmHangSX()
        {
            InitializeComponent();
            cnn = new SqlConnection("Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }
        void LoadDuLieu()
        {
            string strSelect = "Select * from HANGSANXUAT";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_hangsx, "HANGSANXUAT");
            grdKH.DataSource = ds_hangsx.Tables["HANGSANXUAT"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_hangsx.Tables["HANGSANXUAT"].Columns[0];
            ds_hangsx.Tables["HANGSANXUAT"].PrimaryKey = key;
        }
        private void fmHangSX_Load(object sender, EventArgs e)
        {
            LoadDuLieu();
            txtMAHANGSX.Enabled = txtTENHANGSX.Enabled = txtQG.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMAHANGSX.Enabled = txtTENHANGSX.Enabled = txtQG.Enabled = true;
            btnLuu.Enabled = true;
            if (txtMAHANGSX.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma hang san xuat");
                txtMAHANGSX.Focus();
                return;
            }
            if (txtMAHANGSX.Enabled == true)
            {
                DataRow insert_New = ds_hangsx.Tables["HANGSANXUAT"].NewRow();
                insert_New["MAHANGSX"] = txtMAHANGSX.Text;
                insert_New["TENHANGSX"] = txtTENHANGSX.Text;
                insert_New["QUOCGIA"] = txtQG.Text;
                ds_hangsx.Tables["HANGSANXUAT"].Rows.Add(insert_New);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            txtTENHANGSX.Enabled = true;
            txtQG.Enabled = true;
            txtMAHANGSX.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMAHANGSX.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Khach Hang");
                txtMAHANGSX.Focus();
                return;
            }
            if (txtMAHANGSX.Enabled == true)
            {
                DataRow insert_New = ds_hangsx.Tables["HANGSANXUAT"].NewRow();
                insert_New["MAHANGSX"] = txtMAHANGSX.Text;
                insert_New["TENHANGSX"] = txtTENHANGSX.Text;
                insert_New["QUOCGIA"] = txtQG.Text;
                ds_hangsx.Tables["HANGSANXUAT"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_hangsx.Tables["HANGSANXUAT"].Rows.Find(txtMAHANGSX.Text);
                if (update_New != null)
                {
                    update_New["TENHANGSX"] = txtTENHANGSX.Text;
                    update_New["QUOCGIA"] = txtQG.Text;
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_hangsx, "HANGSANXUAT");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataRow update_New = ds_hangsx.Tables["HANGSANXUAT"].Rows.Find(txtMAHANGSX.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_hangsx, "HANGSANXUAT");
                MessageBox.Show("Thanh Cong");
            }
        }

        private void grdKH_SelectionChanged(object sender, EventArgs e)
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

            DataRow foundRow = ds_hangsx.Tables["HANGSANXUAT"].Rows.Find(searchTerm);

            if (foundRow == null)
            {
                DataRow[] foundRows = ds_hangsx.Tables["HANGSANXUAT"].Select("TENHANGSX LIKE '%" + searchTerm + "%'");

                if (foundRows.Length == 0)
                {
                    MessageBox.Show("No matching records found.");
                    return;
                }

                foundRow = foundRows[0];
            }
            
            txtMAHANGSX.Text = foundRow["MAHANGSX"].ToString();
            txtTENHANGSX.Text = foundRow["TENHANGSX"].ToString();
            txtQG.Text = foundRow["QUOCGIA"].ToString();

            grdKH.DataSource = ds_hangsx.Tables["HANGSANXUAT"];
            (grdKH.DataSource as DataTable).DefaultView.RowFilter = "MAHANGSX = '" + txtMAHANGSX.Text + "' OR TENHANGSX LIKE '%" + txtTENHANGSX.Text + "%'";
        }
    }
}
