﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ieditor1
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
        static public int mainImageWidth, mainImageHeight;


        //---------------------------------------------------------------------------------
        //  INVENTORY
        static public string GuiInventoryImgMain = "InvMain";
        static public string[] GuiInventoryControls = {
            "InvSlot1",
            "InvSlot2",
            "InvArmor",
            "InvText",
            "InvChosen",
            "InvInv",
        };
        static public string[] GuiInventoryButtons =
        {
             "InvOk",
            "InvScrDn",
            "InvScrUp"

        };
        //---------------------------------------------------------------------------------
        //  FIXBOY
        static public string GuiFixBoyImgMain = "FixMain";
        static public string[] GuiFixBoyControls = {
            "FixScrUp",
            "FixScrDn",
            "FixFix",
            "FixDone",
            "FixWin",
            "FixNum",
            "FixUp",
            "FixDow",
            "FixButton1",
            "FixButton2",
            "FixButton3",
            "FixButton4",
            "FixButton5",
        };


        //---------------------------------------------------------------------------------
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
            
            int[] ret = { a[2]-a[0], a[3]-a[1] };

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
                string jsonString = System.IO.File.ReadAllText(@jsonPath+"\\config.json");
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
            string ret = (coords[2] - coords[0]) + "x" + (coords[3] - coords[1]);
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
            this.TagDictionary = new Dictionary<string, object>();
        }

        public void Set(string key, object value)
        {
            this.TagDictionary[key] = value;
        }

        public object Get(string key)
        {
            if (TagDictionary.ContainsKey(key))
            {
                return this.TagDictionary[key];
            }
            else return "empty";
            
        }
    }










}