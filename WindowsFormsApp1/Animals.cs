using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения класса в файле
    abstract class Animals
    {
        protected int x, y, id;
        protected Image image;
        public abstract void show(Events ev,Graphics g);
        public int getX() //получить координату животного по х
        {
            return x;
        }
        public int getY() //получить координату животного по у
        {
            return y;
        }
        public int ID() //получить уникальный номер животного (0 - мангуст, 1 - кролик)
        {
            return id;
        }
    }
}
