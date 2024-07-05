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
    public partial class fmLoThuoc : Form
    {
        SqlConnection cnn;
        SqlDataAdapter da;
        DataSet ds_lothuoc = new DataSet();
        public fmLoThuoc()
        {
            InitializeComponent();
            cnn = new SqlConnection("Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }
        void LoadDuLieu()
        {
            string strSelect = "Select * from LOTHUOC";
            da = new SqlDataAdapter(strSelect, cnn);
            da.Fill(ds_lothuoc, "LOTHUOC");
            grdKH.DataSource = ds_lothuoc.Tables["LOTHUOC"];
            DataColumn[] key = new DataColumn[1];
            key[0] = ds_lothuoc.Tables["LOTHUOC"].Columns[0];
            ds_lothuoc.Tables["LOTHUOC"].PrimaryKey = key;
        }
        private void fmLoThuoc_Load(object sender, EventArgs e)
        {
            LoadDuLieu();
            txtMALOTHUOC.Enabled = txtMAHANGSX.Enabled = dtpNgaySX.Enabled = dtpHanSD.Enabled = false;
            btnSua.Enabled = btnXoa.Enabled = btnLuu.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMALOTHUOC.Enabled = txtMAHANGSX.Enabled = dtpNgaySX.Enabled = dtpHanSD.Enabled = true;
            btnLuu.Enabled = true;
            if (txtMALOTHUOC.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma lo thuoc");
                txtMALOTHUOC.Focus();
                return;
            }
            if (txtMALOTHUOC.Enabled == true)
            {
                DataRow insert_New = ds_lothuoc.Tables["LOTHUOC"].NewRow();
                insert_New["MALOTHUOC"] = txtMALOTHUOC.Text;
                insert_New["MAHANGSX"] = txtMAHANGSX.Text;
                insert_New["NGAYSX"] = dtpNgaySX.Text;
                insert_New["HANSUDUNG"] = dtpHanSD.Text;
                ds_lothuoc.Tables["LOTHUOC"].Rows.Add(insert_New);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = true;
            txtMAHANGSX.Enabled = true;
            dtpNgaySX.Enabled = true;
            dtpHanSD.Enabled = true;
            txtMALOTHUOC.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMALOTHUOC.Text == string.Empty)
            {
                MessageBox.Show("Chua nhap Ma lo thuoc");
                txtMALOTHUOC.Focus();
                return;
            }
            if (txtMALOTHUOC.Enabled == true)
            {
                DataRow insert_New = ds_lothuoc.Tables["LOTHUOC"].NewRow();
                insert_New["MALOTHUOC"] = txtMALOTHUOC.Text;
                insert_New["MAHANGSX"] = txtMAHANGSX.Text;
                insert_New["NGAYSX"] = dtpNgaySX.Text;
                insert_New["HANSUDUNG"] = dtpHanSD.Text;
                ds_lothuoc.Tables["LOTHUOC"].Rows.Add(insert_New);
            }
            else
            {
                DataRow update_New = ds_lothuoc.Tables["LOTHUOC"].Rows.Find(txtMALOTHUOC.Text);
                if (update_New != null)
                {
                    update_New["MAHANGSX"] = txtMAHANGSX.Text;
                    update_New["NGAYSX"] = dtpNgaySX.Text;
                    update_New["HANSUDUNG"] = dtpHanSD.Text;

                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_lothuoc, "LOTHUOC");
                MessageBox.Show("Thanh Cong");
                btnLuu.Enabled = false;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co muon xoa", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                DataTable dt_Thuoc = new DataTable();
                SqlDataAdapter da_Thuoc = new SqlDataAdapter("select * from THUOC where MALOTHUOC = '" + txtMALOTHUOC.Text + "'", cnn);
                da_Thuoc.Fill(dt_Thuoc);
                if (dt_Thuoc.Rows.Count > 0)
                {
                    MessageBox.Show("Du Lieu dang su dung khong the xoa");
                    return;
                }
                DataRow update_New = ds_lothuoc.Tables["LOTHUOC"].Rows.Find(txtMALOTHUOC.Text);
                if (update_New != null)
                {
                    update_New.Delete();
                }
                SqlCommandBuilder cmb = new SqlCommandBuilder(da);
                da.Update(ds_lothuoc, "LOTHUOC");
                MessageBox.Show("Thanh Cong");
            }
        }

        private void grdKH_SelectionChanged(object sender, EventArgs e)
        {
            btnSua.Enabled = btnXoa.Enabled = true;
        }

    }
}
