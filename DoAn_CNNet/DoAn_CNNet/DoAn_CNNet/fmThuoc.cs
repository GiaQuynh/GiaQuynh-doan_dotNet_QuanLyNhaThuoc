using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_CNNet
{
    public partial class fmThuoc : Form
    {
       
        SqlConnection conn ;
        DataSet ds;
        SqlDataAdapter da;

        public fmThuoc()
        {
            InitializeComponent();
            conn = new SqlConnection(@"Data Source=LAPTOP-A89D0DVP\SQLEXPRESS;Initial Catalog=QLNHATHUOCTAY;Integrated Security=True");
        }
        void load_dataG()
        {
            ds = new DataSet();
            string sql = "SELECT THUOC.MATHUOC,THUOC.MALOTHUOC,THUOC.MALOAITHUOC,THUOC.TENTHUOC,LOTHUOC.NGAYSX,LOTHUOC.HANSUDUNG,HANGSANXUAT.TENHANGSX,THUOC.DANGTHUOC,THUOC.CONGDUNG,THUOC.THANHPHAN,THUOC.SOLUONGTON, CHITIETPHIEUNHAP.DONGIA FROM THUOC, CHITIETPHIEUNHAP,LOTHUOC,HANGSANXUAT WHERE THUOC.MATHUOC = CHITIETPHIEUNHAP.MATHUOC AND THUOC.MALOTHUOC= LOTHUOC.MALOTHUOC AND LOTHUOC.MAHANGSX=HANGSANXUAT.MAHANGSX";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "Thuoc");
            dataGridView1.DataSource = ds.Tables["Thuoc"];
        }

        void load_cbo_hangSX()
        {
            ds = new DataSet();
            string sql = "Select *from HANGSANXUAT";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "HANGSANXUAT");
            comboBoxHangSanXuat.DataSource = ds.Tables["HANGSANXUAT"];
            comboBoxHangSanXuat.DisplayMember = "TENHANGSX";
            comboBoxHangSanXuat.ValueMember = "MAHANGSX";
        }

        void load_cbo_loaiThuoc()
        {
            ds = new DataSet();
            string sql = "Select *from LOAITHUOC";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "LT");
            comboBoxLoaiThuoc.DataSource = ds.Tables["LT"];
            comboBoxLoaiThuoc.DisplayMember = "TENLOAITHUOC";
            comboBoxLoaiThuoc.ValueMember = "MALOAITHUOC";
        }

        private void fmThuoc_Load(object sender, EventArgs e)
        {
            load_dataG();
            load_cbo_hangSX();
            load_cbo_loaiThuoc();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có dòng được chọn hay không
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị từ các ô của dòng được chọn
                string mathuoc = row.Cells["MATHUOC"].Value.ToString();
                string malothuoc = row.Cells["MALOTHUOC"].Value.ToString();
                string maloaithuoc = row.Cells["MALOAITHUOC"].Value.ToString();
                string tenthuoc = row.Cells["TENTHUOC"].Value.ToString();
                string ngaySX = row.Cells["NGAYSX"].Value.ToString();
                string hanSD = row.Cells["HANSUDUNG"].Value.ToString();
                string hangSX = row.Cells["TENHANGSX"].Value.ToString();
                string dangthuoc = row.Cells["DANGTHUOC"].Value.ToString();

                string congdung = row.Cells["CONGDUNG"].Value.ToString();
                string thanhphan = row.Cells["THANHPHAN"].Value.ToString();
                int soluongton = Convert.ToInt32(row.Cells["SOLUONGTON"].Value);
                float dongia = Convert.ToSingle(row.Cells["DONGIA"].Value);

                // Hiển thị giá trị lên các TextBox
                tbMaThuoc.Text = mathuoc;
                tbMaLo.Text = malothuoc;
                comboBoxLoaiThuoc.Text = maloaithuoc;
                tbTenThuoc.Text = tenthuoc;
                tbDangThuoc.Text = dangthuoc;
                tbCongDung.Text = congdung;
                tbThanhPhan.Text = thanhphan;
                dtpNgaySX.Text = ngaySX;
                dtpHanSD.Text = hanSD;
                comboBoxHangSanXuat.Text = hangSX;
                tbDonGia.Text = dongia.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
           
            string mathuoc = tbMaThuoc.Text;
            string malothuoc = tbMaLo.Text;
            string maloaithuoc = comboBoxLoaiThuoc.Text;
            string tenthuoc = tbTenThuoc.Text;
            string ngaySX = dtpNgaySX.Value.ToString();
            string hanSD = dtpHanSD.Value.ToString();
            string dangthuoc = tbDangThuoc.Text;
            string congdung = tbCongDung.Text;
            string thanhphan = tbThanhPhan.Text;
            string hangSX = comboBoxHangSanXuat.Text;
            float dongia = float.Parse(tbDonGia.Text);

            
            string mahangsx = comboBoxHangSanXuat.SelectedValue.ToString();

            
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            string updateThuoc = "UPDATE THUOC SET MALOTHUOC = @Malothuoc, MALOAITHUOC = @Maloaithuoc, TENTHUOC = @Tenthuoc,  DANGTHUOC = @Dangthuoc, CONGDUNG = @Congdung, THANHPHAN = @Thanhphan WHERE MATHUOC = @Mathuoc";
            int rowsAffected = 0;
            // Mở kết nối và thực hiện cập nhật

            using (SqlCommand command = new SqlCommand(updateThuoc, conn))
            {
                // Thêm các tham số để tránh SQL injection
                command.Parameters.AddWithValue("@Mathuoc", mathuoc);
                command.Parameters.AddWithValue("@Malothuoc", malothuoc);
                command.Parameters.AddWithValue("@Maloaithuoc", maloaithuoc);
                command.Parameters.AddWithValue("@Tenthuoc", tenthuoc);
                command.Parameters.AddWithValue("@Dangthuoc", dangthuoc);
                command.Parameters.AddWithValue("@Congdung", congdung);
                command.Parameters.AddWithValue("@Thanhphan", thanhphan);

               // command.ExecuteNonQuery();

                rowsAffected = command.ExecuteNonQuery();

               

            }
            string updateLoThuoc = "UPDATE LOTHUOC SET NGAYSX= @NGAYSX, HANSUDUNG= @HANSD WHERE MALOTHUOC=@MALO ";
            using (SqlCommand commandLT = new SqlCommand(updateLoThuoc, conn))
            {
                // Thêm các tham số để tránh SQL injection
                commandLT.Parameters.AddWithValue("@MALO", malothuoc);
                commandLT.Parameters.AddWithValue("@NGAYSX", ngaySX);
                commandLT.Parameters.AddWithValue("@HANSD", hanSD);

                rowsAffected=commandLT.ExecuteNonQuery();

               
            }

            string updateCTPN = "UPDATE CHITIETPHIEUNHAP SET DONGIA= @DONGIA WHERE MATHUOC = @Mathuoc";
            using (SqlCommand commandCTN = new SqlCommand(updateCTPN, conn))
            {
                // Thêm các tham số để tránh SQL injection
                commandCTN.Parameters.AddWithValue("@Mathuoc", mathuoc);
                commandCTN.Parameters.AddWithValue("@DONGIA", dongia);


                rowsAffected = commandCTN.ExecuteNonQuery();
            }

            if (rowsAffected > 0)
            {
                MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu nào được cập nhật!");
            }
            load_dataG();
        }

        

        
        
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string mathuoc = tbMaThuoc.Text;
                int rowsAffected = 0;
                // Mở kết nối
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //string deleteCTN = "DELETE FROM CHITIETPHIEUNHAP WHERE MATHUOC = @Mathuoc";
                //using (SqlCommand command = new SqlCommand(deleteCTN, conn))
                //{
                //    // Thêm tham số để tránh SQL injection
                //    command.Parameters.AddWithValue("@Mathuoc", mathuoc);

                //    // Thực hiện xóa
                //    rowsAffected = command.ExecuteNonQuery();
                
                //}

                // Câu lệnh SQL để xóa dữ liệu từ bảng THUOC
                string deleteThuoc = "DELETE FROM THUOC WHERE MATHUOC = @Mathuoc";

                // Sử dụng SqlCommand để thực hiện câu lệnh SQL
                using (SqlCommand command = new SqlCommand(deleteThuoc, conn))
                {
                    // Thêm tham số để tránh SQL injection
                    command.Parameters.AddWithValue("@Mathuoc", mathuoc);

                    // Thực hiện xóa
                    rowsAffected = command.ExecuteNonQuery();

                   
                }

                // Đóng kết nối
                conn.Close();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dữ liệu đã được xóa thành công!");
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được xóa!");
                }
                // Tải lại dữ liệu vào DataGridView
                load_dataG();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
        }
    }
}
