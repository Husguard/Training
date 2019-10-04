using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB4
{
    public partial class Form1 : Form
    {
        int N = 100;
        Pen red = new Pen(Color.Red, 2);
        Pen blue = new Pen(Color.Blue, 2);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Random r = new Random();
            PointF Center = new PointF(500, 250);
            PointF New = new PointF();

            List<int> Radius = new List<int>();
            List<double> Angle = new List<double>();

            PointF[] draw = new PointF[N];
            e.Graphics.FillRectangle(Brushes.Black, Center.X, Center.Y, 3, 3);
            for (int i = 0; i<N; i+=1)
            {
                
                Angle.Add(r.Next(0, 360));
            }
            Angle.Sort();
            for (int i = 0; i < N; i += 1)
            {
                Radius.Add(r.Next(20, 200));
                Angle[i] = (float)(Angle[i] * Math.PI / 180);
                New.X = (float)(Center.X + Radius[i] * Math.Cos(Angle[i]));
                New.Y = (float)(Center.Y + Radius[i] * Math.Sin(Angle[i]));
                draw[i] = New;
            }
           e.Graphics.DrawLines(new Pen(Color.Red, 1), draw);
           e.Graphics.DrawLine(new Pen(Color.Red, 1), draw[0], draw[N-1]);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            PointF Center = new PointF(500, 250);
            PointF New = new PointF();

            List<int> Radius = new List<int>();
            List<double> Angle = new List<double>();

            PointF[] draw = new PointF[N];
            Graphics er = this.CreateGraphics();
            er.Clear(Color.White);
            er.FillRectangle(Brushes.Black, Center.X, Center.Y, 3, 3);
            for (int i = 0; i < N; i += 1)
            {

                Angle.Add(r.Next(0, 360));
            }
            Angle.Sort();
            for (int i = 0; i < N; i += 1)
            {
                Radius.Add(r.Next(50, 250));
                Angle[i] = (float)-(Angle[i] * Math.PI / 180);
                New.X = (float)(Center.X + Radius[i] * Math.Cos(Angle[i]));
                New.Y = (float)(Center.Y + Radius[i] * Math.Sin(Angle[i]));
                draw[i] = New;
            }
            er.DrawLines(red, draw);
            er.DrawLine(red, draw[0], draw[N-1]);
            er.DrawLine(red, draw[N - 2], draw[0]);
            er.DrawLine(red, draw[N - 2], draw[N - 1]);
            Treug(N, draw, er, true);
        }
        public static void Treug(int N, PointF[] draw, Graphics er, bool ch) 
        {
            Random r = new Random();
            if (ch)
            {
                for (int i = 2; i < N; i += 2)
                {
                    Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    Pen p = new Pen(randomColor, 2);
                    er.DrawLine(p, draw[i - 2], draw[i]);
                    er.DrawLine(p, draw[i - 2], draw[i - 1]);
                    er.DrawLine(p, draw[i - 1], draw[i]);
                }
            }
            else
            {
                for (int i = 1; i < N; i++)
                {
                    er.DrawLine(new Pen(Color.Blue, 2), draw[0], draw[i]);
                }
            }
        }
    }
}