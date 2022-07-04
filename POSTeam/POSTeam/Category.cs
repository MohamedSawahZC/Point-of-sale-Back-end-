using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSTeam
{
    public class Category
    {

        public static Int64 Available_ID;
        static BindingList<Category> categories;
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }

        }

        public Category(string name)
        {
            this.name = name;
        }
        public static void Insert(string name,bool hidden)
        {
            Int64 ID = Category.SelectID(name);
            if(ID!=-1)
            {
                throw new Exception("يوجد قسم بهذا الإسم");
            }
            Available_ID = DBConnection.getIDCounter("Category");
            string hid;
            if (hidden) hid = "Y";
            else hid = "N";
            string[] names = { "ID","Name","Hidden"};
            string[] values = {Category.Available_ID.ToString(),name,hid };
            try
            {
                DBConnection.insertRow("Category", names, values);
            }
            catch(Exception)
            {
                throw new Exception("فشل إضافة القسم");
            }
            DBConnection.setIDCounter("Category", Category.Available_ID+1);
        }

        
        public static Int64 SelectID(string name)
        {
            string[] columnsName = {"ID"};
            string[] ConditionName = {"Name"};
            string[] ConditionValue = {name};
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Category", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch(Exception)
            {
                throw new Exception("فشل البحث عن هذا القسم");
            }
            return -1;
        }

        public static void Delete(string name)
        {
            string[] conditionName = { "Name" };
            string[] conditionValue = { name };
            try
            {
                DBConnection.deleteRows("Category", conditionName, conditionValue);
            }
            catch
            {
                throw new Exception("لا يوجد قسم بهذا الاسم");
            }
        }

        public static void Update(string oldName, string name,bool hidden)
        {
            Int64 ID1 = Category.SelectID(oldName);
            Int64 ID2 = Category.SelectID(name);

            if (ID1!=ID2 && ID2!=-1)
            {
                throw new Exception("يوجد قسم آخر بهذا الإسم");
            }

            if (ID1 == -1)
            {
                throw new Exception("لا يوجد قسم بهذا الاسم");
            }

            string[] columnsName = {"Name","Hidden"};
            string hid;
            if (hidden) hid = "Y";
            else hid = "N";
            string[] values = {name,hid};
            string[] ConditionName = {"ID"};
            string[] ConditionValue = { ID1.ToString()};
            try
            {
                DBConnection.updateRows("Category", columnsName, values, ConditionName, ConditionValue);
            }
            catch(Exception )
            {
                throw new Exception("فشل تعديل هذا القسم");
            }
        }

        public static bool GetHidden(string name)
        {
            string[] columnsName = { "Hidden" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Category", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    char c = char.Parse(oracleDataReader.GetOracleValue(0).ToString());
                    if (c == 'Y') return true;
                    return false;
                }
            }
            catch (Exception )
            {
                throw new Exception("فشل البحث عن هذا القسم");
            }
            return false;
        }
        public static void Select_All()
        {
            categories = new BindingList<Category>();
            string[] columnsName = { "Name"};
            string[] ConditionName = {};
            string[] ConditionValue = {};
            OracleDataReader oracleDataReader = DBConnection.selectRows("Category", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    categories.Add(new Category(name));
                }
            }
            catch(Exception )
            {
                throw new Exception("فشل تحميل الأقسام");
            }
        }
        public static void Show_ALL()
        {
            categories = new BindingList<Category>();
            string[] columnsName = {"Name"};
            string[] ConditionName = {"Hidden"};
            string[] ConditionValue = {"N"};
            OracleDataReader oracleDataReader = DBConnection.selectRows("Category", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    categories.Add(new Category(name));
                }
            }
            catch (Exception )
            {
                throw new Exception("فشل تحميل الأقسام");
            }
        }

        new public static List<string>ToString()
        {
            List<string> names = new List<string>();
            for(int i=0;i<categories.Count;i++)
            {
                names.Add(categories[i].name);
            }
            return names;
        }

    }
}
