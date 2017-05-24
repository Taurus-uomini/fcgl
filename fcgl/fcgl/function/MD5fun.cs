using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace fcgl.function
{
    public class MD5fun
    {
        public string getMD5(string s)
        {
            byte[] bs = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(s.ToCharArray()));
            s = null;
            foreach (byte b in bs)
            {
                s += b.ToString("x");
            }
            return s;
        }
    }
}