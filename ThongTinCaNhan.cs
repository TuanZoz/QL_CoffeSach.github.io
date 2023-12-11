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

namespace DuAn1
{
    public partial class ThongTinCaNhan : UserControl
    {
        private Main main;
        public ThongTinCaNhan(Main main)
        {
            InitializeComponent();
            this.main = main;
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        private void ThongTinCaNhan_Load(object sender, EventArgs e)
        {
            HienThiThongTin();
            KiemTraGioHang(main.MaDn);
        }
        private void HienThiThongTin()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM KhachHang WHERE Makh = @MaKhachHang";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKhachHang", main.MaDn);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            txtTen.Text = reader["TenKhach"].ToString();
                            if (DateTime.TryParse(reader["NgaySinh"].ToString(), out DateTime dateValue))
                            {
                                dtpNgaySinh.Value = dateValue;
                            }
                            bool gioiTinh= Convert.ToBoolean(reader["Phai"]);
                            if (gioiTinh)
                            {
                                rbNam.Checked = true;
                            }
                            else
                            {
                                rbNu.Checked = true;
                            }
                            txtEmail.Text = reader["Email"].ToString();
                            txtSDT.Text = reader["DienThoai"].ToString(); ;
                            txtDiaChi.Text = reader["DiaChi"].ToString(); ;
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }
        private void KiemTraGioHang(string makh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_KiemTraSoLuongVaTongGiaTriSPTrongGioHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Makh", makh);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int soLuong = Convert.ToInt32(reader["SoLuongSanPhamTrongGioHang"]);
                                int tongGiaTri = Convert.ToInt32(reader["TongGiaTriSanPhamTrongGioHang"]);
                                SoluongSp.Text = soLuong.ToString();
                                txtTong.Text = tongGiaTri.ToString();
                            }
                        }
                        else
                        {
                            SoluongSp.Text = "0";
                            txtTong.Text = "0";
                        }

                        reader.Close();
                    }
                    catch
                    {
                        SoluongSp.Text = "0";
                        txtTong.Text = "0";
                    }
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            bool phai = false;
            if (rbNam.Checked)
            {
                phai = true;
            }
            CapNhatKhachHang(main.MaDn, txtTen.Text, Convert.ToDateTime(dtpNgaySinh.Text), phai, txtEmail.Text, txtDiaChi.Text, txtSDT.Text);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            main.GioHang();
        }
    }
}
