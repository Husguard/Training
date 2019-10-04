using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB2
{
    public partial class Form1 : Form
    {
        private int predel = 300;
        int smes =  100;

        private List<Segment> rez = new List<Segment>(); // прямые наши
        private List<PointF> polyg = new List<PointF>(); // окно отрезания
        
        public Form1()
        {
            Graphics gr = this.CreateGraphics();
            polyg.Add(new PointF(200, 200));
            polyg.Add(new PointF(500, 50));
            polyg.Add(new PointF(400, 500));
            polyg.Add(new PointF(200, 600));
           
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            double angle = 2;
            List<MyRectangle> each = new List<MyRectangle>();
            for (int i = 0; i < predel; i += 5)
            {
                
                PointF LT = new PointF(smes+i, i);
                PointF LB = new PointF(smes+i, (2*predel) - i);
                PointF RT = new PointF(smes+(2 * predel) - i, i);
                PointF RB = new PointF(smes+(2 * predel) - i, (2 * predel) - i);
                each.Add(new MyRectangle(LT, LB, RT, RB));
                each.Last().Rotate(angle);
                angle += 0.5;

                rez.Add(new Segment(each.Last().LT, each.Last().LB));
                rez.Add(new Segment(each.Last().LB, each.Last().RB));
                rez.Add(new Segment(each.Last().RB, each.Last().RT));
                rez.Add(new Segment(each.Last().RT, each.Last().LT));
                each.Last().DrawLines(e.Graphics, new Pen(Color.Blue, 2));
             }
            e.Graphics.DrawLine(new Pen(Color.Red, 2), polyg[0], polyg[1]);
            e.Graphics.DrawLine(new Pen(Color.Red, 2), polyg[1], polyg[2]);
            e.Graphics.DrawLine(new Pen(Color.Red, 2), polyg[2], polyg[3]);
            e.Graphics.DrawLine(new Pen(Color.Red, 2), polyg[3], polyg[0]);
        }
        

        private void Form1_Click(object sender, EventArgs e)
        {
            Graphics er = this.CreateGraphics();
            Polygon test = new Polygon(polyg);
            List<Segment> rez1 = test.CyrusBeckClip(rez); // вернет сегменты который внутри test

            er.Clear(Color.White);
            foreach (Segment p in rez1)
            {
                er.DrawLine(new Pen(Color.Green, 2), p.A, p.B);
            }
        }
    }
    }

    class MyRectangle
    {
        public PointF LT { get; set; }
        public PointF LB { get; set; }
        public PointF RT { get; set; }
        public PointF RB { get; set; }
        public MyRectangle(PointF LT1, PointF LB1, PointF RT1, PointF RB1)
        {
            LT = LT1;
            LB = LB1;
            RT = RT1;
            RB = RB1;
        }
        public void Rotate(double angle)
        {
            angle = angle * Math.PI / 180;
            LT = RotateP(LT, angle);
            LB = RotateP(LB, angle);
            RT = RotateP(RT, angle);
            RB = RotateP(RB, angle);
        }
        private PointF RotateP(PointF point, double angle)
        {
            float X = (float)((point.X) * Math.Cos(angle) - (point.Y) * Math.Sin(angle));
            float Y = (float)((point.X) * Math.Sin(angle) + (point.Y) * Math.Cos(angle));
            return new PointF(X, Y);
        }
        public void DrawLines(Graphics gr, Pen style)
        {
            gr.DrawLine(style, LT, LB);
            gr.DrawLine(style, LT, RT);
            gr.DrawLine(style, LB, RB);
            gr.DrawLine(style, RT, RB);
        }
    }
public struct Segment
{
    public readonly PointF A, B;

    public Segment(PointF a, PointF b)
    {
        A = a;
        B = b;
    }

    public bool OnLeft(PointF p)
    {
        var ab = new PointF(B.X - A.X, B.Y - A.Y);
        var ap = new PointF(p.X - A.X, p.Y - A.Y);
        return ab.Cross(ap) >= 0;
    }

    public PointF Normal
    {
        get
        {
            return new PointF(B.Y - A.Y, A.X - B.X);
        }
    }

    public PointF Direction
    {
        get
        {
            return new PointF(B.X - A.X, B.Y - A.Y);
        }
    }

    public float IntersectionParameter(Segment that)
    {
        var segment = this;
        var edge = that;

        var segmentToEdge = edge.A.Sub(segment.A);
        var segmentDir = segment.Direction;
        var edgeDir = edge.Direction;

        var t = edgeDir.Cross(segmentToEdge) / edgeDir.Cross(segmentDir);

        if (float.IsNaN(t))
        {
            t = 0;
        }

        return t;
    }

    public Segment Morph(float tA, float tB)
    {
        var d = Direction;
        return new Segment(A.Add(d.Mul(tA)), A.Add(d.Mul(tB)));
    }
}

public class Polygon : List<PointF>
{
    public Polygon()
        : base()
    { }

    public Polygon(int capacity)
        : base(capacity)
    { }

    public Polygon(IEnumerable<PointF> collection)
        : base(collection)
    { }

    public bool IsConvex
    {
        get
        {
            if (Count >= 3)
            {
                for (int a = Count - 2, b = Count - 1, c = 0; c < Count; a = b, b = c, ++c)
                {
                    if (!new Segment(this[a], this[b]).OnLeft(this[c]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public IEnumerable<Segment> Edges
    {
        get
        {
            if (Count >= 2)
            {
                for (int a = Count - 1, b = 0; b < Count; a = b, ++b)
                {
                    yield return new Segment(this[a], this[b]);
                }
            }
        }
    }

    private bool CyrusBeckClip(ref Segment subject)
    {
        var subjDir = subject.Direction;
        var tA = 0.0f;
        var tB = 1.0f;
        foreach (var edge in Edges)
        {
            switch (Math.Sign(edge.Normal.Dot(subjDir)))
            {
                case -1:
                    {
                        var t = subject.IntersectionParameter(edge);
                        if (t > tA)
                        {
                            tA = t;
                        }
                        break;
                    }
                case +1:
                    {
                        var t = subject.IntersectionParameter(edge);
                        if (t < tB)
                        {
                            tB = t;
                        }
                        break;
                    }
                case 0:
                    {
                        if (!edge.OnLeft(subject.A))
                        {
                            return false;
                        }
                        break;
                    }
            }
        }
        if (tA > tB)
        {
            return false;
        }
        subject = subject.Morph(tA, tB);
        return true;
    }

    public List<Segment> CyrusBeckClip(List<Segment> subjects)
    {
        if (!IsConvex)
        {
            Reverse();
            if (!IsConvex)
            {
                throw new InvalidOperationException("Clip polygon must be convex.");
            }
        }

        var clippedSubjects = new List<Segment>();
        foreach (var subject in subjects)
        {
            var clippedSubject = subject;
            if (CyrusBeckClip(ref clippedSubject))
            {
                clippedSubjects.Add(clippedSubject);
            }
        }
        return clippedSubjects;
    }
}

public static class PointExtensions
{
    public static PointF Add(this PointF a, PointF b)
    {
        return new PointF(a.X + b.X, a.Y + b.Y);
    }

    public static PointF Sub(this PointF a, PointF b)
    {
        return new PointF(a.X - b.X, a.Y - b.Y);
    }

    public static PointF Mul(this PointF a, float b)
    {
        return new PointF(a.X * b, a.Y * b);
    }

    public static float Dot(this PointF a, PointF b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static float Cross(this PointF a, PointF b)
    {
        return a.X * b.Y - a.Y * b.X;
    }
}
