using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1
{
    public partial class QL_NhanVien : UserControl
    {
        public QL_NhanVien()
        {
            InitializeComponent();
        }

        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        private void QL_NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetNhanVien", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTableNV = new DataTable();
                adapter.Fill(dataTableNV);

                DgvNhanVien.DataSource = dataTableNV;
                Khoa();
                rbHD.Checked = true;
                rbNam.Checked = true;
                rbNV.Checked = true;
                txtMK.Text = "";
                txtMaNv.Text = "";
                txtTenNV.Text = "";
                txtEmail.Text = "";
                txtDiaChi.Text = "";
                txtSDT.Text = "";
            }
        }
        private void ThemNhanVien(string MaNV, string TenNV, DateTime ngaysinh, bool Phai, string Email, string DiaChi, string DienThoai, string tinhtrang, string MatKhau, string maVT)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("ThemNhanVien", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNv", MaNV);
                    cmd.Parameters.AddWithValue("@TenNV", TenNV);
                    cmd.Parameters.AddWithValue("@ngaysinh", ngaysinh);
                    cmd.Parameters.AddWithValue("@Phai", Phai);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@DiaChi", DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", DienThoai);
                    cmd.Parameters.AddWithValue("@tinhtrang", tinhtrang);
                    cmd.Parameters.AddWithValue("@MatKhau", MatKhau);
                    cmd.Parameters.AddWithValue("@maVT", maVT);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!");
                    Khoa();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm: " + ex.Message);
                }
            }
        }
        private void CapNhatNhanVien(string MaNV, string TenNV, DateTime ngaysinh, bool Phai, string Email, string DiaChi, string DienThoai, string tinhtrang, string MatKhau, string maVT)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("CapNhatNhanVien", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNv", MaNV);
                    cmd.Parameters.AddWithValue("@TenNV", TenNV);
                    cmd.Parameters.AddWithValue("@ngaysinh", ngaysinh);
                    cmd.Parameters.AddWithValue("@Phai", Phai);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@DiaChi", DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", DienThoai);
                    cmd.Parameters.AddWithValue("@tinhtrang", tinhtrang);
                    cmd.Parameters.AddWithValue("@MatKhau", MatKhau);
                    cmd.Parameters.AddWithValue("@maVT", maVT);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");
                    Khoa();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                }
            }
        }
        public void XoaNhanVien(string maKhachHang)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        SqlCommand cmd = new SqlCommand("XoaNhanVien", connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MaNV", maKhachHang);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        Khoa();
                        LoadData();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi xóa: " + ex.Message);
                    }
                }
            }
        }
        public bool ThemMoi = true;
        public bool Xoa = true;
        public void Khoa()
        {
            txtMaNv.Enabled = false;
            txtTenNV.Enabled = false;
            txtEmail.Enabled = false;
            txtDiaChi.Enabled = false;
            txtMK.Enabled = false;
            txtSDT.Enabled = false;
            dtNgaySinh.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            rbNam.Enabled = false;
            rbNu.Enabled = false;
            rbHD.Enabled = false;
            rbKHD.Enabled = false;
            rbNV.Enabled = false;
            rbQL.Enabled = false;
            
        }
        public void MoKhoa()
        {
            txtTenNV.Enabled = true;
            txtMaNv.Enabled = true;
            txtEmail.Enabled = true;
            txtDiaChi.Enabled = true;
            txtMK.Enabled = true;
            txtSDT.Enabled = true;
            dtNgaySinh.Enabled = true;
            btnLuu.Enabled = true;
            rbNam.Enabled = true;
            rbNu.Enabled = true;
            rbHD.Enabled = true;
            rbKHD.Enabled = true;
            rbNV.Enabled = true;
            rbQL.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            MoKhoa();
            ThemMoi = true;
            Xoa = false;
            btnXoa.Enabled = true;
           txtMaNv.Text= GenerateUniqueMaNv();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MoKhoa();
            ThemMoi = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (Xoa)
            {
                XoaNhanVien(txtMaNv.Text);
            }
            else
            {
                LoadData();
            }
        }
        private void btnTK_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("timKiemNV", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Tukhoa", txtTK.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DgvNhanVien.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi : " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
            if (e.RowIndex >= 0)
            {
                LoadData();
                btnSua.Enabled = true;
                Xoa = true;
                btnXoa.Enabled = true;
                DataGridViewRow row = DgvNhanVien.Rows[e.RowIndex];

                txtMaNv.Text = row.Cells["MaNV"].Value.ToString();
                txtTenNV.Text = row.Cells["TenNV"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtSDT.Text = row.Cells["DienThoai"].Value.ToString();
                txtMK.Text = row.Cells["DienThoai"].Value.ToString();
                bool gioiTinh = Convert.ToBoolean(row.Cells["Phai"].Value);
                txtMK.Text = row.Cells["MatKhau"].Value.ToString();
                dtNgaySinh.Text = row.Cells["NgaySinh"].Value.ToString();
                string tinhtrang = row.Cells["Tinhtrang"].Value.ToString();
                if (gioiTinh)
                {
                    rbNam.Checked = true;
                }
                else
                {
                    rbNu.Checked = true;
                }
                if (tinhtrang == "Hoạt Động")
                {
                   rbHD.Checked = true;
                }
                else
                {
                    rbKHD.Checked = true;
                }
                string vaitro = row.Cells["MaVT"].Value.ToString();
                if (vaitro == "1")
                {
                    rbQL.Checked = true;
                }
                else 
                { 
                    rbNV.Checked = true;
                }
            }
        }
        private string GenerateUniqueMaNv()
        {
            string newMaKhachHang = "NV001";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                while (true)
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM NhanVien WHERE MaNV = @MaNV", connection);
                    command.Parameters.AddWithValue("@MaNV", newMaKhachHang);
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        newMaKhachHang = GenerateRandomMaNV();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return newMaKhachHang;
        }

        private string GenerateRandomMaNV()
        {
            Random random = new Random();
            string chars = "0123456789";
            return "NV" + new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static bool IsValidEmail(string email)
        {
            try
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                return Regex.IsMatch(email, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public bool KiemTraEmailVaSDt(string email, string sdt , string maNV)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("EmailPhoneExistNV", connection))
                {
                    bool kt = false;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", sdt);
                    command.Parameters.AddWithValue("@MaNV", maNV);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string result = reader["Result"].ToString();
                            if (result != "null")
                            {
                                MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                kt = true;
                            }
                        }
                    }
                    return kt;
                }
            }
        }
        bool KiemTraSo(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        private void btnluu_Click(object sender, EventArgs e)
        {
            string tinhtrang = "";
            if (rbHD.Checked)
            {
                tinhtrang = "Hoạt Động";
            }
            else
            {
                tinhtrang = "Không Hoạt Động";
            }
            string vaitro = "";
            if (rbQL.Checked == true)
            {
                vaitro = "1";
            }
            else
            {
                vaitro = "2";
            }
            bool phai = false;
            if (rbNam.Checked)
            {
                phai = true;
            }
            DateTime ngayHienTai = DateTime.Now;
            DateTime ngayDuocChon = dtNgaySinh.Value;

            if (string.IsNullOrWhiteSpace(txtTenNV.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text)
                )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (KiemTraEmailVaSDt(txtEmail.Text, txtSDT.Text,txtMaNv.Text))
            {
            }
            else if (rbNam.Checked == false && rbNu.Checked == false)
            {
                MessageBox.Show("Vui lòng chọn giới tính phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!KiemTraSo(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa số, không chứa chữ cái hoặc ký tự đặc biệt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtSDT.Text.Length != 10 || !txtSDT.Text.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rbNV.Checked == false && rbQL.Checked == false)
            {
                MessageBox.Show("Vai trò trống, vui lòng chọn vai trò.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rbHD.Checked == false && rbKHD.Checked == false)
            {
                MessageBox.Show("Trạng thái trống vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (ThemMoi)
            {
               
               
                ThemNhanVien(txtMaNv.Text,txtTenNV.Text, Convert.ToDateTime(dtNgaySinh.Text), phai,txtEmail.Text,txtDiaChi.Text,txtSDT.Text,tinhtrang,txtMK.Text,vaitro);
            }
            else
            {
               CapNhatNhanVien(txtMaNv.Text, txtTenNV.Text, Convert.ToDateTime(dtNgaySinh.Text), phai, txtEmail.Text, txtDiaChi.Text, txtSDT.Text, tinhtrang, txtMK.Text, vaitro);
            }
        }

      
    }
}
