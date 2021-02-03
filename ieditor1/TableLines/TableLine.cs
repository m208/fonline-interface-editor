﻿using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FOIE.TableLines
{
    public class TableLine : Panel
    {
        public ControlInfo cInfo;
        public Color prevColor;

        public TableLine() { }

        public TableLine(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            createTableLine();
        }

        public void createTableLine()
        {
            Name = "panel" + cInfo.name;
            Size = new Size(450, 25);
            Tag = new tableRowTag { };

            // ------ LABELS ---------------------------------------------------

            Label lbl;
            if (cInfo.clType == "Area")
            {
                lbl = new LinkLabelHeader(cInfo.name);
            }
            else
            {
                lbl = new LabelHeader(cInfo.name);
            }

            string hint = Editor.getHintforKey(cInfo.name);     // tool tips from dictionary

            new ToolTip().SetToolTip(lbl, hint);
            this.Controls.Add(lbl);

            // ------ PICTURE ---------------------------------------------------

            PicBoxIcon pb = new PicBoxIcon(cInfo.name, cInfo.clType, cInfo.controlSuccess);
            this.Controls.Add(pb);

            // ------ TEXT BOXES ---------------------------------------------------

            TextBoxForValues tb1 = new TextBoxForValues(cInfo.name, cInfo.textValue, cInfo.clType);
            TextBoxForInfo tb2 = new TextBoxForInfo(cInfo.name, cInfo.textInfo);

            this.Controls.Add(tb1);
            this.Controls.Add(tb2);

        }

        // ------ METODS ---------------------------------------------------

        public void drawErrorIcon()
        {
            PicBoxIcon pb = this.Controls.Find("typePic", false).FirstOrDefault() as PicBoxIcon;
            pb.drawError();
        }

        public void drawOkIcon()
        {
            PicBoxIcon pb = this.Controls.Find("typePic", false).FirstOrDefault() as PicBoxIcon;
            pb.drawIcon(cInfo.clType);
        }

        public void updateValue(string value)
        {
            var tb = this.Controls.Find("tb" + cInfo.name, false).FirstOrDefault() as TextBox;
            tb.Text = value;
        }
        public void updateInfo(string value)
        {
            var tb = this.Controls.Find("size" + cInfo.name, false).FirstOrDefault() as TextBox;
            tb.Text = value;
        }

        public void highLiteRow(bool onOff)
        {
            if (onOff)
            {
                prevColor = this.BackColor;
                BackColor = Color.Violet;
            }
            else
            {
                BackColor = prevColor;
            }
        }

        public void setCheckBox(bool set)
        {
            CheckBoxImageSwitch cb = this.Controls.Find("cb", false).FirstOrDefault() as CheckBoxImageSwitch;
            cb.Checked = set;
        }

        public void showPlayButton(bool show)
        {
            ButtonToAnimate playBttn = this.Controls.Find("play", false).FirstOrDefault() as ButtonToAnimate;
            playBttn.Visible = show;
            playBttn.displayButton = show;

            

        }
    }

    //----   TAG OBJECT  ------------------------------------------------------------------------

    public class tableRowTag
    {
        public string parentName;
        public Color color;
    }
}

