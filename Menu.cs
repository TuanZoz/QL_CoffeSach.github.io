using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DuAn1
{
    public partial class Menu : UserControl
    {
        public string TenSanPham { get; set; }
        private Main main;
        public Menu(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        private void Menu_Load(object sender, EventArgs e)
        {
            llbSach.LinkColor = System.Drawing.Color.Gray;
            llbThucDon.LinkColor = System.Drawing.Color.White;
            Loading();
        }
        public void Loading()
        {

            LoadSach();
            KiemTraGioHang(main.MaDn);
        }
        public void LoadSach()
        {
            string directoryPath = @"C:\Users\ductu\Downloads\DuAn1\DuAn1\Resources\Img_Sp";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                flowLayoutPanel1.Controls.Clear();
                string sqlQuery = "SELECT * FROM Sach";

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string Ma = reader["MaSach"].ToString();
                    string Ten = reader["TenSach"].ToString();
                    string HinhAnh = reader["HinhAnh"].ToString();
                    int giaBan = Int32.Parse(reader["DonGiaBan"].ToString());
                    string soluong = reader["SoLuong"].ToString();
                    GroupBox sp = new GroupBox();
                    Label tensp = new Label();
                    Button btThemVaoGioHang = new Button();
                    Label gia = new Label();
                    PictureBox hinhAnh = new PictureBox();
                    Label sl = new Label();

                    sp.Location = new Point(16, 214);
                    sp.Size = new Size(147, 211);
                    sp.Margin = new Padding(10);
                    sp.TabStop = false;

                    tensp.Size = new Size(59, 22);
                    tensp.Location = new Point(6, 142);
                    tensp.Text = Ten;

                    btThemVaoGioHang.Cursor = Cursors.Hand;
                    btThemVaoGioHang.Location = new Point(98, 165);
                    btThemVaoGioHang.Size = new Size(42, 32);
                    btThemVaoGioHang.Text = "";
                    btThemVaoGioHang.UseVisualStyleBackColor = true;
                    btThemVaoGioHang.Image = global::DuAn1.Properties.Resources._010_add_to_basket;
                    btThemVaoGioHang.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    btThemVaoGioHang.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    btThemVaoGioHang.Click += (sender, e) =>
                    {
                        string makh = main.MaDn;
                        ThemVaoGioHang(makh, Ten, 1, Ma, HinhAnh, "Sach", giaBan);
                    };
                    gia.Size = new Size(39, 22);
                    gia.AutoSize = true;
                    gia.Location = new Point(6, 175);
                    gia.Text = "₫" + giaBan.ToString("#,##0");

                    hinhAnh.Location = new Point(6, 18);
                    string selectedFileName = HinhAnh;
                    hinhAnh.Image = Image.FromFile(Path.Combine(directoryPath, selectedFileName));
                    hinhAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    hinhAnh.Size = new Size(134, 120);
                    hinhAnh.TabStop = false;

                    sl.Location = new Point(130, 5);
                    sl.Size = new Size(30, 22);
                    sl.Text = soluong;

                    sp.Controls.Add(tensp);
                    sp.Controls.Add(btThemVaoGioHang);
                    sp.Controls.Add(gia);
                    sp.Controls.Add(hinhAnh);
                    sp.Controls.Add(sl);
                    flowLayoutPanel1.Controls.Add(sp);
                }

                reader.Close();
            }
        }
      
        public void LoadThucDon()
        {
            string directoryPath = @"C:\Users\ductu\Downloads\DuAn1\DuAn1\Resources\Img_Mon";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                flowLayoutPanel1.Controls.Clear();
                string sqlQuery = "SELECT * FROM ThucDon";

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string Ma = reader["MaMon"].ToString();
                    string Ten = reader["TenMon"].ToString();
                    string HinhAnh = reader["HinhAnh"].ToString();
                    int giaBan = Int32.Parse(reader["DonGiaBan"].ToString());
                    string soluong = reader["SoLuong"].ToString();
                    GroupBox sp = new GroupBox();
                    Label tensp = new Label();
                    Button btThemVaoGioHang = new Button();
                    Label gia = new Label();
                    PictureBox hinhAnh = new PictureBox();
                    Label sl = new Label();

                    sp.Location = new Point(16, 214);
                    sp.Size = new Size(147, 211);
                    sp.Margin = new Padding(10);
                    sp.TabStop = false;

                    tensp.Size = new Size(59, 22);
                    tensp.Location = new Point(6, 142);
                    tensp.Text = Ten;

                    btThemVaoGioHang.Cursor = Cursors.Hand;
                    btThemVaoGioHang.Location = new Point(98, 165);
                    btThemVaoGioHang.Size = new Size(42, 32);
                    btThemVaoGioHang.Text = "";
                    btThemVaoGioHang.UseVisualStyleBackColor = true;
                    btThemVaoGioHang.Image = global::DuAn1.Properties.Resources._010_add_to_basket;
                    btThemVaoGioHang.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    btThemVaoGioHang.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    btThemVaoGioHang.Click += (sender, e) =>
                    {
                        string makh = main.MaDn;
                        ThemVaoGioHang(makh, Ten, 1, Ma, HinhAnh, "Mon", giaBan);
                    };
                    gia.Size = new Size(39, 22);
                    gia.AutoSize = true;
                    gia.Location = new Point(6, 175);
                    gia.Text = "₫" +giaBan.ToString("#,##0");

                    hinhAnh.Location = new Point(6, 18);
                    string selectedFileName = HinhAnh;
                    hinhAnh.Image = Image.FromFile(Path.Combine(directoryPath, selectedFileName));
                    hinhAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    hinhAnh.Size = new Size(134, 120);
                    hinhAnh.TabStop = false;

                    sl.Location = new Point(125, 7);
                    sl.Size = new Size(30, 22);
                    sl.Text = soluong;

                    sp.Controls.Add(tensp);
                    sp.Controls.Add(btThemVaoGioHang);
                    sp.Controls.Add(gia);
                    sp.Controls.Add(hinhAnh);
                    sp.Controls.Add(sl);
                    flowLayoutPanel1.Controls.Add(sp);
                }

                reader.Close();
            }
        }
        public void ThemVaoGioHang(string customerID, string productName, int quantity, string productID, string imagePath, string productType, int Price)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ThemVaoGioHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Makh", customerID);
                    command.Parameters.AddWithValue("@Tensp", productName);
                    command.Parameters.AddWithValue("@so_luong", quantity);
                    command.Parameters.AddWithValue("@MaSp", productID);
                    command.Parameters.AddWithValue("@HinhAnh", imagePath);
                    command.Parameters.AddWithValue("@Loai", productType);
                    command.Parameters.AddWithValue("@DonGia", Price);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Sản phẩm đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK);
                        KiemTraGioHang(main.MaDn);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Lỗi khi thêm sản phẩm vào giỏ hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void lbSach_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llbSach.LinkColor = System.Drawing.Color.Gray;
            llbThucDon.LinkColor = System.Drawing.Color.White;
            LoadSach();
        }

        private void lbThucDon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            llbSach.LinkColor = System.Drawing.Color.White;
            llbThucDon.LinkColor = System.Drawing.Color.Gray;
            LoadThucDon();
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
                                txtTong.Text = "₫" + (tongGiaTri).ToString("#,##0");
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            main.GioHang();
        }

       
    }
}
