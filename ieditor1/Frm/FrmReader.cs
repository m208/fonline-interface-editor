using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

//  fallout.fandom.com/wiki/FRM_file 

namespace FOIE
{

    public static class Pointers
    {
        public static Ptr framesPerSec = new Ptr(0x0004, 2, "unsigned");
        public static Ptr actionFrame = new Ptr(0x0006, 2, "unsigned");
        public static Ptr framesPerDirNum = new Ptr(0x0008, 2, "unsigned");

        public static Ptr dir0_shiftX = new Ptr(0x000A, 2, "signed");
        public static Ptr dir0_shiftY = new Ptr(0x0016, 2, "signed");

        public static Ptr frame0_Width = new Ptr(0x003E, 2, "unsigned");
        public static Ptr frame0_Heigth = new Ptr(0x0040, 2, "unsigned");
    }


    public class Frm
    {
        public int framesPerSec;
        public int actionFrame;
        public int dir0_shiftX, dir0_shiftY;

        public List<FrameImage> frames;
        public List<Bitmap> bitmaps;

        public Frm(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            {
                framesPerSec = readValue(reader, Pointers.framesPerSec);
                actionFrame = readValue(reader, Pointers.actionFrame);
                dir0_shiftX = readValue(reader, Pointers.dir0_shiftX);
                dir0_shiftY = readValue(reader, Pointers.dir0_shiftY);

                int framesPerDir = readValue(reader, Pointers.framesPerDirNum);
                frames = new List<FrameImage>(framesPerDir);

                int startOffset = Pointers.frame0_Width.offset;
                int frameSizeOffset = 0;

                for (int i = 0; i < framesPerDir; i++)
                {
                    int width = readValue(reader, Pointers.frame0_Width, frameSizeOffset);
                    int height = readValue(reader, Pointers.frame0_Heigth, frameSizeOffset);
                    int size = width * height;
                    byte[] data = new byte[size];

                    frameSizeOffset += 12;  // 12 bytes step: 0x003E frame area start; 0x004A pixels data start

                    reader.BaseStream.Seek(startOffset + frameSizeOffset, SeekOrigin.Begin);
                    reader.Read(data, 0, size);

                    FrameImage frame = new FrameImage(width, height, data);
                    frames.Insert(i, frame);

                    frameSizeOffset += size;
                }
                reader.Close();

                FrameImageToBitmaps();
            }
        }

        public int readValue(BinaryReader reader, Ptr item, int offset = 0)
        {
            byte[] data = new byte[item.size];
            reader.BaseStream.Seek(item.offset + offset, SeekOrigin.Begin);
            reader.Read(data, 0, item.size);

            Array.Reverse(data);                        //All values within the FRM file are stored in Big - Endian format as opposed to Little-Endian

            if (item.type == "signed")
                return BitConverter.ToInt16(data, 0);   // ! int16 2 bytes sized values 

            else
                return BitConverter.ToUInt16(data, 0);

        }

        public void FrameImageToBitmaps()
        {
            bitmaps = new List<Bitmap>(frames.Count);

            int i = 0;
            foreach (FrameImage frame in frames)
            {
                Bitmap bmp = new Bitmap(frame.width, frame.height);

                for (int y = 0; y < frame.height; y++)
                {
                    for (int x = 0; x < frame.width; x++)
                    {
                        int n = x + y * frame.width;
                        int value = Convert.ToInt16(frame.data[n]);
                        if (value != 0)
                        {
                            Color pixelColor = Palette.getPixelColor(value);
                            bmp.SetPixel(x, y, pixelColor);
                        }
                    }
                }
                bitmaps.Insert(i, bmp);
                i++;
            }
        }

    }

    //--------------------------------------------------------------------

    public class Ptr
    {
        public int offset, size;
        public string type;

        public Ptr(int _offset, int _size, string _type = "")
        {
            offset = _offset;
            size = _size;
            type = _type;
        }
    }

    //--------------------------------------------------------------------

    public class FrameImage
    {
        public int width, height;
        public byte[] data;

        public FrameImage(int _width, int _height, byte[] _data)
        {
            width = _width;
            height = _height;
            data = _data;
        }
    }

    //--------------------------------------------------------------------



}
