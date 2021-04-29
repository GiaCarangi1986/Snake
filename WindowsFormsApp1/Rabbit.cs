using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения класса в файле
    class Rabbit: Animals
    {
        public Rabbit(int x, int y, Events ev)
        {
            id = 1;
            this.x = x;
            this.y = y;
            ev.id = id;
            image = Properties.Resources.rab;
        }
        public override void show(Events ev, Graphics g) //рисуем кролика
        {
            g.DrawImage(image, new Rectangle(x * ev.constanta+20, y * ev.constanta+20, ev.constanta, ev.constanta));
        }
    }
}
