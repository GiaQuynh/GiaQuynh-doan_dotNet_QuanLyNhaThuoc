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
    public partial class fmNhanVien : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_nhanvien = new DataSet();
        public fmNhanVien()
        {
            InitializeComponent();
            cnn = new SqlConnection("Data Source=LAPTOP-VKUMSO5B\\NGOCMI;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }
        void LoadDuLieuNhanVien()
        {
            string strSelect = "Select * from NHANVIEN";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_nhanvien, "NHANVIEN");
            grdKH.DataSource = ds_nhanvien.Tables["NHANVIEN"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_nhanvien.Tables["NHANVIEN"].Columns[0];
            ds_nhanvien.Tables["NHANVIEN"].PrimaryKey = key;
        }
        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadDuLieuNhanVien();
            txtMaNV.Enabled = txtTenNV.Enabled = txtSdt.Enabled = txtCCCD.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaNV.Enabled = txtTenNV.Enabled = txtSdt.Enabled = txtCCCD.Enabled = true;
            btnLuu.Enabled = true;
            if (txtMaNV.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Nhan Vien");
                txtMaNV.Focus();
                return;
            }
            if (txtMaNV.Enabled == true)
            {
                DataRow insert_New = ds_nhanvien.Tables["NHANVIEN"].NewRow();
                insert_New["MANV"] = txtMaNV.Text;
                insert_New["TENNV"] = txtTenNV.Text;
                insert_New["SDT"] = txtSdt.Text;
                insert_New["CCCD"] = txtCCCD.Text;
                ds_nhanvien.Tables["NHANVIEN"].Rows.Add(insert_New);
            }
            //txtMaNV.Focus();
            //txtMaNV.Clear();
            //txtTenNV.Clear();
            //txtSdt.Clear();
            //txtCCCD.Clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            txtTenNV.Enabled = true;
            txtSdt.Enabled = true;
            txtCCCD.Enabled = true;
            txtMaNV.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Nhan Vien");
                txtMaNV.Focus();
                return;
            }
            if (txtMaNV.Enabled == true)
            {
                DataRow insert_New = ds_nhanvien.Tables["NHANVIEN"].NewRow();
                insert_New["MANV"] = txtMaNV.Text;
                insert_New["TENNV"] = txtTenNV.Text;
                insert_New["SDT"] = txtSdt.Text;
                insert_New["CCCD"] = txtCCCD.Text;
                ds_nhanvien.Tables["NHANVIEN"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_nhanvien.Tables["NHANVIEN"].Rows.Find(txtMaNV.Text);
                if (update_New != null)
                {
                    update_New["TENNV"] = txtTenNV.Text;
                    update_New["SDT"] = txtSdt.Text;
                    update_New["CCCD"] = txtCCCD.Text;

                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_nhanvien, "NHANVIEN");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataTable dt_PhieuXuat = new DataTable();
                SqlDataAdapter da_PhieuXuat = new SqlDataAdapter("select * from PHIEUXUAT where MAKH = '" + txtMaNV.Text + "'", cnn);
                da_PhieuXuat.Fill(dt_PhieuXuat);
                if (dt_PhieuXuat.Rows.Count > 0)
                {
                    MessageBox.Show("Du Lieu dang su dung khong the xoa");
                    return;
                }
                DataRow update_New = ds_nhanvien.Tables["NHANVIEN"].Rows.Find(txtMaNV.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_nhanvien, "NHANVIEN");
                MessageBox.Show("Thanh Cong");
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            string searchTerm = txtSearch.Text.Trim();

            DataRow foundRow = ds_nhanvien.Tables["NHANVIEN"].Rows.Find(searchTerm);

            if (foundRow == null)
            {
                DataRow[] foundRows = ds_nhanvien.Tables["NHANVIEN"].Select("TENNV LIKE '%" + searchTerm + "%'");

                if (foundRows.Length == 0)
                {
                    MessageBox.Show("No matching records found.");
                    return;
                }

                foundRow = foundRows[0];
            }
            

            txtMaNV.Text = foundRow["MANV"].ToString();
            txtTenNV.Text = foundRow["TENNV"].ToString();
            txtSdt.Text = foundRow["SDT"].ToString();
            txtCCCD.Text = foundRow["CCCD"].ToString();

            grdKH.DataSource = ds_nhanvien.Tables["NHANVIEN"];
            (grdKH.DataSource as DataTable).DefaultView.RowFilter = "MANV = '" + txtMaNV.Text + "' OR TENNV LIKE '%" + txtTenNV.Text + "%'";

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
