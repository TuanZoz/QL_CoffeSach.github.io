using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace DuAn1
{
    public partial class QL_DonHang : UserControl
    {
        public string MaDonhang;
        public string trangThaicu;
        public QL_DonHang()
        {
            InitializeComponent();
            LoadData();
            LoadMaKHData();
        }
        public const string connectionString = "Data Source=TUNZZ;Initial Catalog=QLCaffe_Sach;Integrated Security=True";
        private void QL_DonHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("GetDonHang", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTableKH = new DataTable();
                adapter.Fill(dataTableKH);
                DgvDonHang.DataSource = dataTableKH;
                Khoa();
                CleaData();
                ThemMoi = true;
            }
        }
        public void ThemDonHang(string MaDh, DateTime NgayDatHang,int TongGiaTri,string TrangThai,string GhiChu,string MaKh, string TenVaSDT, string DiaChi)
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
                    MessageBox.Show("Thêm thành công!");
                    Khoa();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thêm : " + ex.Message);
                }
            }
        }
        public void CapNhatDonHang(string MaDh, DateTime NgayDatHang, int TongGiaTri, string TrangThai, string GhiChu, string MaKh,string TenVaSDT,string DiaChi)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("CapNhatDonHang", connection);
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
                    MessageBox.Show("Cập nhật thành công!");
                    Khoa();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật: " + ex.Message);
                }
            }
        }
        public void XoaDonHang(string MaDh)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        SqlCommand cmd = new SqlCommand("XoaDonHang", connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MaDh", MaDh);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
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
        public void LoadMaKHData()
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT MaKh FROM KhachHang ", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(result);
            }
            if (result != null && result.Rows.Count > 0)
            {
                cbbMaKH.Items.Clear();
                foreach (DataRow row in result.Rows)
                {
                    string MaKh = row["MaKh"].ToString();
                    cbbMaKH.Items.Add(MaKh);
                }

                cbbMaKH.SelectedIndex = 0;
            }
        }
        List<Tuple<string, int>> cartItems = new List<Tuple<string, int>>();
        Dictionary<Button, Tuple<ComboBox, NumericUpDown>> productControls = new Dictionary<Button, Tuple<ComboBox, NumericUpDown>>();
        private void btThemSP_Click(object sender, EventArgs e)
        {
            ChooseItem chooseItem = new ChooseItem();
            if (chooseItem.ShowDialog() == DialogResult.OK)
            {
                string selectedType = chooseItem.SelectedItemType;
                DataTable dt = new DataTable();
                string displayMember = "";
                string valueMember = "";
                string query = "";
                if (selectedType == "Sách")
                {
                    query = "SELECT TenSach FROM Sach";
                    displayMember = "TenSach";
                    valueMember = "TenSach";
                }
                else if (selectedType == "Thực Đơn")
                {
                    query = "SELECT TenMon FROM ThucDon";
                    displayMember = "TenMon";
                    valueMember = "TenMon";
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return;
                }
                ComboBox comboBoxSp = new ComboBox();
                comboBoxSp.Size = new System.Drawing.Size(90, 34);
                comboBoxSp.Margin = new Padding(5);
                comboBoxSp.DisplayMember = displayMember;
                comboBoxSp.ValueMember = valueMember;
                comboBoxSp.DataSource = dt;

                NumericUpDown numericUpDown = new NumericUpDown();
                numericUpDown.Size = new System.Drawing.Size(30, 20);
                numericUpDown.Margin = new Padding(5);
                numericUpDown.Value = 1;
                numericUpDown.ValueChanged += (s, ev) =>
                {
                    if (numericUpDown.Value >= 0)
                    {
                        int quantity = (int)numericUpDown.Value;
                        string selectedProduct = comboBoxSp.SelectedValue.ToString();

                        bool productExistsInCart = false;

                        for (int i = 0; i < cartItems.Count; i++)
                        {
                            var item = cartItems[i];
                            if (item.Item1 == selectedProduct)
                            {
                                productExistsInCart = true;
                                if (item.Item2 != quantity)
                                {
                                    cartItems[i] = new Tuple<string, int>(selectedProduct, quantity);
                                }
                                break;
                            }
                        }

                        if (!productExistsInCart)
                        {
                            Tuple<string, int> data = new Tuple<string, int>(selectedProduct, quantity);
                            cartItems.Add(data);
                        }

                        UpdateCartDataGridView();
                    }
                };
                Button deleteButton = new Button();
                deleteButton.Text = "X";
                deleteButton.Size = new System.Drawing.Size(28, 28);
                deleteButton.Margin = new Padding(5);
                deleteButton.Click += (deleteSender, deleteEvent) =>
                {
                    Button clickedButton = (Button)deleteSender;

                    if (productControls.ContainsKey(clickedButton))
                    {
                        var productControlTuple = productControls[clickedButton];
                        ComboBox cmbBox = productControlTuple.Item1;
                        NumericUpDown numUpDown = productControlTuple.Item2;
                        string selectedProduct = cmbBox.SelectedValue?.ToString(); 
                        var itemToRemove = cartItems.FirstOrDefault(item => item.Item1 == selectedProduct);
                        if (itemToRemove != null)
                        {
                            cartItems.Remove(itemToRemove);

                            flowLayoutPanel1.Controls.Remove(cmbBox);
                            flowLayoutPanel1.Controls.Remove(numUpDown);
                            flowLayoutPanel1.Controls.Remove(clickedButton);

                            cmbBox.Dispose();
                            numUpDown.Dispose();
                            clickedButton.Dispose();
                            UpdateCartDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Sản phẩm không tồn tại trong giỏ hàng.");
                        }
                    }
                };
                flowLayoutPanel1.Controls.Add(comboBoxSp);
                flowLayoutPanel1.Controls.Add(numericUpDown);
                flowLayoutPanel1.Controls.Add(deleteButton);
                productControls.Add(deleteButton, Tuple.Create(comboBoxSp, numericUpDown));
                UpdateCartDataGridView();
            }
        }

        private void UpdateCartDataGridView()
        {
            richTextBox1.Clear();

            decimal totalAmount = 0; 

            foreach (var data in cartItems)
            {
                string product = data.Item1;
                int quantity = data.Item2;
                string query = "";
                string priceColumn = "";
                string productName = "";
                decimal totalPrice = 0;
                string temp = "";
                bool productInSachTable = IsProductInSachTable(product);

                if (productInSachTable)
                {
                    query = "SELECT TenSach, DonGiaBan FROM Sach WHERE TenSach = @SelectedProduct";
                    priceColumn = "DonGiaBan";
                    temp = "TenSach";
                }
                else
                {
                    query = "SELECT TenMon, DonGiaBan FROM ThucDon WHERE TenMon = @SelectedProduct";
                    priceColumn = "DonGiaBan";
                    temp = "TenMon";
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SelectedProduct", product);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            productName = reader[temp].ToString();
                            decimal productPrice = Convert.ToDecimal(reader[priceColumn]);
                            totalPrice = productPrice * quantity;
                            richTextBox1.AppendText($"Sản phẩm: {productName}\n");
                            richTextBox1.AppendText($" {quantity} x {productPrice}\t\t{totalPrice}\n");
                            totalAmount += totalPrice;
                        }
                        else
                        {
                            MessageBox.Show("Product not found in the database.");
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            txtTong.Text = totalAmount.ToString();
        }
        private bool IsProductInSachTable(string tenSach)
        {
            string query = "SELECT COUNT(*) FROM Sach WHERE TenSach = @TenSach";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TenSach", tenSach);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private Random random = new Random();

        public string LayMa(string maKhachHang)
        {
            string generatedCode = TaoMa(maKhachHang);
            while (KiemTraMa(generatedCode))
            {
                generatedCode = TaoMa(maKhachHang);
            }

            return generatedCode;
        }

        private string TaoMa(string maKhachHang)
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
        public class DonHangInfo
        {
            public string Ma { get; set; }
            public int SoLuong { get; set; }
            public string Loai { get; set; }

            public DonHangInfo(string ma, int soLuong, string loai)
            {
                Ma = ma;
                SoLuong = soLuong;
                Loai = loai;
            }
        }
        private void LuuChiTietDonHang(string Madh,string trangthai)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var data in cartItems)
                {
                    string product = data.Item1;
                    int quantity = data.Item2;
                    string query = "";
                    string query1 = "";
                    string priceColumn = "";
                    string productId = "";
                    string Machitiet = "";
                    decimal totalPrice = 0;
                    string temp = "";
                    bool productInSachTable = IsProductInSachTable(product);
                    if (productInSachTable)
                    {
                        query = "SELECT MaSach, DonGiaBan FROM Sach WHERE TenSach = @SelectedProduct";
                        query1 = "ThemChitietDh_Sach";
                        priceColumn = "DonGiaBan";
                        temp = "MaSach";
                        Machitiet = TaoMaCTDHSach();
                    }
                    else
                    {
                        query = "SELECT MaMon, DonGiaBan FROM ThucDon WHERE TenMon = @SelectedProduct";
                        query1 = "ThemChitietDh_ThucDon";
                        priceColumn = "DonGiaBan";
                        temp = "MaMon";
                        Machitiet = TaoMaCTDHThucDon();
                    }

                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@SelectedProduct", product);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            productId = reader[temp].ToString();
                            decimal productPrice = Convert.ToDecimal(reader[priceColumn]);
                            totalPrice = productPrice * quantity;
                            reader.Close();
                            SqlCommand command1 = new SqlCommand(query1, connection);
                            command1.CommandType = CommandType.StoredProcedure;
                            command1.Parameters.AddWithValue("@MaChiTiet", Machitiet);
                            command1.Parameters.AddWithValue("@MaDh", Madh);
                            command1.Parameters.AddWithValue(temp, productId);
                            command1.Parameters.AddWithValue("@SoLuong", quantity);
                            command1.Parameters.AddWithValue("@GiaTien", totalPrice);
                            command1.ExecuteNonQuery();
                            if (trangthai == "Đang vận chuyển") 
                            {
                                if (temp== "MaSach")
                                {
                                    CapNhatSoLuongSach(productId, quantity);
                                }
                                else
                                {
                                    CapNhatSoLuongMon(productId, quantity);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩn .");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void CapNhatSoLuongSach(string maSach, int soLuongNhap)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("CapNhatSoLuongSach", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void CapNhatSoLuongMon(string maMon, int soLuongNhap)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("CapNhatSoLuongMon", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void LayThongTinSanPham(string MaDH)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT ChitietDH_Sach.MaSach, ChitietDH_Sach.GiaTien, ChitietDH_Sach.SoLuong, Sach.TenSach, Sach.DonGiaBan
                    FROM ChitietDH_Sach
                    JOIN Sach ON ChitietDH_Sach.MaSach = Sach.MaSach
                    WHERE ChitietDH_Sach.MaDh = @MaDH";

            string query2 = @"SELECT ChitietDH_ThucDon.MaMon, ChitietDH_ThucDon.GiaTien, ChitietDH_ThucDon.SoLuong, ThucDon.TenMon, ThucDon.DonGiaBan
                    FROM ChitietDH_ThucDon
                    JOIN ThucDon ON ChitietDH_ThucDon.MaMon = ThucDon.MaMon
                    WHERE ChitietDH_ThucDon.MaDh = @MaDH";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaDH", MaDH);

                    SqlDataReader reader = command.ExecuteReader();

                    richTextBox1.Clear();

                    while (reader.Read())
                    {
                        string maSanPham = reader["MaSach"].ToString();
                        string tenSanPham = reader["TenSach"].ToString();
                        decimal giaSanPham = Convert.ToDecimal(reader["DonGiaBan"]);
                        decimal giatien = Convert.ToDecimal(reader["GiaTien"]);
                        decimal SoLuong = Convert.ToDecimal(reader["SoLuong"]);

                        richTextBox1.AppendText($"Sản phẩm: {tenSanPham}\n");
                        richTextBox1.AppendText($" {SoLuong} x {giaSanPham}\t\t{giatien}\n");
                       
                    }

                    reader.Close();

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@MaDH", MaDH);

                    SqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read())
                    {
                        string maMon = reader2["MaMon"].ToString();
                        string tenMon = reader2["TenMon"].ToString();
                        decimal giaMon = Convert.ToDecimal(reader2["DonGiaBan"]);
                        decimal giatien = Convert.ToDecimal(reader2["GiaTien"]);
                        decimal SoLuong = Convert.ToDecimal(reader2["SoLuong"]);

                        richTextBox1.AppendText($"Sản phẩm: {tenMon}\n");
                        richTextBox1.AppendText($" {SoLuong} x {giaMon}\t\t{giatien}\n");
                    }

                    reader2.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
       
        private void CapNhatSoluong(string MaDH)
        {
            List<DonHangInfo> danhSachDonHang = new List<DonHangInfo>();
            string query = @"SELECT ChitietDH_Sach.MaSach, ChitietDH_Sach.GiaTien, ChitietDH_Sach.SoLuong, Sach.TenSach, Sach.DonGiaBan
                    FROM ChitietDH_Sach
                    JOIN Sach ON ChitietDH_Sach.MaSach = Sach.MaSach
                    WHERE ChitietDH_Sach.MaDh = @MaDH";

            string query2 = @"SELECT ChitietDH_ThucDon.MaMon, ChitietDH_ThucDon.GiaTien, ChitietDH_ThucDon.SoLuong, ThucDon.TenMon, ThucDon.DonGiaBan
                    FROM ChitietDH_ThucDon
                    JOIN ThucDon ON ChitietDH_ThucDon.MaMon = ThucDon.MaMon
                    WHERE ChitietDH_ThucDon.MaDh = @MaDH";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaDH", MaDH);

                    SqlDataReader reader = command.ExecuteReader();

                    richTextBox1.Clear();

                    while (reader.Read())
                    {
                        string maSanPham = reader["MaSach"].ToString();
                        string tenSanPham = reader["TenSach"].ToString();
                        decimal giaSanPham = Convert.ToDecimal(reader["DonGiaBan"]);
                        decimal giatien = Convert.ToDecimal(reader["GiaTien"]);
                        decimal SoLuong = Convert.ToDecimal(reader["SoLuong"]);

                        DonHangInfo donHang = new DonHangInfo(maSanPham, Convert.ToInt32(SoLuong), "Sach");

                        danhSachDonHang.Add(donHang);

                    }

                    reader.Close();

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@MaDH", MaDH);

                    SqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read())
                    {
                        string maMon = reader2["MaMon"].ToString();
                        string tenMon = reader2["TenMon"].ToString();
                        decimal giaMon = Convert.ToDecimal(reader2["DonGiaBan"]);
                        decimal giatien = Convert.ToDecimal(reader2["GiaTien"]);
                        decimal SoLuong = Convert.ToDecimal(reader2["SoLuong"]);

                        DonHangInfo donHang = new DonHangInfo(maMon, Convert.ToInt32(SoLuong), "Mon");

                        danhSachDonHang.Add(donHang);
                    }

                    reader2.Close();
                }
                foreach (DonHangInfo donHang in danhSachDonHang)
                {
                    if(donHang.Loai== "Sach")
                    {
                        CapNhatSoLuongSach(donHang.Ma, donHang.SoLuong);
                    }
                    else
                    {
                        CapNhatSoLuongMon(donHang.Ma, donHang.SoLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public bool ThemMoi = true;
        public bool Xoa = true;
        public void Khoa()
        {
            cbbMaKH.Enabled = false;
            txtTenKH.Enabled = false;
            dtNgayDH.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = false;
            btThemSP.Enabled = false;
            txtSDT.Enabled = false; 
            txtDiaChi.Enabled = false;
            txtTong.Enabled = false;
            rbDnh.Enabled = false;
            rbDvc.Enabled = false;
            rbDxl.Enabled  = false;
            rbHuy.Enabled = false;
        }
        public void CleaData()
        {
            txtTong.Text = string.Empty;
            txtGhiChu.Text = string.Empty;
            richTextBox1.Text = string.Empty;
        }
        public void MoKhoa()
        {
            cbbMaKH.Enabled = true;
            txtTenKH.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = true;
            btThemSP.Enabled = true;
            txtSDT.Enabled = true;
            txtDiaChi.Enabled = true;
            txtTong.Enabled=true;
            groupBox6.Enabled = true;
            rbDnh.Enabled=true;
            rbDvc.Enabled=true;
            rbDxl.Enabled=true;
            rbHuy.Enabled=true;
        }
        
        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            MoKhoa();
            ThemMoi = true;
            Xoa = false;
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            dtNgayDH.Value = DateTime.Now;
            rbDxl.Checked = true;
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
                XoaDonHang(MaDonhang);
            }
            else
            {
                LoadData();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maDh = LayMa(cbbMaKH.Text);
            string TenVaSDT = txtTenKH.Text + " (+84) " + txtSDT;
            if (string.IsNullOrWhiteSpace(cbbMaKH.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào tất cả các ô văn bản.");
            }
            else if (ThemMoi)
            {
                string trangthai = "";
                if (rbDnh.Checked == true)
                {
                    trangthai = "Đã nhận hàng";
                }
                else if (rbDvc.Checked == true)
                {
                    trangthai = "Đang vận chuyển";
                }
                else if (rbDxl.Checked == true)
                {
                    trangthai = "Đang xử lý";
                }
                else
                {
                    trangthai = "Đã Hủy";
                }
                ThemDonHang(maDh, DateTime.Now, Convert.ToInt32(txtTong.Text),trangthai,txtGhiChu.Text,cbbMaKH.Text, TenVaSDT, txtDiaChi.Text);
                LuuChiTietDonHang(maDh,trangthai);
                flowLayoutPanel1.Controls.Clear();
            }
            else if (ThemMoi == false)
            {
                string trangthai = "";
                if (rbDnh.Checked == true)
                {
                    trangthai = "Đã nhận hàng";
                }
                else if (rbDvc.Checked == true)
                {
                    trangthai = "Đang vận chuyển";
                }
                else if (rbDxl.Checked == true)
                {
                    trangthai = "Đang xử lý";
                }
                else
                {
                    trangthai = "Đã Hủy";
                }
                CapNhatDonHang(MaDonhang, DateTime.Now, Convert.ToInt32(txtTong.Text), trangthai, txtGhiChu.Text, cbbMaKH.Text, TenVaSDT, txtDiaChi.Text);
                if(trangThaicu == "Đang xử lý"&&trangthai == "Đang vận chuyển")
                {
                    CapNhatSoluong(MaDonhang);
                }
            }
            cartItems.Clear();
        }

        private void btnTK_Click(object sender, EventArgs e)
        {

        }
        private void DgvDonHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DgvDonHang.Rows[e.RowIndex];
                cbbMaKH.Text = row.Cells["MaKh"].Value?.ToString();
                string trangthai = row.Cells["TrangThai"].Value?.ToString();
                trangThaicu = trangthai;
                if (trangthai == "Đã nhận hàng")
                {
                    rbDnh.Checked = true;
                }
                else if (trangthai == "Đang vận chuyển")
                {

                    rbDvc.Checked = true;
                }
                else if (trangthai == "Đang xử lý")
                {
                    rbDxl.Checked = true;
                }
                else if(trangthai == "Đã Hủy")
                {
                    rbHuy.Checked = true;
                }
               
                string MaDH = row.Cells["MaDh"].Value?.ToString();
                LayThongTinSanPham(MaDH);
                MaDonhang = MaDH;
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                txtTong.Text = row.Cells["TongGiaTri"].Value?.ToString();
                dtNgayDH.Value = Convert.ToDateTime(row.Cells["NgayDatHang"].Value);
            }
        }

        private void cbbMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCustomer = cbbMaKH.Text;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM KhachHang where MaKh =@CustomerId", connection);
                    command.Parameters.AddWithValue("@CustomerId", selectedCustomer);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtTenKH.Text = reader["TenKhach"].ToString();
                        txtDiaChi.Text = reader["DiaChi"].ToString();
                        txtSDT.Text = reader["DienThoai"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin khách hàng.");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
