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
    class Users
    {
        public static BindingList<Users> users;
        public static Int64 Available_ID;
        private string name;
        private string username;
        private string password;
        private string address;
        private string mobile;
        private string qr;




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
        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }

        }
        public string Address
        {
            get
            {
                return address;

            }
            set
            {
                address = value;

            }

        }
        public string Mobile
        {
            get
            {
                return mobile;
            }
            set
            {
                mobile = value;
            }
        }

        public string QR
        {
            get
            {
                return qr;

            }
            set
            {
                qr = value;
            }
        }
        public static Int64 SelectID(string UserName)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "UserName" };
            string[] ConditionValue = { UserName };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Users", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception)
            {
                throw new Exception("فشل البحث عن هذه المستخدم");
            }
            return -1;
        }
        public static void Insert(string UserName,String Password,string name,string mobile,string Address,string qr)
        {
            Int64 ID = Users.SelectID(UserName);

            if (ID != -1)
            {
                throw new Exception("يوجد مستخدم بهذا الإسم");
            }
            if (char.IsNumber(UserName[0]) || char.IsSymbol(UserName[0]))
            {
                throw new Exception("يرجي كتابة الاسم بشكل صحيح");
            }

            Available_ID = DBConnection.getIDCounter("Users");
            string[] names = { "ID", "UserName", "Password", "Name", "Mobile","Address", "QR" };
            string[] values = { Users.Available_ID.ToString(),UserName,Password,name,mobile,Address,qr };
            try
            {
                DBConnection.insertRow("Users", names, values);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            DBConnection.setIDCounter("Users", Users.Available_ID + 1);
        }
        public static void Delete(string UserName)
        {
            string[] conditionName = { "UserName" };
            string[] conditionValue = { UserName};
            try
            {
                DBConnection.deleteRows("Users", conditionName, conditionValue);
            }
            catch
            {
                throw new Exception("لا يوجد مستخدم بهذا الاسم");
            }
        }
        public static void Update( string name,string password,string address,string mobile)
        {
        
            string[] columnsName = {"Name","Password","Address","Mobile",};
            string[] values = {name,password,address,mobile};
            string[] ConditionName = {"ID"};
            string[] ConditionValue = { name,password,address,mobile};
            try
            {
                DBConnection.updateRows("Users", columnsName, values, ConditionName, ConditionValue);
            }
            catch(Exception)
            {
                throw new Exception("فشل تعديل هذا المستخدم");
            }
        }

    }
}

