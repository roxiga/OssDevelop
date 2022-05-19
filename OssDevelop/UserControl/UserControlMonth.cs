using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OssDevelop
{
    public partial class UserControlMonth : UserControl
    {
        private OssDevelop ossDevelop;
        public DateTime theDate;
 
        public UserControlMonth(OssDevelop oss)
        {
            InitializeComponent();
            ossDevelop = oss;
            theDate = oss.theDate;
            updateCalendar(true);
        }

        /* Determination of the day of the week

        Jan 1st 1 AD is a Monday in Gregorian calendar.
        So Jan 0th 1 AD is a Sunday [It does not exist technically].

        Every 4 years we have a leap year. But xy00 cannot be a leap unless xy divides 4 with reminder 0.
        y/4 - y/100 + y/400 : this gives the number of leap years from 1AD to the given year. As each year has 365 days (divdes 7 with reminder 1), unless it is a leap year or the date is in Jan or Feb, the day of a given date changes by 1 each year. In other case it increases by 2.
        y -= m<3 : If the month is not Jan or Feb, we do not count the 29th Feb (if it exists) of the given year. 
        So y + y/4 - y/100 + y/400  gives the day of Jan 0th (Dec 31st of prev year) of the year. (This gives the reminder with 7 of  the number of days passed before the given year began.)

        Array t:  Number of days passed before the month 'm+1' begins.

        So t[m-1]+d is the number of days passed in year 'y' upto the given date. 
        (y + y/4 - y/100 + y/400 + t[m-1] + d) % 7 is reminder of the number of days from Jan 0  1AD to the given date which will be the day (0=Sunday,6=Saturday).

        Description credits: Sai Teja Pratap (quora.com/How-does-Tomohiko-Sakamotos-Algorithm-work).
        */
        static int DayOfWeek(int y, int m, int d)
        {
            int[] t = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            if (m < 3)
                y--;
            return (y + y / 4 - y / 100 + y / 400 + t[m - 1] + d) % 7;

            //  string[] days = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
        }

        // get the first dow for the month
        int getFirstDow(int year, int month)
        {
            var dow = DayOfWeek(year, month, 2);
            // wrap around so monday is the first day of the week instead of sunday
            if (dow > 0)
                return dow - 1;
            else
                return 6;
        }

        private int getDaysInMonth(int year, int month)
        {
            //DateTime.IsLeapYear(year))
            return DateTime.DaysInMonth(year, month);
        }

        private Button getDateButton(int day)
        {
            Button[] buttons = { d0,  d1,  d2,  d3,  d4,  d5,  d6,
                                 d7,  d8,  d9,  d10, d11, d12, d13,
                                 d14, d15, d16, d17, d18, d19, d20,
                                 d21, d22, d23, d24, d25, d26, d27,
                                 d28, d29, d30, d31, d32, d33, d34,
                                 d35, d36, d37, d38, d39, d40, d41};
            return buttons[day];
        }

        private bool isWeekend(int year, int month, int day)
        {
            var dow = DayOfWeek(year, month, day);
            return (dow == 0 || dow == 6);
        }

        private void disableDateButton(int n)
        {
            Button but = getDateButton(n);
            but.Text = "";
            but.BackColor = Color.White;
            but.FlatAppearance.BorderSize = 0;
            but.Enabled = false;
        }

        public void updateCalendar(bool updateText)
        {
            // get the first day of week for the month
            int year = theDate.Year;
            int month = theDate.Month;
            int day = theDate.Day;

            int dow = getFirstDow(year, month);
            int dim = getDaysInMonth(year, month);

            var weekdayColor = Color.FromArgb(224, 224, 224);
            var weekendColor = Color.FromArgb(192, 192, 192);
            var selectedColor = Color.FromArgb(128, 128, 128);
 //           var todayColor = Color.FromArgb(160, 160, 160);

            toolTip1.RemoveAll();

            for (int n = 0; n < dow; n++)
                disableDateButton(n);

            var toolTip = new ToolTip();

            DataBase db = new DataBase();

            for (int n = 1; n <= dim; n++)
            {
                int date = int.Parse(String.Format("{0:D4}{1:D2}{2:D2}",year,month,n));
                Diary diary = db.GetDiary(date);
                Button but = getDateButton(dow + n - 1);
                if (diary.title != "")
                    but.Text = Convert.ToString(n) + " - " + diary.title;
                else
                    but.Text = Convert.ToString(n);
                if (diary.text != "")
                    toolTip1.SetToolTip(but,diary.text);
                if (n == day)
                    but.BackColor = selectedColor;
                else if (isWeekend(year, month, n))
                    but.BackColor = weekendColor;
                else
                    but.BackColor = weekdayColor;
                but.FlatAppearance.BorderSize = 1;
                but.FlatAppearance.BorderColor = Color.White;
                but.Enabled = true;
            }
            for (int n = dim; n + dow <= 41; n++)
                disableDateButton(dow + n);
        }

        private void selectDate(int day)
        {
            int year = theDate.Year;
            int month = theDate.Month;
            int dow = getFirstDow(year, month);
            theDate = new DateTime(year, month, day - dow + 1);
            ossDevelop.theDate = theDate;
            ossDevelop.SetDiary();

        }

        private void d0_Click(object sender, EventArgs e)
        {
            selectDate(0);
        }

        private void d1_Click(object sender, EventArgs e)
        {
            selectDate(1);
        }

        private void d2_Click(object sender, EventArgs e)
        {
            selectDate(2);
        }

        private void d3_Click(object sender, EventArgs e)
        {
            selectDate(3);
        }

        private void d4_Click(object sender, EventArgs e)
        {
            selectDate(4);
        }

        private void d5_Click(object sender, EventArgs e)
        {
            selectDate(5);
        }

        private void d6_Click(object sender, EventArgs e)
        {
            selectDate(6);
        }

        private void d7_Click(object sender, EventArgs e)
        {
            selectDate(7);
        }

        private void d8_Click(object sender, EventArgs e)
        {
            selectDate(8);
        }

        private void d9_Click(object sender, EventArgs e)
        {
            selectDate(9);
        }

        private void d10_Click(object sender, EventArgs e)
        {
            selectDate(10);
        }

        private void d11_Click(object sender, EventArgs e)
        {
            selectDate(11);
        }

        private void d12_Click(object sender, EventArgs e)
        {
            selectDate(12);
        }

        private void d13_Click(object sender, EventArgs e)
        {
            selectDate(13);
        }

        private void d14_Click(object sender, EventArgs e)
        {
            selectDate(14);
        }

        private void d15_Click(object sender, EventArgs e)
        {
            selectDate(15);
        }

        private void d16_Click(object sender, EventArgs e)
        {
            selectDate(16);
        }

        private void d17_Click(object sender, EventArgs e)
        {
            selectDate(17);
        }

        private void d18_Click(object sender, EventArgs e)
        {
            selectDate(18);
        }

        private void d19_Click(object sender, EventArgs e)
        {
            selectDate(19);
        }

        private void d20_Click(object sender, EventArgs e)
        {
            selectDate(20);
        }

        private void d21_Click(object sender, EventArgs e)
        {
            selectDate(21);
        }

        private void d22_Click(object sender, EventArgs e)
        {
            selectDate(22);
        }

        private void d23_Click(object sender, EventArgs e)
        {
            selectDate(23);
        }

        private void d24_Click(object sender, EventArgs e)
        {
            selectDate(24);
        }

        private void d25_Click(object sender, EventArgs e)
        {
            selectDate(25);
        }

        private void d26_Click(object sender, EventArgs e)
        {
            selectDate(26);
        }

        private void d27_Click(object sender, EventArgs e)
        {
            selectDate(27);
        }

        private void d28_Click(object sender, EventArgs e)
        {
            selectDate(28);
        }

        private void d29_Click(object sender, EventArgs e)
        {
            selectDate(29);
        }

        private void d30_Click(object sender, EventArgs e)
        {
            selectDate(30);
        }

        private void d31_Click(object sender, EventArgs e)
        {
            selectDate(31);
        }

        private void d32_Click(object sender, EventArgs e)
        {
            selectDate(32);
        }

        private void d33_Click(object sender, EventArgs e)
        {
            selectDate(33);
        }

        private void d34_Click(object sender, EventArgs e)
        {
            selectDate(34);
        }

        private void d35_Click(object sender, EventArgs e)
        {
            selectDate(35);
        }

        private void d36_Click(object sender, EventArgs e)
        {
            selectDate(36);
        }

        private void d37_Click(object sender, EventArgs e)
        {
            selectDate(37);
        }

        private void d38_Click(object sender, EventArgs e)
        {
            selectDate(38);
        }

        private void d39_Click(object sender, EventArgs e)
        {
            selectDate(39);
        }

        private void d40_Click(object sender, EventArgs e)
        {
            selectDate(40);
        }

        private void d41_Click(object sender, EventArgs e)
        {
            selectDate(41);
        }

        public void updateTheDate(int year, int month, int day)
        {
            if (day > getDaysInMonth(year, month))
                day = getDaysInMonth(year, month);

            theDate = new DateTime(year, month, day);

            ossDevelop.updateLabels(theDate);
            updateCalendar(true);
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = theDate.Year;
            int month = theDate.Month;
            int day = theDate.Day;

            updateTheDate(year, month, day);
        }
    }
}
