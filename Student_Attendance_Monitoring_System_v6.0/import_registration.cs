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
using ZXing;
using AForge.Video.DirectShow;
using System.IO;
using AForge.Video;
using QRCoder;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class import_registration : UserControl
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True");
        public SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=school_attendance_systemV50DB;Integrated Security=True");

        public import_registration()
        {
            InitializeComponent();
        }

        string imglocation = "";
        SqlCommand cmd;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|ALL files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imglocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imglocation;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("no record");
            }
            else
            {
                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("Upload image");
                }
                else
                {
                    byte[] images = null;
                    FileStream strem = new FileStream(imglocation, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(strem);
                    images = br.ReadBytes((int)strem.Length);

                    conn.Open();
                    string check = @"(SELECT COUNT (*) FROM tb_student_records WHERE student_number = '" + textBox1.Text + "')";
                    string sqlQuery = "INSERT INTO tb_student_records (student_number, first_name, middle_name, last_name, section, contact_number, picture) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', @picture)";
                    SqlCommand cmd2 = new SqlCommand(check, conn);
                    cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.Add(new SqlParameter("@picture", images));
                    int count = (int)cmd2.ExecuteScalar();
                    conn.Close();

                    if (count > 0)
                    {
                        MessageBox.Show("Student Already Registered in the system", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Student Registered Successfully", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();

                        QRCodeGenerator qr = new QRCodeGenerator();
                        QRCodeData data = qr.CreateQrCode(textBox1.Text, QRCodeGenerator.ECCLevel.Q);
                        QRCode code = new QRCode(data);
                        pictureBox2.Image = code.GetGraphic(5);

                        string initialDIR = @"C:\Users\63916\Desktop\Computerized_Student_Attendance_v5.0\student_qr_code";
                        var dialog = new SaveFileDialog();
                        dialog.InitialDirectory = initialDIR;
                        dialog.FileName = textBox1.Text + "_" + textBox2.Text + "_" + textBox4.Text + "_" + textBox5.Text;
                        dialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.gif";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            pictureBox2.Image.Save(dialog.FileName);
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Enter student nnumber");
            }
            else
            {
                if (textBox1.Text.Length < 11)
                {
                    MessageBox.Show("Invalid student number");
                    textBox1.Clear();
                }
                else
                {
                    if (textBox1.Text.Length == 11)
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
                            textBox1.Clear();
                        }

                        con.Close();
                    }
                }
            }
        }
    }
}
