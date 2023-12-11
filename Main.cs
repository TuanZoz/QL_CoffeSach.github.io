using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuAn1
{
    public partial class Main : Form
    {
        
        public string MaDn { get; set; }
        public string Vaitro;
        public Main()
        {
            InitializeComponent();
           
        }
        private void Main_Load(object sender, EventArgs e)
        {
            NewLoad();
            if (MaDn != null && Vaitro != null)
            {
                if (String.Equals(Vaitro, "Quản Lý", StringComparison.OrdinalIgnoreCase))
                {
                    QuanLy();
                }
                else if (String.Equals(Vaitro, "Nhân viên", StringComparison.OrdinalIgnoreCase))
                {
                    NhanVien();
                }
                else
                {
                    Khachhang();
                }
            }
        }

        private void NewLoad()
        {
            btnGioHang.Visible = false;
            btnKH.Visible = false;
            btnLichSuDh.Visible = false;
            btnLL.Visible = false;
            btnMN.Visible = false;
            btnNV.Visible = false;
            btThucDon.Visible = false;
            btnThongTinCN.Visible = false;
            btSach.Visible = false;
            btDonHang.Visible = false;
            btnDXuat.Visible = false;
            btnHD.Visible = true;
            btnDNM.Visible = true;
        }
        private void NhanVien()
        {
            btnKH.Visible = true;
            btnMN.Visible = true;
            btDonHang.Visible = true;
            btThucDon.Visible = true;
            btSach.Visible = true;
            btnDXuat.Visible = true;
            btnDNM.Visible = false;
        }
        private void QuanLy()
        {
            btnKH.Visible = true;
            btnLL.Visible = true;
            btnMN.Visible = true;
            btnNV.Visible = true;
            btDonHang.Visible = true;
            btThucDon.Visible = true;
            btSach.Visible = true;
            btnDXuat.Visible = true;
            btnDNM.Visible = false;
        }
        private void Khachhang()
        {
            btnLichSuDh.Visible = true;
            btnGioHang.Visible = true;
            btnThongTinCN.Visible = true;
            btnMN.Visible=true;
            btnDXuat.Visible=true;
            btnDNM.Visible = false;
        }
        public void LayMa(string ma, string vaitro )
        {
            MaDn = ma;
            Vaitro = vaitro;
        }
        private void btnDNM_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            DangNhap loginForm = new DangNhap(mainForm);
            this.Hide();
            loginForm.ShowDialog();
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
            control.Controls.Clear();
            Menu menu = new Menu(this);
            menu.Dock = DockStyle.Fill;
            control.Controls.Add(menu);
        }
        private void btnSP_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_Sach qL_Sach = new QL_Sach(this);
            qL_Sach.Dock = DockStyle.Fill;
            control.Controls.Add(qL_Sach);
        }

        private void btThucDon_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_ThucDon qL_ThucDon = new QL_ThucDon(this);
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

        private void btnDXuat_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            DangNhap loginForm = new DangNhap(mainForm);
            this.Hide();
            loginForm.ShowDialog();
        }
        private void btnKH_Click(object sender, EventArgs e)
        {
            control.Controls.Clear();
            QL_KhachHang qL_KhachHang = new QL_KhachHang();
            qL_KhachHang.Dock = DockStyle.Fill;
            control.Controls.Add(qL_KhachHang);
        }


        private void btnGioHang_Click(object sender, EventArgs e)
        {
            GioHang gioHangForm = new GioHang(this);
            control.Controls.Clear(); 
            gioHangForm.Dock = DockStyle.Fill;
            control.Controls.Add(gioHangForm); 
        }
        public void GioHang()
        {
            GioHang gioHangForm = new GioHang(this);
            control.Controls.Clear();
            gioHangForm.Dock = DockStyle.Fill;
            control.Controls.Add(gioHangForm);
        }
        public void ThanhToan()
        {
            ThanhToan thanhToan =new ThanhToan(this);
            control.Controls.Clear();
            thanhToan.Dock = DockStyle.Fill;
            control.Controls.Add(thanhToan);
        }
        public void LichSuMua()
        {
            LichSuDatHang lichSuDatHang = new LichSuDatHang(this);
            control.Controls.Clear();
            lichSuDatHang.Dock = DockStyle.Fill;
            control.Controls.Add(lichSuDatHang);
        }

        private void btnThongTinCN_Click(object sender, EventArgs e)
        {
            ThongTinCaNhan thongTinCaNhan = new ThongTinCaNhan(this);
            control.Controls.Clear();
            thongTinCaNhan.Dock = DockStyle.Fill;
            control.Controls.Add(thongTinCaNhan);
        }

        private void control_Enter(object sender, EventArgs e)
        {

        }

        private void btnLichSuDh_Click(object sender, EventArgs e)
        {
            LichSuDatHang lichSuDatHang = new LichSuDatHang(this);
            control.Controls.Clear();
            lichSuDatHang.Dock = DockStyle.Fill;
            control.Controls.Add(lichSuDatHang);
        }
    }
}
