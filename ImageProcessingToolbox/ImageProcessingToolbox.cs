using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace PictureViewer
{
    public partial class ImageProcessingToolbox : Form
    {
        public ImageProcessingToolbox()
        {
            InitializeComponent();
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user clicks OK, load the 
            // picture that the user chose. 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Clear the picture.
            pictureBox1.Image = null;
        }
                
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
                
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Refresh();
        }

        private void executeFilterButton_Click(object sender, EventArgs e)
        {
            //Emgu.CV.Image<Hls, Byte> imageHsi = new Image<Hls, Byte>(pictureBox1.Image.Bitmap);
            Bitmap pBoxImage = new Bitmap(pictureBox1.Image);
            Image<Bgr, byte> inputImage = new Image<Bgr, Byte>(pBoxImage);
            
            //Image<Gray, byte> grayImage = inputImage.Convert<Gray, byte>();
            //grayImage._SmoothGaussian(5);
            inputImage._SmoothGaussian(25);
            //pictureBox1.Image = grayImage.ToBitmap();
            pictureBox1.Image = inputImage.ToBitmap();

        }

        
    }
}
