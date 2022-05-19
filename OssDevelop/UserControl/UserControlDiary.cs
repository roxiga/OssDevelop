using System.IO;
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
    public partial class UserControlDiary : UserControl
    {
        private OssDevelop ossDevelop;
        public DateTime theDate;

        public UserControlDiary(OssDevelop oss)
        {
            ossDevelop = oss;
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int date = int.Parse(theDate.ToString("yyyyMMdd"));
            Diary diary = new Diary(date);
            diary.title = this.textBoxTitle.Text;
            diary.text = this.textBoxText.Text;
            DataBase db = new DataBase();
            string sql = db.CreateDiary(diary);
            if (sql == "失敗")
                MessageBox.Show(sql);
        }

        public void LoadDataBase()
        {
            DataBase db = new DataBase();
            int date = int.Parse(theDate.ToString("yyyyMMdd"));
            Diary diary = db.GetDiaryAndImage(date);
            this.textBoxTitle.Text = diary.title;
            this.textBoxText.Text = diary.text;
             this.pictureBoxImage.Image = diary.image;
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png)|*.bmp;*.jpg;*.png|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                this.pictureBoxImage.Load(ofd.FileName);
                DataBase db = new DataBase();
                int date = int.Parse(theDate.ToString("yyyyMMdd"));
                db.AddImage(date,pictureBoxImage.Image);
            }
        }
    }
}
