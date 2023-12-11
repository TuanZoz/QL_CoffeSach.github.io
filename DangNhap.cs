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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

namespace DuAn1
{
    public partial class DangNhap : Form
    {
        private Main main;
        public DangNhap(Main main)
        {
            InitializeComponent();
            this.main = main;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT TaiKhoan FROM GhiNhoDangNhap", connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string taiKhoan = reader["TaiKhoan"].ToString();
                    cbbTk.Items.Add(taiKhoan);
                }

                reader.Close();
            }
        }
        public string Manv;
        public string Vaitro;
        private void DangNhap_Load(object sender, EventArgs e)
        {
        }
        public void GhiNhoDangNhap(string TaiKhoan, string MatKhau)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("GhiNhoDN", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TaiKhoan", TaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", MatKhau);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ghi nhớ đăng nhập thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
          
        }
        public class LoginResult
        {
            public string MaDangNhap { get; set; }
            public string LoaiNguoiDung { get; set; }
        }

        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public LoginResult KiemTraDn(string emailOrPhone, string password)
        {
            string status = "";
            string maDangNhap = "";
            string loaiNguoiDung = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DangNhap", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", emailOrPhone);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        status = reader["Status"].ToString();
                        maDangNhap = reader["MaDangNhap"].ToString();
                        loaiNguoiDung = reader["VaiTro"].ToString();
                    }

                    reader.Close();
                }
            }

            if (status == "Success")
            {

                if (loaiNguoiDung == "Nhân viên")
                {
                    int maVt = GetMaVt(maDangNhap);
                    if (maVt == 1)
                    {
                        loaiNguoiDung = "Quản Lý";
                        if(chkGhiNho.Checked)
                        {
                            GhiNhoDangNhap(emailOrPhone, password);
                        }    
                        MessageBox.Show("Đăng nhập thành công! Bạn là quản lý.", "Thông báo", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thành công! Bạn là nhân viên.", "Thông báo", MessageBoxButtons.OK);
                        if (chkGhiNho.Checked)
                        {
                            GhiNhoDangNhap(emailOrPhone, password);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Đăng nhập thành công! Bạn là khách hàng.", "Thông báo", MessageBoxButtons.OK);
                    if (chkGhiNho.Checked)
                    {
                        GhiNhoDangNhap(emailOrPhone, password);
                    }
                }
                return new LoginResult { MaDangNhap = maDangNhap, LoaiNguoiDung = loaiNguoiDung };
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công! Kiểm tra lại thông tin đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }
        private int GetMaVt(string maDangNhap)
        {
            int maVt = 0; 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MaVt FROM NhanVien WHERE MaNV = @MaDangNhap"; 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaDangNhap", maDangNhap);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        maVt = Convert.ToInt32(result);
                    }
                }
            }

            return maVt;
        }
        
        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            string username = cbbTk.Text;
            string password = txt_MatKhau.Text;
            var userInfo = KiemTraDn(username, password);

            if (userInfo != null)
            {
                string maDangNhap = userInfo.MaDangNhap.ToString();
                string vaitro = userInfo.LoaiNguoiDung.ToString();
                Main mainForm = new Main();
                mainForm.LayMa(maDangNhap, vaitro);
                mainForm.Show();
                this.Close();
            }
        }

        private void btdangki_Click(object sender, EventArgs e)
        {
            DangKy dangKy = new DangKy(main);
            this.Hide();
            dangKy.ShowDialog();
            
        }

        private void linkLabel_QuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuenMatKhau quenMatKhau = new QuenMatKhau(main);   
            this.Hide();
            quenMatKhau.ShowDialog();
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            cbbTk.Text="";
            txt_MatKhau.Text= "";
        }

        private void cbbTk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbTk.SelectedIndex != -1) 
            {
                string selectedAccount = cbbTk.SelectedItem.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT MaKhau FROM GhiNhoDangNhap WHERE TaiKhoan = @TaiKhoan", connection);
                    command.Parameters.AddWithValue("@TaiKhoan", selectedAccount);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string matKhau = reader["MaKhau"].ToString();
                        txt_MatKhau.Text = matKhau;
                        cbbTk.Text = selectedAccount;
                    }

                    reader.Close();
                }
            }
        }
    }
}
