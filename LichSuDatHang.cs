using System;
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
    public partial class LichSuDatHang : UserControl
    {
        private Main Main;
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public LichSuDatHang(Main main)
        {
            InitializeComponent();
            Main = main;
            HienThiGioHang(null);
        }
        private void LichSuDatHang_Load(object sender, EventArgs e)
        {
            HienThiGioHang(null);
        }
        public class DonHangInfo
        {
            public string MaDonHang { get; set; }
            public DateTime Ngay { get; set; }
            public string TrangThai { get; set; }
            public decimal TongGiaTri { get; set; }
        }
        List<DonHangInfo> maDonHangList = new List<DonHangInfo>();
        private void HienThiGioHang( string Trangthai)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM DonHang WHERE Makh = @MaKhachHang";
                if (Trangthai != null)
                {
                    query += " AND TrangThai = @TrangThai";
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKhachHang", Main.MaDn);
                    if (Trangthai != null)
                    {
                        command.Parameters.AddWithValue("@TrangThai", Trangthai);
                    }
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        flowLayoutPanel1.Controls.Clear();
                        maDonHangList.Clear();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("MaDh")) && !reader.IsDBNull(reader.GetOrdinal("NgayDatHang")) &&
                                !reader.IsDBNull(reader.GetOrdinal("TrangThai")) && !reader.IsDBNull(reader.GetOrdinal("TongGiaTri")))
                            {
                                DonHangInfo donHangInfo = new DonHangInfo
                                {
                                    MaDonHang = reader["MaDh"].ToString(),
                                    Ngay = Convert.ToDateTime(reader["NgayDatHang"]),
                                    TrangThai = reader["TrangThai"].ToString(),
                                    TongGiaTri = Convert.ToDecimal(reader["TongGiaTri"])
                                };

                                maDonHangList.Add(donHangInfo);
                            }
                        }
                        if (maDonHangList.Count == 0)
                        {
                            flowLayoutPanel1.Controls.Clear();
                        }
                        reader.Close();
                        foreach (DonHangInfo info in maDonHangList)
                        {
                            GroupBox groupBox2 = new GroupBox();
                            groupBox2.Location = new System.Drawing.Point(43, 3);
                            groupBox2.Size = new System.Drawing.Size(700, 38);

                            Label label2 = new Label();
                            label2.AutoSize = true;
                            label2.BackColor = System.Drawing.Color.Transparent;
                            label2.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            label2.ForeColor = System.Drawing.Color.DimGray;
                            label2.Location = new System.Drawing.Point(3, 10);
                            label2.Name = "label2";

                            Label label3 = new Label();
                            label3.AutoSize = true;
                            label3.BackColor = System.Drawing.Color.Transparent;
                            label3.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            label3.ForeColor = System.Drawing.Color.DimGray;
                            label3.Location = new System.Drawing.Point(225, 10);
                            label3.Name = "label3";
                            Label label4 = new Label();
                            label4.AutoSize = true;
                            label4.BackColor = System.Drawing.Color.Transparent;
                            label4.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            label4.ForeColor = System.Drawing.Color.DimGray;
                            label4.Location = new System.Drawing.Point(370, 10);
                            label4.Name = "label4";

                            LinkLabel label5 = new LinkLabel();
                            label5.AutoSize = true;
                            label5.BackColor = System.Drawing.Color.Transparent;
                            label5.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            label5.ForeColor = System.Drawing.Color.DimGray;
                            label5.LinkColor = Color.Red;
                            label5.ActiveLinkColor = Color.DimGray;
                            label5.LinkBehavior = LinkBehavior.NeverUnderline;
                            label5.Location = new System.Drawing.Point(540, 10);
                            label5.Name = "label5";
                            label2.Text = info.MaDonHang;
                            label3.Text = info.Ngay.ToString("dd/MM/yyyy");
                            label4.Text = info.TrangThai;

                            groupBox2.Controls.Add(label2);
                            groupBox2.Controls.Add(label3);
                            groupBox2.Controls.Add(label4);
                            if (info.TrangThai != "Đã nhận hàng" && info.TrangThai != "Đã Hủy")
                            {
                                label5.LinkClicked += (sender, e) => Huy_LinkClicked(sender, e, info.MaDonHang);
                                label5.Text = "Hủy";
                            }

                            groupBox2.Controls.Add(label5);
                            flowLayoutPanel1.Controls.Add(groupBox2);
                            Controls.Add(flowLayoutPanel1);
                            LayChiTietThucDon(info.MaDonHang);
                            LayChiTietSach(info.MaDonHang);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }
        private void Huy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e, string Ma)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn hàng này?", "Xác nhận hủy đơn hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                HuyDonHang(Ma);
                Main.LichSuMua();
            }
            else
            {

            }
        }
        private void HuyDonHang(string maDonHang)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE DonHang SET TrangThai = N'Đã Hủy' WHERE MaDh = @MaDonHang";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaDonHang", maDonHang);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã cập nhật trạng thái đơn hàng thành 'Đã Hủy'.");
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật trạng thái đơn hàng.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void LayChiTietSach(string madh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                List<DonHangInfo> maDonHangList = new List<DonHangInfo>();
                string query = "SELECT TenSach,ChitietDh_Sach.SoLuong,GiaTien, HinhAnh " +
                               "FROM DonHang " +
                               "LEFT JOIN ChiTietDH_Sach ON DonHang.MaDh = ChiTietDH_Sach.MaDh " +
                               "LEFT JOIN Sach ON Sach.MaSach = ChiTietDH_Sach.MaSach " +
                               "WHERE DonHang.MaDh = @MaDh";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaDh", madh);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string tenSanPham = reader["TenSach"].ToString();
                            int DonGa = Convert.ToInt32(reader["GiaTien"].ToString());
                            int soLuong = Convert.ToInt32(reader["SoLuong"].ToString());
                            int giaTong = DonGa * soLuong;
                            string img = reader["HinhAnh"].ToString();
                            string Loai = "Sach";
                            ThemSanPhamVaoGioHang(tenSanPham, giaTong, DonGa, soLuong, img, Loai);
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
        private void LayChiTietThucDon(string madh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                List<DonHangInfo> maDonHangList = new List<DonHangInfo>();
                string query = "SELECT ThucDon.TenMon, ChitietDh_ThucDon.SoLuong, GiaTien, HinhAnh " +
                               "FROM DonHang " +
                               "LEFT JOIN ChitietDh_ThucDon ON DonHang.MaDh = ChitietDh_ThucDon.MaDh " +
                               "LEFT JOIN ThucDon ON ThucDon.MaMon = ChitietDh_ThucDon.MaMon " +
                               "WHERE DonHang.MaDh = @MaDh";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaDh",madh);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            string tenSanPham = reader["TenMon"].ToString();
                            int DonGa = Convert.ToInt32(reader["GiaTien"].ToString());
                            int soLuong = Convert.ToInt32(reader["SoLuong"].ToString());
                            int giaTong = DonGa * soLuong;
                            string img = reader["HinhAnh"].ToString();
                            string Loai= "ThucDon";
                            ThemSanPhamVaoGioHang(tenSanPham, giaTong, DonGa, soLuong, img, Loai);
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
        private void ThemSanPhamVaoGioHang( string tenSanPham, int giaTong, int DonGa, int soLuong, string img, string Loai)
        {

            GroupBox groupBox = new GroupBox();
            groupBox.Location = new Point(3, 3);
            groupBox.Size = new Size(700, 67);
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

        private void btnDangSuLy_Click(object sender, EventArgs e)
        {
            HienThiGioHang("Đang xử lý");
        }

        private void btnDangVanChuyen_Click(object sender, EventArgs e)
        {
            HienThiGioHang("Đang vận chuyển");
        }

        private void btnDaHuy_Click(object sender, EventArgs e)
        {
            HienThiGioHang("Đã Hủy");
        }

        private void btnDaMua_Click(object sender, EventArgs e)
        {
            HienThiGioHang("Đã nhận hàng");
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            HienThiGioHang(null);
        }

        
    }
}
