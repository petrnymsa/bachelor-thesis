using System.Diagnostics;

namespace BachelorThesis.Helpers
{
    public static class DebugHelper
    {
        public static void Info(string msg)
        {
            Debug.WriteLine($"[info] {msg}");
        }
    }
}
