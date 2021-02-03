using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FOIE
{

    public class ImgPreparer
    {

        public List<Bitmap> images;
        public int fps = 0;

        public ImgPreparer(string path)
        {
            images = new List<Bitmap>();

            if (getFileExtension(path) == ".frm")
            {
                Frm frm = new Frm(path);
                images = frm.bitmaps;
                fps = frm.framesPerSec;
            }
            else if (getFileExtension(path) == ".fofrm")
            {
                FoFrm fofrm = new FoFrm(path);
                fps = fofrm.fps;

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
