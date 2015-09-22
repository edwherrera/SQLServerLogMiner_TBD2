using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBD2PROYECTO2.DataObjects;
using TBD2PROYECTO2.Managers;

namespace TBD2PROYECTO2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getDatabases();
            BackColor = Color.CadetBlue;
            //comboBox1.BackColor = Color.DarkOrange;
            //comboBox1.ForeColor = Color.Black;
            //comboBox2.BackColor = Color.DarkOrange;
            //comboBox2.ForeColor = Color.Black;
            insertListView.BackColor = Color.Black;
            deleteListView.BackColor = Color.Black;
            insertListView.ForeColor = Color.DeepPink;
            deleteListView.ForeColor = Color.DeepPink;
            deleteListView.Font = new Font(deleteListView.Font, FontStyle.Bold);
            insertListView.Font = new Font(insertListView.Font, FontStyle.Bold);
            updateListView.BackColor = Color.Black;
            updateListView.ForeColor = Color.DeepPink;
            updateListView.Font = new Font(insertListView.Font, FontStyle.Bold);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                return;
            }
            insertListView.Columns.Clear();
            deleteListView.Columns.Clear();
            insertListView.Clear();
            deleteListView.Clear();
            updateListView.Columns.Clear();
            updateListView.Clear();
            var metadata = new DataManager().MetaData(this.comboBox1.SelectedItem.ToString(), this.comboBox2.SelectedItem.ToString());
            var redoInsert = "INSERT INTO " + this.comboBox2.SelectedItem.ToString() + "(";
            var undoInsert = "DELETE FROM " + this.comboBox2.SelectedItem.ToString() + " WHERE ";
            for (int i = 0, x = 0; i < metadata.Count - x; i++)
            {
                if (metadata.ElementAt(i).Types == Types.VarChar)
                {
                    var value = metadata.ElementAt(i);
                    metadata.RemoveAt(i);
                    metadata.Add(value);
                    i = -1;
                    x++;
                }
            }
           

            insertListView.Columns.Add("Transaction ID");
            insertListView.Columns.Add("Begin Time");
            deleteListView.Columns.Add("Transaction ID");
            deleteListView.Columns.Add("Begin Time");
            updateListView.Columns.Add("Transaction ID");
            updateListView.Columns.Add("Begin Time");
