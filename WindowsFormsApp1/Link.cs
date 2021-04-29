using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения класса в файле
    class Link
    {
    private int x, y;
        Image image;
    public Link(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void show(Events mes, Graphics g) //рисуем тело (кружочки) 
        {
            image = Properties.Resources.zveno;
            g.DrawImage(image, new Rectangle(x * mes.constanta + 20, y * mes.constanta + 20, mes.constanta, mes.constanta));
        }
        public void hide(Events mes, Graphics g) //скрываем тело (кружочки) 
        {
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(128,255,128));
            g.FillRectangle(solidBrush, new Rectangle(x * mes.constanta+20, y * mes.constanta+20, mes.constanta, mes.constanta));
        }
        public void show_head_vniz(Events ev, Graphics g) //картинка головы вниз
        {
            image = Properties.Resources.head_vniz;
            g.DrawImage(image, new Rectangle(x * ev.constanta + 20, y * ev.constanta + 20, ev.constanta, ev.constanta));
        }
        public void show_head_vverx(Events ev, Graphics g) //картинка головы вверх
        {
            image = Properties.Resources.head_vverx;
            g.DrawImage(image, new Rectangle(x * ev.constanta + 20, y * ev.constanta + 20, ev.constanta, ev.constanta));
        }
        public void show_head_vpravo(Events ev, Graphics g) //картинка головы вправо
        {
            image = Properties.Resources.head_vpravo;
            g.DrawImage(image, new Rectangle(x * ev.constanta + 20, y * ev.constanta + 20, ev.constanta, ev.constanta));
        }
        public void show_head_vlevo(Events ev, Graphics g) //картинка головы влево
        {
            image = Properties.Resources.head_vlevo;
            g.DrawImage(image, new Rectangle(x * ev.constanta + 20, y * ev.constanta + 20, ev.constanta, ev.constanta));
        }
        public int getX() //получить координату для звена по х
        {
            return x;
        }
        public int getY() //получить координату для звена по у
        {
            return y;
        }
    }
}
