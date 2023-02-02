using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class attendance_record : UserControl
    {
        public attendance_record()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Please enter your student number first", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (textBox1.Text.Length < 11)
                {
                    MessageBox.Show("Please enter your complete student number", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // view attendance //

                    //1. address of SQL server and database (Connection String)
                    String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True";

                    //2. establish connection (c# sqlconnection class)
                    SqlConnection con = new SqlConnection(ConnectionString);

                    //3. open connection (c# sqlconnection open)
                    con.Open();

                    //4. prepare query
                    string studentnumber = textBox1.Text;

                    string Query = "SELECT * FROM tb_student_attendance WHERE student_number LIKE '%" + studentnumber + "%'";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    //5. execute query (c# sqlcommand class)
                    var reader = cmd.ExecuteReader();

                    dataGridView1.Rows.Clear();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(reader["student_number"], reader["first_name"], reader["middle_name"], reader["last_name"], reader["time_in"], reader["time_out"], reader["date"]);
                    }
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Record not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
