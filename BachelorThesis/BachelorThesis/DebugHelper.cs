using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BachelorThesis
{
    public static class DebugHelper
    {
        public static void Info(string msg)
        {
            Debug.WriteLine($"[info] {msg}");
        }
    }
}
