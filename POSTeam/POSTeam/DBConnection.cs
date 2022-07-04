using System.Data.OracleClient;

using System;
using System.Windows.Forms;
using System.Drawing;

namespace POSTeam
{
    class DBConnection
    {
        static OracleConnection connection;
        public static void startConnection()
        {
            try
            {
                connection = new OracleConnection("user id=system ; password=@Sawah.142001 ; data source=ORCL");
                connection.Open();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void createTable(string tableName, string[] names, string[] dataTypes)
        {
            string query = "CREATE TABLE " + tableName + " ( ";
            query += (names[0] + " " + dataTypes[0]);
            for (int i = 1; i < names.Length; i++)
            {   
                query += ", ";
                query += (names[i] + " " + dataTypes[i]);
            }
            query += " )";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.ExecuteNonQuery();
        }
        public static void dropTable(string name)
        {
            string query = "DROP TABLE " + name;
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.ExecuteNonQuery();
        }
        public static void addConstraint(string tableName,string constraint)
        {
            string query = "ALTER TABLE " + tableName+" ";
            query += ("ADD Constraint "+ constraint);
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.ExecuteNonQuery();
        }
        public static void insertRow(string tableName,string[] names,string[] values)
        {
            string insertquery = "INSERT INTO " + tableName + " VALUES ( :" + names[0];
            for(int i=1;i<names.Length;i++)
            {
                insertquery += ", :";
                insertquery += names[i];
            }
            insertquery += ")";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = insertquery;
            for (int i = 0; i <names.Length;i++)
            {
                cmd.Parameters.Add(names[i], values[i]);

            }
            cmd.ExecuteNonQuery();
        }
        public static OracleDataReader selectRows(string tableName,string[]columnsName,string[]ConditionName,string[]ConditionValue,bool join=false)
        {
            string query = "Select " + columnsName[0];
            for(int i=1;i<columnsName.Length;i++)
            {
                query += ", ";
                query += columnsName[i];
            }
            query += (" From " + tableName);
            if(ConditionName.Length!=0)
            {
                query += " WHERE ";
                if (join)
                    query += (ConditionName[0] + " = " + ConditionValue[0]);
                else
                    query += (ConditionName[0] + " = :" + ConditionName[0]);
                for (int i=1;i<ConditionName.Length;i++)
                {
                    query += " AND ";
                    if(join)
                        query += (ConditionName[i] + " = " + ConditionValue[i]);
                    else
                        query += (ConditionName[i] + " = :" + ConditionName[i]);
                }
            }
            OracleCommand cmd = new OracleCommand(query, connection);
            if (!join)
            {
                for (int i = 0; i < ConditionName.Length; i++)
                {
                    cmd.Parameters.Add(ConditionName[i], ConditionValue[i]);
                }
            }
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static OracleDataReader selectRows(string tableName, string[] columnsName, string[] ConditionName, string[] ConditionValue,string[] joinConidition)
        {
            string query = "Select " + columnsName[0];
            for (int i = 1; i < columnsName.Length; i++)
            {
                query += ", ";
                query += columnsName[i];
            }
            query += (" From " + tableName);
            {
                query += " WHERE ";
                query += (joinConidition[0]);
                for (int i = 1; i < joinConidition.Length; i++)
                {
                    query += " AND ";
                    query += (joinConidition[i]);
                }
            }
            if (ConditionName.Length != 0)
            {
                for (int i = 0; i < ConditionName.Length; i++)
                {
                    query += " AND ";
                    query += (ConditionName[i] + " Like :" + ConditionName[i]);
                }
            }
            OracleCommand cmd = new OracleCommand(query, connection);
            
            for (int i = 0; i < ConditionName.Length; i++)
            {
                cmd.Parameters.Add(ConditionName[i], ConditionValue[i]);
            }
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public static void deleteRows(string tableName, string[] ConditionName, string[] ConditionValue)
        {
            if (ConditionName.Length == 0) return;
            string query = "Delete From " + tableName;
            query += " WHERE ";
            query += (ConditionName[0] + "= :" + ConditionName[0]);
            for (int i = 1; i < ConditionName.Length; i++)
            {
                query += " AND ";
                query += (ConditionName[i] + "= :" + ConditionName[i]);
            }
            OracleCommand cmd = new OracleCommand(query, connection);
            for (int i = 0; i < ConditionName.Length; i++)
            {
                cmd.Parameters.Add(ConditionName[i], ConditionValue[i]);
            }
            cmd.ExecuteNonQuery();
        }

        public static void deleteTime(string tableName, string Start_Time, string End_Time)
        {
            string query = "Delete From " + tableName;
            query += " WHERE Start_Time Between ";
            query += (":Time1 AND :Time2");
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("Time1", Start_Time);
            cmd.Parameters.Add("Time2", End_Time);
            cmd.ExecuteNonQuery();
        }

        internal static OracleDataReader selectRowsLike(string tableName, string[] columnsName, string ConditionName, string ConditionValue)
        {
            string query = "Select " + columnsName[0];
            for (int i = 1; i < columnsName.Length; i++)
            {
                query += ", ";
                query += columnsName[i];
            }
            query += (" From " + tableName);
            query += " WHERE ";
            query += (ConditionName + " Like :" + ConditionName);
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(ConditionName,ConditionValue);
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public static void updateRows(string tableName, string[] columnsName,string[] values, string[] ConditionName, string[] ConditionValue)
        {
            if (ConditionName.Length == 0 || columnsName.Length==0) return;
            string query = "Update " + tableName+" set ";
            
            query += (columnsName[0] + " = :" + columnsName[0]);
            for (int i = 1; i < columnsName.Length; i++)
            {
                query += ", ";
                query += (columnsName[i] + " = :" + columnsName[i]);
            }
            query += " WHERE ";
            query += (ConditionName[0] + " = :" + ConditionName[0]);
            for (int i = 1; i < ConditionName.Length; i++)
            {
                query += " AND ";
                query += (ConditionName[i] + " = :" + ConditionName[i]);
            }
            OracleCommand cmd = new OracleCommand(query, connection);

            for (int i = 0; i < columnsName.Length; i++)
            {
                cmd.Parameters.Add(columnsName[i], values[i]);
            }

            for (int i = 0; i < ConditionName.Length; i++)
            {
                cmd.Parameters.Add(ConditionName[i], ConditionValue[i]);
            }
            cmd.ExecuteNonQuery();
        }
        public static Int64 getIDCounter(string tableName)
        {
            string[] columnsName = { "LASTID" };
            string[] ConditionName = {"Name"};
            string[] ConditionValue = {tableName };
            OracleDataReader oracleDataReader = DBConnection.selectRows("IDCOUNTER", columnsName, ConditionName, ConditionValue);
            if(oracleDataReader.Read())
            {
                Int64 id = Int64.Parse(oracleDataReader.GetValue(0).ToString());
                return id;
            }
            return -1;
        }
        public static void setIDCounter(string tableName,Int64 ID)
        {
            string[] columnsName = { "LASTID" };
            string[] columnsValue = { ID.ToString()};
            string[] ConditionName = { "Name" };
            string[] ConditionValue = { tableName };
            try
            {
                DBConnection.updateRows("IDCOUNTER", columnsName, columnsValue, ConditionName, ConditionValue);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
