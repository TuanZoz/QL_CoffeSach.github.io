using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuAn1
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();     
        }

        private void btnDNM_Click(object sender, EventArgs e)
        {
            DangNhap dangNhap = new DangNhap();
            dangNhap.ShowDialog();
        }

        private void btnKH_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_KhachHang qL_KhachHang = new QL_KhachHang();
            qL_KhachHang.Dock = DockStyle.Fill;
            control.Controls.Add(qL_KhachHang);
        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_NhanVien QL_NhanVien = new QL_NhanVien();
            QL_NhanVien.Dock = DockStyle.Fill;
            control.Controls.Add(QL_NhanVien);
        }

        private void btnMN_Click(object sender, EventArgs e)
        {
        }


        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnSP_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_Sach qL_Sach = new QL_Sach();
            qL_Sach.Dock = DockStyle.Fill;
            control.Controls.Add(qL_Sach);
        }

        private void btThucDon_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_ThucDon qL_ThucDon = new QL_ThucDon();
            qL_ThucDon.Dock = DockStyle.Fill;
            control.Controls.Add(qL_ThucDon);
        }

        private void btDonHang_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_DonHang qL_DonHang = new QL_DonHang();
            qL_DonHang.Dock = DockStyle.Fill;
            control.Controls.Add(qL_DonHang);
        }
    }
}
