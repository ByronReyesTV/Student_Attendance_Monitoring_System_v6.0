using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Visible = false;
            admin_security1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            admin_security1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Focused == true)
            {
                button2.BackColor = Color.RoyalBlue;

                label2.Text = "Student Attendance";
                admin_security1.Visible = false;
                student stud = new student();
                stud.Show();
                this.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            admin_security1.Visible = true;
            label2.Text = "Admin Security";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
