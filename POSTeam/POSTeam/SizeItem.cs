using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTeam
{
    class SizeItem
    {

        public static BindingList<SizeItem> sizeItems;
        
        private string name;
        private float percentage;

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

        public float Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                percentage = value;
            }
        }

        public SizeItem()
        {
            name = "";
            percentage = 1;
        }
        public SizeItem(string name, float percentage)
        {
            this.name = name;
            this.percentage = percentage;
        }


        public static void Select_All()
        {
            sizeItems = new BindingList<SizeItem>();
            sizeItems.AllowNew = true;
            sizeItems.AllowEdit = true;
            sizeItems.AllowRemove = true;
            string[] columnsName = { "Name", "Percentage" };
            string[] ConditionName = {};
            string[] ConditionValue = {};
            OracleDataReader oracleDataReader = DBConnection.selectRows("SizeItems", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    float percentage = float.Parse(oracleDataReader.GetValue(1).ToString());
                    sizeItems.Add(new SizeItem(name,percentage));
                }
            }
           catch(Exception)
            {
                throw new Exception("فشل التحديد");
            }
        }

        public static void addRow(string name, float percentage)
        {
            sizeItems.Add(new SizeItem(name, percentage));
        }
        public static void delete(int rowIndex)
        {
            sizeItems.RemoveAt(rowIndex);
        }
        public static void Insert_All(string productName)
        {
            try
            {
                Delete_All(productName);
                for (int i = 0; i < sizeItems.Count; i++)
                {
                    SizeItem sizeItem = sizeItems[i];
                    string[] names = {"Name", "Percentage" };
                    string[] values = { sizeItem.name, sizeItem.percentage.ToString() };
                    try
                    {
                        DBConnection.insertRow("SizeItems", names, values);
                    }
                    catch(Exception)
            {
                        throw new Exception("فشل الاضافة");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void Delete_All(string productName)
        {
            string[] conditionName = {""};
            string[] conditionValue = {""};
            try
            {
                DBConnection.deleteRows("SizeItems", conditionName, conditionValue);
            }
            catch
            {
                throw new Exception("فشل تعديل الأحجام");
            }
        }
    }
}
