using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DancePadServer
{
    class DT
    {     
        private DataTable tblDancers;     
        private static string fileName = @"..\..\DB\DancerTable.ndb";

        /// <summary>
        /// פעולה בונה של המחלקה,יוצרת קובץ במידה ואינו קיים עוד.
        /// אם הקובץ קיים היא מציגה את תוכן השחקנים 
        /// </summary>
        public DT()
        {
            if (!File.Exists(@"..\..\DB\DancerTable.ndb"))
            {                                                                                                                                    
                tblDancers = CreateDB("DancerTable.ndb");
                tblDancers.WriteXml(@"..\..\DB\DancerTable.ndb", XmlWriteMode.WriteSchema);
            }
            else
            {
                try
                {
                    tblDancers = new DataTable();
                    tblDancers.ReadXml(@"..\..\DB\DancerTable.ndb");
                    foreach (DataRow row in tblDancers.Rows)
                    {
                        Console.WriteLine("User name : {0} , Password : {1}", row["Username"], row["Password"]);
                        Console.WriteLine("From DT User name length " + row["Username"].ToString().Length + "Password length " + row["Password"].ToString().Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// פעולה המוסיפה פרטים של שחקן חדש לטבלה ושומרת מחדש את הטבלה
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AddDancer(string userName, string password)
        {
            try
            {
                DataRow dr = tblDancers.NewRow();                
                dr["Username"] = userName;
                dr["Password"] = password;
                dr["Record"] = "0";
                tblDancers.Rows.Add(dr);
                SaveDT(tblDancers, fileName);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// פעולה היוצרת טבלה
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private DataTable CreateDB(string tableName)
        {
            DataTable table = new DataTable(tableName);
            DataColumn userName = new DataColumn();
            userName.ColumnName = "Username";
            userName.DataType = typeof(System.String);
            userName.Unique = true;
            table.Columns.Add(userName);
            table.PrimaryKey = new DataColumn[] { table.Columns["Username"] };
        

            DataColumn password = new DataColumn();
            password.ColumnName = "Password";
            password.DataType = typeof(System.String);          
            password.Unique = false;
            table.Columns.Add(password);

            DataColumn record = new DataColumn();
            record.ColumnName = "Record";
            record.DataType = typeof(System.String);          
            record.Unique = false;
            table.Columns.Add(record);            
            return table;
        }

        /// <summary>
        /// פעולה השומרת את השינויים שנערכו בטבלה
        /// </summary>
        /// <param name="tblDancers"></param>
        /// <param name="destinationFilePath"></param>
        public void SaveDT(DataTable tblDancers, string destinationFilePath)
        {
            tblDancers.WriteXml(destinationFilePath, XmlWriteMode.WriteSchema);           
        }       
            /// <summary>
            /// פעולה שבודקת האם פרטים של שחקן מסויים קיימים במערכת
            /// </summary>
            /// <param name="userName"></param>
            /// <param name="password"></param>
            /// <returns></returns>
        public bool IsExist(string userName, string password)
        {
            DataTable dt = new DataTable();            
            dt.ReadXml(@"..\..\DB\DancerTable.ndb");             
            if (dt.Rows.Count != 0)  ///אם יש שורות-בדיקה כדי שלא יהיו שגיאות בזימון פעולה שמחפשת והיא לא תוכל לחפש מקום שלא קיים
            {
                DataRow dr = dt.Rows.Find(userName);               

                if (dr != null)
                {
                    if (dr["password"].ToString() == password)
                    {
                        return true;
                    }
                }
            }     
            return false;
        }

        /// <summary>
        /// פעולה המשנה את ערך השיא של השחקן בטבלה
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="rec"></param>
        public void SetRecord(string userName,int rec)
        {
            DataTable dt = new DataTable();
            dt.ReadXml(@"..\..\DB\DancerTable.ndb");
            if (dt.Rows.Count != 0)
            {
                DataRow dr = dt.Rows.Find(userName);

                if (dr != null)
                {
                    if (  (int.Parse(dr["Record"].ToString()) <rec))
                    {
                        dr["Record"]= rec.ToString();
                        SaveDT(dt, fileName);
                    }
                }
           }          
                               
        }

    }
}
