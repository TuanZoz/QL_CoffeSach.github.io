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

    public partial class GioHang : UserControl
    {
        private Main main;
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        public GioHang(Main main)
        {
            InitializeComponent();
            this.main = main;
            HienThiGioHang();
            CapNhatTong();
        }

        private void HienThiGioHang()
        {
            GroupBox groupBox2 = new GroupBox();
            groupBox2.Location = new System.Drawing.Point(43, 3);
            groupBox2.Size = new System.Drawing.Size(770, 38);

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

            Label label6 = new Label();
            label6.AutoSize = true;
            label6.BackColor = System.Drawing.Color.Transparent;
            label6.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label6.ForeColor = System.Drawing.Color.DimGray;
            label6.Location = new System.Drawing.Point(670, 10);
            label6.Name = "label6";
            label6.Text = "Thao Tác";

            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            flowLayoutPanel1.Controls.Add(groupBox2);
            Controls.Add(flowLayoutPanel1);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id, Tensp, so_luong, HinhAnh, DonGia, Loai FROM GioHang WHERE Makh = @MaKhachHang";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKhachHang", main.MaDn);

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
        private void CapNhatTong()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_KiemTraSoLuongVaTongGiaTriSPTrongGioHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Makh", main.MaDn);

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
                                LbTong.Text = "Tổng thanh toán(" + soLuong.ToString() + "):";
                                txtTong.Text = "₫" + (tongGiaTri).ToString("#,##0");
                            }
                        }
                        else
                        {
                            LbTong.Text = "Tổng thanh toán(0):";
                            txtTong.Text = "0";
                        }

                        reader.Close();
                    }
                    catch
                    {
                        LbTong.Text = "Tổng thanh toán(0):";
                        txtTong.Text = "0";
                    }
                }
            }
        }
        private void ThemSanPhamVaoGioHang(int id, string tenSanPham, int giaTong, int DonGa, int soLuong, string img, string Loai)
        {

            GroupBox groupBox = new GroupBox();
            groupBox.Location = new Point(3, 3);
            groupBox.Size = new Size(770, 67);
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

            NumericUpDown numericUpDown = new NumericUpDown();
            numericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            numericUpDown.Location = new Point(370, 24);
            numericUpDown.Size = new Size(80, 30);
            numericUpDown.Value = soLuong;
            numericUpDown.Minimum = 1;
            numericUpDown.ValueChanged += (s, ev) =>
            {
                int newValue = (int)numericUpDown.Value;
                soLuong = newValue;
                int tong = soLuong * DonGa;
                Totalprice.Text = "₫" + (tong).ToString("#,##0");
                CapNhatSoLuong(id,soLuong, tong);
                CapNhatTong();
            };
            groupBox.Controls.Add(numericUpDown);


            Button deleteButton = new Button();
            deleteButton.Location = new Point(700, 12);
            deleteButton.Size = new Size(58, 49);
            deleteButton.Text = "Xóa";
            groupBox.Controls.Add(deleteButton);
            deleteButton.Click += (sender, e) =>
            {
                XoaSanPhamKhoiGioHang(id);
                flowLayoutPanel1.Controls.Clear();
                HienThiGioHang();
                CapNhatTong();
            };

            flowLayoutPanel1.Controls.Add(groupBox);
            Controls.Add(flowLayoutPanel1);
        }

        private void XoaSanPhamKhoiGioHang(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM GioHang WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
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
        public void CapNhatSoLuong(int id, int soluong, int giaTong)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_CapNhatGioHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id); 
                    command.Parameters.AddWithValue("@so_luong", soluong); 
                    command.Parameters.AddWithValue("@giaTong", giaTong); 

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        private void btnMuaHang_Click(object sender, EventArgs e)
        {
            main.ThanhToan();
        }
    }
}

    

