﻿using System;
using System.Windows.Forms;

namespace PowerGridMapDemo
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ForceDirectedGraphForm());
        }
    }
}
