using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace TimeTable
{
    public class BD
    {
        string strAccessConn = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" + Application.StartupPath.ToString() + "\\DB.mdb";
        string strAccessSelect = "SELECT Name FROM Practices";
        DataSet myDataSet = new DataSet();
        OleDbConnection myAccessConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\BD.mdb");
        public BD()
        {
            try
            {
                myAccessConn.Open();
                OleDbCommand myAccessCommand = new OleDbCommand(strAccessSelect, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                myDataAdapter.Fill(myDataSet);
                myAccessConn.Close();
            }
            catch(Exception ex)
            {
                

                MessageBox.Show("Не удалось найти базу данных");
            }
        }   

        static public List<string> getListFromBD(string query)
        {
            List<string> outValues = new List<string>();
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\DB.mdb";
            OleDbConnection myAccessConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\BD.mdb");
            DataSet dataset = new DataSet();
            myAccessConn.Open();
            {
                OleDbCommand myAccessCommand = new OleDbCommand(query, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                try
                {
                    myDataAdapter.Fill(dataset);
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        outValues.Add(dataset.Tables[0].Rows[i][0].ToString());
                    }
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }

            if (outValues.Count != 0)
                return outValues;
            return null;
        }

        static public DataSet getDataSetFromBD(string query)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\DB.mdb";
            OleDbConnection myAccessConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\BD.mdb");
            DataSet dataset = new DataSet();
            myAccessConn.Open();
            {
                OleDbCommand myAccessCommand = new OleDbCommand(query, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                try
                {
                    myDataAdapter.Fill(dataset);
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            return dataset;
        }

        static public DataTable getDataTableFromBD(string query)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\DB.mdb";
            OleDbConnection myAccessConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath.ToString() + "\\BD.mdb");
            DataTable dataset = new DataTable();
            myAccessConn.Open();
            {
                OleDbCommand myAccessCommand = new OleDbCommand(query, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                try
                {
                    myDataAdapter.Fill(dataset);
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            return dataset;
        }
        /*public OleDbDataReader findInCollection(string queryStr)
        {
            myAccessConn.Open();
            OleDbCommand c = new OleDbCommand(queryStr, myAccessConn);
            return c.ExecuteReader();
        }*/

        public void Select(string queryStr)
        {
            this.myDataSet.Clear();
            myAccessConn.Open();
            { 
                OleDbCommand myAccessCommand = new OleDbCommand( queryStr, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                try
                {
                    myDataAdapter.Fill(this.myDataSet);
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            myAccessConn.Close();
        }

        public void SelectList(string queryStr, ref DataGrid dataGrid)
        {
            this.myDataSet.Clear();
            myAccessConn.Open();
            {
                OleDbCommand myAccessCommand = new OleDbCommand(queryStr, myAccessConn);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
                try
                {
                    myDataAdapter.Fill(this.myDataSet);
                }
                catch (OleDbException exp)
                {
                    MessageBox.Show(exp.Message);
                    DefSelect();
                }
            }
            myAccessConn.Close();
        }

        public string SelectMaxObjID(int numberOfObj, ref DataGridView dataGrid)
        {
            myAccessConn.Open();
            OleDbCommand myAccessCommand = new OleDbCommand("SELECT Max(Object.IDobj) AS [Max-IDobj] FROM [object]", myAccessConn);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
            DataSet boofer = new DataSet();
            try
            {
                myDataAdapter.Fill(boofer);
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.Message);
            }
            dataGrid.DataSource = boofer.Tables[0];
            string output = dataGrid[0, 0].Value.ToString();
            myAccessConn.Close();
            return output;
        }
        public string SelectFirstQery(string qery)
        {
            myAccessConn.Open();
            OleDbCommand myAccessCommand = new OleDbCommand(qery, myAccessConn);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
            DataSet boofer = new DataSet();
            try
            {
                myDataAdapter.Fill(boofer);
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.Message);
            }

            try
            {
                string output = boofer.Tables[0].Rows[0][0].ToString();
                myAccessConn.Close();
                return output;
            }
            catch
            {
                myAccessConn.Close();
                return "";
            }
            
        }
        //public static GetString
        public void DefSelect()
        {
            this.myDataSet.Clear();
            myAccessConn.Open();


            OleDbCommand myAccessCommand = new OleDbCommand(strAccessSelect, myAccessConn);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
            try
            {
                myDataAdapter.Fill(this.myDataSet);
            }
            catch (OleDbException exp)
            {
                MessageBox.Show(exp.Message);
            }

            myAccessConn.Close();
        }

        public void Update(string query)
        {
            myAccessConn.Open();
            OleDbCommand update = new OleDbCommand(query, myAccessConn);
            //[Object].IDObj = \"555\"
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(update);
            myDataAdapter.InsertCommand = update;
            update.ExecuteNonQuery();
            myAccessConn.Close();
        }
        public void Add(string query)
        {
            myAccessConn.Open();
            OleDbCommand add = new OleDbCommand(query, myAccessConn);
            
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(add);
            myDataAdapter.InsertCommand = add;
            add.ExecuteNonQuery();
            myAccessConn.Close();
        }

        public void Delete(string query)
        {
            myAccessConn.Open();
            OleDbCommand myAccessCommand = new OleDbCommand(query, myAccessConn);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
            myDataAdapter.Fill(myDataSet);
            myAccessCommand.ExecuteNonQuery();
            myAccessConn.Close();
            DefSelect();


        }
        public DataSet MyDataSet
        {
            get { return myDataSet; }
        }
        public string StrAccessSelect
        {
            get { return strAccessSelect; }
        }
    }
}
