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
    public partial class QL_DonHang : UserControl
    {
        public QL_DonHang()
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
                SqlCommand cmd = new SqlCommand("GetDonHang", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTableKH = new DataTable();
                adapter.Fill(dataTableKH);
                DgvDonHang.DataSource = dataTableKH;
            }
        }
    }
}
