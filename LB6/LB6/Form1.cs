using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB6
{
    public partial class Form1 : Form
    {
        double v11, v12, v13, v21, v22, v23, v32, v33, v43, screen_dist = 250, c1 = 4.5F, c2 = 3.5F;
        double rho, theta, phi, H = 50.0;
        PointF curr = new PointF(0,0);
        Graphics g = null;
        public Form1()
        {
            rho = 200;
            theta = 100;
            phi = 70;
            InitializeComponent();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            theta += 10;
            phi += 10;
            coeff(rho, theta, phi);
            this.Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            coeff(rho, theta, phi);
            g.TranslateTransform(400, 200);
            drawing();
        }
        void perspective(double x, double y, double z, ref double pX, ref double pY)
        {
            double xe, ye, ze;
            xe = v11 * x + v21 * y;
            ye = v12 * x + v22 * y + v32 * z;
            ze = v13 * x + v23 * y + v33 * z + v43;

            pX = screen_dist * xe / ze + c1;
            pY = screen_dist * ye / ze + c2;
        }
        void coeff(double rho, double theta, double phi)
        {
            double th, ph, costh, sinth, cosph, sinph, factor;
            factor = Math.Atan(1.0) / 45.0;
            th = theta * factor; ph = phi * factor;
            costh = Math.Cos(th); sinth = Math.Sin(th);
            cosph = Math.Cos(ph); sinph = Math.Sin(ph);

            v11 = -sinth; v12 = -cosph * costh; v13 = -sinph * costh;
            v21 = costh; v22 = -cosph * sinth; v23 = -sinph * sinth;
            v32 = sinph; v33 = -cosph;
            v43 = rho;
        }
        void dw(double x, double y, double z)
        {
            double X = 0, Y = 0;
            perspective(x, y, z, ref X, ref Y);
            draw(X, Y);
        }
        void draw(double X, double Y)
        {
            Random r = new Random();
            PointF s = new PointF((float)X, (float)Y);
            g.DrawLine(new Pen(Color.Blue, 2), curr, s);
            curr.X = (float)X;
            curr.Y = (float)Y;
        }
        void mv(double x, double y, double z)
        {
            double X = 0, Y = 0;
            perspective(x, y, z, ref X, ref Y);
            move(X, Y);
        }
        void move(double X, double Y)
        {
            curr.X = (float) X;
            curr.Y = (float) Y;
        }
       public void drawing()
        {
            mv(H, -H, -H); dw(H, H, -H); // ab
            dw(-H, H, -H); // bc
            dw(-H, H, H); // cg
            dw(-H, -H, H); // gh
            dw(H, -H, H); // he
            dw(H, -H, -H); // ea
            mv(H, H, -H); dw(H, H, H); // bf
            dw(-H, H, H); // fg
            mv(H, H, H); dw(H, -H, H); //fe
            mv(H, -H, -H); dw(-H, -H, -H); // ad
            dw(-H, H, -H); // dc
            mv(-H, -H, -H); dw(-H, -H, H); // dh
        }
    }
}
/* 
 * mv(H, -H, -H);
            dw(H, H, -H);
            dw(-H, H, -H);
            dw(-H, H, H);
            dw(-H, -H, H);
            dw(H, -H, H);
            dw(H, -H, -H);
            mv(H, H, H); dw(H, -H, H);
            mv(H, -H, -H); dw(-H, -H, -H);
            dw(-H, H, -H);
            mv(-H, -H, -H); dw(-H, -H, H);
 * 
 * 
 Console.WriteLine("rasstoyanie do nabl rho EO");
            rho = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("theta");
            theta = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("phi");
            phi = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("rasstoyanie ot tochki nabl do ekrana");
            screen_dist = Convert.ToDouble(Console.ReadLine());
            coeff(rho, theta, phi);*/
