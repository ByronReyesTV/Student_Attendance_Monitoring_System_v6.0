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
using ZXing;
using QRCoder;
using System.Drawing.Printing;

namespace Student_Attendance_Monitoring_System_v6._0
{
    public partial class qr_print : UserControl
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-JSNHHF1\SQLEXPRESS;Initial Catalog=Attendance_Monitoring_System_v40;Integrated Security=True");

        public qr_print()
        {
            InitializeComponent();
        }

        SqlCommand cmd;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("enter student number");
            }
            else
            {
                if (textBox1.Text.Length < 11)
                {
                    MessageBox.Show("invalid student number");
                }
                else
                {
                    if (textBox1.Text.Length == 11)
                    {
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
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("search your record first");
            }
            else
            {
                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(textBox1.Text, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);
                pictureBox1.Image = code.GetGraphic(5);

                PrintDialog pd = new PrintDialog();
                PrintDocument pdoc = new PrintDocument();
                pdoc.DocumentName = textBox1.Text + "-" + textBox2.Text + "-" + textBox4.Text + "-" + "(Student Copy)";
                pdoc.PrintPage += PrintPicture;
                pd.Document = pdoc;
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    pdoc.Print();
                }
            }
        }

        private void PrintPicture(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            e.Graphics.DrawImage(bmp, 0, 0);
            bmp.Dispose();
        }
    }
}
