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
    class Elip:HinhChuNhat
    {


#region Khởi tạo
        public Elip()
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
            loaiHinh = 1;
        }

        public Elip(Color mauVe, int doDamNet)
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
            loaiHinh = 1;
        }
        public Elip(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
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
       public Elip(SerializationInfo info, StreamingContext ctxt)
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
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(mauVe, doDamNet);
            g.DrawEllipse(pen, VeHCN(diemBatDau, diemKetThuc));
            pen.Dispose();
        }

        public override void VeKhung(Graphics g)
        {
            
            base.VeKhung(g);
        
            Pen p = new Pen(Color.Blue, 1);
            p.DashStyle = DashStyle.Dash;
            p.DashOffset = 20;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(p, VeHCN(diemBatDau, diemKetThuc));
            p.Dispose();
            
        }

#endregion

    }
}
