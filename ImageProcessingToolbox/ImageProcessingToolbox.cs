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
        int gaussKernel;
        
           

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
            gaussKernel = 5;

        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Refresh();
        }

        private void runFilter2Button_Click(object sender, EventArgs e)
        {
            Bitmap pBoxImage = new Bitmap(pictureBox1.Image);
            Image<Bgr, byte> inputImage = new Image<Bgr, Byte>(pBoxImage);

            //Image<Gray, byte> grayImage = inputImage.Convert<Gray, byte>();
            
            // Kernel must be an odd number
            if (gaussKernel%2 == 0)
            {
                gaussKernel--;
                textBox1.Text = gaussKernel.ToString();
            }

            inputImage._SmoothGaussian(gaussKernel);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = inputImage.ToBitmap();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int tryParseResult;
            if (int.TryParse(textBox1.Text, out tryParseResult))
            {
                gaussKernel = tryParseResult;
            }
            else
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void runFilter3Button_Click(object sender, EventArgs e)
        {
            Bitmap pBoxImage = new Bitmap(pictureBox1.Image);
            Image<Bgr, byte> inputImage = new Image<Bgr, Byte>(pBoxImage);
            inputImage._EqualizeHist();
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Image = inputImage.ToBitmap();

        }
    }
}
