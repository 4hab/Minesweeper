using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class Btn : Button
    {
        public int val;
        public int x, y;
        private bool o = false, l = false;
        public bool isopen()
        {
            return this.o;
        }
        public void open()
        {
            this.o = true;
        }
        public bool islocked()
        {
            return l;
        }
        public void Lock()
        {
            if (l)
                l = false;
            else
                l = true;
        }
        
        public Btn(int X,int Y,int v)
        {
            this.BackColor = Color.LightBlue;
            this.FlatStyle = FlatStyle.Popup;
            this.val = v;
            this.ForeColor = Color.Red;
            this.x = X;
            this.y = Y;
            this.Font = new Font(this.Font.FontFamily, 16,FontStyle.Bold);
            
            
        }

        
    }
}
