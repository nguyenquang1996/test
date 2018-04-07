using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
 #region Thuộc tính
        private listHinhVe lHV;          // List đối tượng
        private HinhVe hinhHienTai;       // đối tượng hình hiện tại sẽ vẽ
        private int IDhinhHienTai;       // ID của đối tượng hình hiện tại


        private Color mauVe; // Màu vẽ
        private int doDamNet; // Độ đậm
        private int penLevel; //số cột sóng thể hiện độ đậm nét vẽ


        private Bitmap hinhNenChim;     //hình nền chìm
        private Bitmap hinhNenNoi;      // hình nền nổi

        bool isMoving;

        int lastBtnxID;



        List<Label> listLabel = new List<Label>();
        List<Button> listButton = new List<Button>();

      //  HinhVe hinhChinhSuaCuoiCung;

        HinhVe hinhCopy;
        #endregion

        public Form1()
        {
            InitializeComponent();
            KhoiTao();
        }
        public void KhoiTao()
        {
            lHV = new listHinhVe();

            hinhNenChim = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics()); //tạo 1 hình bitmap
            Graphics g = Graphics.FromImage(hinhNenChim);   //lấy đối tượng Graphics từ bitmap
            g.Clear(Color.White);                           //xóa trắng bề mặt

            hinhNenNoi = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics());
            g = Graphics.FromImage(hinhNenNoi);
            g.Clear(Color.White);

            penLevel = 1;
            mauVe = Color.Black;
            doDamNet = 1;

            listButton.Add(btnColor1);
            listButton.Add(btnColor2);
            listButton.Add(btnColor3);
            listButton.Add(btnColor4);
            listButton.Add(btnColor5);
            listButton.Add(btnColor6);

            colorPicker.SelectedColor = Color.Black;

            listLabel.Add(lbl5);
            listLabel.Add(lbl4);
            listLabel.Add(lbl3);
            listLabel.Add(lbl2);
            listLabel.Add(lbl1);

            UpdatePen(penLevel, mauVe);

            lastBtnxID = 0;                  //ID của nút bấm
            btnxConTro.Enabled = false;      

            isMoving = false;

            hinhCopy = null;

            IDhinhHienTai = 0;
            lHV.listHinh.Clear();

            pictureBox.Refresh();       //vẽ lại pictureBox-làm mới

        }
        #region Sự kiện Pic_Paint trên pictureBox
        private void Pic_Paint(object sender, PaintEventArgs e)
        {
            hinhNenChim = hinhNenNoi.Clone(new Rectangle(0, 0, hinhNenNoi.Width, hinhNenNoi.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb); //sao chép hình nền
            
            Graphics g = Graphics.FromImage(hinhNenChim);

            if (lHV.listHinh.Count > 0)     
                lHV.Ve(g);                  //vẽ các hình có trong listHinh lên hình nền
            e.Graphics.DrawImageUnscaled(hinhNenChim, 0, 0);    

        }
        #endregion

        #region Xử lí sự kiện chuột
        private void PictureBox_Mouse_Down(object sender, MouseEventArgs e)
        {
           

 
            if (e.Button == MouseButtons.Left)
            {
           
                if (isMoving)
                {
                    //đang di chuyển hình
                }
                else
                {
                    if (hinhHienTai == null || hinhHienTai.KiemTraViTri(e.Location) == -1)
                    {
                        hinhHienTai = LayHinhVeHienTai(IDhinhHienTai);
                        

                    }
                    if (hinhHienTai != null)
                    {
                        hinhHienTai.Mouse_Down(e);          //gọi sự kiện mouse_down của hình
                        pictureBox.Refresh();               //làm mới
                        hinhHienTai.VeKhung(pictureBox.CreateGraphics());      //vẽ 8 hình chữ nhật nhỏ (chấm vuông nhỏ) làm khung xung quanh   
                        lHV.listHinh.Insert(lHV.listHinh.Count, hinhHienTai);            //thêm hình mới vào list
                    }
                }
            }
            else
            {
                hinhHienTai = null;

            }
        }

        private void PictureBox_Mouse_Move(object sender, MouseEventArgs e)
        {


            lblPosition.Text = "Vị trí chuột: (" + e.Location.X.ToString() + ", " + e.Location.Y.ToString()+")";

            if (IDhinhHienTai == -1)  //nút "Vị trí và kích cỡ" đã được bấm
            {
               
                if (isMoving == false)
                {
                    for (int i = lHV.listHinh.ToArray().Length - 1; i >= 0; i--)        //kiểm tra từng hình xem hình nào bị...
                    {
                        
                        int vt = (lHV.listHinh.ToArray())[i].KiemTraViTri(e.Location);
                        if (vt == 0)        //...chuột di chuyển trên bề mặt    
                        {
                            hinhHienTai = (lHV.listHinh.ToArray())[i];
                            if (e.Button == MouseButtons.Left)      //=> sẽ di chuyển hình này
                            {
                                Cursor = Cursors.Hand;
                                hinhHienTai.diChuyen = true;             //cho phép di chuyển
                                hinhHienTai.thayDoiKichThuoc = false;   //không cho phép thay đổi kích thước
                                isMoving = true;                        //bật cờ đang di chuyển

                             
                               
                                BtnxCircle_Click(btnxConTro, e);        //mượn nút "Chuột" để di chuyển (bản thân nút "Vị trí và kích cỡ" sẽ không di chuyển hình
                                pictureBox.Refresh();
                                hinhHienTai.VeKhung(pictureBox.CreateGraphics());        //vẽ khung
                                lHV.listHinh.RemoveAt(i);                                           //sau khi di chuyển sẽ phát sinh hình mới tại vị trí mới=>xóa hình cũ
                            }
                            else // chuột đi qua mà không bấm
                            {
                                isMoving = false;
                                hinhHienTai.diChuyen = false;
                            }


                            Cursor = Cursors.Hand;
                            pictureBox.Refresh();
                            hinhHienTai.VeKhung(pictureBox.CreateGraphics());
                            break;
                        }
                        else if (vt > 0) //chuột chỉ đúng điểm điều khiển (1 trong 8 chấm vuông nhỏ làm khung)   => sẽ thay đổi kích thước hình này
                        {
                            hinhHienTai = (lHV.listHinh.ToArray())[i];
                            if (e.Button == MouseButtons.Left)
                            {
                                hinhHienTai.thayDoiKichThuoc = true;        //cho phép thay đổi kích thước
                                hinhHienTai.diChuyen = false;               //không cho phép di chuyển
                                isMoving = true;

                                
                                BtnxCircle_Click(btnxConTro, e);
                           

                                pictureBox.Refresh();
                                hinhHienTai.VeHCNDiemDieuKhien(pictureBox.CreateGraphics(),5);
                                lHV.listHinh.RemoveAt(i);
                            }
                            else
                            {
                                isMoving = false;
                                hinhHienTai.thayDoiKichThuoc = false;

                            }

                          

                            Cursor = Cursors.Cross;
                            pictureBox.Refresh();                            
                            hinhHienTai.VeHCNDiemDieuKhien(pictureBox.CreateGraphics(),5);                            
                            break;
                        }

                        else //tìm trong danh sách không có hình nào bị chuột đi qua
                        {
                            Cursor = Cursors.Default;
                          
                        }
                    }
                }
            }
            else  //không phải nút "Vị trí và kích cỡ" => là nút vẽ hình hoặc nút "Chuột"
            {
                if (hinhHienTai != null)
                {
                   
                    if (hinhHienTai.KiemTraViTri(e.Location) > 0)   //nếu chuột chỉ đúng 1 trong 8 chấm vuông nhỏ => đổi chuột thành hình dấu +
                        Cursor = Cursors.Cross;
                            
                    else if (hinhHienTai.KiemTraViTri(e.Location) == 0)     //tương tự với lúc chuột nằm trong hình => chuột hình bàn tay
                        Cursor = Cursors.Hand;
                    else
                        Cursor = Cursors.Default;       //còn lại thì mặc định
                }
            
                if (e.Button == MouseButtons.Left)
                {
                    
                    if (hinhHienTai != null)    
                    {
                      
                        hinhHienTai.Mouse_Move(e);
                        pictureBox.Refresh();
                        hinhHienTai.VeKhung(pictureBox.CreateGraphics());
                    }
                }
            }
        }

        private void PictureBox_Mouse_Up(object sender, MouseEventArgs e)
        {
            
            if (hinhHienTai!=null && hinhHienTai.loaiHinh == 0 && isMoving==false)
            {
                lHV.XoaHinhCuoi();
                pictureBox.Refresh();

                hinhHienTai = null;
            }
            if (hinhHienTai != null && isMoving==false)
            {
                lHV.listHinh.Insert(lHV.listHinh.Count , hinhHienTai); //thêm hình mới vào list
                hinhHienTai.Mouse_Up(sender);
                hinhHienTai.VeKhung(pictureBox.CreateGraphics());
              
               
            }
            if (isMoving)
            {

                hinhHienTai.Mouse_Up(sender);
                pictureBox.Refresh();
                BtnxCircle_Click(btnxSizeMove, e);
                isMoving = false;

            }

        }
        #endregion

        private void Form1_Resize(object sender, EventArgs e)       //sự kiện thay đổi kích thước của form=>hình nền và pictureBox cũng thay đổi theo
        {
            hinhNenChim = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics());
            Graphics g = Graphics.FromImage(hinhNenChim);
            g.Clear(Color.White);
            hinhNenNoi = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics());
            g = Graphics.FromImage(hinhNenNoi);
            g.Clear(Color.White);
        }
        HinhVe LayHinhVeHienTai(int IDHinhAnhHienTai)
        {
            
            switch (IDHinhAnhHienTai)
            {

                
                case -1: return null;
                case 0: return new ConTro();
                case 1: return new Elip(mauVe, doDamNet);
                case 2: return new HinhChuNhat(mauVe, doDamNet);
                case 3: return new TamGiac(mauVe, doDamNet);
                case 4: return new DuongThang(mauVe, doDamNet);
                default: return new ConTro(mauVe, doDamNet);
            }
        }
        private void BtnxCircle_Click(object sender, EventArgs e)
        {
            ((ButtonX)sender).Enabled = false;

            switch (lastBtnxID)
            {
                case -1: btnxSizeMove.Enabled = true;
                    break;
                case 0: btnxConTro.Enabled = true;
                    break;
                case 1: btnxElip.Enabled = true;
                    break;
                case 2: btnxHCN.Enabled = true;
                    break;
                case 3: btnxTamGiac.Enabled = true;
                    break;
                case 4: btnxDuongThang.Enabled = true;
                    break;
                default: break;
            }

            
            IDhinhHienTai = Convert.ToInt16(((ButtonX)sender).Tag);
            lastBtnxID = IDhinhHienTai;
            if (IDhinhHienTai == -1)
            {                
                return;
            }
            ((ButtonX)sender).Enabled = false;
           

        }


        private void ColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            
                mauVe = colorPicker.SelectedColor;
                UpdateColorButton(mauVe);
                UpdatePen(penLevel, mauVe);
                if (hinhHienTai != null)
                {
                    hinhHienTai.mauVe = mauVe;
                    pictureBox.Refresh();
                    hinhHienTai.VeKhung(pictureBox.CreateGraphics());
                }
            
        }

        void UpdateColorButton(Color newColor)
        {
            for (int i = 0; i < listButton.ToArray().Length-1; i++)
            {
                listButton[i].BackColor = listButton[i + 1].BackColor;
            }
            listButton[listButton.ToArray().Length-1].BackColor = newColor;
        }


        private void BtnxColor_Click(object sender, EventArgs e)
        {
            mauVe = ((Button)sender).BackColor;
            UpdatePen(penLevel, mauVe);
            if (hinhHienTai != null)
            {
                hinhHienTai.mauVe = mauVe;
                pictureBox.Refresh();
                hinhHienTai.VeKhung(pictureBox.CreateGraphics());
            }
        }
        private void UpdatePen(int penLevel, Color cl)
        {
            if (penLevel <= listLabel.ToArray().Length)
            {
                
                foreach (Label lbl in listLabel)
                {
                    lbl.BackColor = Color.White;
                }
                for (int i = 0; i < penLevel; i++)
                {
                    listLabel[i].BackColor = cl;
                }
            }
            doDamNet = 2 * penLevel;
        }
        private void BtnxPen_Click(object sender, EventArgs e)
        {
            penLevel++;
            if (penLevel == 6)
                penLevel = 1;
            UpdatePen(penLevel, mauVe);
            if (hinhHienTai!=null)
            {
                hinhHienTai.doDamNet = doDamNet; //làm mới độ đậm
                hinhHienTai.mauVe = mauVe;
                pictureBox.Refresh();
                hinhHienTai.VeKhung(pictureBox.CreateGraphics());
            }
        }

        private void LblDoDamNet_Click(object sender, EventArgs e)
        {
            
            Label lbl = ((Label)sender);
            int index = listLabel.IndexOf(lbl);
            if (index > 0)
            {
                if (index == 1)
                {
                    UpdatePen(2, mauVe);
                }
                if (index >= penLevel)
                {
                    penLevel = index+1;
                    UpdatePen(penLevel, mauVe);
                }
                else if (index < penLevel)
                {
                    penLevel = index;
                    UpdatePen(penLevel, mauVe);
                }
                
                if (hinhHienTai != null)
                {
                    hinhHienTai.doDamNet = doDamNet; //làm mới độ đậm
                    pictureBox.Refresh();
                    hinhHienTai.VeKhung(pictureBox.CreateGraphics());
                }
            }
        }


    


        private void BtnxDelete_Click(object sender, EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh==0)
            {
                lHV.XoaHinhCuoi();
                lHV.listHinh.Remove(hinhHienTai);
                pictureBox.Refresh();
                hinhHienTai = null;
            }
            else if (hinhHienTai != null && hinhHienTai.loaiHinh != 0)
            {
                lHV.listHinh.Remove(hinhHienTai);
                pictureBox.Refresh();
                hinhHienTai = null;
            }
        }

        private void BtnxSizeMove_Click(object sender, EventArgs e)
        {
            BtnxCircle_Click(sender, e);
        }

        private void BtnxConTro_Click(object sender, EventArgs e)
        {
            BtnxCircle_Click(btnxConTro, e);

        }



        private void BtnxCut_Click(object sender, EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh != 0)
            {
                hinhCopy = hinhHienTai;
                lHV.XoaHinhCuoi();
                lHV.listHinh.Remove(hinhHienTai);
                pictureBox.Refresh();
            }
        }


        private void Copy(HinhVe hinhHienTai , HinhVe hinhCopy)
        {
            hinhCopy.diemBatDau = hinhHienTai.diemBatDau;
            hinhCopy.diemKetThuc = hinhHienTai.diemKetThuc;
            hinhCopy.khuVuc = hinhHienTai.khuVuc;
            hinhCopy.mauVe = hinhHienTai.mauVe;
            hinhCopy.doDamNet = hinhHienTai.doDamNet;
        }
        private void BtnxCopy_Click(object sender , EventArgs e)
        {
            if (hinhHienTai != null && hinhHienTai.loaiHinh != 0)
            {

                switch (hinhHienTai.loaiHinh)
                {
                    case 1: hinhCopy = new Elip(); Copy(hinhHienTai , hinhCopy);
                        break;
                    case 2: hinhCopy = new HinhChuNhat(); Copy(hinhHienTai , hinhCopy);
                        break;
                    case 3: hinhCopy = new TamGiac(); Copy(hinhHienTai , hinhCopy);
                        break;
                    case 4: hinhCopy = new DuongThang(); Copy(hinhHienTai , hinhCopy);
                        break;
                    default: break;
                }

                
                pictureBox.Refresh();
            }
        }
        private void ButtonItem1_Click(object sender, EventArgs e)
        {

        }



        private void SubBtnXoaHet_Click(object sender , EventArgs e)
        {
            lHV.listHinh.Clear();
            pictureBox.Refresh();
        }

        private void SubBtnXoaHinhCuoi_Click(object sender , EventArgs e)
        {
            lHV.XoaHinhCuoi();
            pictureBox.Refresh();
            hinhHienTai = null;
        }



        private void RibbonControl1_Click(object sender , EventArgs e)
        {

        }



        private void Form1_Load(object sender , EventArgs e)
        {

        }

        private void PictureBox_Click(object sender , EventArgs e)
        {

        }

        private void RibbonTabItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
