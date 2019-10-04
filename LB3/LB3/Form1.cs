using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LB3
{
    public partial class Form1 : Form
    {
        public Graphics g; //Графика
        public Bitmap map; //Битмап
        public Pen p; //Ручка
        public double angle = Math.PI / 2; //Угол поворота на 90 градусов
        public double ang1 = Math.PI / 4;  //Угол поворота на 45 градусов
        public double ang2 = Math.PI / 4;  //Угол поворота на 45 градусов


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(map); //Подключаем графику
            p = new Pen(Color.Green);   //Зеленая ручка

            DrawTree(300, 450, 200, angle);

            pictureBox1.BackgroundImage = map;
        }

        //Рекурсивная функция отрисовки дерева
        //x и y - координаты родительской вершины
        //a - параметр, который фиксирует количество итераций в рекурсии
        //angle - угол поворота на каждой итерации
        public int DrawTree(double x, double y, double a, double angle)
        {

            if (a > 3)
            {
                a *= 0.7;

                //Считаем координаты для вершины-ребенка
                double xnew = Math.Round(x + a * Math.Cos(angle)),
                       ynew = Math.Round(y - a * Math.Sin(angle));

                //рисуем линию между вершинами
                g.DrawLine(p, (float)x, (float)y, (float)xnew, (float)ynew);

                //Переприсваеваем координаты
                x = xnew;
                y = ynew;

                //Вызываем рекурсивную функцию для левого и правого ребенка
                DrawTree(x, y, a, angle + ang1);
                DrawTree(x, y, a, angle - ang2);
            }
            return 0;
        }
    }
}
