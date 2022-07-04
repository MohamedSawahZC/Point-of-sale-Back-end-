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
    class Invoice
    {
        public static BindingList<Invoice> invoice;
        public static Int64 Available_ID;
        public Int64 User_ID;
        public Int64 Shift_ID;
        private float price;
        private float discount;
        private float total;
        private float taxs;
        private DateTime Datetime;

        public float Discount
        {
            get
            {
                return discount;
            }
            set
            {
                discount = value;
            }
        }
        public float Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }
        public float Taxs
        {
            get
            {
                return total;
            }
            set
            {
                taxs = value;
            }
        }
        public float Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        public DateTime GetDatetime()
        {
            return Datetime;
        }
        public void SetDatetime(DateTime value)
        {
            Datetime = value;
        }
        public Invoice(  Int64 User_ID, Int64 Shift_ID,float price, float discount,  float total,float taxs)
        {
            this.User_ID = User_ID;
            this.Shift_ID = Shift_ID;
            this.price = price;
            this.discount = discount;
            this.total = total;
            this.taxs = taxs;
        }
        public Invoice(Int64 User_ID, Int64 Shift_ID, float price, float discount, float total)
        {
            this.User_ID = User_ID;
            this.Shift_ID = Shift_ID;
            this.price = price;
            this.discount = discount;
            this.total = total;
            this.taxs = taxs;

        }
        public static void init()
        {
            invoice = new BindingList<Invoice>();

        }
        public static Int64 SelectID(DateTime Datetime)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Datetime" };
            string[] ConditionValue = { Datetime.ToString("dd,MMM,yyyy:hh:mm:ss") };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Invoice", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception )
            {
                throw new Exception("فشل البحث عن هذه الفاتورة");
            }
            return -1;
        }
        public static void Insert(Int64 User_ID,Int64 Shift_ID,DateTime Datetime,float price,float discount,float taxs,float total)
        {
            Int64 ID = Invoice.SelectID(Datetime);
            if (ID != -1)
            {
                throw new Exception("يوجد قسم بهذا الإسم");
            }
            Available_ID = DBConnection.getIDCounter("Invoice");
            string[] names = { "ID", "User_ID","Shift_ID","Datetime","Price","discount","taxs","total" };
            string[] values = { Invoice.Available_ID.ToString(),User_ID.ToString(),Shift_ID.ToString(),Datetime.ToString("dd,MMM,yyyy:hh:mm:ss"),price.ToString(),discount.ToString(),taxs.ToString(),total.ToString()  };
            try
            {
                DBConnection.insertRow("Invoice", names, values);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            DBConnection.setIDCounter("Invoice", Category.Available_ID + 1);
        }
        public static void Delete(DateTime Datetime)
        {
            string[] conditionName = { "Datetime" };
            string[] conditionValue = { Datetime.ToString("dd,MMM,yyyy:hh:mm:ss") };
            try
            {
                DBConnection.deleteRows("Invoice", conditionName, conditionValue);
            }
            catch
            {
                throw new Exception("لا يوجد فاتورة بهذا الاسم");
            }
        }
        


    }






}

