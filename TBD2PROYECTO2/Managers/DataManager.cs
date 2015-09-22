using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TBD2PROYECTO2.DataObjects;

namespace TBD2PROYECTO2.Managers
{
    public class DataManager
    {
        private  string connection;
        public DataManager()
        {
            connection = "Server=localhost;Database=localdb;Trusted_Connection=True;MultipleActiveResultSets=True;";
        }

        public static Tuple<string, string> getBeginTimeAndName(string id, string conn)
        {
            using (
                var con =
                    new SqlConnection("Server=localhost;Database=" + conn +
                                      ";Trusted_Connection=True;MultipleActiveResultSets=True;"))
            {
                con.Open();
                using (
                    var command =
                        new SqlCommand(
                            "select [Begin Time], [Transaction name] from sys.fn_dblog(null,null) where [operation] = 'LOP_BEGIN_XACT' and [Transaction id] = '" +
                            id + "';", con))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return Tuple.Create(reader[0].ToString(), reader[1].ToString());
                    }
                }
            }

            return Tuple.Create("", "");
        }

        public static  string getDeleteFromUpdate(string id, string conn)
        {
            using (
                var con =
                    new SqlConnection("Server=localhost;Database=" + conn+
                                      ";Trusted_Connection=True;MultipleActiveResultSets=True;"))
            {
                con.Open();
                using (
                    var command =
                        new SqlCommand(
                            "select [RowLog Contents 0] from sys.fn_dblog(null,null) where [operation] = 'LOP_DELETE_ROWS' and [Transaction id] = '" +
                            id + "';", con))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var bytes = reader[0] as byte[];

                        if (bytes == null) continue;
                        var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
                        if (!String.IsNullOrEmpty(hex))
                        {
                            return hex;
                        }
                    }
                }
            }
            return "";
        }

        public List<ColumnEntity> MetaData(string db, string table)
        {
            var primaryKey = PrimaryKeys(db, table);
            var result = new List<ColumnEntity>();
            var query =
                "USE "+ db +" SELECT [name], [xtype], [length] FROM syscolumns WHERE id = (SELECT id FROM sysobjects WHERE xtype = 'u' and name = '" +
                table + "');";
            var conn = new SqlConnection(connection);
            var myCommand = new SqlCommand(query, conn);
            conn.Open();
            var reader = myCommand.ExecuteReader();
            var l = new List<string>();
            while (reader.Read())
            {
                var newCol = new ColumnEntity(Convert.ToInt32(reader[1]), Convert.ToInt16(reader[2]),
                    reader[0].ToString(), primaryKey.Contains(reader[0].ToString()));
                result.Add(newCol);
            }
            reader.Close();
            conn.Close();
            return result;
        }

        public string PrimaryKeys(string db, string table)
        {
            var query = "USE " + db+ " SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '"+table+"'";
            var conn = new SqlConnection(connection);
            var myCommand = new SqlCommand(query, conn);
            conn.Open();
            var reader = myCommand.ExecuteReader();
            var result = "";
            if(reader.Read())
            result = reader[0].ToString();
            reader.Close();
            conn.Close();
            return result;
        }

        public List<string> LogContent(string db, string table, string action)
        {
            var result = new List<string>(); ;
            var query = "USE " + db + " SELECT [RowLog Contents 0], [Transaction ID], [Begin Time] FROM fn_dblog(null, null) WHERE Operation = '"+action+"' AND AllocUnitName like 'dbo." + table + ".PK%'";

            var conn = new SqlConnection(connection);
            var myCommand = new SqlCommand(query, conn);
            conn.Open();
            var reader = myCommand.ExecuteReader();
			if(!(reader.HasRows))
			{
				conn.Close();
				query = "USE " + db + " SELECT [RowLog Contents 0], [Transaction ID], [Begin Time] FROM fn_dblog(null, null) WHERE Operation = '"+action+"' AND AllocUnitName = 'dbo." + table + "'";
				myCommand = new SqlCommand(query, conn);
				conn.Open();
				reader = myCommand.ExecuteReader();
			}
            bool first = true;
            while (reader.Read())
            {
                result.Add(reader[1].ToString());
                result.Add(reader[2].ToString());

                var bytes = reader[0] as byte[];

                if (bytes == null) continue;
                var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
                if (!String.IsNullOrEmpty(hex))
                {
                    result.Add(hex);
                }
            }
            reader.Close();
            conn.Close();
            return result;
        }



    }
}
