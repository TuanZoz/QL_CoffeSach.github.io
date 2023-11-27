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
    public partial class QL_ThucDon : UserControl
    {
        public QL_ThucDon()
        {
            InitializeComponent();
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public DataTable LoadData()
        {
            DataTable booksTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetThucDon", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvThucDon.DataSource = dataTable;
                connection.Open();
                adapter.Fill(booksTable);
                Khoa();
                LoadLoaiSachData();
                CleaData();
                ThemMoi = true;
                Xoa = true;
                themloai = true;
            }

            return booksTable;
        }

        private void ThemMonVaoThucDon(string maMon, string tenMon, int soLuong, int donGiaNhap, int donGiaBan, DateTime ngayNhap, string hinhAnh, string moTa, string ghiChu, string maNv, string maLoaiMon)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("ThemMonVaoThucDon", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@TenMon", tenMon);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                    command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@HinhAnh", hinhAnh);
                    command.Parameters.AddWithValue("@MoTa", moTa);
                    command.Parameters.AddWithValue("@GhiChu", ghiChu);
                    command.Parameters.AddWithValue("@MaNv", maNv);
                    command.Parameters.AddWithValue("@MaLoaiMon", maLoaiMon);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm món vào thực đơn thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm món vào thực đơn: " + ex.Message);
            }
        }
        private void CapNhatMonTrongThucDon(string maMon, string tenMon, int soLuong, int donGiaNhap, int donGiaBan, DateTime ngayNhap, string hinhAnh, string moTa, string ghiChu, string maNv, string maLoaiMon)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("CapNhatMonTrongThucDon", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@TenMon", tenMon);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                    command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@HinhAnh", hinhAnh);
                    command.Parameters.AddWithValue("@MoTa", moTa);
                    command.Parameters.AddWithValue("@GhiChu", ghiChu);
                    command.Parameters.AddWithValue("@MaNv", maNv);
                    command.Parameters.AddWithValue("@MaLoaiMon", maLoaiMon);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Đã cập nhật thông tin món trong thực đơn thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin món trong thực đơn: " + ex.Message);
            }
        }
        public DataTable XemLoaiMon()
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetLoaiMon", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(result);
            }
            return result;
        }
        public string LayMa()
        {
            DataTable XemLoaiSachTable = XemLoaiMon();
            List<int> existingMaLoaiValues = new List<int>();

            if (XemLoaiSachTable != null && XemLoaiSachTable.Rows.Count > 0)
            {
                foreach (DataRow row in XemLoaiSachTable.Rows)
                {
                    string maLoai = row["MaLoaiMon"].ToString();
                    int temp;
                    if (int.TryParse(maLoai, out temp))
                    {
                        existingMaLoaiValues.Add(temp);
                    }
                    else
                    {
                        Console.WriteLine("Chuỗi không thể chuyển đổi thành số nguyên.");
                    }
                }
            }
            int newMa = 1;
            while (existingMaLoaiValues.Contains(newMa))
            {
                newMa++;
            }

            return newMa.ToString();
        }
        public void ThemLoaiMon(string Loai)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("ThemLoaiMon", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaLoaiMon", LayMa());
                    command.Parameters.AddWithValue("@Loai", Loai);
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm: " + ex.Message);
                }
            }
        }
        public void XoaMon(string maMon)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("XoaMonKhoiThucDon", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaMon", maMon);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        LoadData();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi xóa: " + ex.Message);
                    }
                }
            }
        }
        

        public void LoadLoaiSachData()
        {
            DataTable loaiSachTable = XemLoaiMon();

            if (loaiSachTable != null && loaiSachTable.Rows.Count > 0)
            {
                cbbLoai.Items.Clear();
                foreach (DataRow row in loaiSachTable.Rows)
                {
                    string Loai = row["Loai"].ToString();
                    cbbLoai.Items.Add(Loai);
                }

                cbbLoai.SelectedIndex = 0;
            }
        }
        public void TimSach(string tuKhoa)
        {
            DataTable booksTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                booksTable.Clear();
                SqlCommand command = new SqlCommand("TimKiemMonTrongThucDon", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TuKhoa", tuKhoa);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvThucDon.DataSource = dataTable;
                connection.Open();
                adapter.Fill(booksTable);
            }
        }
       
        private void LoadDataToComboBox(string bien)
        {
            string loai = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Loai FROM LoaiMon WHERE MaLoaiMon = @MaLoaiMon";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaLoaiMon", bien);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        loai = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            string maLoaiCanTim = LayMaLoaiSachTuLoaiSach(loai);
            foreach (var item in cbbLoai.Items)
            {
                string itemText = cbbLoai.GetItemText(item);
                string maLoaiComboBox = LayMaLoaiSachTuLoaiSach(itemText);

                if (maLoaiComboBox == maLoaiCanTim)
                {
                    cbbLoai.SelectedItem = item;
                    break;
                }
            }
        }

        private string LayMaLoaiSachTuLoaiSach(string loai)
        {
            string maLoai = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MaLoaiMon FROM LoaiMon WHERE Loai = @Loai";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Loai", loai);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        maLoai = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return maLoai;
        }

        public bool ThemMoi = true;
        public bool Xoa = true;
        public bool themloai = true;
        public void Khoa()
        {
            txtDonGiaBan.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            txtGhiChu.Enabled = false;
            txtHinh.Enabled = false;
            txtMaMon.Enabled = false;
            txtMoTa.Enabled = false;
            txtSoLuong.Enabled = false;
            txtTenMon.Enabled = false;
            dtNgayNhap.Enabled = false;
            cbbLoai.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btLuu.Enabled = false;
            txtTenLoai.Visible = false;
            btThemLoai.Enabled = false;

        }
        public void CleaData()
        {
            txtTenLoai.Text = "";
            txtDonGiaBan.Text = "";
            txtDonGiaNhap.Text = "";
            txtGhiChu.Text = "";
            txtHinh.Text = "";
            txtMaMon.Text = "";
            txtMoTa.Text = "";
            txtSoLuong.Text = "";
            txtTenMon.Text = "";
            dtNgayNhap.Text = "";



        }
        public void MoKhoa()
        {
            txtDonGiaBan.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtGhiChu.Enabled = true;
            txtHinh.Enabled = true;
            txtMaMon.Enabled = true;
            txtMoTa.Enabled = true;
            txtSoLuong.Enabled = true;
            txtTenMon.Enabled = true;
            dtNgayNhap.Enabled = true;
            cbbLoai.Enabled = true;
        }

        private void QL_ThucDon_Load(object sender, EventArgs e)
        {
            LoadData();
            Khoa();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            MoKhoa();
            btThemLoai.Enabled = true;
            ThemMoi = true;
            Xoa = false;
            btnXoa.Enabled = true;
            btLuu.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MoKhoa();
            ThemMoi = false;
            btLuu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (Xoa)
            {
                XoaMon(txtMaMon.Text);
            }
            else
            {
                LoadData();
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            string maloai = LayMaLoaiSachTuLoaiSach(cbbLoai.Text);
            if (themloai == false)
            {
                if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
                {
                    MessageBox.Show("Vui lòng điền loai sách.");
                }
                else
                {
                    ThemLoaiMon(txtTenLoai.Text);
                }

            }
            else if (string.IsNullOrWhiteSpace(txtMaMon.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô văn bản.");
            }
            else if (ThemMoi)
            {
                ThemMonVaoThucDon(txtMaMon.Text, txtTenMon.Text, Convert.ToInt32(txtSoLuong.Text), Convert.ToInt32(txtDonGiaNhap.Text), Convert.ToInt32(txtDonGiaBan.Text), DateTime.Now, txtHinh.Text, txtMoTa.Text, txtGhiChu.Text, "0001", maloai);
            }
            else if (ThemMoi == false)
            {
                CapNhatMonTrongThucDon(txtMaMon.Text, txtTenMon.Text, Convert.ToInt32(txtSoLuong.Text), Convert.ToInt32(txtDonGiaNhap.Text), Convert.ToInt32(txtDonGiaBan.Text), DateTime.Now, txtHinh.Text, txtMoTa.Text, txtGhiChu.Text, "0001", maloai);
            }

        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            try
            {
                TimSach(txtTK.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        private void dgvThucDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvThucDon.Rows[e.RowIndex];
                txtMaMon.Text = row.Cells["MaSach"].Value?.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString();
                txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value?.ToString();
                txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value?.ToString();
                dtNgayNhap.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);
                txtHinh.Text = row.Cells["HinhAnh"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                LoadDataToComboBox(row.Cells["MaLoai"].Value?.ToString());
                txtTenMon.Text = row.Cells["TenSach"].Value?.ToString();
            }
        }

        private void btThemLoaiSach_Click(object sender, EventArgs e)
        {
            themloai = false;
            txtTenLoai.Visible = true;
            btnXoa.Enabled = true;
            btLuu.Enabled = true;
        }

        
    }
}
