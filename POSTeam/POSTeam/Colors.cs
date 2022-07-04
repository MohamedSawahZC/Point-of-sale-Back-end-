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
         public class Colors
    {
        public static Int64 Available_ID;
        public static BindingList<Colors> colors;

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
        public Colors(string name)
        {
            this.name = name;
        }
        public static Int64 SelectID(string name)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Colors", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return -1;
        }


        public static void Insert(string name)
        {
            Int64 ID = Colors.SelectID(name);
            if (ID != -1)
            {
                throw new Exception(" يوجد لون بهذا الاسم");
            }
           
            string[] names = { "ID", "Name" };
            string[] values = { Colors.Available_ID.ToString(), name,};
            try
            {
                DBConnection.insertRow("Colors", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
            Colors.Available_ID++;
            DBConnection.setIDCounter("Colors", Colors.Available_ID++);
        }
        public static void Delete(string name)
        {
            string[] conditionName = { "Name" };
            string[] conditionValue = { name };
            MessageBox.Show(name);
            if (name == "بدون")
            {
                throw new Exception("لا يمكن حذف هذا اللون");
            }
            try
            {
                DBConnection.deleteRows("Colors", conditionName, conditionValue);
            }
            catch (Exception )
            {
                throw new Exception("لا يوجد لون بهذا الاسم");
            }
        }

        public static void Update(string oldName, string name)
        {
            Int64 ID1 = Colors.SelectID(oldName);
            Int64 ID2 = Colors.SelectID(name);

            if (ID1 != ID2 && ID2 != -1)
            {
                throw new Exception("يوجد لون آخر بهذا الإسم");
            }

            if (ID1 == -1)
            {
                throw new Exception("لا يوجد لون بهذا الاسم");
            }

            string[] columnsName = { "Name" };
            string[] values = { name};
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID1.ToString() };
            DBConnection.updateRows("Colors", columnsName, values, ConditionName, ConditionValue);
        }

        public static void Select_All()
        {
            colors =new BindingList<Colors>();
        
            string[] columnsName = { "Name"};
            string[] ConditionName = { };
            string[] ConditionValue = { };
            OracleDataReader oracleDataReader = DBConnection.selectRows("Colors", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetOracleValue(0).ToString();
                    colors.Add(new Colors(name));
                    
                }
            }
            catch (Exception )
            {
                throw new Exception("لا يوجد عناصر");
            }
            Available_ID = DBConnection.getIDCounter("Colors");
        }
    }
}
