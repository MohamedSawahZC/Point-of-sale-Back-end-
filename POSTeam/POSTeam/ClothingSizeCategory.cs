using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.ComponentModel;

namespace POSTeam
{
    public class ClothingSizeCategory
    {
        public static Int64 Available_ID;
        public static BindingList<ClothingSizeCategory> sizeCategories;


        public bool Active;
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
        public ClothingSizeCategory(string name)
        {
            this.name = name;
        }

        public ClothingSizeCategory(string name, bool active)
        {
            this.name = name;
            this.Active = active;
        }

        public static Int64 SelectID(string name)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeCategory", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return -1;
        }
        public static void Insert(string name, bool active)
        {
            Int64 ID = ClothingSizeCategory.SelectID(name);
            if (ID != -1)
            {
                throw new Exception("يوجد تصنيف بهذا الإسم");
            }
            string act;
            if (active) act = "Y";
            else act = "N";
            string[] names = { "ID", "Name", "Active" };
            string[] values = { ClothingSizeCategory.Available_ID.ToString(), name, act };
            try
            {
                DBConnection.insertRow("ClothingSizeCategory", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
            sizeCategories.Add(new ClothingSizeCategory(name));
            ClothingSizeCategory.Available_ID++;
            DBConnection.setIDCounter("ClothingSizeCategory", ClothingSizeCategory.Available_ID);
        }
        public static void Delete(string name)
        {
            string[] conditionName = { "Name" };
            string[] conditionValue = { name };
            try
            {
                DBConnection.deleteRows("ClothingSizeCategory", conditionName, conditionValue);
            }
            catch (Exception )
            {
                throw new Exception("لا يوجد تصنيف بهذا الاسم");
            }
        }
        public static void Update(string oldName, string name, bool active,int index)
        {
            Int64 ID1 = ClothingSizeCategory.SelectID(oldName);
            Int64 ID2 = ClothingSizeCategory.SelectID(name);

            if (ID1 != ID2 && ID2 != -1)
            {
                throw new Exception("يوجد تصنيف آخر بهذا الإسم");
            }

            if (ID1 == -1)
            {
                throw new Exception("لا يوجد تصنيف بهذا الاسم");
            }

            string[] columnsName = { "Name", "Active" };
            string act;
            if (active) act = "Y";
            else act = "N";
            string[] values = { name, act };
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID1.ToString() };
            DBConnection.updateRows("ClothingSizeCategory", columnsName, values, ConditionName, ConditionValue);
            sizeCategories[index] = new ClothingSizeCategory(name);
        }

        public static bool Select(string name)
        {
            string[] columnsName = { "Active" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeCategory", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    char c = char.Parse(oracleDataReader.GetOracleValue(0).ToString());
                    if (c == 'Y') return true;
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }
        public static void Select_All()
        {
            sizeCategories = new BindingList<ClothingSizeCategory>();
            string[] columnsName = { "Name" };
            string[] ConditionName = { };
            string[] ConditionValue = { };
            OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeCategory", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    sizeCategories.Add(new ClothingSizeCategory(name));
                   
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            Available_ID = DBConnection.getIDCounter("ClothingSizeCategory");
        }
        new public static List<string> ToString()
        {

            List<string> names = new List<string>();
            for (int i = 0; i < sizeCategories.Count; i++)
            {
                names.Add(sizeCategories[i].name);
            }
            return names;
        }
    }
}
