using ieditor1;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FOIE
{
    public class AddControlButton : AddControlArea
    {
        public List<Bitmap> bitmaps = new List<Bitmap>();

        public AddControlButton(string[] line, Form1 parentForm)
        {
            generateControl(line[0], parentForm);

            bool fileExist;
            string infoFieldTxt;
            Bitmap img;

            string value = "";

            for (int i = 1; i < line.Length; i++)
            {

                if (Editor.iniArray.ContainsKey(line[i]))
                {
                    value = Editor.iniArray[line[i]];
                }

                string path = getFullPath(value);
                if (File.Exists(path))
                {
                    img = GetBitmap(path);
                    infoFieldTxt = img.Width + "x" + img.Height;
                    fileExist = true;
                }
                else
                {
                    img = new Bitmap(FOIE.Properties.Resources.nofile1);
                    infoFieldTxt = "error";
                    fileExist = false;
                }
                bitmaps.Add(img);

                ControlInfo cInfo = new ControlInfo
                {
                    name = line[i],
                    clType = "Picture",
                    textValue = value,
                    textInfo = infoFieldTxt,
                    controlSuccess = fileExist,
                    picIndex = i - 1,
                    parentForm = parentForm
                };
                controlInfo.Add(cInfo);

                //controlInfo.Add(new ControlInfo(line[i], "Picture", value, infoFieldTxt, fileExist));
            }

            if (bitmaps != null && bitmaps.Count > 0)
            {
                picBox.Image = bitmaps[0];
                picBox.BackgroundImage = null;
            }
        }
    }
}
