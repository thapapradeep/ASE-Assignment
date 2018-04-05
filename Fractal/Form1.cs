using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal
{
    public partial class Form1 : Form
    {
        private  int MAX = 256;      // max iterations
        private  double SX = -2.025; // start value real
        private  double SY = -1.125; // start value imaginary
        private  double EX = 0.6;    // end value real
        private  double EY = 1.125;  // end value imaginary
        private static int x1, y1, xs, ys, xe, ye;
        private static double xstart, ystart, xende, yende, xzoom, yzoom;
        private static float xy;
        private Bitmap picture;
        private Graphics g1, obj;
        private HSB HSBcol = new HSB();
        private Pen pen;
        private bool clicked;
        private bool isNew , first, onn;
        private int num1, num2, num3, num4, value,timerInt;



        private static bool action, rectangle, finished;

     

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fn = new Form1();
            fn.Show(); 
        }

        private void startCyclingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void stopCyclingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RandomNum();
            value = 1;
            mandelbrot();
            update();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult messagebox = MessageBox.Show("Are you sure ?", "Exit", MessageBoxButtons.YesNo);
            if (messagebox == DialogResult.Yes)
            {
                this.Close();
            }
    
        }
       

        public Form1()
        {
            InitializeComponent();
            init();
            start();

        }

       

      

       
        public void init()
        {
            finished = false;
            x1 = pictureBox1.Width;
            y1 = pictureBox1.Height;
            xy = (float)x1 / (float)y1;
            picture = new Bitmap(x1, y1);
            g1 = Graphics.FromImage(picture);
            finished = true;
            clicked = false;
            isNew = true;
            first = true;
            value = 0;
            onn = true;
            timerInt = 0;
        }

        private void changeColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerInt++;
            RandomNum();
            value = 1;
            mandelbrot();
            update();
        }

        public void start()
        {
            action = false;
            rectangle = false;
            initvalues();
            //RandomNumber();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();
            
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.DefaultExt = "jpg";
            sv.Filter = "JPG images (*.jpg)|*.jpg";
            if (sv.ShowDialog() == DialogResult.OK)
            {
             
                pictureBox1.DrawToBitmap(picture, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
               
                picture.Save(sv.FileName, ImageFormat.Jpeg);
                
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            obj = e.Graphics;
            obj.DrawImage(picture, new Point(0, 0));
            

        }
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initvalues();
            start();
            value = 0;
            mandelbrot();
            update();
            
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pictureBox1.Hide();
            editToolStripMenuItem.Enabled = false;
            cloneToolStripMenuItem.Enabled = false;
            restartToolStripMenuItem.Enabled = false;
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Show();
            editToolStripMenuItem.Enabled = true;
            cloneToolStripMenuItem.Enabled = true;
            restartToolStripMenuItem.Enabled = true;
        }


        private void RandomNum()
        {
            Random ran = new Random();
            num1 = ran.Next(255);
            num2= ran.Next(255);
            num3= ran.Next(255);

        }
        private void mandelbrot() // calculate all points
        {
            int x, y;
            float h, b, alt = 0.0f;
            
            action = false;
            //Dispalying message in text box before mandelbrot is ready
            info.Text = "Mandelbrot-Set will be produced - please wait...";
            info.Enabled = false;
            for (x=0 ; x < x1; x += 2)
            {
                for (y = 0; y < y1; y++)
                {

                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value

                    if (h != alt)
                    {
                        b = 1.0f - h * h; // brightnes
                                          ///djm added
                      
                        
                            HSBcol.fromHSB(h * 255, 0.8f * 255, b * 255, value, num1, num2, num3); //convert hsb to rgb then make a Java Color
                        
                        Color col = Color.FromArgb((int)HSBcol.rChan, (int)HSBcol.gChan, (int)HSBcol.bChan);
                        pen = new Pen(col);
                        
                        alt = h;
                    }

                    g1.DrawLine(pen, x, y, x + 1, y);
                    
                }
                
            }
            //Displaying message after mandlebrot is ready
            Cursor.Current = Cursors.Cross;
            info.Text = "Mandelbrot-Set ready - please select zoom area with pressed mouse.";
            info.Enabled=false;
            action = true;
        }

        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            //Reading the values from saved text file
            if (isNew)
            {
                StreamReader sr = new StreamReader("property.txt");
                String line = "";
                int counter = 0;
                string[] array = new String[7];
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    array[counter - 1] = line;
                }
                sr.Close();
                xstart = Double.Parse(array[0]);
                ystart = Double.Parse(array[1]);
                xende = Double.Parse(array[2]);
                yende = Double.Parse(array[3]);
                num1 = int.Parse(array[4]);
                num2 = int.Parse(array[5]);
                num3 = int.Parse(array[6]);
                value = 1;
                isNew = false;
            }
            else
            {
                 xstart = SX;
                 ystart = SY;
                 xende = EX;
                 yende = EY;
            }
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }
        public void destroy() // delete all instances 
        {
            if (finished)
            {
              
                picture = null;
                g1 = null;
                
            }
        }
        public void update()
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(picture, 0, 0);
            if (rectangle)
            {
            
                Pen mypen = new Pen(Color.White, 1);
                if (xs < xe)
                {
                    if (ys < ye) g.DrawRectangle(mypen,xs, ys, (xe - xs), (ye - ys));
                    else g.DrawRectangle(mypen,xs, ye, (xe - xs), (ys - ye));
                }
                else
                {
                    if (ys < ye) g.DrawRectangle(mypen,xe, ys, (xs - xe), (ye - ys));
                    else g.DrawRectangle(mypen,xe, ye, (xs - xe), (ys - ye));
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            clicked = true;
            {
                action = true;
                if (action)
                {
                    xs = e.X;
                    ys = e.Y;
                }
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (clicked)
            {
                if (action)
                {
                    xe = e.X;
                    ye = e.Y;
                    rectangle = true;
                    update();

                }
           }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int z, w;
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                if (xs > xe)
                {
                    z = xs;
                    xs = xe;
                    xe = z;
                }
                if (ys > ye)
                {
                    z = ys;
                    ys = ye;
                    ye = z;
                }
                w = (xe - xs);
                z = (ye - ys);
                if ((w < 2) && (z < 2)) initvalues();



                else
                {
                    if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
                    else xe = (int)((float)xs + (float)z * xy);
                    xende = xstart + xzoom * (double)xe;
                    yende = ystart + yzoom * (double)ye;
                    xstart += xzoom * (double)xs;
                    ystart += yzoom * (double)ys;
                }
                xzoom = (xende - xstart) / (double)x1;
                yzoom = (yende - ystart) / (double)y1;
                mandelbrot();
                rectangle = false;
                clicked = false;
                //isNew = false;
                update();
                StreamWriter File = new StreamWriter("property.txt");
                File.Write(xstart +Environment.NewLine);
                File.Write(ystart  + Environment.NewLine);
                File.Write(xende + Environment.NewLine);
                File.Write(yende+ Environment.NewLine);
                File.Write(num1 + Environment.NewLine);
                File.Write(num2 + Environment.NewLine);
                File.Write(num3);
                
                File.Close();
                Console.WriteLine(num1 + " " + num2 + " " + num3);
            }
        }
       
    }
}
