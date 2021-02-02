using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOIE
{
    class ImgPreparer
    {
        public Bitmap[] bitmaps;
        public List<Bitmap> images;

        public ImgPreparer(string path)
        {
            images = new List<Bitmap>();

            if (getFileExtension(path) == ".frm")
            {
                Frm frm = new Frm(path);
                images = frm.bitmaps;
            }
            else if (getFileExtension(path) == ".fofrm")
            {
                FoFrm fofrm = new FoFrm(path);
               
                foreach (string img in fofrm.imagePaths)
                {
                    images.Add(LoadBitmapUnlocked(Editor.fullPath + img));
                }
            } 
            else
            {
                images.Add(LoadBitmapUnlocked(path));
            }
        }

        private string getFileExtension(string path)
        {
            return Path.GetExtension(path).ToLower();
        }

        private Bitmap LoadBitmapUnlocked(string path)
        {
            using (Bitmap bm = new Bitmap(path))
            {
                return new Bitmap(bm);
            }
        }

    }
}
