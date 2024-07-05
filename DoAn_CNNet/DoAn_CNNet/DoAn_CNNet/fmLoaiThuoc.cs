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
    public partial class fmLoaiThuoc : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_loathuoc = new DataSet();
        public fmLoaiThuoc()
        {
            InitializeComponent();
            cnn = new SqlConnection("Data Source=LAPTOP-VKUMSO5B\\NGOCMI;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }

        private void fmLoaiThuoc_Load(object sender, EventArgs e)
        {
            string strSelect = "Select * from LOAITHUOC";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_loathuoc, "LOAITHUOC");
            grdKH.DataSource = ds_loathuoc.Tables["LOAITHUOC"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_loathuoc.Tables["LOAITHUOC"].Columns[0];
            ds_loathuoc.Tables["LOAITHUOC"].PrimaryKey = key;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaLoaiThuoc.Enabled = txtTenLoaiThuoc.Enabled = true;
            btnLuu.Enabled = true;
            if (txtMaLoaiThuoc.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Khoa");
                txtMaLoaiThuoc.Focus();
                return;
            }
            if (txtMaLoaiThuoc.Enabled == true)
            {
                DataRow insert_New = ds_loathuoc.Tables["LOAITHUOC"].NewRow();
                insert_New["MALOAITHUOC"] = txtMaLoaiThuoc.Text;
                insert_New["TENLOAITHUOC"] = txtTenLoaiThuoc.Text;
                ds_loathuoc.Tables["LOAITHUOC"].Rows.Add(insert_New);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            txtTenLoaiThuoc.Enabled = true;
            txtMaLoaiThuoc.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaLoaiThuoc.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Khoa");
                txtMaLoaiThuoc.Focus();
                return;
            }
            if (txtMaLoaiThuoc.Enabled == true)
            {
                DataRow insert_New = ds_loathuoc.Tables["LOAITHUOC"].NewRow();
                insert_New["MALOAITHUOC"] = txtMaLoaiThuoc.Text;
                insert_New["TENLOAITHUOC"] = txtTenLoaiThuoc.Text;
                ds_loathuoc.Tables["LOAITHUOC"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_loathuoc.Tables["LOAITHUOC"].Rows.Find(txtMaLoaiThuoc.Text);
                if (update_New != null)
                {
                    update_New["TENLOAITHUOC"] = txtTenLoaiThuoc.Text;
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_loathuoc, "LOAITHUOC");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataRow update_New = ds_loathuoc.Tables["LOAITHUOC"].Rows.Find(txtMaLoaiThuoc.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_loathuoc, "LOAITHUOC");
                MessageBox.Show("Thanh Cong");
            }
        }

        private void grdKH_SelectionChanged(object sender, EventArgs e)
        {
            btnSua.Enabled = btnXoa.Enabled = true;
        }
   }
}
