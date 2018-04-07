using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    [Serializable()]
    class ConTro:HinhChuNhat
    {

#region Khởi tạo
        public ConTro()
            : base()
        {
            soDiemDieuKhien = 0;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 0;
        }
        public ConTro(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 0;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 0;
        }
#endregion

#region Phương thức
        // Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(Color.BlueViolet, 1);
            pen.DashStyle = DashStyle.DashDot;
            pen.DashOffset = 10;
            g.DrawRectangle(pen, VeHCN(diemBatDau, diemKetThuc));
            pen.Dispose();
        }
        public override void Mouse_Up(object sender)
        {
            
        }
#endregion
    }
}
