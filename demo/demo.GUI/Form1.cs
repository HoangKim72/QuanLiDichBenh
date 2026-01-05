using demo.BLL;
using demo.BLL.Service;
using demo.BLL.Servicer;
using demo.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo.GUI
{
    public partial class Form1 : Form
    {
        private TrangThaiService ttService;
        private DiaPhuongService dpService;
        public Form1()
        {
            InitializeComponent();
            ttService = new TrangThaiService();
            dpService = new DiaPhuongService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadTrangThai();
            TaoHeader();
            DocDuLieuDIaPhuong();

        }
        public void TaoHeader()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("MaDP", "Mã Địa Phương");
            dataGridView1.Columns.Add("TenDP", "Tên Địa Phương");
            dataGridView1.Columns.Add("CaNhiem", "Ca Nhiễm");
            dataGridView1.Columns.Add("TT", "Trạng Thái");
        }

        public void DocDuLieuDIaPhuong()
        {
            dataGridView1.Rows.Clear();
            var listDiaPhuong = new DiaPhuongService().GetAllDiaPhuong();
            foreach (var item in listDiaPhuong)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = item.MaDP;
                row.Cells[1].Value = item.TenDP;
                row.Cells[2].Value = item.SoCaNhiemMoi;
                row.Cells[3].Value = item.TrangThai.TenTT;
                dataGridView1.Rows.Add(row);
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void btn_capnhat_Click(object sender, EventArgs e)
        {
            try
            {
                DiaPhuong dpMoi = DocDuLieu();
                DiaPhuong dpCu = dpService.GetDiaPhuongById(dpMoi.MaDP);
                if (dpCu == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin địa phương");
                    return;
                }
                if (dpCu.MaTT == dpMoi.MaTT)
                {
                    string tenTrangThaiCu = dpCu.TrangThai?.TenTT;
                    string tenTrangThaiMoi = cbo_tranghtai.Text;
                    DialogResult result = MessageBox.Show(
                        $"Địa phương có sự thay đổi từ {tenTrangThaiCu} -> {tenTrangThaiMoi}?",
                        "Xác nhận thay đổi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                dpService.UpdateDiaPhuong(dpMoi);
                MessageBox.Show("Cập nhật địa phương thành công");
                txt_madiaphuong.Clear();
                txt_tendiaphuong.Clear();
                txt_socanhiem.Clear();
                cbo_tranghtai.SelectedIndex = -1;
                DocDuLieuDIaPhuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật địa phương: " + ex.Message);
            }
        }


        private void loadTrangThai()
        {
            TrangThaiService trangThaiService = new TrangThaiService();
            var listTrangThai = trangThaiService.GetAllTrangThai();
            cbo_tranghtai.DataSource = listTrangThai;
            cbo_tranghtai.DisplayMember = "TenTT";
            cbo_tranghtai.ValueMember = "MaTT";
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            txt_madiaphuong.Text = selectedRow.Cells["MaDP"].Value?.ToString();
            txt_tendiaphuong.Text = selectedRow.Cells["TenDP"].Value?.ToString();
            txt_socanhiem.Text = selectedRow.Cells["CaNhiem"].Value?.ToString();
            cbo_tranghtai.Text = selectedRow.Cells["TT"].Value?.ToString();
        }

        private DiaPhuong DocDuLieu()
        {
            DiaPhuong dp = new DiaPhuong();
            string madiaphuong = txt_madiaphuong.Text;
            if (string.IsNullOrEmpty(madiaphuong))
            {
                MessageBox.Show("Mã địa phương không được để trống");
            }
            if (madiaphuong.Length != 3)
            {
                MessageBox.Show("Mã địa phương phải đúng 3 ký tự");

            }
            dp.MaDP = madiaphuong;

            string tendiaphuong = txt_tendiaphuong.Text;
            if (string.IsNullOrEmpty(tendiaphuong))
            {
                MessageBox.Show("Tên địa phương không được để trống");
            }
            dp.TenDP = tendiaphuong;
            int socanhiem;
            if (!int.TryParse(txt_socanhiem.Text, out socanhiem) || socanhiem < 0)
            {
                MessageBox.Show("Số ca nhiễm phải là số nguyên dương");
            }

            dp.SoCaNhiemMoi = socanhiem;
            if (cbo_tranghtai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái");
            }
            dp.MaTT = (int)cbo_tranghtai.SelectedValue;
            return dp;
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            try
            {
                DiaPhuong dp = DocDuLieu();
                DiaPhuong existingdp = dpService.GetDiaPhuongById(dp.MaDP);
                if (existingdp != null)
                {
                    MessageBox.Show("Địa phương với mã này đã tồn tại");
                    return;
                }
                else
                {
                    dpService.AddDiaPhuong(dp);
                    MessageBox.Show("Thêm địa phương mới thành công");
                    txt_madiaphuong.Clear();
                    txt_tendiaphuong.Clear();
                    txt_socanhiem.Clear();
                    cbo_tranghtai.SelectedIndex = -1;
                    DocDuLieuDIaPhuong();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm địa phương: " + ex.Message);
            }
        }
        private bool isSortDesc = true;

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SapXepTheoSoCaNhiem();
        }
        private void SapXepTheoSoCaNhiem()
        {
            var list = dpService.GetAllDiaPhuong();

            if (isSortDesc)
                list = list.OrderByDescending(x => x.SoCaNhiemMoi).ToList();
            else
                list = list.OrderBy(x => x.SoCaNhiemMoi).ToList();

            isSortDesc = !isSortDesc; 

            DocDuLieuDIaPhuong2(list);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void DocDuLieuDIaPhuong2(List<DiaPhuong> list)
        {
            dataGridView1.Rows.Clear();

            foreach (var item in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);

                row.Cells[0].Value = item.MaDP;
                row.Cells[1].Value = item.TenDP;
                row.Cells[2].Value = item.SoCaNhiemMoi;
                row.Cells[3].Value = item.TrangThai.TenTT;

                dataGridView1.Rows.Add(row);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DiaPhuongNhomNguyCo();
        }
        private void DiaPhuongNhomNguyCo()
        {
            var list = dpService.GetAllDiaPhuong()
                .Where(x => x.TrangThai != null && x.TrangThai.MaTT != 1)
                .ToList();

            DocDuLieuDIaPhuong2(list);
        }

    }

}



