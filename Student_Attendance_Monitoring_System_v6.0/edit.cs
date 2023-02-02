using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using QRCoder;
using System.IO;
using System.Drawing.Printing;
using System.Data.SqlClient;
using ZXing;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class edit : UserControl
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True");
        public SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=school_attendance_systemV50DB;Integrated Security=True");

        public edit()
        {
            InitializeComponent();
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice captureDevice;

        SqlCommand cmd;

        private void edit_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                comboBox1.Items.Add(filterInfo.Name);
            comboBox1.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pictureBox2.Image);
                if (result != null)
                {
                    textBox1.Text = result.ToString();
                    timer1.Stop();
                    if (captureDevice.IsRunning)
                    {
                        captureDevice.Stop();
                    }

                    conn.Open();
                    string sqlQuery = "SELECT first_name, middle_name, last_name, section, contact_number, picture FROM tb_student_records WHERE student_number = '" + textBox1.Text + "'";
                    cmd = new SqlCommand(sqlQuery, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();


                    if (reader.HasRows)
                    {
                        textBox2.Text = reader[0].ToString();
                        textBox3.Text = reader[1].ToString();
                        textBox4.Text = reader[2].ToString();
                        textBox5.Text = reader[3].ToString();
                        textBox6.Text = reader[4].ToString();
                        byte[] images = (byte[])reader[5];

                        if (images == null)
                        {
                            pictureBox1.Image = null;
                        }
                        else
                        {
                            MemoryStream mstrem = new MemoryStream(images);
                            pictureBox1.Image = Image.FromStream(mstrem);
                        }
                    }
                    else
                    {
                        MessageBox.Show("no record");

                        captureDevice.Stop();
                        pictureBox2.Image = null;
                        textBox1.Clear();
                    }

                    conn.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void PrintPicture(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.DrawToBitmap(bmp, new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height));
            e.Graphics.DrawImage(bmp, 0, 0);
            bmp.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                textBox1.Focus();
            }
            else
            {
                if (textBox2.Text.Length == 0)
                {
                    textBox2.Focus();
                }
                else
                {
                    if (textBox3.Text.Length == 0)
                    {
                        textBox3.Focus();
                    }
                    else
                    {
                        if (textBox4.Text.Length == 0)
                        {
                            textBox4.Focus();
                        }
                        else
                        {
                            if (textBox5.Text.Length == 0)
                            {
                                textBox5.Focus();
                            }
                            else
                            {
                                if (textBox6.Text.Length == 0)
                                {
                                    textBox6.Focus();
                                }
                                else
                                {
                                    if (pictureBox1.Image == null)
                                    {
                                        MessageBox.Show("Upload Image");
                                    }
                                    else
                                    {
                                        //1. address of SQL server and database (Connection String)
                                        String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True";

                                        //2. establish connection (c# sqlconnection class)
                                        SqlConnection con = new SqlConnection(ConnectionString);

                                        //3. open connection (c# sqlconnection open)
                                        con.Open();

                                        //4. prepare query
                                        string studentnumber = textBox1.Text;

                                        string Query = "UPDATE tb_student_records SET first_name = '" + textBox2.Text + "', middle_name = '" + textBox3.Text + "', last_name = '" + textBox4.Text + "', section = '" + textBox5.Text + "', contact_number = '" + textBox6.Text + "' WHERE student_number = " + studentnumber;
                                        SqlCommand cmd = new SqlCommand(Query, con);

                                        //5. execute query (c# sqlcommand class)
                                        var reader = cmd.ExecuteNonQuery();

                                        MessageBox.Show("Record Updated successfully", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        pictureBox1.Image = null;
                                        pictureBox2.Image = null;

                                        textBox1.Clear();
                                        textBox2.Clear();
                                        textBox3.Clear();
                                        textBox4.Clear();
                                        textBox5.Clear();
                                        textBox6.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                textBox1.Focus();
            }
            else
            {
                if (textBox2.Text.Length == 0)
                {
                    textBox2.Focus();
                }
                else
                {
                    if (textBox3.Text.Length == 0)
                    {
                        textBox3.Focus();
                    }
                    else
                    {
                        if (textBox4.Text.Length == 0)
                        {
                            textBox4.Focus();
                        }
                        else
                        {
                            if (textBox5.Text.Length == 0)
                            {
                                textBox5.Focus();
                            }
                            else
                            {
                                if (textBox6.Text.Length == 0)
                                {
                                    textBox6.Focus();
                                }
                                else
                                {
                                    //1. address of SQL server and database (Connection String)
                                    String ConnectionString = "Data Source=DESKTOP-JSNHHF1\\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True";

                                    //2. establish connection (c# sqlconnection class)
                                    SqlConnection con = new SqlConnection(ConnectionString);

                                    //3. open connection (c# sqlconnection open)
                                    con.Open();

                                    //4. prepare query
                                    string studentnumber = textBox1.Text;

                                    string Query = "DELETE FROM tb_student_records WHERE student_number = " + studentnumber;
                                    SqlCommand cmd = new SqlCommand(Query, con);

                                    //5. execute query (c# sqlcommand class)
                                    var reader = cmd.ExecuteNonQuery();

                                    MessageBox.Show("Record deleted successufully");

                                    pictureBox1.Image = null;
                                    pictureBox2.Image = null;

                                    textBox1.Clear();
                                    textBox2.Clear();
                                    textBox3.Clear();
                                    textBox4.Clear();
                                    textBox5.Clear();
                                    textBox6.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
