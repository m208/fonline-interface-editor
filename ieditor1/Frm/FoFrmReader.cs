using System.Collections.Generic;
using System.IO;

namespace FOIE
{
    class FoFrm
    {
        public int fps;
        public int count;
        public int shiftX, shiftY;

        public List<string> imagePaths;

        public FoFrm(string path)
        {
            if (File.Exists(path))
            {
                imagePaths = new List<string>();
                string[] lines = System.IO.File.ReadAllLines(@path);

                foreach (string line in lines)
                {
                    string[] param = line.Trim().Split('=');

                    if (param.Length == 2)
                    {
                        string item = param[0];
                        string value = param[1];


                        if (item == "fps") this.fps = int.Parse(value);
                        else if (item == "count") this.count = int.Parse(value);
                        else if (item == "offs_x") this.shiftX = int.Parse(value);
                        else if (item == "offs_y") this.shiftY = int.Parse(value);

                        else if (item.Contains("frm_"))
                        {
                            int index = int.Parse(item.Substring("frm_".Length));
                            imagePaths.Insert(index, value);
                        }
                    }
                }
            }
        }

    }
}
