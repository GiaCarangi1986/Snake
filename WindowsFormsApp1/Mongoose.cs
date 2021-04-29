using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения класса в файле
    class Mongoose: Animals
    {
        public Mongoose(int x, int y, Events ev)
        {
            id = 0;
            this.x = x;
            this.y = y;
            ev.id = id;
            image = Properties.Resources.mon;
        }
        
        public override void show(Events ev, Graphics g) //рисуем мангуста
        {
            g.DrawImage(image, new Rectangle(x * ev.constanta+20, y * ev.constanta+20, ev.constanta, ev.constanta));
        }
    }
    
}
