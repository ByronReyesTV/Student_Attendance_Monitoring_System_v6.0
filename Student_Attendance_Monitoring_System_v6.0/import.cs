using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Z.Dapper.Plus;
using ExcelDataReader;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class import : Form
    {
        public import()
        {
            InitializeComponent();
        }

        DataTableCollection tableCollection;

        string imglocation = "";
        SqlCommand cmd;

        private void import_Load(object sender, EventArgs e)
        {
            registrationn1.Visible = false;
            label1.Text = "Import Records";

            // TODO: This line of code loads data into the 'school_attendance_systemV50DBDataSet.Registration' table. You can move, or remove it, as needed.
            this.registrationTableAdapter.Fill(this.school_attendance_systemV50DBDataSet.Registration);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[comboBox1.SelectedItem.ToString()];
            //dataGridView1.DataSource = dt;

            if (dt != null)
            {
                List<Registration> registrations = new List<Registration>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Registration registration = new Registration();
                    registration.student_number = dt.Rows[i]["student_number"].ToString();
                    registration.first_name = dt.Rows[i]["first_name"].ToString();
                    registration.middle_name = dt.Rows[i]["middle_name"].ToString();
                    registration.last_name = dt.Rows[i]["last_name"].ToString();
                    registration.section = dt.Rows[i]["section"].ToString();
                    registration.contact_number = dt.Rows[i]["contact_number"].ToString();
                    registrations.Add(registration);
                }
                registrationBindingSource.DataSource = registrations;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xlsx|Excel Workbook|*.xlsx|All Excel Files|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                            comboBox1.Items.Clear();
                            foreach (DataTable table in tableCollection)
                            {
                                comboBox1.Items.Add(table.TableName);
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //1. address of SQL server and database (Connection String)
            String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=school_attendance_systemV50DB;Integrated Security=True";

            //2. establish connection (c# sqlconnection class)
            SqlConnection con = new SqlConnection(ConnectionString);

            //3. open connection (c# sqlconnection open)
            con.Open();

            //4. prepare query
            string Query = "DELETE FROM Registration";
            SqlCommand cmd = new SqlCommand(Query, con);

            //5. execute query (c# sqlcommand class)
            cmd.ExecuteNonQuery();

            try
            {
                string connectionSting = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=school_attendance_systemV50DB;Integrated Security=True";
                DapperPlusManager.Entity<Registration>().Table("Registration");
                List<Registration> registrations = registrationBindingSource.DataSource as List<Registration>;
                if (registrations != null)
                {
                    using (IDbConnection db = new SqlConnection(connectionSting))
                    {
                        db.BulkInsert(registrations);
                    }
                }
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "Import Records";
            registrationn1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "Registration";
            registrationn1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Admin ad = new Admin();
            ad.Show();
            this.Hide();
        }

        private void registrationn1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
