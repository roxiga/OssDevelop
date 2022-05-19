using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OssDevelop
{
    public partial class OssDevelop : Form
    {
        private AboutBox? aboutBox = null;
        private SearchForm? searchForm = null;
        private int predate = 0;
        public DateTime theDate;

        public OssDevelop()
        {
            userControlYear = new UserControlYear(this);
            userControlMonth = new UserControlMonth(this);
            userControlDiary = new UserControlDiary(this);
            InitializeComponent();
            panel1.Controls.Add(userControlYear);
            panel1.Controls.Add(userControlMonth);
            panel1.Controls.Add(userControlDiary);
            theDate = DateTime.Today;
            SetMonth();
        }

        public void updateLabels(DateTime date)
        {
            theDate = date;
            userControlMonth.theDate = date;
            userControlDiary.theDate = date;
            lbToday.Text = theDate.ToString("yyyy年 MMMM d日 dddd");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutBox == null)
                aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchForm == null)
                searchForm = new SearchForm();
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                predate = 0;
                predate = search(predate);
            }
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            predate = search(predate);
        }

        private int search(int pre)
        {
           if (searchForm != null)
           {
                var keyword = searchForm.GetText();
                bool matchCase = searchForm.MatchCase();
                bool wholeWord = searchForm.WholeWord();
                bool regularX = searchForm.RegularExpression();
                DataBase db = new DataBase();
                Diary diary = db.findText(keyword, matchCase, wholeWord, regularX, pre);
                if (diary.date > 0)
                {
                    int year = (int)(diary.date / 10000);
                    int month = (int)(diary.date / 100) % 100;
                    int day = diary.date % 100;
                    theDate = new DateTime(year, month, day);
                    SetDiary();
                    return diary.date;
                }
            }
            return 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonYear_Click(object sender, EventArgs e)
        {
            userControlYear.Visible = true;
            userControlMonth.Visible = false;
            userControlDiary.Visible = false;
            userControlYear.SetSelectDate(theDate);
        }

        private void ButtonMonth_Click(object sender, EventArgs e)
        {
            SetMonth();
        }

        public void SetMonth()
        {
            userControlYear.Visible = false;
            userControlMonth.Visible = true;
            userControlDiary.Visible = false;
            updateLabels(theDate);
            userControlMonth.updateCalendar(true);
        }

        private void ButtonDiary_Click(object sender, EventArgs e)
        {
            SetDiary();
        }

        public void SetDiary()
        {
            userControlYear.Visible = false;
            userControlMonth.Visible = false;
            userControlDiary.Visible = true;
            updateLabels(theDate);
            userControlMonth.updateCalendar(true);
            userControlDiary.LoadDataBase();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            theDate = DateTime.Today;
            if (userControlYear.Visible)
            {
                updateLabels(theDate);
                userControlMonth.updateCalendar(true);
                userControlYear.SetSelectDate(DateTime.Today);
            }
            else if (userControlMonth.Visible)
            {
                updateLabels(theDate);
                userControlMonth.updateCalendar(true);
            }
            else
            {
                SetDiary();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (userControlYear.Visible)
            {
                theDate = theDate.AddYears(-1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
                userControlYear.SetSelectDate(theDate);
            }
            else if (userControlMonth.Visible)
            {
                theDate = theDate.AddMonths(-1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
            }
            else
            {
                theDate = theDate.AddDays(-1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
                SetDiary();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (userControlYear.Visible)
            {
                theDate = theDate.AddYears(1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
                userControlYear.SetSelectDate(theDate);
            }
            else if (userControlMonth.Visible)
            {
                theDate = theDate.AddMonths(1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
            }
            else
            {
                theDate = theDate.AddDays(1);
                userControlMonth.updateTheDate(theDate.Year, theDate.Month, theDate.Day);
                SetDiary();
            }
        }
    }
}

