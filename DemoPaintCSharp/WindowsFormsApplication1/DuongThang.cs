using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace WindowsFormsApplication1
{

    [Serializable()]
    class DuongThang:Elip
    {

#region Khởi tạo
        public DuongThang()
            : base()
        {
            soDiemDieuKhien = 2;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(diemBatDau, diemKetThuc);
            graphicsPath.Widen(pen);
            khuVuc = new Region(graphicsPath);
            loaiHinh = 4;
        }

        public DuongThang(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 2;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(diemBatDau, diemKetThuc);
            graphicsPath.Widen(pen);
            khuVuc = new Region(graphicsPath);
            loaiHinh = 4;
        }
     public DuongThang(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
     int sodiemdieukhien , GraphicsPath graphicspath , Region khuvuc , int vitrichuotsovoihinhve ,
     bool dichuyen , bool thaydoikichthuoc , int loaihinh)
            : base(mauve , dodamnet , diembatdau , diemketthuc , diemmousedown , sodiemdieukhien , graphicspath , khuvuc , vitrichuotsovoihinhve , dichuyen , thaydoikichthuoc , loaihinh)
        {
            mauVe = mauve;
            doDamNet = dodamnet;
            diemBatDau = diembatdau;
            diemKetThuc = diemketthuc;
            diemMouseDown = diemmousedown;
            soDiemDieuKhien = sodiemdieukhien;
            graphicsPath = graphicspath;
            khuVuc = khuvuc;
            viTriChuotSoVoiHinhVe = vitrichuotsovoihinhve;
            diChuyen = dichuyen;
            thayDoiKichThuoc = thaydoikichthuoc;
            loaihinh = loaiHinh;

        }
#endregion
        #region Tuần tự hóa và giải tuần tự hóa
        public DuongThang(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
#region Phương thức
        // Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(mauVe, doDamNet);
            g.DrawLine(pen, diemBatDau, diemKetThuc);
            pen.Dispose();
        }

        // Tạo điểm điều khiển
        protected override Point DiemDieuKhien(int viTriDiemDieuKhien)
        {
            if (viTriDiemDieuKhien == 1)
                return diemBatDau;
            return diemKetThuc;
        }

        // Thay đổi điểm Start, End khi Click vào 1 điểm điều khiển
        protected override void ThayDoiDiem(int viTriDiemDieuKhien)
        {
            if (viTriDiemDieuKhien == 1)
            {
                Point point = diemBatDau;
                diemBatDau = diemKetThuc;
                diemKetThuc = point;
            }
        }

        // Di chuyển đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuoc(int viTriDiemDieuKhien, Point point)
        {
            diemKetThuc = point;
        }
        //
        public override void VeKhung(Graphics g)
        {
            Pen pen = new Pen(Color.Blue, 1);
            // SolidBrush brush = new SolidBrush(Color.White);
            for (int i = 1; i <= soDiemDieuKhien; i++)
            {
                g.DrawRectangle(pen, VeChamVuong(i, 3));
                g.FillRectangle(new SolidBrush(Color.Blue), VeChamVuong(i, 6));
            }
            //            g.DrawRectangle(pen, Rectangle(diemBatDau, diemKetThuc));
            
            //  brush.Dispose();
            pen.DashStyle = DashStyle.Dot;
            pen.DashOffset = 20;
            g.DrawLine(pen, new Point(diemBatDau.X+10, diemBatDau.Y), new Point(diemKetThuc.X+10, diemKetThuc.Y));
            g.DrawLine(pen, new Point(diemBatDau.X -10, diemBatDau.Y), new Point(diemKetThuc.X- 10, diemKetThuc.Y));
            g.DrawLine(pen, new Point(diemBatDau.X , diemBatDau.Y+10), new Point(diemKetThuc.X , diemKetThuc.Y+10));
            g.DrawLine(pen, new Point(diemBatDau.X , diemBatDau.Y-10), new Point(diemKetThuc.X, diemKetThuc.Y-10));
            pen.Dispose();
        }
        // Sự kiện chuột
        public override void Mouse_Down(MouseEventArgs e)
        {
            viTriChuotSoVoiHinhVe = KiemTraViTri(e.Location);
            if (viTriChuotSoVoiHinhVe > 0)
            {
                thayDoiKichThuoc = true;
                ThayDoiDiem(viTriChuotSoVoiHinhVe);
            }
            else if (viTriChuotSoVoiHinhVe == 0)
            {
                diChuyen = true;
                diemMouseDown = e.Location;
            }
            else
            {
                diemBatDau = e.Location;
                diemKetThuc.X = e.X; diemKetThuc.Y = e.Y - 1;
            }
        }

        public override void Mouse_Move(MouseEventArgs e)
        {
            if (thayDoiKichThuoc == true)
            {
                ThayDoiKichThuoc(viTriChuotSoVoiHinhVe, e.Location);
            }
            else if (diChuyen == true)
            {
                int deltaX = e.X - diemMouseDown.X;
                int deltaY = e.Y - diemMouseDown.Y;
                diemMouseDown = e.Location;
                DiChuyen(deltaX, deltaY);
            }
            else
            {
                diemKetThuc = e.Location;
            }
        }

        public override void Mouse_Up(Object sender)
        {
            graphicsPath = new GraphicsPath();
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath.AddLine(diemBatDau, diemKetThuc);
            
            //graphicsPath.Widen(pen);
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(diemBatDau.X + 10, diemBatDau.Y), new Point(diemKetThuc.X + 10, diemKetThuc.Y));
            gp.AddLine(new Point(diemBatDau.X - 10, diemBatDau.Y), new Point(diemKetThuc.X - 10, diemKetThuc.Y));
            gp.AddLine(new Point(diemBatDau.X , diemBatDau.Y+10), new Point(diemKetThuc.X , diemKetThuc.Y+10));
            gp.AddLine(new Point(diemBatDau.X, diemBatDau.Y - 10), new Point(diemKetThuc.X, diemKetThuc.Y - 10));
            khuVuc = new Region(gp);

            diChuyen = false;
            thayDoiKichThuoc = false;
            viTriChuotSoVoiHinhVe = -1;
        }
 
#endregion
        
    }
}
