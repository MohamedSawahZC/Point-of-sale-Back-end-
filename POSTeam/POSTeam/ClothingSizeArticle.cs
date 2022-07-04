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
    class ClothingSizeArticle
    {

        public static Int64 Available_ID;
        public static BindingList<ClothingSizeArticle> sizeArticles;


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
            public ClothingSizeArticle(string name)
            {
                this.name = name;
            }

            public ClothingSizeArticle(string name, bool active)
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
                    OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeArticle", columnsName, ConditionName, ConditionValue);
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
        public static bool Search(string name, Int64 Category_ID)
        {
            string[] columnsName = { "Name" };
            string[] ConditionName = { "Name", "ClothingSizeCategory_ID" };
            string[] ConditionValue = { name, Category_ID.ToString() };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeArticle", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return false;
        }
        public static void Insert(string name, bool active,string category_Name)
            {
                Int64 Category_ID = ClothingSizeCategory.SelectID(category_Name);
                bool found = ClothingSizeArticle.Search(name, Category_ID);
                if (found)
                {
                    throw new Exception("يوجد أرتكل بهذا الإسم");
                }
           
            string act;

                if (active) act = "Y";
                else act = "N";
                string[] names = { "ID", "ClothingSizeCategory_ID", "Name", "Active" };
                string[] values = { ClothingSizeArticle.Available_ID.ToString(), Category_ID.ToString(), name, act };
                try
                {
                    DBConnection.insertRow("ClothingSizeArticle", names, values);
                }
                catch (Exception e)
                {
                    throw e;
                }
            sizeArticles.Add(new ClothingSizeArticle(name));
                ClothingSizeArticle.Available_ID++;
                DBConnection.setIDCounter("ClothingSizeArticle", ClothingSizeArticle.Available_ID);
            }
            public static void Delete(string name)
            {
                string[] conditionName = { "Name" };
                string[] conditionValue = { name };
                try
                {
                    DBConnection.deleteRows("ClothingSizeArticle", conditionName, conditionValue);
                }
                catch (Exception )
                {
                    throw new Exception("لا يوجد أرتكل بهذا الاسم");
                }
            }
            public static void Update(string oldName, string name, bool active,string Category_Name,int index)
            {

            Int64 ID1 = ClothingSizeArticle.SelectID(oldName);
            Int64 ID2 = ClothingSizeArticle.SelectID(name);

            if (ID1 != ID2 && ID2 != -1)
            {
                throw new Exception("يوجد أرتكل آخر بهذا الإسم");
            }

            if (ID1 == -1)
            {
                throw new Exception("لا يوجد أرتكل بهذا الاسم");
            }

            ID2 = ClothingSizeCategory.SelectID(name);
            if (ID2 != -1)
            {
                throw new Exception("هذا تصنيف  وليس أرتكل");
            }

            ID2 = ClothingSizeCategory.SelectID(Category_Name);
            if (ID2 == -1)
            {
                throw new Exception("لا يوجد تصنيف بهذا الإسم");
            }
            string[] columnsName = { "Name", "Active", "ClothingSizeCategory_ID" };
            string act;
            if (active) act = "Y";
            else act = "N";
            string[] values = { name, act, ID2.ToString() };
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID1.ToString() };
            DBConnection.updateRows("ClothingSizeArticle", columnsName, values, ConditionName, ConditionValue);
            sizeArticles[index] = new ClothingSizeArticle(name);
        }

            public static void Select_All()
            {
                sizeArticles = new BindingList<ClothingSizeArticle>();
                string[] columnsName = { "Name" };
                string[] ConditionName = { };
                string[] ConditionValue = { };
                OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeArticle", columnsName, ConditionName, ConditionValue);
                try
                {
                    while (oracleDataReader.Read())
                    {
                        string name = oracleDataReader.GetValue(0).ToString();
                        sizeArticles.Add(new ClothingSizeArticle(name));

                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                Available_ID = DBConnection.getIDCounter("ClothingSizeArticle");
            }
        public static void Select_All(string Category_Name)
        {
            sizeArticles = new BindingList<ClothingSizeArticle>();
            Int64 Category_ID = ClothingSizeCategory.SelectID(Category_Name);
            string[] columnsName = { "Name" };
            string[] ConditionName = { "ClothingSizeCategory_ID" };
            string[] ConditionValue = { Category_ID.ToString() };
            OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeArticle", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    sizeArticles.Add(new ClothingSizeArticle(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            Available_ID = DBConnection.getIDCounter("ClothingSizeArticle");
        }

        new public static List<string> ToString()
            {

                List<string> names = new List<string>();
                for (int i = 0; i < sizeArticles.Count; i++)
                {
                    names.Add(sizeArticles[i].name);
                }
                return names;
            }
        }
    }



