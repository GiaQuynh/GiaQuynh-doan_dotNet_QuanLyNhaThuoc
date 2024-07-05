using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace DoAn_CNNet
{
    public partial class fmHoaDonXuat : Form
    {
        SqlConnection conn;
        public fmHoaDonXuat()
        {
            InitializeComponent();
            conn = new SqlConnection(@"Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }

        private void txbCMTND_TextChanged(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            if (ctr.Text.Length > 12)
            {
                MessageBox.Show("Nhập dữ liệu không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ctr.Text.Length > 0 && !Char.IsDigit(ctr.Text[ctr.Text.Length - 1]))
            {
                this.errorProvider1.SetError(ctr, "This is not avalid number");
                MessageBox.Show("Nhập dữ liệu không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                this.errorProvider1.Clear();
        }

        private void txbSDT_TextChanged(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            if (ctr.Text.Length > 10)
            {
                MessageBox.Show("Nhập dữ liệu không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ctr.Text.Length > 0 && !Char.IsDigit(ctr.Text[ctr.Text.Length - 1]))
            {
                this.errorProvider1.SetError(ctr, "This is not avalid number");
                MessageBox.Show("Nhập dữ liệu không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                this.errorProvider1.Clear();
        }
        void load_cbo_thuoc()
        {
            DataSet ds = new DataSet();
            string sql = "select *from THUOC";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "thuoc");
            comboBoxTenThuoc.DataSource = ds.Tables["thuoc"];
            comboBoxTenThuoc.DisplayMember = "TENTHUOC";
            comboBoxTenThuoc.ValueMember = "MATHUOC";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbTen.Text.Trim().Length == 0 || txbCMTND.Text.Trim().Length==0|| txbSDT.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn chưa nhập đủ thông tin khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                txt_maKH.Text = GenerateMaKH();
                string sql = "insert into KHACHHANG VALUES(@MAKH, @TENKH,@SDT,@CMND)";
                using (SqlCommand cmd= new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MAKH", txt_maKH.Text);
                    cmd.Parameters.AddWithValue("@TENKH", txbTen.Text);
                    cmd.Parameters.AddWithValue("@SDT", txbSDT.Text);
                    cmd.Parameters.AddWithValue("@CMND", txbCMTND.Text);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                MessageBox.Show("Đã thêm thông tin khách hàng thành công!", "Thông báo!",MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi", ex.Message);
            }
        }
        private string GenerateMaKH()
        {
            // Hàm này để sinh mã KH  mới, bạn có thể tùy chỉnh theo nhu cầu

            string pn_max = GetNextIDFromDatabase("KHACHHANG", "MAKH");
            int soPN = GetNextID(pn_max);
            return "KH0" + soPN;
        }
        private string GenerateMaPX()
        {
            // Hàm này để sinh mã phiếu XUAT mới, bạn có thể tùy chỉnh theo nhu cầu

            string pn_max = GetNextIDFromDatabase("PHIEUXUAT", "MAPX");
            int soPN = GetNextID(pn_max);
            return "PX0" + soPN;
        }
        int GetNextID(string MAKH)
        {
            // Sử dụng biểu thức chính quy để tách số từ chuỗi
            string pattern = @"\d+";
            Match match = Regex.Match(MAKH, pattern);

            if (match.Success)
            {
                int currentID = int.Parse(match.Value);
                return currentID + 1;
            }

            return 1; // Trong trường hợp không tìm thấy số, trả về giá trị mặc định
        }
        private string GetNextIDFromDatabase(string tableName, string idColumnName)
        {
            // Hàm này để lấy số tự tăng tiếp theo từ cơ sở dữ liệu
            // Bạn cần thay đổi nó phù hợp với cơ sở dữ liệu của bạn
            string sqlQuery = "SELECT MAX(" + idColumnName + ") FROM " + tableName;

            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }

                    return null; // Trường hợp đầu tiên
                }
            }

        }

        private void btn_xoaKH_Click(object sender, EventArgs e)
        {
            txbTen.Text = "";
            txbSDT.Text = "";
            txbCMTND.Text = "";
            txt_maKH.Text = "";
        }

        private void fmHoaDonXuat_Load(object sender, EventArgs e)
        {
            load_cbo_thuoc();
        }

        private void comboBoxTenThuoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maThuoc = comboBoxTenThuoc.SelectedValue.ToString();
            string sql = "select THUOC.MATHUOC, THANHPHAN,CONGDUNG,DONGIA from THUOC,CHITIETPHIEUNHAP WHERE THUOC.MATHUOC =CHITIETPHIEUNHAP.MATHUOC AND THUOC.MATHUOC= @MA";
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MA", maThuoc);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txt_MaThuoc.Text = dr["MATHUOC"].ToString();
                        txt_CongDung.Text = dr["CONGDUNG"].ToString();
                        txt_ThanhPhan.Text = dr["THANHPHAN"].ToString();
                        txt_DonGia.Text = dr["DONGIA"].ToString();
                    }
                   
                }
               
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        bool kt_maNV(string ma)
        {
            conn.Open();
            string sql = "SELECT COUNT(*) FROM NHANVIEN WHERE MANV = @MaNV";
            using( SqlCommand cmd= new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MANV", ma);
                if ((int)cmd.ExecuteScalar() == 1)
                    return true;
            }
            conn.Close();
            return false;
        }

        private void txt_SoLuongMua_TextChanged(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            if (ctr.Text.Length > 0 && !Char.IsDigit(ctr.Text[ctr.Text.Length - 1]))
            {
                this.errorProvider1.SetError(ctr, "This is not avalid number");
                MessageBox.Show("Nhập dữ liệu không phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                this.errorProvider1.Clear();
        }

        private void buttonThemVaoGio_Click(object sender, EventArgs e)
        {
            if (txt_MaThuoc.Text == "" || txt_SoLuongMua.Text == "" || txt_CongDung.Text == "" || txt_ThanhPhan.Text == "" || txt_DonGia.Text == "" ||comboBoxTenThuoc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (txt_maNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (kt_maNV(txt_maNV.Text) == false)
            {
                MessageBox.Show("Bạn nhập mã nhân viên không đúng!", "Thông báo", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            int slMua = int.Parse(txt_SoLuongMua.Text);
            double dg = double.Parse(txt_DonGia.Text);
            double thanhTien = (slMua * dg);
            DataRowView selectedRow = (DataRowView)comboBoxTenThuoc.SelectedItem;
            string tenThuoc = selectedRow["TENTHUOC"].ToString();
            dataGridViewGioHang.Rows.Add(txt_MaThuoc.Text, tenThuoc, txt_SoLuongMua.Text, txt_DonGia.Text, thanhTien);
            labelTongTien.Text =  tongTien().ToString();
        }
        double tongTien()
        {
            double tong = 0;
            foreach(DataGridViewRow row in dataGridViewGioHang.Rows)
            {
                tong=tong+ Convert.ToDouble(row.Cells[4].Value);
            }
            return tong;
        }
        private void buttonXoa_Click(object sender, EventArgs e)
        {
            if (dataGridViewGioHang.SelectedRows.Count > 0)
            {
                dataGridViewGioHang.Rows.RemoveAt(dataGridViewGioHang.SelectedRows[0].Index);
            }
            txt_MaThuoc.Text = "";
            txt_SoLuongMua.Text = "";
            txt_CongDung.Text = "";
            txt_ThanhPhan.Text = "";
            txt_DonGia.Text = "";
            comboBoxTenThuoc.Text = "";
        }

        private void buttonXuatHoaDon_Click(object sender, EventArgs e)
        {
            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                // Chèn thông tin vào bảng PHIEUXUAT
                string sqlInsertPhieuXuat = "INSERT INTO PHIEUXUAT (MAPX, NGAYXUAT, MAKH, MANV) " + "VALUES (@MAPX, @NGAYXUAT, @MAKH, @MANV)";
                string maPX = GenerateMaPX();
                using (SqlCommand cmd = new SqlCommand(sqlInsertPhieuXuat, conn))
                {
                    cmd.Parameters.AddWithValue("@MAPX", maPX);
                    cmd.Parameters.AddWithValue("@NGAYXUAT", DateTime.Today.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@MAKH", txt_maKH.Text);
                    cmd.Parameters.AddWithValue("@MANV",txt_maNV.Text);

                    cmd.ExecuteNonQuery();
                }

                //Chèn thông tin vào bảng CHITIETPHIEUXUAT
                foreach (DataGridViewRow row in dataGridViewGioHang.Rows)
                {
                    // Lấy thông tin từ mỗi dòng
                    string maThuoc = row.Cells[0].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells[2].Value);
                    double donGia = Convert.ToDouble(row.Cells[3].Value);
                   

                    // Thực hiện câu lệnh INSERT vào bảng CHITIETPHIEUXUAT
                    string sqlInsertChiTietPhieuXuat = "INSERT INTO CHITIETPHIEUXUAT (MAPX, MATHUOC, SOLUONG, DONGIA) VALUES (@MAPX, @MATHUOC, @SOLUONG, @DONGIA)";
                    using (SqlCommand cmd = new SqlCommand(sqlInsertChiTietPhieuXuat, conn))
                    {
                        cmd.Parameters.AddWithValue("@MAPX", maPX);
                        cmd.Parameters.AddWithValue("@MATHUOC", maThuoc);
                        cmd.Parameters.AddWithValue("@SOLUONG", soLuong);
                        cmd.Parameters.AddWithValue("@DONGIA", donGia);

                        cmd.ExecuteNonQuery();
                    }
                }
               
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                MessageBox.Show("Nhập thông tin thành công!", "Thông báo");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi");
            }
        }
        

    }
}
