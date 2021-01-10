using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FOIE
{
    class Editor
    {

        static public Dictionary<string, string> controlTypesResources = new Dictionary<string, string>
        {
            ["area"] = "icon_hatch",
            ["picture"] = "icon_image",
            ["custom"] = "typography",
            ["error"] = "icon_error",
        };

        static public string fullPath = "";
        static public string iniPath = "";
        static public string currentBackground = "";

        static public Dictionary<string, string> iniArray = new Dictionary<string, string>();
        static public int lineCounter = 0;
        static public int mainImageWidth = 0, mainImageHeight = 0;

        //---------------------------------------------------------------------------------
        
        
        //static public void 
        
        
        
        static public void iniRead(string path)
        {
            iniArray.Clear();
            //string path = fullPath + "default.ini";
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
                    //MessageBox.Show(key+val);
                }
                
            }

            //      RESOLUTION BLOCKS

            int resBX = 0, resBY = 0;
            int linec = 0;

            foreach (string line in lines.Reverse()) 
            {
                string l = line.Trim();
                int indexOfCharSharp = l.IndexOf('#');
                bool resBlockHeader = l.Contains("resolution");

                if (indexOfCharSharp != 0 && resBlockHeader) {

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
                            // MessageBox.Show(line + " 1");

                            int index = Array.IndexOf(lines, line);
                            readResolutionBlock(lines, index+1);
                        }
                    }
                }
            }

            //MessageBox.Show(resBX.ToString()+ resBY.ToString());

            //      END RES BLOCKS
        }

        static public void readResolutionBlock(string[] lines, int index)
        {
            for (int i = index; i < lines.Length; i++)
                {
                    string key = "";
                    string val = "";
                    string l = lines[i].Trim();
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
                        //MessageBox.Show(key+val);
                    }

                    if (i+1<lines.Length && lines[i + 1].Contains("resolution"))
                     {
                        i = lines.Length;
                     }


               // }
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

        static public List<string> jsonKeys = new List<string>();

        static public JObject json = new JObject();

        static public bool GetJson()
        {
            json.RemoveAll();
            string jsonPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            try
            {
                string jsonString = System.IO.File.ReadAllText(@jsonPath + "\\config.json");
                json = JObject.Parse(jsonString);
                jsonKeys.Clear();
                foreach (JProperty property in json.Properties())
                {
                    jsonKeys.Add(property.Name);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

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
    }


    public class tableRowTag
    {
        public Dictionary<string, object> TagDictionary { get; set; }

        public tableRowTag()              //Cunstractor
        {
            TagDictionary = new Dictionary<string, object>();
        }

        public void Set(string key, object value)
        {
            TagDictionary[key] = value;
        }

        public object Get(string key)
        {
            if (TagDictionary.ContainsKey(key))
            {
                return TagDictionary[key];
            }
            else return "empty";

        }
    }


}
