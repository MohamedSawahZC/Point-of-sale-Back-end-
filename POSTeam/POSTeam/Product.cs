using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSTeam
{
    class Product
    {
        public static Int64 Available_ID;
        public static List<Product> products;

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

        private string barcode;
        public string Barcode
        {
            get
            {
                return barcode;

            }

            set
            {
                barcode = value;
            }
        }
        private float in_Price; 
        public float InPrice
        {
            get
            {
                return in_Price;
            }

            set
            {
                in_Price = value;
            }
        }
        private float out_Price;


        public float OutPrice
        {
            get
            {
                return out_Price;
            }

            set
            {
                out_Price = value;
            }
        }


        public Product(string name)
        {
            this.name = name;
        }

        public Product(string name,float in_Price,float out_Price,string barcode)
        {
            this.name = name;
            this.in_Price = in_Price;
            this.out_Price = out_Price;
            this.barcode = barcode;
        }

        public static bool checkDeleted(Int64 ID)
        {
            string[] columnsName = { "Deleted" };
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID.ToString() };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    char deleted =  char.Parse(oracleDataReader.GetOracleValue(0).ToString());
                    if (deleted == 'Y') return true;
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("فشل البحث عن هذا القسم");
            }
            return false;

        }
        public static void Recover(Int64 ID)
        {
            string[] columnsName = {"Deleted"};
            string[] values = { "N"};
            string[] ConditionName = { "ID" };
            string[] ConditionValue = { ID.ToString() };
            try
            {
                DBConnection.updateRows("Product", columnsName, values, ConditionName, ConditionValue);
            }
            catch (Exception )
            {
                throw new Exception("فشل استرجاع هذا المنتج");
            }
        }
        public static Int64 Insert(string name, string barcode, string SubCategory_Name, string unitName, string productType)
        {
            Int64 checkID = SelectID(name);
            if(checkID!=-1)
            {
                bool deleted = checkDeleted(checkID);
                if(deleted)
                {
                    DialogResult dialogResult = MessageBox.Show("هذا المنتج تم حذفه هل تريد إسترجاعه؟", "رسالة تأكيد", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Recover(checkID);
                        return -2;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return -1;
                    }
                }
            }
            Available_ID = DBConnection.getIDCounter("Product");
            Int64 ID = SubCategory.SelectID(SubCategory_Name);
            string[] names = { "ID", "Name", "Barcode","SubCategory_ID","unitName","ProductType","Deleted" };
            string[] values = { Product.Available_ID.ToString(),name,barcode,ID.ToString(),unitName,productType,"N"};
            try
            {
                DBConnection.insertRow("Product", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
            DBConnection.setIDCounter("Product",Product.Available_ID+1);
            return Available_ID;
        }
        public static void InsertNormal (string In_Price,string Out_Price,Int64 ID)
        {

            string[] names = { "ID", "In_Price", "Out_Price"};
            string[] values = { ID.ToString(), In_Price, Out_Price};
            try
            {
                DBConnection.insertRow("ProductNormal", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void InsertNormal(string In_Price, string Out_Price, string name)
        {
            Int64 ID = SelectID(name);
            string[] names = { "ID", "In_Price", "Out_Price" };
            string[] values = { ID.ToString(), In_Price, Out_Price };
            try
            {
                DBConnection.insertRow("ProductNormal", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void InsertLarge(string Small_Name, string Large_Name,string quantity)
        {
            Int64 Small_ID = SelectID(Small_Name);
            Int64 Large_ID = SelectID(Large_Name);

            string[] names = { "SMALL_PRODUCT_ID", "LARGE_PRODUCT_ID", "QUANTITY" };
            string[] values = { Small_ID.ToString(), Large_ID.ToString(), quantity };
            try
            {
                DBConnection.insertRow("ProductLargeUnit", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Int64 SelectID(string name)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return -1;
        }

        public static string  SelectName(string barcode)
        {
            string[] columnsName = { "Name" };
            string[] ConditionName = { "Barcode" };
            string[] ConditionValue = { barcode };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return oracleDataReader.GetOracleValue(0).ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return "";
        }
        public static void Select(string text,bool type)
        {
            products = new List<Product>();
            string[] columnsName = { "Product.Name,ProductNormal.In_Price,ProductNormal.Out_Price","Product.Barcode" };

            string[] ConditionName= {"Name"};
            if(!type)
                ConditionName[0] = "Barcode";
            string[] ConditionValue = {"%"+text+"%"};
            string[] joinColumns = { "Product.ID = ProductNormal.Product_ID" };
            OracleDataReader oracleDataReader = DBConnection.selectRows("Product, ProductNormal", columnsName, ConditionName, ConditionValue,joinColumns);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    float inPrice = float.Parse(oracleDataReader.GetValue(1).ToString());
                    float outPrice = float.Parse(oracleDataReader.GetValue(2).ToString());
                    string barcode = oracleDataReader.GetValue(3).ToString();
                    products.Add(new Product(name,inPrice,outPrice, barcode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static float selectPrice(string name)
        {
            Int64 ID = SelectID(name); string[] columnsName = { "OUT_Price" };
            string[] ConditionName = { "Product_ID" };
            string[] ConditionValue = { ID.ToString() };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ProductNormal", columnsName, ConditionName, ConditionValue);
                if(oracleDataReader.Read())
                {
                    float Out_Price = float.Parse(oracleDataReader.GetValue(0).ToString());
                    return Out_Price;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return 0;
        }

        public static Int64 SelectID(string name,bool barcode)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Barcode" };
            string[] ConditionValue = { name };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return -1;
        }

        public static float selectPrice(string barcode,bool bar)
        {

            Int64 ID = SelectID(barcode,true); 
            string[] columnsName = { "OUT_Price" };
            string[] ConditionName = { "Product_ID" };
            string[] ConditionValue = { ID.ToString() };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("ProductNormal", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    float Out_Price = float.Parse(oracleDataReader.GetValue(0).ToString());
                    return Out_Price;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return 0;
        }

        public static void Select_All(string subcategory)
        {
            products = new List<Product>();
            Int64 Category_ID = SubCategory.SelectID(subcategory);

            if (Category_ID == -1)
            {
                return;
                //throw new Exception("لا يوجد قسم رئيسي بهذا الاسم");
            }
            string[] columnsName = { "Name" };
            string[] ConditionName = { "SubCATEGORY_ID" };
            string[] ConditionValue = { Category_ID.ToString()};
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product", columnsName, ConditionName, ConditionValue);
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    products.Add(new Product(name));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
