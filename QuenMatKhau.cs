using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1
{
    public partial class QuenMatKhau : Form
    {
        private Main main;
        private string MaGuiVe;
        private string MaCaptchar;
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public QuenMatKhau(Main main)
        {
            InitializeComponent();
            this.main = main;  
        }

        private void QuenMatKhau_Load(object sender, EventArgs e)
        {
            txtMaCT.Enabled = false;
            MaCaptchar = TaoMaNgauNhien(6);
            txtMaCT.Text = MaCaptchar;
        }
        private void llb_DangNhap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangNhap dangNhap = new DangNhap(main);
            this.Hide();
            dangNhap.ShowDialog();
        }

        private void llb_DangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKy dangKy = new DangKy(main);
            this.Hide();
            dangKy.ShowDialog();
        }
        public string TaoMaNgauNhien(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; 

            Random random = new Random();
            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            return new string(code);
        }

        private void btnRdCapchar_Click(object sender, EventArgs e)
        {
            MaCaptchar = TaoMaNgauNhien(6);
            txtMaCT.Text = MaCaptchar.ToString();
        }

        private void btSendCode_Click(object sender, EventArgs e)
        {
            if(KiemTraTaiKhoan(txt_TenDN.Text))
            {
                MaGuiVe = TaoMaNgauNhien(6);
                string ThongBao = "Mã xác nhận của bạn là: " + MaGuiVe;
                ThongBaoMa.ShowBalloonTip(30000, "Thông báo từ Cà phê sách", ThongBao, ToolTipIcon.Info);
            }
            else if (String.IsNullOrEmpty(txt_TenDN.Text))
            {
                MessageBox.Show("Vui lòng điền thông tin tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản! Kiểm tra lại thông tin tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       
        private bool KiemTraTaiKhoan(string Tk)
        {
            bool taiKhoanKhongTonTai = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_KiemTraTaiKhoanDangNhap", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenDangNhap", Tk);

                    try
                    {
                        connection.Open();
                        string loaiTaiKhoan = (string)command.ExecuteScalar();
                       if(loaiTaiKhoan == "KhongTonTai")
                        {
                            taiKhoanKhongTonTai = false;
                        }
                        else
                        {
                            taiKhoanKhongTonTai = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thực thi Stored Procedure: " + ex.Message);
                        taiKhoanKhongTonTai = false;
                    }
                }
            }
            return taiKhoanKhongTonTai;
        }
        private void ResetMatKhau(string tenDangNhap, string matKhauMoi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_QuenMatKhau", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                    command.Parameters.AddWithValue("@MatKhau", matKhauMoi);

                    try
                    {
                        connection.Open();
                        string result = command.ExecuteScalar().ToString();
                        if(result == "ThanhCong")
                        {
                            MessageBox.Show("Cập nhật mật khẩu thành công", "Thông báo", MessageBoxButtons.OK);
                            DangNhap dangNhap = new DangNhap(main);
                            this.Hide();
                            dangNhap.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật mật khẩu thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private bool KiemtraNhap()
        {
            bool result = true;
            if (String.IsNullOrEmpty(txt_TenDN.Text))
            {
                MessageBox.Show("Vui lòng điền thông tin tài khoản!!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (String.IsNullOrEmpty(txtMaXN.Text))
            {
                MessageBox.Show("Vui lòng nhập mã xác nhận đã gửi về!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (txtMaXN.Text!= MaGuiVe)
            {
                MessageBox.Show("Mã xác nhận không chính xác!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (String.IsNullOrEmpty(txt_MatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    result = false;
            }
            else if (String.IsNullOrEmpty(txtNhapLaimk.Text))
            {
                MessageBox.Show("Vui lòng nhập xác nhận lại mật khẩu!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (txtNhapLaimk.Text!= txt_MatKhau.Text)
            {
                MessageBox.Show("Nhập lại Mật Khẩu không đúng !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (String.IsNullOrEmpty(txtCaptcha.Text))
            {
                MessageBox.Show("Vui lòng nhập mã Captcha !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            else if (txtCaptcha.Text!=MaCaptchar)
            {
                MessageBox.Show("Mã Captcha không chính xác!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            return result;
        }
        private void btXacNhan_Click(object sender, EventArgs e)
        {
            bool result = KiemtraNhap();
            if (result)
            {
                ResetMatKhau(txt_TenDN.Text,txtNhapLaimk.Text);
            }
        }

        private void btHienMkNL_Click(object sender, EventArgs e)
        {
            if (txtNhapLaimk.PasswordChar == '*')
            {
                txtNhapLaimk.PasswordChar = '\0';
            }
            else
            {
                txtNhapLaimk.PasswordChar = '*';
            }
        }

        private void btHienMk_Click(object sender, EventArgs e)
        {
            if (txt_MatKhau.PasswordChar == '*')
            {
                txt_MatKhau.PasswordChar = '\0';
            }
            else
            {
                txt_MatKhau.PasswordChar = '*';
            }
        }
    }
}
