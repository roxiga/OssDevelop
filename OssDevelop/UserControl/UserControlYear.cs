using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OssDevelop
{
    public partial class UserControlYear : UserControl
    {
        OssDevelop ossDevelop;

        public UserControlYear(OssDevelop oss)
        {
            InitializeComponent();
            ossDevelop = oss;
            this.monthCalendar1.TodayDate = DateTime.Today;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            ossDevelop.theDate = e.Start;
            ossDevelop.SetMonth();
        }

        public void SetSelectDate(DateTime date)
        {
            this.monthCalendar1.SetDate(date);
            this.monthCalendar1.Invalidate();
        }
    }
}
