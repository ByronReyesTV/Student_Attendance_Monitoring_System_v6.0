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
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class student : Form
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True");

        public student()
        {
            InitializeComponent();
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice captureDevice;

        SqlCommand cmd;

        private void student_Load(object sender, EventArgs e)
        {
            attendance_record1.Visible = false;

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                comboBox1.Items.Add(filterInfo.Name);
            comboBox1.SelectedIndex = 0;

            captureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void student_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning)
                captureDevice.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pictureBox2.Image);
                if (result != null)
                {
                    label11.Text = result.ToString();
                    timer1.Stop();
                    if (captureDevice.IsRunning)
                        captureDevice.Stop();

                    // DataBase search

                    conn.Open();
                    string sqlQuery = "SELECT first_name, middle_name, last_name, section, contact_number, picture FROM tb_student_records WHERE student_number = '" + label11.Text + "'";
                    cmd = new SqlCommand(sqlQuery, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows)
                    {
                        label12.Text = reader[0].ToString();
                        label13.Text = reader[1].ToString();
                        label14.Text = reader[2].ToString();
                        label15.Text = reader[3].ToString();
                        label16.Text = reader[4].ToString();
                        byte[] images = (byte[])reader[5];

                        // scanner page hide

                        pictureBox2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        comboBox1.Visible = false;

                        // student details view

                        label5.Text = "Student Numeber:";
                        label6.Text = "First Name:";
                        label7.Text = "Middle Name:";
                        label8.Text = "last Name:";
                        label9.Text = "Section:";
                        label10.Text = "Parent/Guardian Contact Number:";
                        
                        pictureBox3.Visible = true;

                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = true;
                        label8.Visible = true;
                        label9.Visible = true;
                        label10.Visible = true;

                        label11.Visible = true;
                        label12.Visible = true;
                        label13.Visible = true;
                        label14.Visible = true;
                        label15.Visible = true;
                        label16.Visible = true;

                        if (images == null)
                        {
                            pictureBox3.Image = null;
                        }
                        else
                        {
                            MemoryStream mstrem = new MemoryStream(images);
                            pictureBox3.Image = Image.FromStream(mstrem);
                        }

                        conn.Close();

                        conn.Open();
                        string check = @"(SELECT COUNT (*) FROM tb_student_attendance WHERE student_number = '" + label11.Text + "')";
                        string SqlQuery = "INSERT INTO tb_student_attendance (student_number, first_name, middle_name, last_name, contact_number, time_in, time_out, date) VALUES ('" + label11.Text + "', '" + label12.Text + "', '" + label13.Text + "', '" + label14.Text + "', '" + label16.Text + "', '" + DateTime.Now.ToString("t") + "', '" + "    " + "', '" + DateTime.Now.ToString("d") + "')";
                        SqlCommand cmd2 = new SqlCommand(check, conn);
                        cmd = new SqlCommand(SqlQuery, conn);
                        int count = (int)cmd2.ExecuteScalar();
                        conn.Close();

                        if (count > 0)
                        {

                            label16.Visible = false;
                            label18.Visible = false;

                            //1. address of SQL server and database
                            String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True";

                            //2. establish connection
                            SqlConnection con = new SqlConnection(ConnectionString);

                            //3. open connection
                            con.Open();

                            //4. prepare query
                            string Query = "UPDATE tb_student_attendance SET time_out = '" + DateTime.Now.ToString("t") + "' WHERE student_number = " + label11.Text;

                            //5. execute query
                            SqlCommand cmd = new SqlCommand(Query, con);
                            cmd.ExecuteNonQuery();

                            //6. close connection
                            con.Close();

                            label16.Visible = true;

                            label17.Text = "Time out:";
                            label17.Visible = true;

                            label18.Text = DateTime.Now.ToString("t");
                            label18.Visible = true;

                            label19.Text = "Date:";
                            label19.Visible = true;

                            label22.Text = DateTime.Now.ToString("d");
                            label22.Visible = true;

                            MessageBox.Show("time out recorded");

                            
                            //Find your Account SID and Auth Token at twilio.com / console
                            // and set the environment variables. See http://twil.io/secure
                            string accountSid = Environment.GetEnvironmentVariable("Twilio SID");
                            string authToken = Environment.GetEnvironmentVariable("Twilio Auth Token");

                            TwilioClient.Init(accountSid, authToken);

                            var message = MessageResource.Create(
                                body: "Student" + " " + label12.Text + " " + label13.Text + " " + label14.Text + "\n" + "has departed in the School at" + " " + DateTime.Now.ToString("t") + "\n" + "Date: " + DateTime.Now.ToString("D") + "\n \n" + "Auto Generated Text by: School Attendance Monitoring System v5.0",
                                from: new Twilio.Types.PhoneNumber("+18124322746"),
                                to: new Twilio.Types.PhoneNumber("+639271899042") // alea
                            );

                            Console.WriteLine(message.Sid);

                            // to: new Twilio.Types.PhoneNumber("+639271899042") //byron
                            // to: new Twilio.Types.PhoneNumber("+639694987431") //johnny
                            // to: new Twilio.Types.PhoneNumber("+639154957894") //jaymar

                        }
                        else
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();

                            label17.Text = "Time in:";
                            label17.Visible = true;

                            label18.Text = DateTime.Now.ToString("t");
                            label18.Visible = true;

                            label19.Text = DateTime.Now.ToString("d");
                            label19.Visible = true; label19.Text = "Date:";
                            label19.Visible = true;

                            label22.Text = DateTime.Now.ToString("d");
                            label22.Visible = true;

                            MessageBox.Show("time in recorded");

                            conn.Close();

                            
                            //Find your Account SID and Auth Token at twilio.com / console
                            // and set the environment variables. See http://twil.io/secure
                            string accountSid = Environment.GetEnvironmentVariable("Twilio SID");
                            string authToken = Environment.GetEnvironmentVariable("Twilio Auth Token");

                            TwilioClient.Init(accountSid, authToken);

                            var message = MessageResource.Create(
                                body: "Student" + " " + label12.Text + " " + label13.Text + " " + label14.Text + "\n" + "has arrived in the School at" + " " + DateTime.Now.ToString("t") + "\n" + "Date: " + DateTime.Now.ToString("D") + "\n \n" + "Auto Generated Text by: School Attendance Monitoring System v5.0",
                                from: new Twilio.Types.PhoneNumber("+18124322746"),
                                to: new Twilio.Types.PhoneNumber("+639271899042") // alea
                            );

                            Console.WriteLine(message.Sid);

                            // to: new Twilio.Types.PhoneNumber("+639271899042") //byron
                            // to: new Twilio.Types.PhoneNumber("+639694987431") //johnny
                            // to: new Twilio.Types.PhoneNumber("+639154957894") //jaymar



                        }
                    }
                    else
                    {
                        // scanner page hide

                        pictureBox2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        comboBox1.Visible = false;

                        // student details

                        label5.Text = "Student Numeber:";
                        label6.Text = "First Name:";
                        label7.Text = "Middle Name:";
                        label8.Text = "last Name:";
                        label9.Text = "Section:";
                        label10.Text = "Parent/Guardian Contact Number:";

                        pictureBox3.Visible = true;

                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = true;
                        label8.Visible = true;
                        label9.Visible = true;
                        label10.Visible = true;

                        label11.Visible = false;
                        label12.Visible = false;
                        label13.Visible = false;
                        label14.Visible = false;
                        label15.Visible = false;
                        label16.Visible = false;

                        MessageBox.Show("no record");

                        Form1 frm = new Form1();
                        frm.Show();
                        this.Hide();
                    }

                    conn.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            captureDevice.Stop();
            attendance_record1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            captureDevice.Stop();

            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            captureDevice.Stop();

            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }
    }
}
