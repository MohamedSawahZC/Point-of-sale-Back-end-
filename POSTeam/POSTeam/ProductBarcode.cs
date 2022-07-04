
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSTeam
{
    class ProductBarcode
    {
        public static BindingList<ProductBarcode> barcodes;
        private string name;
        private string barcode;

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

        public ProductBarcode(string name,string barcode)
        {
            this.name = name;
            this.barcode = barcode;
        }
        public static void Select_All(string productName)
        {
            barcodes = new BindingList<ProductBarcode>();
            Int64 ID = Product.SelectID(productName);
            if (ID == -1) return;


            string[] columnsName = { "Name","Barcode"};
            string[] ConditionName = {"Product_ID" };
            string[] ConditionValue = {ID.ToString() };
            OracleDataReader oracleDataReader = DBConnection.selectRows("ProductBarcode", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    string name = oracleDataReader.GetValue(0).ToString();
                    string barcode = oracleDataReader.GetValue(1).ToString();
                    barcodes.Add(new ProductBarcode(name, barcode));
                }
            }
            catch(Exception)
            {
                throw new Exception("لا يوجد عناصر");
            }
        }

        public static void addRow(string name,string barcode)
        {
            barcodes.Add(new ProductBarcode(name, barcode));
        }
        public static void delete(int rowIndex)
        {
            barcodes.RemoveAt(rowIndex);
        }
        public static void Insert_All(string productName)
        {
            try
            {
                Delete_All(productName);
                Int64 product_ID = Product.SelectID(productName);
                for (int i = 0; i < barcodes.Count; i++)
                {
                    ProductBarcode bar = barcodes[i];
                    string[] names = { "Product_ID", "Name", "Barcode" };
                    string[] values = { product_ID.ToString(), bar.name, bar.barcode };
                    try
                    {
                        DBConnection.insertRow("ProductBarcode", names, values);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            catch(Exception)
            {
                throw new Exception("لا يوجد عناصر");
            }
        }
        public static void Delete_All(string productName)
        {
            Int64 product_ID = Product.SelectID(productName);

            if (product_ID == -1)
                return;

            string[] conditionName = { "Product_ID" };
            string[] conditionValue = { product_ID.ToString()};
            try
            {
                DBConnection.deleteRows("ProductBarcode", conditionName, conditionValue);
            }
            catch
            {
                throw new Exception("فشل تعديل الباركود الاضافي");
            }
        }
    }
}