;            for (var i = 0; i < metadata.Count; i++)
            {

                if (i == metadata.Count - 1)
                {
                    this.deleteListView.Columns.Add(metadata[i].Name);
                    insertListView.Columns.Add(metadata[i].Name);
                    updateListView.Columns.Add(metadata[i].Name);
                    redoInsert += metadata[i].Name + ")";
                }
                else
                {
                    this.deleteListView.Columns.Add(metadata[i].Name);
                    insertListView.Columns.Add(metadata[i].Name);
                    updateListView.Columns.Add(metadata[i].Name);
                    redoInsert += metadata[i].Name + ",";
                }
            }
            this.deleteListView.Columns.Add("Redo SQL");
            this.deleteListView.Columns.Add("Undo SQL");
            insertListView.Columns.Add("Redo SQL");
            insertListView.Columns.Add("Undo SQL");
            updateListView.Columns.Add("Redo SQL");
            updateListView.Columns.Add("Undo SQL");

            redoInsert += " values(";

            AddValues(metadata, "LOP_DELETE_ROWS", undoInsert, redoInsert, metadata);
            AddValues(metadata, "LOP_INSERT_ROWS", redoInsert, undoInsert, metadata);
        }

        void getTables()
        {
            comboBox2.Items.Clear();
            var databaseName = comboBox1.GetItemText(comboBox1.SelectedItem).Trim();
            if (String.IsNullOrEmpty(databaseName))
                return;
            var connection = new SqlConnection("Server=localhost;Database=" + databaseName + ";Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var command = new SqlCommand("SELECT [name] FROM sysobjects WHERE xtype = 'u';", connection);
            var reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
            {
                var tmp = new ComboBoxEntity()
                {
                    Text = reader[0].ToString(),
                    Value = i++
                };
                comboBox2.Items.Add(tmp);
            }
            connection.Close();
        }

        private void getDatabases()
        {
            comboBox1.Items.Clear();
            var connection = new SqlConnection("Server=localhost;Trusted_Connection=True;MultipleActiveResultSets=True;");
            connection.Open();
            var command = new SqlCommand("SELECT name FROM Sys.Databases;", connection);
            var reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
            {
                var tmp = new ComboBoxEntity()
                {
                    Text = reader[0].ToString(),
                    Value = i++
                };
                comboBox1.Items.Add(tmp);
            }
            connection.Close();
        }
        public void AddValues(List<ColumnEntity> metadata, string action, string redo, string undo, List<ColumnEntity> data)
        {
            var rowlog = new DataManager().LogContent(this.comboBox1.SelectedItem.ToString(), this.comboBox2.SelectedItem.ToString(), action);
            //var converter = new Converter();
            var redoContinue = redo;
            var undoContinue = undo;
            var types = TableTypes();
            for (int i = 0; i < rowlog.Count; i++)
            {
                var trans = rowlog[i++];
                i++;
                var mts = DataManager.getBeginTimeAndName(trans, comboBox1.SelectedItem.ToString());
                var time = mts.Item1;
                var tName = mts.Item2;
                var values = Converter.ParseLog(rowlog[i], metadata);
                if (tName.ToLower().Equals("update") && action.Equals("LOP_INSERT_ROWS"))
                {
                    buildForUpdateList(data, values, trans, time);
                    continue;
                }
                for (int j = 0; j < values.Count; j++)
                {

                    if (j == values.Count - 1)
                    {
                        if (action.Equals("LOP_INSERT_ROWS"))
                        {
                            if (types.ElementAt(j).Types == Types.VarChar && values[j] != "NULL")
                            {
                                redoContinue += "'" + values[j] + "')";
                                undoContinue += data[j].Name + "= '" + values[j] + "'";
                            }
                            else
                            {
                                redoContinue += values[j] + ")";
                                undoContinue += data[j].Name + "=" + values[j];
                            }
                            
                        }
                        else
                        {
                            if (types.ElementAt(j).Types == Types.VarChar && values[j] != "NULL")
                            {
                                undoContinue += "'" + values[j] + "')";
                                redoContinue += data[j].Name + "= '" + values[j] + "'";
                            }
                            else
                            {
                                undoContinue += values[j] + ")";
                                redoContinue += data[j].Name + "=" + values[j];
                            }
                        }
                    }
                    else
                    {
                        if (action.Equals("LOP_INSERT_ROWS"))
                        {
                            if (types.ElementAt(j).Equals("VarChar") && values[j] != "NULL")
                            {
                                redoContinue += "'" + values[j] + "', ";
                                undoContinue += data[j].Name + "= '" + values[j] + "' AND ";
                            }
                            else
                            {
                                redoContinue += values[j] + ",";
                                undoContinue += data[j].Name + "=" + values[j] + " AND ";
                            }
                        }
                        else
                        {
                            if (types.ElementAt(j).Equals("VarChar") && values[j] != "NULL")
                            {
                                undoContinue += "'" + values[j] + "', ";
                                redoContinue += data[j].Name + "= '" + values[j] + "' AND ";
                            }
                            else
                            {
                                undoContinue += values[j] + ",";
                                redoContinue += data[j].Name + "=" + values[j] + " AND ";
                            }
                        }

                    }
                }

                if (values.Count > 0)
                {
                    values.Insert(0, trans);
                    values.Insert(1, time);
                    values.Add(redoContinue);
                    values.Add(undoContinue);
                    var lvi = new ListViewItem(values.ToArray());
                    if (action == "LOP_DELETE_ROWS")
                    {
                        if(!(tName.ToLower().Equals("update")))this.deleteListView.Items.Add(lvi);
                    }
                    else
                    {
                        if (tName.ToLower().Equals("update"))
                        {
                            updateListView.Items.Add(lvi);
                        }
                        else this.insertListView.Items.Add(lvi);
                    }
                }
                redoContinue = redo;
                undoContinue = undo;
            }
            
            for (int i = 0; i < insertListView.Columns.Count - 1; i++)
            {
                    insertListView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
                    insertListView.AutoResizeColumn(i + 1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            for (int i = 0; i < updateListView.Columns.Count - 1; i++)
            {
                    updateListView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
                    updateListView.AutoResizeColumn(i + 1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            
            for (int i = 0; i < deleteListView.Columns.Count - 1; i++)
            {
                    deleteListView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
                    deleteListView.AutoResizeColumn(i + 1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            
        }

        private void buildForUpdateList(List<ColumnEntity> data, List<string> values, string trans, string time)
        {
            var deleted = DataManager.getDeleteFromUpdate(trans, comboBox1.SelectedItem.ToString());
            //var converter = new Converter();
            var delValues = Converter.ParseLog(deleted, data);
            var redo = "UPDATE " + comboBox2.SelectedItem + " SET ";
            var undo = "UPDATE " + comboBox2.SelectedItem + " SET ";
            var whereUndo = "WHERE ";
            var whereRedo = "WHERE ";
            for (int j = 0; j < values.Count; j++)
            {
                if (data[j].Types == Types.VarChar)
                {
                    redo += data[j].Name + " = '" + values.ElementAt(j) + "'";
                    whereRedo += data[j].Name + " = '" + values.ElementAt(j) + "'";
                }
                else
                {
                    redo += data[j].Name + " = " + values.ElementAt(j);
                    whereRedo += data[j].Name + " = " + values.ElementAt(j);
                }
                if (j < values.Count-1)
                {
                    redo += ", ";
                    whereRedo += ", ";
                }
            }
            for (int j = 0; j < delValues.Count; j++)
            {
                if (data[j].Types == Types.VarChar)
                {
                    undo += data[j].Name + " = '" + delValues.ElementAt(j) + "'";
                    whereUndo += data[j].Name + " = '" + delValues.ElementAt(j) + "'";
                }
                else
                {
                    undo += data[j].Name + " = " + delValues.ElementAt(j);
                    whereUndo += data[j].Name + " = " + delValues.ElementAt(j);
                }
                if (j < delValues.Count - 1)
                {
                    undo += ", ";
                    whereUndo += ", ";
                }
            }
            if (values.Count > 0)
            {
                redo += " " + whereUndo;
                undo += " " + whereRedo;
                values.Insert(0, trans);
                values.Insert(1, time);
                values.Add(redo);
                values.Add(undo);
                var lvi = new ListViewItem(values.ToArray());
                updateListView.Items.Add(lvi);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getTables();
        }

        private void deleteListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private string getDeleteFromUpdate(string id)
        //{
        //    using (
        //        var con =
        //            new SqlConnection("Server=localhost;Database=" + comboBox1.SelectedItem.ToString() +
        //                              ";Trusted_Connection=True;MultipleActiveResultSets=True;"))
        //    {
        //        con.Open();
        //        using (
        //            var command =
        //                new SqlCommand(
        //                    "select [RowLog Contents 0] from sys.fn_dblog(null,null) where [operation] = 'LOP_DELETE_ROWS' and [Transaction id] = '" +
        //                    id + "';", con))
        //        {
        //            var reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                var bytes = reader[0] as byte[];

        //                if (bytes == null) continue;
        //                var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
        //                if (!String.IsNullOrEmpty(hex))
        //                {
        //                    return hex;
        //                }
        //            }
        //        }
        //    }
        //    return "";
        //}

        //private Tuple<string, string> getBeginTimeAndName(string id)
        //{
        //    using (
        //        var con =
        //            new SqlConnection("Server=localhost;Database=" + comboBox1.SelectedItem.ToString() +
        //                              ";Trusted_Connection=True;MultipleActiveResultSets=True;"))
        //    {
        //        con.Open();
        //        using (
        //            var command =
        //                new SqlCommand(
        //                    "select [Begin Time], [Transaction name] from sys.fn_dblog(null,null) where [operation] = 'LOP_BEGIN_XACT' and [Transaction id] = '" +
        //                    id + "';", con))
        //        {
        //            var reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                return Tuple.Create(reader[0].ToString(), reader[1].ToString());
        //            }
        //        }
        //    }

        //    return Tuple.Create("", "");
        //}


        private List<ColumnEntity> TableTypes()
        {
           var types = new List<ColumnEntity>();
            var query =
                "USE " + comboBox1.SelectedItem + " SELECT [xtype] FROM syscolumns WHERE id = (SELECT id FROM sysobjects WHERE xtype = 'u' and name = '" +
                comboBox2.SelectedItem + "');";
            var cnn = new SqlConnection("Server=localhost;Database=localdb;Trusted_Connection=True;MultipleActiveResultSets=True;");
            var myCommand = new SqlCommand(query, cnn);
            cnn.Open();
            var reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                types.Add(new ColumnEntity(Convert.ToInt32(reader[0]), 0, "", false));
            }
            return types;
        }
    }
}
