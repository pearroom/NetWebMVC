﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Command
{
    public class Log
    {
        public static void Print(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " +msg);
        }
    }
}
