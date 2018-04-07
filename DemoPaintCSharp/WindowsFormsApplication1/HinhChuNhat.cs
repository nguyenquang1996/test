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
    class HinhChuNhat:HinhVe
    {


#region Khởi tạo
        public HinhChuNhat()
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
            loaiHinh = 2;
        }
        public HinhChuNhat(Color cl, int pw)
            : base(cl, pw)
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
            loaiHinh = 2;
        }
        public HinhChuNhat(Color mauve , int dodamnet , Point diembatdau , Point diemketthuc , Point diemmousedown ,
            int sodiemdieukhien , GraphicsPath graphicspath , Region khuvuc , int vitrichuotsovoihinhve ,
            bool dichuyen , bool thaydoikichthuoc , int loaihinh)
            :base(mauve,dodamnet,diembatdau,diemketthuc,diemmousedown,sodiemdieukhien,graphicspath,khuvuc,vitrichuotsovoihinhve,dichuyen,thaydoikichthuoc,loaihinh)
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
        public HinhChuNhat(SerializationInfo info, StreamingContext ctxt)
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

        // Tạo hình chữ nhật từ tọa độ 2 điểm
        protected virtual Rectangle VeHCN(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                int tam = x1;
                x1 = x2;
                x2 = tam;
            }
            if (y1 > y2)
            {
                int tam = y1;
                y1 = y2;
                y2 = tam;
            }
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        protected virtual Rectangle VeHCN(Point A, Point B)
        {
            return VeHCN(A.X, A.Y, B.X, B.Y);
        }

         //Vẽ
        public override void Ve(Graphics g)
        {
            Pen pen = new Pen(mauVe, doDamNet);
            g.DrawRectangle(pen, VeHCN(diemBatDau, diemKetThuc));
            pen.Dispose();
        }

        // Tạo điểm điều khiển từ tọa độ của điểm bắt đầu và điểm kết thúc, lấy các giá trị trung bình để tạo các trung điểm
        protected override Point DiemDieuKhien(int viTriDiemDieuKhien)
        {

            int xCenter = (diemBatDau.X + diemKetThuc.X) / 2;
            int yCenter = (diemBatDau.Y + diemKetThuc.Y) / 2;
            int x = 0, y = 0;
            switch (viTriDiemDieuKhien)
            {
                case 1:
                    {
                        x = diemBatDau.X;
                        y = diemBatDau.Y;
                        break;
                    }
                case 2:
                    {
                        x = xCenter;
                        y = diemBatDau.Y;
                        break;
                    }
                case 3:
                    {
                        x = diemKetThuc.X;
                        y = diemBatDau.Y;
                        break;
                    }
                case 4:
                    {
                        x = diemBatDau.X;
                        y = yCenter;
                        break;
                    }
                case 5:
                    {
                        x = diemKetThuc.X;
                        y = yCenter;
                        break;
                    }
                case 6:
                    {
                        x = diemBatDau.X;
                        y = diemKetThuc.Y;
                        break;
                    }
                case 7:
                    {
                        x = xCenter;
                        y = diemKetThuc.Y;
                        break;
                    }
                case 8:
                    {
                        x = diemKetThuc.X;
                        y = diemKetThuc.Y;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return new Point(x, y);
        }

        // Chọn lại điểm bắt đầu, kết thúc khi bấm vào 1 điểm điều khiển nào đó
        
        protected override void ThayDoiDiem(int viTriDiemDieuKhien)
        {
            if (viTriDiemDieuKhien == 1 || viTriDiemDieuKhien == 2 || viTriDiemDieuKhien == 4)
            {
                Point point = diemBatDau;
                diemBatDau = diemKetThuc;
                diemKetThuc = point;
            }
            if (viTriDiemDieuKhien == 3)
            {
                diemBatDau = DiemDieuKhien(6);
                diemKetThuc = DiemDieuKhien(3);
            }
            if (viTriDiemDieuKhien == 6)
            {
                diemBatDau = DiemDieuKhien(3);
                diemKetThuc = DiemDieuKhien(6);
            }
            
        }
        
        // Thay đổi kích thước đối tượng khi biết 1 điểm điều khiển và điểm đến
        protected override void ThayDoiKichThuoc(int viTriDiemDieuKhien, Point newPoint)
        {
            int deltaX = newPoint.X - diemMouseDown.X;
            int deltaY = newPoint.Y - diemMouseDown.Y;
            diemMouseDown = newPoint;
            if (viTriDiemDieuKhien == 2 || viTriDiemDieuKhien == 7)
            {
                diemKetThuc.Y += deltaY;    //2 cạnh nằm ngang tịnh tiến lên xuống
            }
            else if (viTriDiemDieuKhien == 4 || viTriDiemDieuKhien == 5)
            {
                diemKetThuc.X += deltaX;    //2 cạnh đứng tịnh tiến trái phải
            }
            else
            {
                diemKetThuc = newPoint;     //các góc di chuyển theo chuột
            }
            
        }

        // Sự kiện chuột
        public override void Mouse_Down(MouseEventArgs e)
        {
            viTriChuotSoVoiHinhVe = KiemTraViTri(e.Location);
            if (viTriChuotSoVoiHinhVe > 0)  //đánh dấu bắt đầu thay đổi kích thước
            {
                thayDoiKichThuoc = true;
                ThayDoiDiem(viTriChuotSoVoiHinhVe);
                diemMouseDown = e.Location;
            }
            else if (viTriChuotSoVoiHinhVe == 0)    //đánh dấu băt đầu di chuyển
            {
                
                diChuyen = true;
                diemMouseDown = e.Location;
            }
            else //vẽ hình mới
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
#endregion

#region Chiến thuật thi hành lệnh bắt đối tượng
        public override void Mouse_Up(Object sender)                    
        {
            graphicsPath = new GraphicsPath();                          
            Pen pen = new Pen(mauVe, doDamNet);                         
            graphicsPath.AddRectangle(VeHCN(diemBatDau, diemKetThuc));  
            graphicsPath.Widen(pen);                                    
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));        
            khuVuc.Union(graphicsPath);                                 
            diChuyen = false;           
            thayDoiKichThuoc = false;   
            viTriChuotSoVoiHinhVe = -1;
            
        }
#endregion
    }
}
