using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_XYZ_analysis
{
    public abstract class Utility
    {
        public static bool IsNum(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }
            return true;
        }
    }
}
