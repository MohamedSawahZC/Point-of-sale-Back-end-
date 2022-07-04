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
    class InvoiceProduct
    {
        static BindingList<InvoiceProduct> invoiceProduct;
        public static Int64 Available_ID;
        private Int64 invoice_ID;
        private Int64 product_ID;
        private float out_price;
        private float in_price;
        private float discount;
        private float quantity;

        public Int64 Invoice_ID
        {
            get
            {
                return invoice_ID;
            }
            set
            {
                invoice_ID = value;
            }
        }
        public Int64 Product_ID
        {
            get
            {
                return product_ID;
            }
            set
            {
                product_ID = value;
            }
        }
        public float Out_price
        {
            get
            {
                return out_price;
            }
            set
            {
                out_price = value;
            }

        }
        public float In_price
        {
            get
            {
                return in_price;
            }
            set
            {
                in_price = value;
            }
        }
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
        public float Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }
        public static Int64 SelectID(Int64 Product_ID)
        {
            string[] columnsName = { "ID" };
            string[] ConditionName = { "Product_ID" };
            string[] ConditionValue = { Product_ID.ToString() };
            try
            {
                OracleDataReader oracleDataReader = DBConnection.selectRows("Product_ID", columnsName, ConditionName, ConditionValue);
                if (oracleDataReader.Read())
                {
                    return Int64.Parse(oracleDataReader.GetOracleValue(0).ToString());
                }
            }
            catch (Exception)
            {
                throw new Exception("فشل البحث عن هذه عن المنتج");
            }
            return -1;
        }
        public static void Insert(Int64 Invoice_ID,Int64 Product_ID,float out_price,float in_price,float discount,float quantity)
        {
            Int64 ID = Users.SelectID(Product_ID.ToString());

            Available_ID = DBConnection.getIDCounter("InvoiceProduct");
            string[] names = { "ID", "Invoice_ID", "Product_ID", "Out_Price", "In_Price", "Discount", "Quantity" };
            string[] values = {InvoiceProduct.Available_ID.ToString(), Invoice_ID.ToString(), Product_ID.ToString(), out_price.ToString(), in_price.ToString(), discount.ToString(), quantity.ToString() };
            try
            {
                DBConnection.insertRow("InvoiceProduct", names, values);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            DBConnection.setIDCounter("InvoiceProduct", InvoiceProduct.Available_ID + 1);
        }



    }



}
