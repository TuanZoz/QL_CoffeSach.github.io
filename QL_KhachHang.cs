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
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuAn1
{
    public partial class QL_KhachHang : UserControl
    {
        public QL_KhachHang()
        {
            InitializeComponent();
            LoadData();
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        private void QL_KhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetKhachHang", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTableKH = new DataTable();
                adapter.Fill(dataTableKH);
                DgvKhachHang.DataSource = dataTableKH;
                Khoa();
                txtMakh.Text = "";
                txtTenKH.Text = "";
                txtEmail.Text = "";
                txtDiaChi.Text = "";
                txtSDT.Text = "";
            }
        }
        public void ThemKhachHang(string maKhachHang, string tenKhachHang,DateTime ngaySinh, bool phai, string email, string diaChi, string dienThoai, string matKhau)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("ThemKhachHang", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaKh", maKhachHang);
                    cmd.Parameters.AddWithValue("@TenKhach", tenKhachHang);
                    cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                    cmd.Parameters.AddWithValue("@Phai", phai);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", dienThoai);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm khách hàng thành công!");
                    Khoa();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm khách hàng: " + ex.Message);
                }
            }
        }
        public void CapNhatKhachHang(string maKhachHang, string tenKhachHang, DateTime ngaySinh, bool phai, string email, string diaChi, string dienThoai)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("CapNhatKhachHang", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaKh", maKhachHang);
                    cmd.Parameters.AddWithValue("@TenKhach", tenKhachHang);
                    cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                    cmd.Parameters.AddWithValue("@Phai", phai);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", dienThoai);

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
        public void XoaKhachHang(string maKhachHang)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        SqlCommand cmd = new SqlCommand("XoaKhachHang", connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);

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
            txtMakh.Enabled = false;
            txtTenKH.Enabled = false;
            txtEmail.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSDT.Enabled = false;
            dtNgaySinh.Enabled = false;
            btnLuu.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            rbNam.Enabled = false;
            rbNu.Enabled = false;
        }
        public void MoKhoa()
        {
            txtMakh.Enabled = true;
            txtTenKH.Enabled = true;
            txtEmail.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSDT.Enabled = true;
            dtNgaySinh.Enabled = true;
            btnLuu.Enabled = true;
            rbNam.Enabled = true;
            rbNu.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            MoKhoa();
            ThemMoi = true;
            Xoa = false;
            btnXoa.Enabled = true;
           txtMakh.Text= GenerateUniqueMaKhachHang();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            
            MoKhoa();
            ThemMoi = false;
        }

        private void DgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DgvKhachHang.Rows[e.RowIndex];

                txtMakh.Text = row.Cells["MaKh"].Value.ToString();
                txtTenKH.Text = row.Cells["TenKhach"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                bool gioiTinh = Convert.ToBoolean(row.Cells["Phai"].Value);
                txtSDT.Text = row.Cells["DienThoai"].Value.ToString();
                dtNgaySinh.Text = row.Cells["NgaySinh"].Value.ToString();
                if (gioiTinh)
                {
                    rbNam.Checked = true;
                }
                else
                {
                    rbNu.Checked = true;
                }
            }
        }
        private string GenerateUniqueMaKhachHang()
        {
            string newMaKhachHang = "KH001";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                while (true)
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM KhachHang WHERE MaKh = @MaKhachHang", connection);
                    command.Parameters.AddWithValue("@MaKhachHang", newMaKhachHang);
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        newMaKhachHang = GenerateRandomMaKhachHang();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return newMaKhachHang;
        }

        private string GenerateRandomMaKhachHang()
        {
            Random random = new Random();
            string chars = "0123456789";
            return "KH" + new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
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
        public bool KiemTraEmailVaSDt(string email, string sdt,string ma)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("EmailPhoneExistKh", connection))
                {
                    bool kt = false;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", sdt);
                    command.Parameters.AddWithValue("@MaKH", ma);
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
        private void btnLuu_Click(object sender, EventArgs e)
        {
            DateTime ngayHienTai = DateTime.Now;
            DateTime ngayDuocChon = dtNgaySinh.Value;

            if (string.IsNullOrWhiteSpace(txtTenKH.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) 
                )
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (KiemTraEmailVaSDt(txtEmail.Text, txtSDT.Text,txtMakh.Text))
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
            else if (ngayDuocChon >= ngayHienTai)
            {
                MessageBox.Show("Ngày sinhh không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (ThemMoi)
            {
                bool phai = false;
                if (rbNam.Checked)
                {
                    phai = true;
                }
                ThemKhachHang(txtMakh.Text, txtTenKH.Text, Convert.ToDateTime(dtNgaySinh.Text), phai, txtEmail.Text, txtDiaChi.Text, txtSDT.Text, "1");
            }
            else
            {
                bool phai = false;
                if (rbNam.Checked)
                {
                    phai = true;
                }
                CapNhatKhachHang(txtMakh.Text, txtTenKH.Text, Convert.ToDateTime(dtNgaySinh.Text), phai, txtEmail.Text, txtDiaChi.Text, txtSDT.Text);
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("timKiemKH", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Tukhoa", txtTK.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    if (dataTable == null)
                    {
                        MessageBox.Show("Không có khách hàng với thông tin cần tìm ");
                    }
                    else
                    {
                        DgvKhachHang.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi : " + ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            XoaKhachHang(txtMakh.Text);
        }

       
    }
        
}
