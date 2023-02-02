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
    public partial class impoert_registration : UserControl
    {
        public SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=school_attendance_systemV50DB;Integrated Security=True");

        public impoert_registration()
        {
            InitializeComponent();
        }

        SqlCommand cmd;

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string sqlQuery = "SELECT first_name, middle_name, last_name, section, contact_number FROM Registration WHERE student_number = '" + textBox1.Text + "'";
            cmd = new SqlCommand(sqlQuery, con);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                textBox2.Text = reader[0].ToString();
                textBox3.Text = reader[1].ToString();
                textBox4.Text = reader[2].ToString();
                textBox5.Text = reader[3].ToString();
                textBox6.Text = reader[4].ToString();
            }
            else
            {
                MessageBox.Show("no record");
            }

            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
