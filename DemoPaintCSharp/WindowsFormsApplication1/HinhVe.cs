using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace WindowsFormsApplication1
{
    [Serializable()]    //đánh dấu là đối tượng có thể tuần tự hóa
    class HinhVe : ISerializable
    {
#region Thuộc tính
        public Color mauVe;                // Màu bút vẽ
        public int doDamNet;               // Độ đậm bút vẽ
        public Point diemBatDau;           // Điểm bắt đầu vẽ
        public Point diemKetThuc;             // Điểm kết thúc vẽ
        protected Point diemMouseDown;         // Điểm hiện hành
        protected int soDiemDieuKhien;            // Số điểm điều khiển
        protected GraphicsPath graphicsPath;  // Công cụ vẽ
        public Region khuVuc;              // Tạo ra 1 vùng
        protected int viTriChuotSoVoiHinhVe;                 // Vị trí tương đối của 1 điểm và đối tượng
        public bool diChuyen;                  // Di chuyển = true/false
        public bool thayDoiKichThuoc;                // Thay đổi kích thước = true/false
        public int loaiHinh;            //Mã loại của hình vẽ.
#endregion

#region Khởi tạo
        
        public HinhVe()
        {
            mauVe = Color.Black;
            doDamNet = 1;
        }
        public HinhVe(Color cl, int pw)
        {
            mauVe = cl;
            doDamNet = pw;
        }
        public HinhVe(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
            int sodiemdieukhien , GraphicsPath graphicspath , Region khuvuc , int vitrichuotsovoihinhve ,
            bool dichuyen , bool thaydoikichthuoc , int loaihinh)
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
            loaiHinh = loaihinh;
        }

#endregion
        #region tuần tự và giải tuần tự, có một số thuộc tính không cần thiết hoặc không thể tuần tự hóa, tuy nhiên với những thuộc tính được tuần tự hóa bên dưới là đủ tạo hình

        public HinhVe(SerializationInfo info, StreamingContext ctxt)    //Khoi tao bang cach giai tuan tu
        {
            mauVe = (Color)info.GetValue("mauVe", typeof(Color));
            doDamNet = (int)info.GetValue("doDamNet", typeof(int));
            diemBatDau = (Point)info.GetValue("diemBatDau", typeof(Point));
            diemKetThuc = (Point)info.GetValue("diemKetThuc", typeof(Point));
            soDiemDieuKhien = (int)info.GetValue("soDiemDieuKhien", typeof(int));
            loaiHinh = (int)info.GetValue("loaiHinh" , typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("mauVe", mauVe);
            info.AddValue("doDamNet", doDamNet);
            info.AddValue("diemBatDau", diemBatDau);
            info.AddValue("diemKetThuc", diemKetThuc);
            info.AddValue("soDiemDieuKhien" , soDiemDieuKhien);
            info.AddValue("loaiHinh" , loaiHinh);
        }
        #endregion
        #region Phương thức
        // hoán đổi điểm bắt đầu và kết thúc
        public virtual void ThayDoi()
        {
            if (diemBatDau.X > diemKetThuc.X)
            {
                int tam = diemKetThuc.X;
                diemKetThuc.X = diemBatDau.X;
                diemBatDau.X = tam;
            }
            if (diemBatDau.Y > diemKetThuc.Y)
            {
                int tam = diemKetThuc.Y;
                diemKetThuc.Y = diemBatDau.Y;
                diemBatDau.Y = tam;
            }
        }

        // Vẽ
        public virtual void Ve(Graphics g)
        {
        }

        // Tạo điểm điều khiển
        protected virtual Point DiemDieuKhien(int viTriDiemDieuKhien)
        {
            return new Point(0, 0);
        }

        // Tạo HCN quanh điểm điều khiển (8 chấm vuông)
        protected virtual Rectangle VeChamVuong(int viTriDiemDieuKhien)
        {
            Point point = DiemDieuKhien(viTriDiemDieuKhien);
            return new Rectangle(point.X - 5, point.Y - 5, 10, 10);
        }
        protected virtual Rectangle VeChamVuong(int viTriDiemDieuKhien, int doRongHCN)
        {
            Point point = DiemDieuKhien(viTriDiemDieuKhien);
            return new Rectangle(point.X - doRongHCN/2, point.Y - doRongHCN/2, doRongHCN, doRongHCN);
        }
        // Vẽ điểm điều khiển
        public virtual void VeKhung(Graphics g)
        {

            for (int i = 1; i <= soDiemDieuKhien; i++)
            {
                Pen pen = new Pen(Color.Blue, 1);
                if (i == 1)
                    pen = new Pen(Color.Red, 1);        //vẽ điểm số 1 màu đỏ cho đặc biệt

                g.DrawRectangle(pen, VeChamVuong(i,3));
                g.FillRectangle(new SolidBrush(pen.Color), VeChamVuong(i,6));   //tô chấm vuông
                pen.Dispose();
            }

        }
        
        public virtual void VeHCNDiemDieuKhien(Graphics g, int doDamNet)
        {

            for (int i = 1; i <= soDiemDieuKhien; i++)
            {
                Pen pen = new Pen(Color.Blue , doDamNet);
                if (i == 1)
                    pen = new Pen(Color.Red , doDamNet);
                g.DrawRectangle(pen, VeChamVuong(i,5));
                g.FillRectangle(Brushes.Blue, VeChamVuong(i,4));
                pen.Dispose();
            }

        }
        // Kiểm tra xem 1 điểm có thuộc khu vực chiếm giữ đối tượng này hay không
        protected virtual bool KiemTraThuoc(Point point)
        {
            if (khuVuc.IsVisible(point) == true)
                return true;
            return false;
        }

        // Kiểm tra vị trí tương đối của 1 điểm và 1 đối tượng
        // - 1 : Nằm ngoài đối tượng
        // =0   : Trong đối tượng
        // >= 1 : Điểm điều khiển 
        public virtual int KiemTraViTri(Point point)
        {
            for (int i = 1; i <= soDiemDieuKhien; i++)
            {
                if (VeChamVuong(i).Contains(point) == true) //điểm đó nằm trên hình chữ nhật bao quanh 1 điểm điều khiển (8 chấm vuông nhỏ)
                    return i;
            }
            if (KiemTraThuoc(point) == true)    // điểm đó thuộc khu vực bên trong hình bao quanh
                return 0;
            return -1;  //điểm và hình tách biệt nhau
        }

        // Xác định lại điểm Start và End khi Click vào 1 điểm điều khiển
        protected virtual void ThayDoiDiem(int viTriDiemDieuKhien)
        {
        }

        // Di chuyển đối tượng khi move = true
        public virtual void DiChuyen(int deltaX, int deltaY)
        {
            diemBatDau.X += deltaX;
            diemBatDau.Y += deltaY;
            diemKetThuc.X += deltaX;
            diemKetThuc.Y += deltaY;

        }

        // Thay đổi kích thước đối tượng khi biết điểm điều khiển và điển đến, resize = true
        protected virtual void ThayDoiKichThuoc(int viTriDiemDieuKhien, Point point)
        {

        }


        // Sự kiện chuột
        public virtual void Mouse_Down(MouseEventArgs e)
        {
        }

        public virtual void Mouse_Move(MouseEventArgs e)
        {
            
        }
        public virtual void Mouse_Up(Object sender)
        {

        }

#endregion
    }
}
