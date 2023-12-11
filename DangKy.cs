using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuAn1
{
    public partial class DangKy : Form
    {
        private Main main;
        public DangKy(Main main)
        {
            InitializeComponent();
            this.main = main;
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public void ThemKhachHang(string maKhachHang, string tenKhachHang, DateTime ngaySinh, bool phai, string email, string diaChi, string dienThoai, string matKhau)
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm khách hàng: " + ex.Message);
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
            return "MKH" + new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
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
        public bool KiemTraEmailVaSDt(string email,string sđt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CheckEmailPhoneExist", connection))
                {
                    bool kt = false;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", email);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string result = reader["Result"].ToString();
                            if(result != "null")
                            {
                                MessageBox.Show(result,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void button_DangKy_Click(object sender, EventArgs e)
        {
            DateTime ngayHienTai = DateTime.Now;
            DateTime ngayDuocChon = dtpNgaySinh.Value;

            if (string.IsNullOrWhiteSpace(txtTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
             {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (KiemTraEmailVaSDt(txtEmail.Text, txtSDT.Text))
            {
            }
            else if (rbNam.Checked==false && rbNu.Checked==false)
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
            else if (txtCheckMatKhau.Text != txtMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else if (ngayDuocChon >= ngayHienTai)
            {
                MessageBox.Show("Ngày sinhh không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string maKhachHang = GenerateUniqueMaKhachHang(); ;
                bool phai = false;
                if (rbNam.Checked)
                {
                    phai = true;
                }
                ThemKhachHang(maKhachHang, txtTen.Text, ngayDuocChon, phai, txtEmail.Text, txtDiaChi.Text, txtSDT.Text, txtCheckMatKhau.Text);
                DangNhap dangNhap = new DangNhap(main);
                this.Hide();
                dangNhap.ShowDialog();
            }
        }

        private void button_DangNhap_Click(object sender, EventArgs e)
        {
            DangNhap dangNhap = new DangNhap(main);
            this.Hide();
            dangNhap.ShowDialog();
        }

        private void linkLabel_QuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuenMatKhau quenMatKhau = new QuenMatKhau(main);
            this.Hide();
            quenMatKhau.ShowDialog();
        }

        private void btHienMk_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar == '*')
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
               txtMatKhau.PasswordChar = '*';
            }
        }

        private void btnHienMkNhapLai_Click(object sender, EventArgs e)
        {
            if (txtCheckMatKhau.PasswordChar == '*')
            {
                txtCheckMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtCheckMatKhau.PasswordChar = '*';
            }
        }
    }
}
