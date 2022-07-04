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
    class Shift
    {
        static BindingList<Shift> shifts;
        public Int64 ID;
        public DateTime Start_time;
        public DateTime End_time;
        public static void Insert(DateTime Start_time,DateTime End_time)
        {
           
            Available_ID = DBConnection.getIDCounter("Shift");
           
           
            string[] names = { "ID", "Start_time", "End_Time"};
            string[] values = { Available_ID.ToString(), Start_time.ToString("dd,MMM,yyyy:hh:mm:ss"), End_time.ToString("dd,MMM,yyyy:hh:mm:ss") };
            try
            {
                DBConnection.insertRow("Shift", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
            DBConnection.setIDCounter("Shift", Available_ID + 1);
        }

        public static void Insert(DateTime Start_time)
        {

            Available_ID = DBConnection.getIDCounter("Shift");


            string[] names = { "ID", "Start_time" ,"End_Time"};
            string[] values = { Available_ID.ToString(), Start_time.ToString("dd,MMM,yyyy:hh:mm:ss"),"01,NOV,1900:01:00:00"};
            try
            {
                DBConnection.insertRow("Shift", names, values);
            }
            catch (Exception e)
            {
                throw e;
            }
            DBConnection.setIDCounter("Shift", Available_ID + 1);
        }

        public static void Update(DateTime End_time)
        {

            Available_ID = DBConnection.getIDCounter("Shift")-1;

            string[] names = { "End_Time" };
            string[] values = {End_time.ToString("dd,MMM,yyyy:hh:mm:ss") };
            string[] conditionColumns = { "ID" };
            string[] conditionValues = { Available_ID.ToString() };
            try
            {
                DBConnection.updateRows("Shift",names,values,conditionColumns,conditionValues);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Shift(Int64 ID,DateTime Start_time,DateTime End_time)
        {
            this.ID = ID;
            this.Start_time = Start_time;
            this.End_time = End_time;
        }


        public static Int64 Available_ID;
        public static Shift Select(Int64 ID)
        {
            string[] columnsName = {  "Start_Time", "End_Time" };
            string[] ConditionName = { "ID"};
            string[] ConditionValue = {ID.ToString() };
            OracleDataReader oracleDataReader = DBConnection.selectRows("Shift", columnsName, ConditionName, ConditionValue);
            try
            {
                if (oracleDataReader.Read())
                {
                    DateTime Start_Time = DateTime.Parse(oracleDataReader.GetOracleDateTime(0).ToString());
                    DateTime End_Time = DateTime.Parse(oracleDataReader.GetOracleDateTime(1).ToString());

                    return new Shift(ID, Start_Time, End_Time);
                }
            }
            catch (Exception )
            {
                throw new Exception("فشل تحميل الورديات ");
            }
            throw new NotImplementedException();
        }
        public static void Select_All()
        {
            shifts = new BindingList<Shift>();
            string[] columnsName = { "ID","Start_Time","End_Time" };
            string[] ConditionName = { };
            string[] ConditionValue = { };
            OracleDataReader oracleDataReader = DBConnection.selectRows("Shift", columnsName, ConditionName, ConditionValue);
            try
            {
                while (oracleDataReader.Read())
                {
                    Int64 ID = Int64.Parse(oracleDataReader.GetValue(0).ToString());
                    DateTime Start_Time = DateTime.Parse(oracleDataReader.GetOracleDateTime(1).ToString());
                    DateTime End_Time = DateTime.Parse(oracleDataReader.GetOracleDateTime(2).ToString());



                    shifts.Add(new Shift(ID,Start_Time,End_Time));
                }
            }
            catch (Exception e)
            {
                throw new Exception("فشل تحميل الورديات ");
            }
        }
        
        public static void Delete(DateTime End_Time)
        {
            try
            {
                DBConnection.deleteTime("Shift", "01,NOV,2020:10:06:00", End_Time.ToString("dd,MMM,yyyy:hh:mm:ss"));
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
