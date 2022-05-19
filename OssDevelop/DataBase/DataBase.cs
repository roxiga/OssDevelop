using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing.Imaging;

namespace OssDevelop
{
    internal class DataBase
    {
        DatabaseConnection? dbConnection;
        public string CreateDiary(Diary diary)
        {
            dbConnection = new DatabaseConnection();
            string sql;
            if (GetCount(dbConnection, diary.date) > 0)
            {
                sql = "UPDATE DiaryTable SET title = @Title, text = @Text WHERE date = @Date";
            }
            else
            {
                sql = "INSERT INTO DiaryTable(date, title, text) VALUES(@Date, @Title, @Text)";
            }
            SqlParameter prm = new SqlParameter("@Title", SqlDbType.NVarChar);
            prm.Value = diary.title;
            dbConnection.AddParameter(prm);
            prm = new SqlParameter("@Text", SqlDbType.NVarChar);
            prm.Value = diary.text;
            dbConnection.AddParameter(prm);
            dbConnection.CreateQuery(sql);
            if ((dbConnection.DoNoQuery()) < 1)
            {
                dbConnection.Dispose();
                dbConnection = null;
                return "失敗";
            }
            dbConnection.Dispose();
            dbConnection = null;
            return sql;
        }

        public string AddImage(int date,Image image)
        {
            dbConnection = new DatabaseConnection();
            string sql;
            if (GetCount(dbConnection, date) > 0)
            {
                sql = "UPDATE DiaryTable SET image = @BLOBData WHERE date = @Date";
            }
            else
            {
                sql = "INSERT INTO DiaryTable(date, image) VALUES(@Date, @BLOBData)";
            }
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            Byte[] bytBLOBData = new Byte[ms.Length];
            ms.Position = 0;
            ms.Read(bytBLOBData, 0, Convert.ToInt32(ms.Length));
            SqlParameter prm = new SqlParameter("@BLOBData", SqlDbType.VarBinary, bytBLOBData.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, bytBLOBData);
            dbConnection.AddParameter(prm);
            dbConnection.CreateQuery(sql);
            if ((dbConnection.DoNoQuery()) < 1)
            {
                dbConnection.Dispose();
                dbConnection = null;
                return "失敗";
            }
            dbConnection.Dispose();
            dbConnection = null;
            return sql;
        }

        public int GetCount(DatabaseConnection dbConnection,int date)
        {
            dbConnection.CreateQuery("SELECT COUNT(*) FROM DiaryTable WHERE date = @Date");
            SqlParameter prm = new SqlParameter("@Date", SqlDbType.Int);
            prm.Value = date;
            dbConnection.AddParameter(prm);
            return dbConnection.ExecuteScalar();
        }

        public Diary GetDiary(int date)
        {
            Diary diary = new Diary(date);
            dbConnection = new DatabaseConnection();
            dbConnection.CreateQuery("SELECT title,text FROM DiaryTable WHERE date = @Date");
            SqlParameter prm = new SqlParameter("@Date", SqlDbType.Int);
            prm.Value = date;
            dbConnection.AddParameter(prm);
            SqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                string? title = reader["title"].ToString();
                if (title != null)
                    diary.title = title;
                string? text = reader["text"].ToString();
                if (text != null)
                    diary.text = text;
            }
            dbConnection.Dispose();
            dbConnection = null;
            return diary;
        }

        public Diary GetDiaryAndImage(int date)
        {
            Diary diary = new Diary(date);
            dbConnection = new DatabaseConnection();
            dbConnection.CreateQuery("SELECT title,text,image FROM DiaryTable WHERE date = @Date");
            SqlParameter prm = new SqlParameter("@Date", SqlDbType.Int);
            prm.Value = date;
            dbConnection.AddParameter(prm);
            SqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                string? title = reader["title"].ToString();
                if (title != null)
                    diary.title = title;
                string? text = reader["text"].ToString();
                if (text != null)
                    diary.text = text;
                if (reader["image"] != DBNull.Value)
                {
                    var image = (Byte[])(reader["image"]);
                    MemoryStream stmBLOBData = new MemoryStream(image);
                    diary.image = Image.FromStream(stmBLOBData);
                }
            }
            dbConnection.Dispose();
            dbConnection = null;
            return diary;
        }

        public Diary findText(string keyword, bool matchCase, bool wholeWord, bool regularX, int pre)
        {
            Diary diary = new Diary(0);
            dbConnection = new DatabaseConnection();
            dbConnection.CreateQuery("SELECT date FROM DiaryTable WHERE (title LIKE @Keyword OR text LIKE @Keyword) AND date > @Pre");
            SqlParameter prm = new SqlParameter("@Pre", SqlDbType.Int);
            prm.Value = pre;
            dbConnection.AddParameter(prm);
            prm = new SqlParameter("@Keyword", SqlDbType.NVarChar);
            prm.Value = "%"+keyword+"%";
            dbConnection.AddParameter(prm);
            SqlDataReader reader = dbConnection.DoQuery();
            while (reader.Read())
            {
                diary.date = (int)reader["date"];
            }
            dbConnection.Dispose();
            dbConnection = null;
            return diary;
        }
    }
}
