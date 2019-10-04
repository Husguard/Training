using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB6._1
{
    public partial class Form1 : Form
    {
        int H = 200;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(400, 300);
            e.Graphics.DrawLine(new Pen(Color.Blue, 2), -H, -H, -H, H);
            e.Graphics.DrawLine(new Pen(Color.Blue, 2), -H, H, H, H);
            e.Graphics.DrawLine(new Pen(Color.Blue, 2), -H, -H, H, H);
        }
    public static void perspective(double x, double y, double z, ref double pX, ref double pY)
        {
            float xe, ye, ze;
            xe = v11 * x + v21 * y;
            ye = v12 * x + v22 * y + v32 * z;
            ze = v13 * x + v23 * y + v33 * z + v43;

            pX = 50 * xe / ze + 4.5;
            pY = 50 * ye / ze + 3.5;
        }
    }
}
