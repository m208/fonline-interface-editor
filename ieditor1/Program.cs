using System;
using System.Windows.Forms;

namespace ieditor1
{
    static class Program
    {
        /// App entry point

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


    }




}
