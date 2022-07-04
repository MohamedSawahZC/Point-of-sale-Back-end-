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
    class ClothingSizeName
    {

      public static Int64 Available_ID;
        public static BindingList<ClothingSizeName> sizeNames;


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
        public ClothingSizeName(string name)
        {
            this.name = name;
        }

        public ClothingSizeName(string name, bool active)
        {
            this.name = name;
            this.Active = active;
        }

        public static bool Search(string name, Int64 Article_ID)
        {
            string[] columnsName = { "Name" };
            string[] ConditionName = { "Name" ,"ClothingSizeArticle_ID"};
            string[] ConditionValue = { name , Article_ID .ToString()};
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeName", columnsName, ConditionName, ConditionValue);
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
        public static void Insert(string name, bool active, string SizeArticle_Name)
        {
            Int64 Article_ID = ClothingSizeArticle.SelectID(SizeArticle_Name);
            bool found = ClothingSizeName.Search(name, Article_ID);
            if (found)
            {
                throw new Exception("يوجد مقاس بهذا الإسم");
            }
      
            string act;

            if (active) act = "Y";
            else act = "N";
            string[] names = { "CLOTHINGSIZEARTICLE_ID", "Name", "Active" };
            string[] values = { Article_ID.ToString(), name, act };
            try
            {
                DBConnection.insertRow("ClothingSizeName", names, values);
            }
            catch (Exception e)
            {
                MessageBox.Show(Article_ID.ToString());
                throw e;
            }
            sizeNames.Add(new ClothingSizeName(name));
            //ClothingSizeName.Available_ID++;
            //DBConnection.setIDCounter("ClothingSizeName", ClothingSizeName.Available_ID);
        }
   
        public static void Update(string oldName, string name, bool active, string SizeArticle_Name,int index)
        {
            Int64 Article_ID = ClothingSizeArticle.SelectID(SizeArticle_Name);
            bool found1 = ClothingSizeName.Search(oldName, Article_ID);
            bool found2 = ClothingSizeName.Search(name, Article_ID);
    
            if (found2 && oldName != name)
            {
                throw new Exception("يوجد مقاس آخر بهذا الإسم");
            }
            if (found1 == false)
            {
                throw new Exception("لا يوجد مقاس بهذا الاسم");
            }

            string[] columnsName = { "ClothingSizeArticle_ID", "Name", "Active" };
            string act;
            if (active) act = "Y";
            else act = "N";
            string[] values = { Article_ID.ToString(),name, act  };
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { Article_ID.ToString() };
            DBConnection.updateRows("ClothingSizeName", columnsName, values, ConditionName, ConditionValue);
            sizeNames[index] = new ClothingSizeName(name);
        }

        public static void Select_All()
        {
            sizeNames = new BindingList<ClothingSizeName>();
            string[] columnsName = { "Name" };
            string[] ConditionName = { };
            string[] ConditionValue = { };
            OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeName", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    sizeNames.Add(new ClothingSizeName(name));

                }
            }
            catch (Exception e)
            {
                throw e;
            }
           // Available_ID = DBConnection.getIDCounter("ClothingSizeName");
        }
        public static void Select_All(string Article_Name)
        {
            sizeNames = new BindingList<ClothingSizeName>();
            Int64 Article_ID = ClothingSizeArticle.SelectID(Article_Name);
            string[] columnsName = { "Name" };
            string[] ConditionName = { "ClothingSizeArticle_ID" };
            string[] ConditionValue = { Article_ID.ToString() };
            OracleDataReader oracleDataReader = DBConnection.selectRows("ClothingSizeName", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    sizeNames.Add(new ClothingSizeName(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //Available_ID = DBConnection.getIDCounter("ClothingSizeName");
        }

        new public static List<string> ToString()
        {

            List<string> names = new List<string>();
            for (int i = 0; i < sizeNames.Count; i++)
            {
                names.Add(sizeNames[i].name);
            }
            return names;
        }
    }
}












