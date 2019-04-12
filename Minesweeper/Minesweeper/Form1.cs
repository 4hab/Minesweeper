using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Btn[,] grid;
        private int[,] val;
        private int[] dx = { 1, 1, 1, -1, -1, -1, 0, 0 };
        private int[] dy = { 1, 0, -1, 1, 0, -1, 1, -1 };
        private Panel panel1;
        private int boom,rest;
        private Stack<Point> boomLoc = new Stack<Point>();
        private Color[] colors = new Color[9];
        private int N;
        private int bc;
        public Form1()
        {
            InitializeComponent();
            panel1 = new Panel();
            int w = this.Width-60, h = this.Height-120;
            panel1.SetBounds(30, 60, w, h);
            this.Controls.Add(panel1);
            colors[1] = Color.Blue;
            colors[2] = Color.Green;
            colors[3] = Color.Red;
            colors[4] = Color.DarkRed;
            colors[5] = Color.DarkBlue;
            colors[6] = Color.DarkOrange;
            colors[7] = Color.Brown;
            colors[8] = Color.DarkViolet;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            int n = 9,m=10;
            if (levelBox.SelectedIndex == 1)
            {
                n = 16;
                m = 35;
            }
            else if (levelBox.SelectedIndex == 2)
            {
                n = 20;
                m = 60;
            }
            boom = m;
            rest = n * n;
            grid = new Btn[n, n];
            val = new int[n, n];
            N = n;
            bc = m;
            Generate1(n, m);
            Generate2(n);
        }
        private void Generate1(int n,int m)
        {
            for(int i=1;i<=m;i++)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, n - 1);
                int y = rnd.Next(0, n - 1);
                while(val[x,y]==-1)
                {
                    x = rnd.Next(0, n - 1);
                    y = rnd.Next(0, n - 1);
                }
                Point p = new Point(x, y);
                boomLoc.Push(p);
                val[x, y] = -1;
                for(int j=0;j<8;j++)
                {
                    int xx = x + dx[j], yy = y + dy[j];
                    if(xx>=0&&xx<n&&yy>=0&&yy<n&&val[xx,yy]!=-1)
                        val[xx, yy]++;
                }
            }
        }
        private void Generate2(int n)
        {
            int h = Math.Min(panel1.Width, panel1.Height);
            int a = (h*h) / (n * n);
            a = (int)(Math.Sqrt(a));
            panel1.Width = panel1.Height = a * n;
            int px = 0, py = 0;
            for(int i=0;i<n;i++,py+=a,px=0)
            {
                for(int j=0;j<n;j++,px+=a)
                {
                    grid[i,j] = new Btn(i,j,val[i,j]);
                    grid[i,j].SetBounds(px,py,a,a);
                    panel1.Controls.Add(grid[i,j]);
                    grid[i, j].MouseDown += BtnClick;
                }
            }
        }
        private bool open(int x,int y)
        {
            if(x<0||x>=N||y<0||y>=N)
                return true;
            if (grid[x, y].isopen())
                return true;
            if (grid[x, y].islocked())
                return true;
            if(val[x,y]==-1)//Boom
            {
                SoundPlayer player = new SoundPlayer("Bomb.wav");
                player.Play();
                grid[x, y].BackgroundImage = imageList1.Images[0];
                grid[x, y].BackgroundImageLayout = ImageLayout.Center;
                while(boomLoc.Count>0)
                {
                    Point p = new Point();
                    p = boomLoc.Pop();
                    open(p.X, p.Y);
                }
                return false;
            }
            else if(val[x,y]==0)
            {
                grid[x, y].open();
                grid[x, y].BackColor = Color.LightGray;
                for(int i=0;i<8;i++)
                {
                    open(x + dx[i], y + dy[i]);
                }
                rest--;
            }
            else
            {
                SoundPlayer player = new SoundPlayer("Click1.wav");
                player.Play();
                rest--;
                grid[x, y].Text = val[x, y].ToString();
                int num = val[x, y];
                grid[x, y].ForeColor = colors[num];
                grid[x, y].BackColor = Color.LightGray;

            }
            //grid[x, y].Enabled = false;
            grid[x, y].open();
            return true;
        }
        private void mark(int x,int y)
        {
            grid[x, y].Enabled = false;

        }
        private void BtnClick(object sender,EventArgs e)
        {
            MouseEventArgs M = (MouseEventArgs)e;
            Btn s = (sender as Btn);
            if(M.Button==MouseButtons.Right)
            {
                s.Lock();
                if (s.islocked())
                {
                    bc--;
                    s.BackgroundImage = imageList1.Images[1];
                    s.BackgroundImageLayout = ImageLayout.Center;
                }
                else
                {
                    s.BackgroundImage = null;
                    bc++;
                }
                return;
            }
            bool gameOver = !open(s.x, s.y);
            if (s.val == 0)
            {
                SoundPlayer player = new SoundPlayer("magic.wav");
                player.Play();
            }
            if (gameOver)
            {

                DialogResult d = MessageBox.Show("Game Over!!\nTry again??", "Over", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    button1_Click(button1, null);
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
            else if(rest==boom)
            {
                SoundPlayer player = new SoundPlayer("winner.wav");
                player.Play();
                DialogResult d = MessageBox.Show("Winner Winner!!\nPlay again??", "Winer", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    button1_Click(button1, null);
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
            label1.Text = bc.ToString();
        }

    }
}
