﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuAn1
{
    public partial class QL_Sach : UserControl
    {
        private Main main;
        public QL_Sach(Main mainForm)
        {
            InitializeComponent();
            main = mainForm;
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public string directoryPath = @"C:\Users\ductu\Downloads\DuAn1\DuAn1\Resources\Img_Sp";
        public DataTable LoadData()
        {
            DataTable booksTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetSach", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvSach.DataSource = dataTable;
                connection.Open();
                adapter.Fill(booksTable);
                Khoa();
                LoadLoaiSachData();
                CleaData();
                ThemMoi = true;
                Xoa = true;
                themloaiSach = true;
            }
            

            string[] imageFiles = Directory.GetFiles(directoryPath, "*.png"); 
            foreach (string filePath in imageFiles)
            {
                string fileName = Path.GetFileName(filePath);
                cbbHinhAnh.Items.Add(fileName);
            }
            return booksTable;
        }

        public void ThemSach(string maSach, int soLuong, int donGiaNhap, int donGiaBan, DateTime ngayNhap,
                            string hinhAnh, int namXuatBan, string tacGia, string moTa, string ghiChu,
                            string maNv, string maLoai, string tenSach)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("ThemSach", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                    command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@HinhAnh", hinhAnh);
                    command.Parameters.AddWithValue("@NamXuatBan", namXuatBan);
                    command.Parameters.AddWithValue("@TacGia", tacGia);
                    command.Parameters.AddWithValue("@MoTa", moTa);
                    command.Parameters.AddWithValue("@GhiChu", ghiChu);
                    command.Parameters.AddWithValue("@MaNv", maNv);
                    command.Parameters.AddWithValue("@MaLoai", maLoai);
                    command.Parameters.AddWithValue("@TenSach", tenSach);
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
        public void ThemLoaiSach(string LoaiSach)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("ThemLoaiSach", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaSach", LayMaSach());
                    command.Parameters.AddWithValue("@Loai", LoaiSach);
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

        public void CapNhatSach(string maSach, int soLuong, int donGiaNhap, int donGiaBan, DateTime ngayNhap,
                               string hinhAnh, int namXuatBan, string tacGia, string moTa, string ghiChu,
                               string maNv, string maLoai, string tenSach)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("CapNhatSach", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                    command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@HinhAnh", hinhAnh);
                    command.Parameters.AddWithValue("@NamXuatBan", namXuatBan);
                    command.Parameters.AddWithValue("@TacGia", tacGia);
                    command.Parameters.AddWithValue("@MoTa", moTa);
                    command.Parameters.AddWithValue("@GhiChu", ghiChu);
                    command.Parameters.AddWithValue("@MaNv", maNv);
                    command.Parameters.AddWithValue("@MaLoai", maLoai);
                    command.Parameters.AddWithValue("@TenSach", tenSach);
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                }
            }
        }

        public void XoaSach(string maSach)
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
                        SqlCommand command = new SqlCommand("XoaSach", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MaSach", maSach);
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
        public DataTable XemLoaiSach()
        {
            DataTable result = new DataTable(); 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetLoaiSach", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(result);
            }
            return result; 
        }

        public void LoadLoaiSachData()
        {
            DataTable loaiSachTable = XemLoaiSach(); 

            if (loaiSachTable != null && loaiSachTable.Rows.Count > 0)
            {
                cbbLoaiSach.Items.Clear();
                foreach (DataRow row in loaiSachTable.Rows)
                {
                    string Loai = row["LoaiSach"].ToString();
                    cbbLoaiSach.Items.Add(Loai);
                }

                cbbLoaiSach.SelectedIndex = 0; 
            }
        }
        public void TimSach(string tuKhoa)    
        {
            DataTable booksTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                booksTable.Clear();
                SqlCommand command = new SqlCommand("timKiemSach", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TuKhoa", tuKhoa);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvSach.DataSource = dataTable;
                connection.Open();
                adapter.Fill(booksTable);
            }
        }
        public string LayMaSach()
        {
            DataTable XemLoaiSachTable = XemLoaiSach();
            List<int> existingMaLoaiValues = new List<int>();

            if (XemLoaiSachTable != null && XemLoaiSachTable.Rows.Count > 0)
            {
                foreach (DataRow row in XemLoaiSachTable.Rows)
                {
                    string maLoai = row["MaLoai"].ToString();
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
            int newMaSach = 1;
            while (existingMaLoaiValues.Contains(newMaSach))
            {
                newMaSach++;
            }

            return newMaSach.ToString();
        }
        private void LoadDataToComboBox(string bien)
        {
            string loaiSach = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT LoaiSach FROM LoaiSach WHERE MaLoai = @MaLoai";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaLoai", bien);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        loaiSach = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            string maLoaiCanTim = LayMaLoaiSachTuLoaiSach(loaiSach);
            foreach (var item in cbbLoaiSach.Items)
            {
                string itemText = cbbLoaiSach.GetItemText(item);
                string maLoaiComboBox = LayMaLoaiSachTuLoaiSach(itemText);

                if (maLoaiComboBox == maLoaiCanTim)
                {
                    cbbLoaiSach.SelectedItem = item;
                    break;
                }
            }
        }

        private string LayMaLoaiSachTuLoaiSach(string loaiSach)
        {
            string maLoai = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MaLoai FROM LoaiSach WHERE LoaiSach = @LoaiSach";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LoaiSach", loaiSach);

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
        public bool themloaiSach = true;
        public void Khoa()
        {
            txtDonGiaBan.Enabled = false;
            txtDonGiaNhap.Enabled = false;
            txtGhiChu.Enabled = false;
            txtMaSach.Enabled = false;
            txtMoTa.Enabled = false;
            txtSoLuong.Enabled = false;
            txtTacGia.Enabled = false;
            txtTenSach.Enabled = false;
            dtNgayNhap.Enabled = false;
            txtNamXB.Enabled = false;  
            cbbLoaiSach.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = false;
            txtTenLoaiSach.Visible = false;
            btThemLoaiSach.Enabled=false;

        }
        public void CleaData()
        {
            txtTenLoaiSach.Text = "";
            txtDonGiaBan.Text = "";
            txtDonGiaNhap.Text = "";
            txtGhiChu.Text = "";
            txtMaSach.Text = "";
            txtMoTa.Text = "";
            txtSoLuong.Text = "";
            txtTacGia.Text = "";
            txtTenSach.Text = "";
            dtNgayNhap.Text = "";
            txtNamXB.Text = "";

            

        }
        public void MoKhoa()
        {
            txtDonGiaBan.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            txtGhiChu.Enabled = true;
            txtMaSach.Enabled = true;
            txtMoTa.Enabled = true;
            txtSoLuong.Enabled = true;
            txtTacGia.Enabled = true;
            txtTenSach.Enabled = true;
            txtNamXB.Enabled = true;
            cbbLoaiSach.Enabled=true;
        }

        private void QL_Sach_Load(object sender, EventArgs e)
        {
           LoadData();
            Khoa();
        }
        private string GenerateUniqueMaSach()
        {
            string newMaSach = "MS001";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                while (true)
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Sach WHERE MaSach = @MaSach", connection);
                    command.Parameters.AddWithValue("@MaSach", newMaSach);
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        newMaSach = GenerateRandomMaSach();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return newMaSach;
        }

        private string GenerateRandomMaSach()
        {
            Random random = new Random();
            string chars = "0123456789";
            return "MS" + new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            MoKhoa();
            btThemLoaiSach.Enabled = true;
            ThemMoi = true;
            Xoa = false;
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            txtMaSach.Text = GenerateUniqueMaSach();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MoKhoa();
            ThemMoi = false;
            btnLuu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (Xoa)
            {
                XoaSach(txtMaSach.Text);
            }
            else
            {
                LoadData();
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
        private void btLuu_Click(object sender, EventArgs e)
        {
            string maloai = LayMaLoaiSachTuLoaiSach(cbbLoaiSach.Text);
            string manv = main.MaDn;
            if (themloaiSach==false)
            {
                if (string.IsNullOrWhiteSpace(txtTenLoaiSach.Text))
                {
                    MessageBox.Show("Vui lòng điền loai sách.");
                }
                else
                {
                    ThemLoaiSach(txtTenLoaiSach.Text);
                }

            }
            else if (string.IsNullOrWhiteSpace(txtMaSach.Text)||
                string.IsNullOrWhiteSpace(txtDonGiaBan.Text) ||
                string.IsNullOrWhiteSpace(txtDonGiaNhap.Text) ||
                string.IsNullOrWhiteSpace(txtNamXB.Text) ||
                string.IsNullOrWhiteSpace(txtTacGia.Text) ||
                string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô văn bản.");
            }
            else if (!KiemTraSo(txtSoLuong.Text))
            {
                MessageBox.Show("Số lượng không hợp lê.");
            }
            else if (!KiemTraSo(txtDonGiaBan.Text))
            {
                MessageBox.Show("Đơn giá bán không hợp lê.");
            }
            else if (!KiemTraSo(txtDonGiaNhap.Text))
            {
                MessageBox.Show("Đơn giá nhập không hợp lê.");
            }
            else if (ThemMoi)
            {
                ThemSach(txtMaSach.Text, Convert.ToInt32(txtSoLuong.Text), Convert.ToInt32(txtDonGiaNhap.Text), Convert.ToInt32(txtDonGiaBan.Text), Convert.ToDateTime(dtNgayNhap.Text), cbbHinhAnh.Text, Convert.ToInt32(txtNamXB.Text), txtTacGia.Text, txtMoTa.Text, txtGhiChu.Text, manv, maloai, txtTenSach.Text);
                LoadData();
            }
            else if ( ThemMoi== false)
            {
               CapNhatSach(txtMaSach.Text, Convert.ToInt32(txtSoLuong.Text), Convert.ToInt32(txtDonGiaNhap.Text), Convert.ToInt32(txtDonGiaBan.Text), Convert.ToDateTime(dtNgayNhap.Text), cbbHinhAnh.Text, Convert.ToInt32(txtNamXB.Text), txtTacGia.Text, txtMoTa.Text, txtGhiChu.Text, manv, maloai, txtTenSach.Text);
                LoadData();
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

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow row = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value?.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString();
                txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value?.ToString();
                txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value?.ToString();
                dtNgayNhap.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);
                cbbHinhAnh.Text = row.Cells["HinhAnh"].Value?.ToString();
                txtNamXB.Text = row.Cells["NamXuatBan"].Value?.ToString();
                txtTacGia.Text = row.Cells["TacGia"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                LoadDataToComboBox(row.Cells["MaLoai"].Value?.ToString());
                txtTenSach.Text = row.Cells["TenSach"].Value?.ToString();
                string selectedFileName = cbbHinhAnh.SelectedItem.ToString();
                pcbSanPham.Image = Image.FromFile(Path.Combine(directoryPath, selectedFileName));
            }
        }

        private void btThemLoaiSach_Click(object sender, EventArgs e)
        {
            if(txtTenLoaiSach.Visible)
            {
                themloaiSach = true;
                txtTenLoaiSach.Visible = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
            }
            else
            {
                themloaiSach = false;
                txtTenLoaiSach.Visible = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = true;
            }
        }

        private void cbbHinhAnh_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFileName = cbbHinhAnh.SelectedItem.ToString();
            pcbSanPham.Image = Image.FromFile(Path.Combine(directoryPath, selectedFileName));
        }

      
    }
}
