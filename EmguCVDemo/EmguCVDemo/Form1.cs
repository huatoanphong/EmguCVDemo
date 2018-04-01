using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCVDemo
{
    public partial class frmHome : Form
    {
        Image<Bgr, byte> _imgInput;
        Image<Bgr, byte> imgDraw;
        Point startPoint;
        Point endPoint;
        bool isSetA = false;
        bool isSetB = false;
        int numClick = 0;
        int valuePointX = 0;
        int valuePointY = 0;
        public frmHome()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _imgInput = new Image<Bgr, byte>(ofd.FileName);
                imgbInput.Image = _imgInput;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imgbInput_MouseMove(object sender, MouseEventArgs e)
        {

            if (_imgInput == null)
            {
                return;
            }
            else
            {
                int offsetX = (int)(e.Location.X / imgbInput.ZoomScale);
                int offsetY = (int)(e.Location.Y / imgbInput.ZoomScale);
                int horizontalScrollBarValue = imgbInput.HorizontalScrollBar.Visible ? (int)imgbInput.HorizontalScrollBar.Value : 0;
                int verticalScrollBarValue = imgbInput.VerticalScrollBar.Visible ? (int)imgbInput.VerticalScrollBar.Value : 0;
                valuePointX = offsetX + horizontalScrollBarValue;
                valuePointY = offsetY + verticalScrollBarValue;
                if (isSetA == false)
                {
                    txtAxPos.Text = valuePointX.ToString();
                    txtAyPos.Text = valuePointY.ToString();
                }
                else if (isSetB == false)
                {
                    txtBxPos.Text = valuePointX.ToString();
                    txtByPos.Text = valuePointY.ToString();
                }
            }
        }

        private void imgbInput_MouseClick(object sender, MouseEventArgs e)
        {
            switch (numClick)
            {
                case 0:
                    isSetA = true;
                    imgDraw  = new Image<Bgr, byte>(_imgInput.Bitmap);
                    startPoint = new Point(valuePointX, valuePointY);
                    CvInvoke.Circle(imgDraw, startPoint, 1, new MCvScalar(255, 0, 0), 2, Emgu.CV.CvEnum.LineType.EightConnected, 0);
                    numClick++;
                    imgbInput.Image = imgDraw;
                    break;
                case 1:
                    endPoint = new Point(valuePointX, valuePointY);
                    CvInvoke.Circle(imgDraw, endPoint, 1, new MCvScalar(255, 0, 0), 2, Emgu.CV.CvEnum.LineType.EightConnected, 0);
                    CvInvoke.Line(imgDraw, startPoint, endPoint, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);
                    numClick = 0;
                    txtDistance.Text = Distance().ToString();
                    imgbInput.Image = imgDraw;
                    isSetA = false;
                    isSetB = false;
                    break;
                default:
                    break;
            }
        }
        public double Distance()
        {
            int Ax = Convert.ToInt32(txtAxPos.Text);
            int Bx = Convert.ToInt32(txtBxPos.Text);
            int Ay = Convert.ToInt32(txtAyPos.Text);
            int By = Convert.ToInt32(txtByPos.Text);

            double tempX = Math.Pow((Math.Abs(Ax - Bx)), 2);
            double tempY = Math.Pow((Math.Abs(Ay - By)), 2);
            double result = Math.Sqrt(tempX + tempY);
            return result;
        }
    }
}
