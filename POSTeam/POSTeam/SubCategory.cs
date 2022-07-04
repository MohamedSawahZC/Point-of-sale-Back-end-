using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSTeam
{
    class SubCategory
    {
        public static Int64 Available_ID;
        public static BindingList<SubCategory> subCategories;

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

        public SubCategory(string name)
        {
            this.name = name;
        }
        
        public static void Insert(string name, bool hidden, String Category_Name)
        {
            Int64 ID = SubCategory.SelectID(name);
            if (ID != -1)
            {
                throw new Exception("يوجد قسم بهذا الإسم");
            }

            ID = Category.SelectID(name);
            if (ID != -1)
            {
                throw new Exception("هذا القسم رئيسي وليس فرعي");
            }
            
            Int64 ID2 = Category.SelectID(Category_Name);
            string hid;
            if (hidden) hid = "Y";
            else hid = "N";
            string[] names = { "ID", "Name", "Hidden" , "Category_ID"};
            string[] values = { SubCategory.Available_ID.ToString(), name, hid,ID2.ToString() };
            try
            {
                DBConnection.insertRow("SubCategory", names, values);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            SubCategory.Available_ID++;
            DBConnection.setIDCounter("SubCategory", SubCategory.Available_ID);
        }

        public static Int64 SelectID(string name)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("SubCategory", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return -1;
        }
        public static void Delete(string name)
        {
            string[] conditionName = { "Name" };
            string[] conditionValue = { name };
            try
            {
                DBConnection.deleteRows("SubCategory", conditionName, conditionValue);
            }
            catch (Exception )
            {
                throw new Exception("فشل حذف القسم");
            }
        }

        public static void Update(string oldName, string name, bool hidden, string categoryName)
        {
            Int64 ID1 = SubCategory.SelectID(oldName);
            Int64 ID2 = SubCategory.SelectID(name);

            if (ID1 != ID2 && ID2 != -1)
            {
                throw new Exception("يوجد قسم آخر بهذا الإسم");
            }

            if(ID1==-1)
            {
                throw new Exception("لا يوجد قسم بهذا الاسم");
            }

            ID2 = Category.SelectID(name);
            if (ID2 != -1)
            {
                throw new Exception("هذا القسم رئيسي وليس فرعي");
            }

            ID2 = Category.SelectID(categoryName);
            if(ID2 == -1)
            {
                throw new Exception("لا يوجد قسم بهذا الإسم");
            }

            string[] columnsName = { "Name", "Hidden","Category_ID"};
            string hid;
            if (hidden) hid = "Y";
            else hid = "N";
            string[] values = { name, hid, ID2.ToString()};
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID1.ToString()};
            DBConnection.updateRows("SubCategory", columnsName, values, ConditionName, ConditionValue);

        }

        public static bool Select(string name)
        {
            string[] columnsName = { "Hidden" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("SubCategory", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    char c = char.Parse(oracleDataReader.GetOracleValue(0).ToString());
                    if (c == 'Y') return true;
                    return false;
                }
            }
            catch (Exception )
            {
                throw new Exception("لا يوجد قسم رئيسي بهذا الاسم");
            }
            return false;
        }

        public static void Select_All()
        {
            subCategories = new BindingList<SubCategory>();
            string[] columnsName = { "Name"};
            string[] ConditionName = {};
            string[] ConditionValues = {};
            OracleDataReader oracleDataReader = DBConnection.selectRows("SubCategory", columnsName, ConditionName, ConditionValues);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    subCategories.Add(new SubCategory(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            Available_ID = DBConnection.getIDCounter("SubCategory");
        }

        public static void Select_All(string Category_Name)
        {
            subCategories = new BindingList<SubCategory>();
            Int64 Category_ID = Category.SelectID(Category_Name);

            if(Category_ID==-1)
            {
                throw new Exception("لا يوجد قسم رئيسي بهذا الاسم");
            }
            string[] columnsName = { "Name" };
            string[] ConditionName = { "CATEGORY_ID" };
            string[] ConditionValue = { Category_ID.ToString()};
            OracleDataReader oracleDataReader = DBConnection.selectRows("SubCategory", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    subCategories.Add(new SubCategory(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            Available_ID = DBConnection.getIDCounter("SubCategory");
        }

        public static void Show_All(string Category_Name)
        {
            subCategories = new BindingList<SubCategory>();
            Int64 Category_ID = Category.SelectID(Category_Name);

            if (Category_ID == -1)
            {
                throw new Exception("لا يوجد قسم رئيسي بهذا الاسم");
            }

            string[] columnsName = {"Name" };
            string[] ConditionName = {"CATEGORY_ID" ,"Hidden"};
            string[] ConditionValue = {Category_ID.ToString(),"N"};
            OracleDataReader oracleDataReader = DBConnection.selectRows("SubCategory", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    subCategories.Add(new SubCategory(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        new public static List<string> ToString()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < subCategories.Count; i++)
            {
                names.Add(subCategories[i].name);
            }
            return names;
        }

    }
}
