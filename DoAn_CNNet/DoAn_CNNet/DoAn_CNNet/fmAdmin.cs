using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_CNNet
{
    public partial class fmAdmin : Form
    {
        public fmAdmin()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem_DangKy_Click(object sender, EventArgs e)
        {
            fmDangKy dn = new fmDangKy();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripMenuItem_DangNhap_Click(object sender, EventArgs e)
        {
            fmDangNhap dn = new fmDangNhap();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripMenuItem_QuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            fmAccount dn = new fmAccount();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripDropDownButton_QLNV_Click(object sender, EventArgs e)
        {
            fmNhanVien dn = new fmNhanVien();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripMenuItem_ThongTinThuoc_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem_NhapThuoc_Click(object sender, EventArgs e)
        {
            fmHoaDonNhap dn = new fmHoaDonNhap();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripMenuItem_BanThuoc_Click(object sender, EventArgs e)
        {
            fmHoaDonXuat dn = new fmHoaDonXuat();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripDropDownButton_QLNCC_Click(object sender, EventArgs e)
        {
            fmNhaCungCap dn = new fmNhaCungCap();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripDropDownButton_KHACH_Click(object sender, EventArgs e)
        {
            fmKhachHang dn = new fmKhachHang();
            dn.ShowDialog();
            this.Close();
        }

        private void toolStripDropDownButton_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
