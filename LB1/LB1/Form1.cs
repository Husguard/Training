using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB1
{
    public partial class Form1 : Form
    {
        PointF RotatePoint = new PointF(150, 100);
        Line RotateLine = new Line(new PointF(100, 100), new PointF(200, 200), new Pen(Color.Blue, 2));
        public Form1()
        {
            Graphics gr = this.CreateGraphics();
            InitializeComponent();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {      
            RotateLine.View(e.Graphics, RotatePoint);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Graphics gr = this.CreateGraphics();
            RotateLine.Rotate(15, RotatePoint);
            gr.Clear(Color.White);
            RotateLine.View(gr, RotatePoint);
        }

        private void lB1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f2 = new Form1();
            f2.Show();
            this.Hide();
        }

        private void lB2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f2 = new Form2();
            f2.ShowDialog();
            this.Hide();
        }
    }

    class Line
    {
        private PointF Start { get; set; }
        private PointF End { get; set; }
        public int Length { get; set; }
        private Pen Style { get; set; }
        public int Angle { get; set; }
        public Line(PointF start, PointF end, Pen style)
        {
            Start = start;
            End = end;
            Style = style;
        }
        public void View(Graphics gr, PointF RP)
        {
            gr.DrawLine(Style, Start, End);
            gr.FillRectangle(Brushes.Black, RP.X, RP.Y, 3, 3);
        }
        public void Rotate(double angle, PointF RotatePoint)
        {
            angle = angle * Math.PI / 180;
            float X = (float) ((End.X - RotatePoint.X) * Math.Cos(angle) - (End.Y - RotatePoint.Y) * Math.Sin(angle) + RotatePoint.X);
            float Y = (float) ((End.X - RotatePoint.X) * Math.Sin(angle) + (End.Y - RotatePoint.Y) * Math.Cos(angle) + RotatePoint.Y);
            End = new PointF(X, Y);

            X = (float)((Start.X - RotatePoint.X) * Math.Cos(angle) - (Start.Y - RotatePoint.Y) * Math.Sin(angle) + RotatePoint.X);
            Y = (float)((Start.X - RotatePoint.X) * Math.Sin(angle) + (Start.Y - RotatePoint.Y) * Math.Cos(angle) + RotatePoint.Y);
            Start = new PointF(X,Y);
        }
    }
}
/* private void Form1_Paint(object sender, PaintEventArgs e)
        {
            RectangleF[] each = new RectangleF[20];
            for (int i = 10; i < predel; i += 10)
            {
                int angle = 2;
                PointF point = RotateRectangle(angle, i);
                each[(i / 10) - 1] = (new RectangleF(point.X, point.Y, 2 * (predel - i), 2 * (predel - i)));

                angle += 2;
                e.Graphics.DrawRectangles(new Pen(Color.Blue, 2), each);
                Console.WriteLine(each[(i / 10) - 1]);
                Console.Write(" {0}", (i / 10) - 1);
    //            e.Graphics.RotateTransform(2);              
            }
            // несмотря на поворот координаты остаются те же
            // свой прямоугольник с регулируемыми 4мя вершинами
    //        e.Graphics.RotateTransform(320);
            
            e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 15, 15, 15, 15);
        }
        public PointF RotateRectangle(double angle, int i)
        {
            angle = angle * Math.PI / 180;
            float X = (float)((i) * Math.Cos(angle) - (i) * Math.Sin(angle));
            float Y = (float)((i) * Math.Sin(angle) + (i) * Math.Cos(angle));
            PointF end = new PointF(X, Y);
            return end;
        }

    */