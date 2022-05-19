using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssDevelop
{
    public class Diary
    {
        public int date = 12340101;
        public string title = "";
        public string text = "";
        public Image? image = null;

        public Diary(int date)
        {
            this.date = date;
        }
    }
}
