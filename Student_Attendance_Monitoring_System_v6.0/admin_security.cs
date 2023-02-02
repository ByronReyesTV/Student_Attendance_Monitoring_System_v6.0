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
    public partial class admin_security : UserControl
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True");

        public admin_security()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Enter your username first!!");
            }
            else
            {
                if (textBox2.Text.Length == 0)
                {
                    MessageBox.Show("Enter your password!!");
                }
                else
                {
                    string username, password;

                    username = textBox1.Text;
                    password = textBox2.Text;

                    try
                    {
                        string query = "SELECT * FROM tb_admin_security WHERE username = '" + username + "' AND password = '" + password + "'";
                        SqlDataAdapter sda = new SqlDataAdapter(query, conn);

                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            username = textBox1.Text;
                            password = textBox2.Text;

                            Admin frm = new Admin();
                            frm.Show();
                            this.Hide();

                            Form1 frm1 = new Form1();
                            frm1.Close();
                        }
                        else
                        {
                            MessageBox.Show("Access Denied", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBox1.Clear();
                            textBox2.Clear();

                            textBox1.Focus();

                         
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Username or Password is incorrect", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
