using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace II1
{
    public partial class Form1 : Form
    {
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier(@"C:\Users\vasil\source\repos\II1\II1\haarcascade_frontalface_alt_tree.xml");
        static readonly CascadeClassifier cascadeClassifierEye = new CascadeClassifier(@"C:\Users\vasil\source\repos\II1\II1\haarcascade_eye_tree_eyeglasses.xml");
        FilterInfoCollection filter;
        VideoCaptureDevice device;

       

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }



        private void button1_Click(object sender, EventArgs e)
        {
            device = new VideoCaptureDevice(filter[cboDevice.SelectedIndex].MonikerString);
            device.NewFrame += Device_NewFrame;
            device.Start();
        }

        private void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap);
            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.2, 1);
            foreach(Rectangle rectangle in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Red, 1))
                    {
                        graphics.DrawRectangle(pen, rectangle);
                    }
                }
                Rectangle[]recEyes = cascadeClassifierEye.DetectMultiScale(grayImage,1.2,1);
                foreach(var rec in recEyes)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Pen pen = new Pen(Color.Blue, 1))
                        {
                            graphics.DrawRectangle(pen, rec);
                        }
                    }
                }
                pictureBox1.Image = bitmap;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filter)
            {
                cboDevice.Items.Add(device.Name);
            }
            cboDevice.SelectedIndex = 0;
            device = new VideoCaptureDevice();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (device.IsRunning)
            {
                device.Stop();
            }
        }
    }
}
