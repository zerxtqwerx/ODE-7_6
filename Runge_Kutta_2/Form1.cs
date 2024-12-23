using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Runge_Kutta_2
{
    public partial class Form1 : Form
    {
        static double y;
        static double x;
        static double y2;
        static double h;
        PointPairList rungekuttPoints = new PointPairList();
        PointPairList adamsPoints = new PointPairList();
        public Form1()
        {
            InitializeComponent();
        }

        public double f(double x, double y) {
            return x*y;
        }

        public double f_solved(double x)
        {
            return Math.Pow(Math.E,x*x/2);
        }

        

            public double RungeKutt(double x0, double y0, double h, int n)
            {
                double x = x0;
                double y = y0;
                rungekuttPoints.Add(new PointPair(x, y));
                for (int i = 0; i < n; i++)
                {
                    double k1 = h * f(x, y);
                    double k2 = h * f(x + h, y + k1);
                    y += (k1 + k2) / 2;
                    x += h;
                rungekuttPoints.Add((new PointPair(x, y)));
                }

                return y;
            }

        private double SolveImplicit(double x, double y, double h)
        {
            return y + h * f(x, y); 
        }

        public double Adams(double x0, double y0, double h, int n)
        {
            double x = x0;
            double y = y0;
            adamsPoints.Add(new PointPair(x, y));
            for (int i = 0; i < n; i++)
            {
                double yNext = SolveImplicit(x, y, h);
                y = yNext;
                x += h;
                adamsPoints.Add(new PointPair( x, y));
            }

            return y;
        }


        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            rungekuttPoints = new PointPairList();
            adamsPoints = new PointPairList();
            PointPairList functionPoints = new PointPairList();
            int count = 0;
            double x0=0;
            double y0=Convert.ToDouble(textBox2.Text);
            double h=Convert.ToDouble(textBox3.Text);
            int n = Convert.ToInt32(1 / h);
            RungeKutt(x0, y0, h, n);
            Adams(x0, y0, h, n);
            for (double x = 0; x < 1; x = x + h) {
                functionPoints.Add(new PointPair(x, f_solved(x)));
            }
            GraphPane graphPane = zedGraphControl1.GraphPane;
            graphPane.CurveList.Clear();
            graphPane.Title.Text = "Решение дифф ур";
            graphPane.XAxis.Title.Text = "x";
            graphPane.YAxis.Title.Text = "y";

            LineItem rungekuttCurve = graphPane.AddCurve("Рунге-Кутта 2 порядка", rungekuttPoints, System.Drawing.Color.Blue, SymbolType.None);
            LineItem adamsCurve = graphPane.AddCurve("Неявный метод Адамса 2 порядка", adamsPoints, System.Drawing.Color.Red, SymbolType.None);
            LineItem functionCurve = graphPane.AddCurve("Решённая функция", functionPoints, System.Drawing.Color.Green, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        
    }
}
