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
                    DangKy dangKy = new DangKy();
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
                    DangKy dangKy = new DangKy();
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
                        DangKy dangKy = new DangKy();
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
                XoaKhachHang(txtMakh.Text);
            }
            else
            {
                txtMakh.Text = "";
                txtTenKH.Text = "";
                txtEmail.Text = "";
                txtDiaChi.Text = "";
                txtSDT.Text = "";
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

                    DgvKhachHang.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi : " + ex.Message);
                }
            }
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtMakh.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô văn bản.");
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

      
    }
        
}
