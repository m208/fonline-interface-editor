using FOIE.TableLines;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;


namespace FOIE
{
    class Editor
    {


        static public List<TableLine> tableRows = new List<TableLine>();

        static public Dictionary<string, string> controlTypesResources = new Dictionary<string, string>
        {
            ["Area"] = "icon_hatch",
            ["AreaMain"] = "icon_hatch",
            ["Picture"] = "icon_image",
            ["Custom"] = "typography",
            ["error"] = "icon_error",
        };

        static public string currentBackground = "";
        static public string fullPath = "";
        static public string iniPath = "";

        static public string cfgPath = "default.cfg";   // default name


        static public Dictionary<string, string> hintBook = new Dictionary<string, string>();
        static public Dictionary<string, string> iniArray = new Dictionary<string, string>();
        static public int lineCounter = 0;
        static public int mainImageWidth = 0, mainImageHeight = 0;


        //      Registry job   -------------------------------------------
        static string regKeyName = "Software\\FOnline Interface Editor";

        public static void WriteParam()
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.CurrentUser.CreateSubKey(regKeyName);
                if (rk == null) return;

                rk.SetValue("ConfigFile", cfgPath);

            }
            finally
            {
                if (rk != null) rk.Close();
            }
        }

        public static void ReadParam()
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(regKeyName);
                if (rk != null)
                {
                    string val = (string)rk.GetValue("ConfigFile");
                    if (val != "") cfgPath = val;
                }
            }
            finally
            {
                if (rk != null) rk.Close();
            }
        }


        //  --------------  INI READING     -----------------------

        static public void iniRead(string path)
        {
            iniArray.Clear();
            string[] lines = System.IO.File.ReadAllLines(@path);

            foreach (string line in lines.Reverse())
            //foreach (string line in lines)
            {
                string key = "";
                string val = "";
                string l = line.Trim();
                int indexOfCharEquals = l.IndexOf('=');
                int indexOfCharSharp = l.IndexOf('#');

                if (indexOfCharEquals > 0 && indexOfCharSharp != 0)
                {
                    key = l.Substring(0, indexOfCharEquals);
                    key = key.Trim();
                    val = l.Substring(indexOfCharEquals + 1);
                    if (val.IndexOf('#') > 0)
                    {
                        val = val.Substring(0, val.IndexOf('#'));
                    }
                    val = val.Trim();
                    iniArray[key] = val;
                }

            }

            //      RESOLUTION BLOCKS

            int resBX = 0, resBY = 0;

            foreach (string line in lines.Reverse())
            {
                string l = line.Trim();
                int indexOfCharSharp = l.IndexOf('#');
                bool resBlockHeader = l.Contains("resolution");

                if (indexOfCharSharp != 0 && resBlockHeader)
                {

                    string[] subs = l.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    int x = Int32.Parse(subs[1]);
                    if (x > resBX) { resBX = x; }

                    int y = Int32.Parse(subs[2]);
                    if (y > resBY) { resBY = y; }
                }
            }

            if (resBX > 0 || resBY > 0)
            {
                foreach (string line in lines.Reverse())
                {
                    string l = line.Trim();

                    if (l.IndexOf('#') != 0)
                    {
                        if ((l.Contains("resolution") && l.Contains(resBX.ToString()) && l.Contains(resBY.ToString())) || (l.Contains("resolution") && l.Contains(resBX.ToString())))
                        {
                            int index = Array.IndexOf(lines, line);
                            readResolutionBlock(lines, index + 1);
                        }
                    }
                }
            }

            //      END RES BLOCKS
        }

        static public void readResolutionBlock(string[] lines, int index)
        {
            bool breakvar = false;
            for (int i = index; !breakvar; i++)
            {
                string l = lines[i].Trim();
                int indexOfCharEquals = l.IndexOf('=');
                int indexOfCharSharp = l.IndexOf('#');

                if (indexOfCharEquals > 0 && indexOfCharSharp != 0)
                {
                    string key = l.Substring(0, indexOfCharEquals);
                    key = key.Trim();
                    string val = l.Substring(indexOfCharEquals + 1);
                    if (val.IndexOf('#') > 0)
                    {
                        val = val.Substring(0, val.IndexOf('#'));
                    }
                    val = val.Trim();
                    iniArray[key] = val;
                }

                if (i + 1 == lines.Length)
                {
                    breakvar = true;
                }
                else if (i + 1 < lines.Length && lines[i + 1].Contains("resolution"))
                {
                    breakvar = true;
                }
            }
        }


        //---------------------------------------------------------------------------------
        static public int[] stringToRectArray(string str)
        {
            try
            {
                return str.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();
            }
            catch
            {
                return null;
            }

        }
        //---------------------------------------------------------------------------------
        static public int[] stringToRectSize(string str)
        {
            int[] a = str.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToArray();

            int[] ret = { a[2] - a[0], a[3] - a[1] };

            return ret;
        }

        static public bool isValidRect(int[] coords)
        {
            if (coords.Length == 4 && coords[2] >= coords[0] && coords[3] >= coords[1])
                return true;
            else return false;
        }

        //---------------------------------------------------------------------------------

        //      READ JSON CONFIG

        static public List<string> configJsonKeys = new List<string>();
        static public JObject configJSON = new JObject();

        static public bool GetJson()
        {
            configJSON.RemoveAll();
            string jsonPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            try
            {
                string jsonString = System.IO.File.ReadAllText(@jsonPath + "\\" + @cfgPath);
                configJSON = JObject.Parse(jsonString);

                configJsonKeys.Clear();
                foreach (JProperty property in configJSON.Properties())
                {
                    configJsonKeys.Add(property.Name);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //-------------------------------------------------------------------------------
        static public void getHints()
        {
            hintBook.Clear();
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@path + "\\hintbook.ini");

                foreach (string line in lines)
                {
                    string l = line.Trim();
                    int indexOfCharSharp = l.IndexOf('#');

                    if (indexOfCharSharp > 0)
                    {
                        string key = l.Substring(0, indexOfCharSharp);
                        key = key.Trim();
                        string val = l.Substring(indexOfCharSharp + 1);
                        val = val.Trim();
                        hintBook[key] = val;
                    }
                }
            }
            catch
            {
                // hints file cant be read
            }
        }

        static public string getHintforKey(string name)
        {
            string hint = name + " ";
            if (hintBook.ContainsKey(name))
            {
                hint += "#" + Editor.hintBook[name];
            }
            return hint;
        }

        //-------------------------------------------------------------------------------

        static public string getSizeFromStringCoords(string str)
        {
            int[] coords = stringToRectArray(str);
            string ret = coords[2] - coords[0] + "x" + (coords[3] - coords[1]);
            return ret;
        }

        static public bool IsEven(int a)
        {
            return (a & 1) == 0;
        }
        //----------------------------------------------------------------------------

        public static string getFullPath(string filename)
        {
            return Editor.fullPath + filename;
        }

        public static Bitmap GetBitmapFromPath(string path)
        {
            Bitmap controlImage;
            if (getFileExtension(path) == ".frm")
            {
                Frm frmImg = new Frm(path);
                controlImage = frmImg.bitmaps[0];
            }
            else
            {
                controlImage = LoadBitmapUnlocked(path);
            }
            return controlImage;
        }

        private static string getFileExtension(string path)
        {
            return Path.GetExtension(path).ToLower();
        }

        private static Bitmap LoadBitmapUnlocked(string path)
        {
            using (Bitmap bm = new Bitmap(path))
            {
                return new Bitmap(bm);
            }
        }

        //----------------------------------------------------------------------------



    }


}
