using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1
{
    public partial class ThanhToan : UserControl
    {
        public Main Main;
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public int TongGiaTien = 0;
        public ThanhToan(Main main)
        {
            InitializeComponent();
            Main = main;
            HienThiGioHang();
            CapNhatTong();
            gbTheNganHang.Visible = false;
            gbTTKhiNhanHang.Visible = true;
            gbViDienTu.Visible = false;
            LayDiaChi();
        }
        private void HienThiGioHang()
        {
            GroupBox groupBox2 = new GroupBox();
            groupBox2.Location = new System.Drawing.Point(43, 3);
            groupBox2.Size = new System.Drawing.Size(700, 38);

            Label label2 = new Label();
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.ForeColor = System.Drawing.Color.DimGray;
            label2.Location = new System.Drawing.Point(57, 10);
            label2.Name = "label2";
            label2.Text = "Sản Phẩm";

            Label label3 = new Label();
            label3.AutoSize = true;
            label3.BackColor = System.Drawing.Color.Transparent;
            label3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.ForeColor = System.Drawing.Color.DimGray;
            label3.Location = new System.Drawing.Point(225, 10);
            label3.Name = "label3";
            label3.Text = "Đơn Giá";

            Label label4 = new Label();
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label4.ForeColor = System.Drawing.Color.DimGray;
            label4.Location = new System.Drawing.Point(370, 10);
            label4.Name = "label4";
            label4.Text = "Số Lượng";

            Label label5 = new Label();
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.Color.Transparent;
            label5.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label5.ForeColor = System.Drawing.Color.DimGray;
            label5.Location = new System.Drawing.Point(540, 10);
            label5.Name = "label5";
            label5.Text = "Tổng";

            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label5);
            flowLayoutPanel1.Controls.Add(groupBox2);
            Controls.Add(flowLayoutPanel1);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id, Tensp, so_luong, HinhAnh, DonGia, Loai FROM GioHang WHERE Makh = @MaKhachHang";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKhachHang", Main.MaDn);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            int DonGia = Convert.ToInt32(reader["DonGia"]); ;
                            string tenSanPham = reader["Tensp"].ToString();
                            int soLuong = Convert.ToInt32(reader["so_luong"]);
                            int gia = DonGia * soLuong;
                            string imgPath = reader["HinhAnh"].ToString();
                            string Loai = reader["Loai"].ToString(); ;
                            ThemSanPhamVaoGioHang(id, tenSanPham, gia, DonGia, soLuong, imgPath, Loai);
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
        public void LayDiaChi()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TenKhach,DiaChi,DienThoai FROM KhachHang WHERE Makh = @MaKhachHang";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKhachHang", Main.MaDn);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string Ten = reader["TenKhach"].ToString();
                            string SDT = reader["DienThoai"].ToString(); ;
                            string DiaChi = reader["DiaChi"].ToString();
                            txtTenVaSDT.Text = Ten+ " (+84) "+ SDT;
                            txtDiaChi.Text = DiaChi;
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
        private void CapNhatTong()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_KiemTraSoLuongVaTongGiaTriSPTrongGioHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Makh", Main.MaDn);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int tongGiaTri = Convert.ToInt32(reader["TongGiaTriSanPhamTrongGioHang"]);
                                lbTong.Text = "₫" + (tongGiaTri).ToString("#,##0");
                                TongGiaTien = tongGiaTri + 30000;
                                lbTongGiaTriDH.Text = "₫" + (TongGiaTien).ToString("#,##0");
                            }
                        }
                        else
                        {
                            lbTong.Text = "0";
                        }

                        reader.Close();
                    }
                    catch
                    {
                        lbTong.Text = "0";
                    }
                }
            }
        }
        private void ThemSanPhamVaoGioHang(int id, string tenSanPham, int giaTong, int DonGa, int soLuong, string img, string Loai)
        {

            GroupBox groupBox = new GroupBox();
            groupBox.Location = new Point(3, 3);
            groupBox.Size = new Size(700, 67);
            groupBox.Padding = new Padding(30);
            this.Controls.Add(groupBox);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(6, 10);
            pictureBox.Size = new Size(74, 51);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = Image.FromFile(LoadHinh(img, Loai));
            groupBox.Controls.Add(pictureBox);

            Label nameLabel = new Label();
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            nameLabel.Location = new Point(86, 14);
            nameLabel.Size = new Size(64, 25);
            nameLabel.Text = tenSanPham;
            groupBox.Controls.Add(nameLabel);

            Label priceLabel = new Label();
            priceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            priceLabel.Location = new Point(225, 26);
            priceLabel.Size = new Size(53, 20);
            priceLabel.AutoSize = true;
            priceLabel.Text = "₫" + DonGa.ToString("#,##0");
            groupBox.Controls.Add(priceLabel);

            Label Totalprice = new Label();
            Totalprice.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Totalprice.ForeColor = System.Drawing.Color.Red;
            priceLabel.AutoSize = true;
            Totalprice.Location = new Point(524, 24);
            Totalprice.Size = new Size(120, 32);
            Totalprice.Text = "₫" + giaTong.ToString("#,##0");
            groupBox.Controls.Add(Totalprice);

            Label SoLuong = new Label();
            SoLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            SoLuong.Location = new Point(415, 24);
            SoLuong.Size = new Size(80, 30);
            SoLuong.Text = soLuong.ToString();
            groupBox.Controls.Add(SoLuong);
            flowLayoutPanel1.Controls.Add(groupBox);
            Controls.Add(flowLayoutPanel1);
        }
        private string LoadHinh(string imageName, string Loai)
        {
            string directoryPath;
            if (Loai == "Sach")
            {
                directoryPath = @"C:\Users\ductu\Downloads\DuAn1\DuAn1\Resources\Img_Sp";
            }
            else
            {
                directoryPath = @"C:\Users\ductu\Downloads\DuAn1\DuAn1\Resources\Img_Mon";
            }
            string imagePath = Path.Combine(directoryPath, imageName);

            return imagePath;
        }
        public void ThemDonHang(string MaDh, DateTime NgayDatHang, int TongGiaTri, string TrangThai, string GhiChu, string MaKh, string TenVaSDT, string DiaChi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("ThemDonHang", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaDh", MaDh);
                    cmd.Parameters.AddWithValue("@NgayDatHang", NgayDatHang);
                    cmd.Parameters.AddWithValue("@TongGiaTri", TongGiaTri);
                    cmd.Parameters.AddWithValue("@TrangThai", TrangThai);
                    cmd.Parameters.AddWithValue("@GhiChu", GhiChu);
                    cmd.Parameters.AddWithValue("@MaKh", MaKh);
                    cmd.Parameters.AddWithValue("@TenVaSDT", TenVaSDT);
                    cmd.Parameters.AddWithValue("@DiaChi", DiaChi);
                    cmd.ExecuteNonQuery();
                    LuuChiTietDonHang(MaDh);
                    MessageBox.Show("Đặt hàng thành công!");
                    XoaSanPhamKhoiGioHang(MaKh);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm : " + ex.Message);
                }
            }
        }
        public class ChiTietDonHang
        {
            public string query { get; set; }
            public string MaDh { get; set; }
            public string Temp { get; set; }
            public string ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }
        List<ChiTietDonHang> chiTietDonHangList = new List<ChiTietDonHang>();
        private void LuuChiTietDonHang(string Madh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT MaSp, so_luong, DonGia, Loai FROM GioHang WHERE Makh = @MaKhachHang", connection);
                    command.Parameters.AddWithValue("@MaKhachHang", Main.MaDn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int quantity = Convert.ToInt32(reader["so_luong"]);
                            string productId = reader["MaSp"].ToString();
                            decimal productPrice = Convert.ToDecimal(reader["DonGia"]);
                            string Loai = reader["Loai"].ToString();
                            string query1 = "";
                            string temp = "";
                            string Machitiet = "";

                            if (Loai == "Sach")
                            {
                                query1 = "ThemChitietDh_Sach";
                                temp = "MaSach";
                                Machitiet = TaoMaCTDHSach();
                            }
                            else
                            {
                                query1 = "ThemChitietDh_ThucDon";
                                temp = "MaMon";
                                Machitiet = TaoMaCTDHThucDon();
                            }
                            decimal Total = quantity * productPrice;
                            ChiTietDonHang chiTietDonHang = new ChiTietDonHang
                            {
                                query = query1,
                                MaDh = Madh,
                                Temp = temp,
                                ProductId = productId,
                                Quantity = quantity,
                                Total = Total
                            };

                            chiTietDonHangList.Add(chiTietDonHang);

                        }
                    }

                    foreach (ChiTietDonHang item in chiTietDonHangList)
                    {
                        InsertChiTietDonHang(connection, item.query, item.MaDh, item.Temp, item.ProductId, item.Quantity, item.Total);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        private void InsertChiTietDonHang(SqlConnection connection, string query, string Madh, string temp, string productId, int quantity, decimal Total)
        {
            try
            {
                string Machitiet;
                if (temp == "MaSach")
                {
                    
                    Machitiet = TaoMaCTDHSach();
                }
                else
                {
                    Machitiet = TaoMaCTDHThucDon();
                }
                using (SqlCommand command1 = new SqlCommand(query, connection))
                {
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@MaChiTiet", Machitiet);
                    command1.Parameters.AddWithValue("@MaDh", Madh);
                    command1.Parameters.AddWithValue(temp, productId);
                    command1.Parameters.AddWithValue("@SoLuong", quantity);
                    command1.Parameters.AddWithValue("@GiaTien", Total);
                    command1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting detail: " + ex.Message);
            }
        }

        private void XoaSanPhamKhoiGioHang(string id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM GioHang WHERE MaKh = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }

        public string TaoMaCTDHSach()
        {
            int newMa = 1;
            DataTable Table = MaCTDHSach();
            List<int> MaCTSach = new List<int>();

            if (Table != null && Table.Rows.Count > 0)
            {
                foreach (DataRow row in Table.Rows)
                {
                    string maLoai = row["MaChiTiet"].ToString();
                    int temp;
                    if (int.TryParse(maLoai, out temp))
                    {
                        MaCTSach.Add(temp);
                    }
                    else
                    {
                        Console.WriteLine("Chuỗi không thể chuyển đổi thành số nguyên.");
                    }
                }
            }
            while (MaCTSach.Contains(newMa))
            {
                newMa++;
            }
            return newMa.ToString();
        }
        public DataTable MaCTDHSach()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();
                string query = "SELECT * FROM ChitietDh_Sach";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        public string TaoMaCTDHThucDon()
        {
            int newMa = 1;
            DataTable Table = MaCTDHThucDon();
            List<int> MaCTThucDon = new List<int>();

            if (Table != null && Table.Rows.Count > 0)
            {
                foreach (DataRow row in Table.Rows)
                {
                    string maLoai = row["MaChiTiet"].ToString();
                    int temp;
                    if (int.TryParse(maLoai, out temp))
                    {
                        MaCTThucDon.Add(temp);
                    }
                    else
                    {
                        Console.WriteLine("Chuỗi không thể chuyển đổi thành số nguyên.");
                    }
                }
            }
            while (MaCTThucDon.Contains(newMa))
            {
                newMa++;
            }
            return newMa.ToString();
        }
        public DataTable MaCTDHThucDon()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();
                string query = "SELECT * FROM ChitietDh_ThucDon";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        
        private void btnViDienTu_Click(object sender, EventArgs e)
        {
            gbTheNganHang.Visible = false;
            gbTTKhiNhanHang.Visible = false;
            gbViDienTu.Visible = true;
        }

        private void btnTheNganHang_Click(object sender, EventArgs e)
        {
            gbTheNganHang.Visible = true;
            gbTTKhiNhanHang.Visible = false;
            gbViDienTu.Visible = false;
        }

        private void btnTTKNH_Click(object sender, EventArgs e)
        {
            gbTheNganHang.Visible = false;
            gbTTKhiNhanHang.Visible = true;
            gbViDienTu.Visible = false;
        }
        private Random random = new Random();
        public string layMa(string maKhachHang)
        {
            string generatedCode = TaoLaiMa(maKhachHang);
            while (KiemTraMa(generatedCode))
            {
                generatedCode = TaoLaiMa(maKhachHang);
            }

            return generatedCode;
        }

        private string TaoLaiMa(string maKhachHang)
        {
            string twoRandomChars = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string ngayHomNay = DateTime.Now.ToString("ddMMyyyy");

            string threeRandomDigits = random.Next(100, 999).ToString();

            return $"{twoRandomChars}{maKhachHang}{ngayHomNay}{threeRandomDigits}";
        }

        private bool KiemTraMa(string code)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM DonHang WHERE MaDh = @Code"; 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        private void BtnDatHang_Click(object sender, EventArgs e)
        {
            string ma = layMa(Main.MaDn);
            DialogResult result = MessageBox.Show("Bạn có muốn xác nhận đặt hàng!", "Xác nhận đặt hàng", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                ThemDonHang(ma, DateTime.Now , TongGiaTien , "Đang xử lý", "  ",Main.MaDn, txtTenVaSDT.Text, txtDiaChi.Text);
            }
            Main.GioHang();
        }

        private void lbThayDoiDC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtDiaChi.ReadOnly = false;
            txtTenVaSDT.ReadOnly = false;
        }
    }

}
