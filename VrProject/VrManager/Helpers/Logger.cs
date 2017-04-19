using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrManager.Helpers
{
    public class Logger
    {
        private FileStream _stream;

        public  void Write(string @string)
        {
            //using (_stream = new FileStream(@"D:\myLog.txt", FileMode.Append, FileAccess.Write))
            //{
            //    using (StreamWriter sw = new StreamWriter(_stream))
            //    {
            //        sw.WriteLine(DateTime.Now.ToString() + @string);
            //    }
            //}

        }

    }
}
