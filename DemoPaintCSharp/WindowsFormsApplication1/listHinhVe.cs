using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{

    class listHinhVe
    {
#region Thuộc tính
        public List<HinhVe> listHinh; // List các đối tượng        
#endregion

#region Khởi tạo
        public listHinhVe()
        {
            listHinh = new List<HinhVe>();
        }
#endregion

#region Phương thức
        // Vẽ
        public void Ve(Graphics g)
        {
            foreach (HinhVe hinhVe in listHinh)
            {
                hinhVe.Ve(g);
            }
        }

        // Xóa phần tử cuối
        public void XoaHinhCuoi()
        {
            try
            {
                for(int i=0;i<listHinh.Count;i++)
                    for(int j=i+1;j<listHinh.Count;j++)
                        if(listHinh[i].Equals(listHinh[j]))
                            listHinh.RemoveAt(j);


                listHinh.RemoveAt(listHinh.Count - 1);
            }
            catch
            {
                
            }
        }
#endregion
    }
}
