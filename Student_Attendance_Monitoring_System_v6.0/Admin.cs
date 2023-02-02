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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void qr_print1_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void admin_report1_Load(object sender, EventArgs e)
        {

        }

        private void edit1_Load(object sender, EventArgs e)
        {

        }

        private void registration1_Load(object sender, EventArgs e)
        {

        }

        private void Admin_Load(object sender, EventArgs e)
        {
            edit1.Visible = false;
            qr_print1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edit1.Visible = false;
            qr_print1.Visible = false;

            // view attendance //

            //1. address of SQL server and database (Connection String)
            String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True";

            //2. establish connection (c# sqlconnection class)
            SqlConnection con = new SqlConnection(ConnectionString);

            //3. open connection (c# sqlconnection open)
            con.Open();

            //4. prepare query
            string Query = "SELECT * FROM tb_student_attendance";
            SqlCommand cmd = new SqlCommand(Query, con);

            //5. execute query (c# sqlcommand class)
            var reader = cmd.ExecuteReader();

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["student_number"], reader["first_name"], reader["last_name"], reader["contact_number"], reader["time_in"], reader["time_out"], reader["date"]);
            }
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Record not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            edit1.Visible = true;
            qr_print1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            import imp = new import();
            imp.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            qr_print1.Visible = true;
            edit1.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }
    }
}
