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
using System.Text.RegularExpressions;

namespace DoAn_CNNet
{
    public partial class fmHoaDonNhap : Form
    {
        SqlConnection conn;
        public fmHoaDonNhap()
        {
            InitializeComponent();
            conn = new SqlConnection(@"Data Source=DESKTOP-V6MCQL7;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");

        }

        private void fmHoaDonNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn thoát", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.No)
                e.Cancel = true;
        }

        void load_cboNCC()
        {
            DataSet ds = new DataSet();
            string sql = "select *from NHACUNGCAP";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds,"NCC");
            cbo_tenNCC.DataSource = ds.Tables["NCC"];
            cbo_tenNCC.DisplayMember = "TENNHACUNGCAP";
            cbo_tenNCC.ValueMember = "MANHACUNGCAP";
            
        }
        void load_hangSX()
        {

            DataSet ds = new DataSet();
            string sql = "select *from HANGSANXUAT";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
            comboBoxHangSanXuat.DataSource = ds.Tables[0];
            comboBoxHangSanXuat.DisplayMember = "TENHANGSX";
            comboBoxHangSanXuat.ValueMember = "MAHANGSX";
        }
        void load_LoaiThuoc()
        {
            DataSet ds = new DataSet();
            string sql = "select *from LOAITHUOC";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
            comboBoxLoaiThuoc.DataSource = ds.Tables[0];
            comboBoxLoaiThuoc.DisplayMember = "TENLOAITHUOC";
            comboBoxLoaiThuoc.ValueMember = "MALOAITHUOC";
        }
        private void fmHoaDonNhap_Load(object sender, EventArgs e)
        {
            load_cboNCC();
            load_hangSX();
            load_LoaiThuoc();
        }
        private void btn_NV_ok_Click(object sender, EventArgs e)
        {
           
            if(txtMaNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            string sql = "select TENNV, SDT FROM NHANVIEN WHERE MANV=@maNV";
            using(SqlCommand cmd= new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@maNV", txtMaNV.Text);
                using(SqlDataReader dr= cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtTenNV.Text = dr["TENNV"].ToString();
                        txtSDT.Text = dr["SDT"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                conn.Close();
            }
            
        }

        private void btn_huyNV_Click(object sender, EventArgs e)
        {
            txtMaNV.Text = " ";
            txtTenNV.Text = " ";
            txtSDT.Text = " ";
        }

       

        private void btn_NCC_ok_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbo_tenNCC.SelectedValue.ToString()))
            {
                txtMaNCC.Text = cbo_tenNCC.SelectedValue.ToString();

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btn_huyNCC_Click(object sender, EventArgs e)
        {
            cbo_tenNCC.Text = "";
            
            txtMaNCC.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem tất cả các thông tin cần thiết đã được nhập đầy đủ chưa
                if (string.IsNullOrEmpty(tbMaThuoc.Text) ||
                    string.IsNullOrEmpty(tbMaLo.Text) ||
                    comboBoxLoaiThuoc.SelectedValue == null ||
                    string.IsNullOrEmpty(tbTenThuoc.Text) ||
                    string.IsNullOrEmpty(tbDangThuoc.Text) ||
                    string.IsNullOrEmpty(tbCongDung.Text) ||
                    string.IsNullOrEmpty(tbThanhPhan.Text) ||
                    comboBoxHangSanXuat.SelectedValue == null ||
                    string.IsNullOrEmpty(tbSoLuong.Text) ||
                    string.IsNullOrEmpty(tbDonGia.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }

                // Lấy thông tin từ các controls trên form
                string maThuoc = tbMaThuoc.Text;
                string maLoThuoc = tbMaLo.Text;
                string maLoaiThuoc = comboBoxLoaiThuoc.SelectedValue.ToString();
                string tenThuoc = tbTenThuoc.Text;
                string dangThuoc = tbDangThuoc.Text;
                string congDung = tbCongDung.Text;
                string thanhPhan = tbThanhPhan.Text;
                int soLuongTon = 0;
                string maHangSX = comboBoxHangSanXuat.SelectedValue.ToString();
                DateTime ngaySX = Convert.ToDateTime(dtpNgaySX.Value.ToString());
                DateTime hanSD = Convert.ToDateTime(dtpHanSD.Value.ToString());
                int soLuongNhap = int.Parse(tbSoLuong.Text); 
                double donGia = double.Parse(tbDonGia.Text); 

                
                conn.Open();
                //  INSERT vào bảng LOTHUOC
                string insertLoThuocQuery = "INSERT INTO LOTHUOC (MALOTHUOC, MAHANGSX, NGAYSX, HANSUDUNG) " +
                                             "VALUES (@MALOTHUOC, @MAHANGSX, @NGAYSX, @HANSUDUNG)";
                SqlCommand insertLoThuocCommand = new SqlCommand(insertLoThuocQuery, conn);
                insertLoThuocCommand.Parameters.AddWithValue("@MALOTHUOC", maLoThuoc);
                insertLoThuocCommand.Parameters.AddWithValue("@MAHANGSX", maHangSX);
                insertLoThuocCommand.Parameters.AddWithValue("@NGAYSX", ngaySX);
                insertLoThuocCommand.Parameters.AddWithValue("@HANSUDUNG", hanSD);
                insertLoThuocCommand.ExecuteNonQuery();

                // INSERT vào bảng THUOC
                string insertThuocQuery = "INSERT INTO THUOC (MATHUOC, MALOTHUOC, MALOAITHUOC, TENTHUOC, DANGTHUOC, CONGDUNG, THANHPHAN, SOLUONGTON) " +
                                          "VALUES (@MATHUOC, @MALOTHUOC, @MALOAITHUOC, @TENTHUOC, @DANGTHUOC, @CONGDUNG, @THANHPHAN, @SOLUONGTON)";
                SqlCommand insertThuocCommand = new SqlCommand(insertThuocQuery, conn);
                insertThuocCommand.Parameters.AddWithValue("@MATHUOC", maThuoc);
                insertThuocCommand.Parameters.AddWithValue("@MALOTHUOC", maLoThuoc);
                insertThuocCommand.Parameters.AddWithValue("@MALOAITHUOC", maLoaiThuoc);
                insertThuocCommand.Parameters.AddWithValue("@TENTHUOC", tenThuoc);
                insertThuocCommand.Parameters.AddWithValue("@DANGTHUOC", dangThuoc);
                insertThuocCommand.Parameters.AddWithValue("@CONGDUNG", congDung);
                insertThuocCommand.Parameters.AddWithValue("@THANHPHAN", thanhPhan);
                insertThuocCommand.Parameters.AddWithValue("@SOLUONGTON", soLuongTon);
                insertThuocCommand.ExecuteNonQuery();

                

                ///them PHIEUNHAP
                string maPhieuNhap = GenerateMaPhieuNhap(); // Hàm này để sinh mã phiếu nhập mới
                string ngayNhap = DateTime.Today.ToString("yyyy-MM-dd"); /*Value.ToString("yyyy-MM-dd");*/
                string sqlInsertPhieuNhap = "INSERT INTO PHIEUNHAP VALUES (@maPN ,@ngayLap,@maNCC,@maNV)";
                SqlCommand cmd = new SqlCommand(sqlInsertPhieuNhap, conn);
                cmd.Parameters.AddWithValue("@maPN",maPhieuNhap);
                cmd.Parameters.AddWithValue("@ngayLap", ngayNhap);
                cmd.Parameters.AddWithValue("@maNCC", txtMaNCC.Text);
                cmd.Parameters.AddWithValue("@maNV", txtMaNV.Text);
                cmd.ExecuteNonQuery();

                // INSERT vào bảng CHITIETPHIEUNHAP
                string insertChiTietPhieuNhapQuery = "INSERT INTO CHITIETPHIEUNHAP (MAPN,MATHUOC, SOLUONG, DONGIA) " +
                                                     "VALUES (@MAPN,@MATHUOC, @SOLUONG, @DONGIA)";
                SqlCommand insertChiTietPhieuNhapCommand = new SqlCommand(insertChiTietPhieuNhapQuery, conn);
                insertChiTietPhieuNhapCommand.Parameters.AddWithValue("@MAPN", maPhieuNhap);
                insertChiTietPhieuNhapCommand.Parameters.AddWithValue("@MATHUOC", maThuoc);
                insertChiTietPhieuNhapCommand.Parameters.AddWithValue("@SOLUONG", soLuongNhap);
                insertChiTietPhieuNhapCommand.Parameters.AddWithValue("@DONGIA", donGia);
                insertChiTietPhieuNhapCommand.ExecuteNonQuery();

                conn.Close();
                lbTongTien.Text = Convert.ToString(tongTien(soLuongNhap, donGia));
                MessageBox.Show("Đã thêm thông tin thuốc thành công!","Thông báo!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        double tongTien(int sl, double donGia)
        {
            return sl * donGia;
        }
        private string GenerateMaPhieuNhap()
        {
            // Hàm này để sinh mã phiếu nhập mới, bạn có thể tùy chỉnh theo nhu cầu
           
            string pn_max = GetNextIDFromDatabase("PHIEUNHAP", "MAPN");
            int soPN = GetNextID(pn_max);
            return "PN0" + soPN;
        }
        int GetNextID(string maPhieuNhap)
        {
            // Sử dụng biểu thức chính quy để tách số từ chuỗi
            string pattern = @"\d+";
            Match match = Regex.Match(maPhieuNhap, pattern);

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

        private void tbSoLuong_TextChanged(object sender, EventArgs e)
        {
            Control ctr = (Control)sender;
            if (ctr.Text.Length > 0 && !Char.IsDigit(ctr.Text[ctr.Text.Length - 1]))
            {
                this.errorProvider1.SetError(ctr, "This is not avalid number");
                MessageBox.Show("Nhập dữ liệu không phù hợp","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
                this.errorProvider1.Clear();
        }

        private void tbDonGia_TextChanged(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
        
            tbTenThuoc.Text = "";
            tbMaThuoc.Text = "";
            tbMaLo.Text = "";
            dtpHanSD.Value = DateTime.Now;
            dtpNgaySX.Value = DateTime.Now;
            comboBoxHangSanXuat.Text = "";
            comboBoxLoaiThuoc.Text = "";
            tbThanhPhan.Text = "";
            tbCongDung.Text = "";
            tbDangThuoc.Text = "";
            tbSoLuong.Text = "";
            tbDonGia.Text = "";
            lbTongTien.Text = "__________________";
        }
    }
}
