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
using Emgu.CV.CvEnum;

namespace PictureViewer
{
    public partial class ImageProcessingToolbox : Form
    {
        int gaussKernel;
        bool colorFlag = true;

        string[] filterList = { "Gaussian", "HistEqual", "MorphClose", "MorphCloseB&W","Monochrome","Canny","Laplacian","Sobel",
                                  "SmoothBilatral","ThresholdAdaptive","SmoothMedian","CLAHE"};

        public ImageProcessingToolbox()
        {
            InitializeComponent();
            foreach (string s in filterList)
            {
                filtersComboBox.Items.Add(s);
            }
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
            try
            {
                Image<Bgr, byte> inputImage = new Image<Bgr, Byte>(pBoxImage);
                
                //Image<Gray, byte> grayImage = inputImage.Convert<Gray, byte>();

                // Kernel must be an odd number
                if (gaussKernel % 2 == 0)
                {
                    gaussKernel--;
                    textBox1.Text = gaussKernel.ToString();
                }

                inputImage._SmoothGaussian(gaussKernel);
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox2.Image = inputImage.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            

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
        

        private void runFilterButton_Click(object sender, EventArgs e)
        {
            Bitmap pBoxImage;
            Image<Bgr, byte> inputImage;
            Image<Gray, Byte> grayImage;
            
            try
            {
                pBoxImage = new Bitmap(pictureBox1.Image);
                inputImage = new Image<Bgr, Byte>(pBoxImage);
                grayImage = inputImage.Convert<Gray, Byte>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }  

            switch (filtersComboBox.SelectedIndex)
            {
                case 0:         //Gaussian
                    colorFlag = true;
                    filterLabel1.Visible = false;
                    int otherGaussKernal = 5;
                    if (otherGaussKernal % 2 == 0)
                    {
                        gaussKernel--;
                        //textBox1.Text = otherGaussKernal.ToString();
                    }
                    inputImage._SmoothGaussian(otherGaussKernal);
                    break;
                case 1:         //Histogram Equalization
                    colorFlag = true;
                    inputImage._EqualizeHist();
                    break;
                case 2:         //MorphClose
                    colorFlag = true;
                    Mat kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(15, 15), new Point(1, 1));
                    Emgu.CV.CvEnum.MorphOp operation = Emgu.CV.CvEnum.MorphOp.Close;
                    Point anchor = new Point(-1,-1);
                    int iterations = 1;
                    Emgu.CV.CvEnum.BorderType borderType = Emgu.CV.CvEnum.BorderType.Default;
                    Emgu.CV.Structure.MCvScalar borderValue = new MCvScalar();
                    inputImage._MorphologyEx(operation, kernel, anchor, iterations, borderType, borderValue);
                    break;
                case 3:         //MorphCloseB&W
                    colorFlag = false;
                    //Image<Gray, Byte> grayImage = inputImage.Convert<Gray, Byte>();
                    Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(15, 15), new Point(1, 1));
                    Emgu.CV.CvEnum.MorphOp operation1 = Emgu.CV.CvEnum.MorphOp.Close;
                    Point anchor1 = new Point(-1,-1);
                    int iterations1 = 1;
                    Emgu.CV.CvEnum.BorderType borderType1 = Emgu.CV.CvEnum.BorderType.Default;
                    Emgu.CV.Structure.MCvScalar borderValue1 = new MCvScalar();
                    grayImage._MorphologyEx(operation1, kernel1, anchor1, iterations1, borderType1, borderValue1);                    
                    break;
                case 4:         //Monochrome
                    colorFlag = false;
                    break;
                case 5:         //Canny
                    colorFlag = false;
                    double cannyThresh = 10;
                    double cannyThreshLinking = 500;
                    grayImage = grayImage.Canny(cannyThresh, cannyThreshLinking);
                    break;
                case 6:         //Laplace
                    colorFlag = false;
                    int laplaceApertureSize = 1;
                    //grayImage = grayImage.Laplace(apertureSize);
                    //grayImage.Laplace(apertureSize).ConvertFrom();
                    Image<Gray, float> cannyGrayImageFloat = grayImage.Laplace(laplaceApertureSize);
                    grayImage = cannyGrayImageFloat.Convert<Gray, Byte>();
                    break;
                case 7:         //Sobel
                    colorFlag = false;
                    int sobelXOrder = 1;
                    int sobelYOrder = 1;
                    int sobelApertureSize = 5;
                    Image<Gray, float> sobelGrayImageFloat = grayImage.Sobel(sobelXOrder,sobelYOrder,sobelApertureSize);
                    grayImage = sobelGrayImageFloat.Convert<Gray, Byte>();
                    break;
                case 8:         //SmoothBilatral
                    colorFlag = true;
                    int kernelSize = 25;
                    int colorSigma = 55;
                    int spaceSigma = 55;
                    inputImage = inputImage.SmoothBilatral(kernelSize, colorSigma, spaceSigma);
                    break;
                case 9:         //ThresholdAdaptive 
                    colorFlag = false;
                    Emgu.CV.Structure.Gray maxValue = new Gray(150);
                    AdaptiveThresholdType adaptiveType = AdaptiveThresholdType.GaussianC;
                    ThresholdType thresholdType = ThresholdType.Binary;
                    int blockSize = 3;
                    Emgu.CV.Structure.Gray param1 = new Gray();
                    grayImage = grayImage.ThresholdAdaptive(maxValue, adaptiveType, thresholdType, blockSize, param1);
                    break;
                case 10:         //SmoothMedian
                    colorFlag = true;
                    int smoothMedianSize = 25;
                    inputImage = inputImage.SmoothMedian(smoothMedianSize);
                    break;
                case 11:         //CLAHE
                    colorFlag = true;
                    IInputArray CLAHEsrc = inputImage.GetOutputArray();
                    double CLAHEClipLimit = 40;
                    Size CLAHETileGridSize = new System.Drawing.Size(8, 8);
                    OutputArray CLAHEdst;
                    Emgu.CV.CvInvoke.CLAHE(CLAHEsrc, CLAHEClipLimit, CLAHETileGridSize, CLAHEdst);
                    break;
                    


                default:
                    MessageBox.Show("No filter selected, or failure occured");
                    break;
            }

            filterPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            if (colorFlag)
            {
                filterPictureBox.Image = inputImage.ToBitmap();
            }
            else
            {
                filterPictureBox.Image = grayImage.ToBitmap();
            }
            
            
        }
    }
}
