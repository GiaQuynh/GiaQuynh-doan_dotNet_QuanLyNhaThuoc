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
    public partial class fmKhachHang : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_khachhang = new DataSet();
        //DataTable dt = new DataTable();
        //DataTable dtcom = new DataTable();
        public fmKhachHang()
        {
            InitializeComponent();            
            cnn = new SqlConnection("Data Source=LAPTOP-VKUMSO5B\\NGOCMI;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }

        void LoadDuLieuKhachHang()
        {
            string strSelect = "Select * from KHACHHANG";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_khachhang, "KHACHHANG");
            grdKH.DataSource = ds_khachhang.Tables["KHACHHANG"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_khachhang.Tables["KHACHHANG"].Columns[0];
            ds_khachhang.Tables["KHACHHANG"].PrimaryKey = key;
        }

        void Databingding(DataTable pDT)
        {
            txtMaKH.DataBindings.Clear();
            txtTenKH.DataBindings.Clear();
            txtSdt.DataBindings.Clear();
            txtCCCD.DataBindings.Clear();

            txtMaKH.DataBindings.Add("Text", pDT, "MAKH");
            txtTenKH.DataBindings.Add("Text", pDT, "TENKHACHHANG");
            txtSdt.DataBindings.Add("Text", pDT, "SDT");
            txtCCCD.DataBindings.Add("Text", pDT, "CCCD");
        }

        void load_grid()
        {
            grdKH.DataSource = ds_khachhang.Tables[0];
            Databingding(ds_khachhang.Tables[0]);
        }

        private void fmKhachHang_Load(object sender, EventArgs e)
        {
            //load_grid();
            LoadDuLieuKhachHang();
            txtMaKH.Enabled = txtTenKH.Enabled = txtSdt.Enabled = txtCCCD.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
            //Databingding(ds_khachhang.Tables["KhachHang"]);
        }
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaKH.Enabled = txtTenKH.Enabled = txtSdt.Enabled = txtCCCD.Enabled = true;
            btnLuu.Enabled = true;
            //DataRow newrow = ds_khachhang.Tables[0].NewRow();
            //newrow["MAKH"] = txtMaKH.Text;
            //newrow["TENKHACHHANG"] = txtTenKH.Text;
            //newrow["SDT"] = txtSdt.Text;
            //newrow["CCCD"] = txtCCCD.Text;

            //ds_khachhang.Tables[0].Rows.Add(newrow);
            //da.Update(ds_khachhang, "KHACHHANG");

            if (txtMaKH.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Khoa");
                txtMaKH.Focus();
                return;
            }
            //if (txtTenKH.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap Ten Khach Hang");
            //    txtTenKH.Focus();
            //    return;
            //}
            //if (txtSdt.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap SDT");
            //    txtSdt.Focus();
            //    return;
            //}
            //if (txtCCCD.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap CCCD");
            //    txtCCCD.Focus();
            //    return;
            //}
            if (txtMaKH.Enabled == true)
            {
                DataRow insert_New = ds_khachhang.Tables["KHACHHANG"].NewRow();
                insert_New["MAKH"] = txtMaKH.Text;
                insert_New["TENKHACHHANG"] = txtTenKH.Text;
                insert_New["SDT"] = txtSdt.Text;
                insert_New["CCCD"] = txtCCCD.Text;
                ds_khachhang.Tables["KHACHHANG"].Rows.Add(insert_New);
            }

            //txtMaKH.Focus();
            //txtMaKH.Clear();
            //txtTenKH.Clear();
            //txtSdt.Clear();
            //txtCCCD.Clear();

            
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            //DataRow dr = ds_khachhang.Tables["KHACHHANG"].Rows.Find(txtMaKH.Text);
            //if (dr != null)
            //{
            //    dr["TENKHACHHANG"] = txtTenKH.Text;
            //}
            //SqlCommandBuilder cB = new SqlCommandBuilder(da);
            //da.Update(ds_khachhang, "KHACHHANG");
            btnLuu.Enabled = true;
            txtTenKH.Enabled = true;
            txtSdt.Enabled = true;
            txtCCCD.Enabled = true;
            txtMaKH.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma Khach Hang");
                txtMaKH.Focus();
                return;
            }
            //if (txtTenKH.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap Ten Khach Hang");
            //    txtTenKH.Focus();
            //    return;
            //}
            //if (txtSdt.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap SDT");
            //    txtSdt.Focus();
            //    return;
            //}
            //if (txtCCCD.Text == string.Empty)
            //{
            //    MessageBox.Show("Chua nhap CCCD");
            //    txtCCCD.Focus();
            //    return;
            //}
            if (txtMaKH.Enabled == true)
            {
                DataRow insert_New = ds_khachhang.Tables["KHACHHANG"].NewRow();
                insert_New["MAKH"] = txtMaKH.Text;
                insert_New["TENKHACHHANG"] = txtTenKH.Text;
                insert_New["SDT"] = txtSdt.Text;
                insert_New["CCCD"] = txtCCCD.Text;
                ds_khachhang.Tables["KHACHHANG"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_khachhang.Tables["KHACHHANG"].Rows.Find(txtMaKH.Text);
                if (update_New != null)
                {
                    update_New["TENKHACHHANG"] = txtTenKH.Text;
                    update_New["SDT"] = txtSdt.Text;
                    update_New["CCCD"] = txtCCCD.Text;

                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_khachhang, "KHACHHANG");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataTable dt_PhieuXuat = new DataTable();
                SqlDataAdapter da_PhieuXuat = new SqlDataAdapter("select * from PHIEUXUAT where MAKH = '" + txtMaKH.Text + "'", cnn);
                da_PhieuXuat.Fill(dt_PhieuXuat);
                if (dt_PhieuXuat.Rows.Count > 0)
                {
                    MessageBox.Show("Du Lieu dang su dung khong the xoa");
                    return;
                }
                DataRow update_New = ds_khachhang.Tables["KHACHHANG"].Rows.Find(txtMaKH.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_khachhang, "KHACHHANG");
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

            DataRow foundRow = ds_khachhang.Tables["KHACHHANG"].Rows.Find(searchTerm);

            if (foundRow == null)
            {
                DataRow[] foundRows = ds_khachhang.Tables["KHACHHANG"].Select("TENKHACHHANG LIKE '%" + searchTerm + "%'");

                if (foundRows.Length == 0)
                {
                    MessageBox.Show("No matching records found.");
                    return;
                }

                foundRow = foundRows[0];
            }

            txtMaKH.Text = foundRow["MAKH"].ToString();
            txtTenKH.Text = foundRow["TENKHACHHANG"].ToString();
            txtSdt.Text = foundRow["SDT"].ToString();
            txtCCCD.Text = foundRow["CCCD"].ToString();

            grdKH.DataSource = ds_khachhang.Tables["KHACHHANG"];
            (grdKH.DataSource as DataTable).DefaultView.RowFilter = "MAKH = '" + txtMaKH.Text + "' OR TENKHACHHANG LIKE '%" + txtTenKH.Text + "%'";

        }
    }
}
