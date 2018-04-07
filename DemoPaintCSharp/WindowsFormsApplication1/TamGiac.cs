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
    class TamGiac:HinhChuNhat
    {


        #region Khởi tạo
        public TamGiac()
           : base()
        {
            soDiemDieuKhien = 8;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 4;
        }

        public TamGiac(Color mauVe, int doDamNet)
            : base(mauVe, doDamNet)
        {
            soDiemDieuKhien = 8;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(mauVe, doDamNet);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 4;
        }
        public TamGiac(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
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
        public TamGiac(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)

        {
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
#region Phương thức
        // Vẽ tam giac cho các trường hợp di chuyển khác nhau,
        public override void Ve(Graphics g)
        {
            if(diemBatDau==DiemDieuKhien(6) || diemKetThuc==DiemDieuKhien(3))
            {
                Pen pen = new Pen(mauVe, doDamNet);
                g.DrawLine(pen, DiemDieuKhien(3), DiemDieuKhien(7));
                g.DrawLine(pen, DiemDieuKhien(7), DiemDieuKhien(1));
                g.DrawLine(pen, DiemDieuKhien(1), DiemDieuKhien(3));
                pen.Color = Color.Yellow;
                g.DrawLine(pen, DiemDieuKhien(1), DiemDieuKhien(3));
                pen.Dispose();
            }
            else
            {
                Pen pen = new Pen(mauVe, doDamNet);
                g.DrawLine(pen, DiemDieuKhien(2), DiemDieuKhien(6));
                g.DrawLine(pen, DiemDieuKhien(6), DiemDieuKhien(8));
                g.DrawLine(pen, DiemDieuKhien(8), DiemDieuKhien(2));
                pen.Dispose();

            }

        }
#endregion
    }
}
