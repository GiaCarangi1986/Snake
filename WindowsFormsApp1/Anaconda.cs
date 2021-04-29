using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения класса в файле
    class Anaconda
    {
        int len, Direction_x, Direction_y; //направление змейки
    List<Link> piece = new List<Link>(); //лист звеньев
    public Anaconda(int len)
        {
            this.len = len;
            this.Direction_x = 1;
            this.Direction_y = 0;
            for (int i = 0; i < len; i++) //создаем лист звеньев под названием piece
            {
                piece.Insert(0, new Link(5 + i, 9));
            }
            Inizilizaziy();
        }

        public void Inizilizaziy() //подписываемся на события
        {
            Form2.Event_ProvBodySnake += prov_xy;
            Form2.Event_ProvSnakeWall += event_wall;
        }

        public void draw_head(Events mes, Graphics g)  //спросить про поворот картинки
        {
            Link it_head = piece[0];
            switch (getDirection_x()) //в зависимости от направления головы рисуем ту или иную картинку
            {
                case 1: it_head.show_head_vpravo(mes, g); break;
                case -1: it_head.show_head_vlevo(mes, g); break;
                case 0:
                    {
                        switch (getDirection_y())
                        {
                            case 1: it_head.show_head_vniz(mes, g); break;
                            case -1: it_head.show_head_vverx(mes, g); break;
                        }
                    }
                    break;
            }
        }

        public void draw_body(Events mes, Graphics g) //прогоняем от начала тела до конца и рисуем каждое звено
        {
            for (int i = 1; i < len; i++)
            {
                Link it = piece[i];
                it.show(mes, g);
            }
        }

        public void draw_second(Events mes, Graphics g) //отрисовка второго элемента (первого после головы)
        {
                Link it = piece[1];
                it.show(mes, g);
        }

        public void move(Events mes, Graphics g) //движение змейки на 1 клетку (в новое положение)
        {
            int headX = getHeadX(), headY = getHeadY();
            headX += Direction_x; //к координатам головы прибавляем координаты направления 
            headY += Direction_y;
            piece.Insert(0,new Link(headX, headY)); //вставляем эти новые координаты на 1 место в лист звеньев
            if (mes.code != 2) //если змейка не съела кролика, то удаляем хвост, иначе нет (так осуществляется рост тела)
            {
                Link it = piece[len];
                it.hide(mes, g); //затирание последнего звена
                piece.Remove(piece[len]);
            }
            else len++; //увеличиваем длину тела
        }

    public void change_direction(int new_vx, int new_vy) //если нажали клавишу управления, то меняем направление змейки
        { //проверка на то, чтобы не поменяли направление на прямо противоположное (на 180 градусов)
            if (Direction_x * new_vx == Direction_y * new_vy) 
            {
                Direction_x = new_vx;
                Direction_y = new_vy;
            }
        }

    public int getHeadX() //получить координату головы по х
        {
            return piece[0].getX();
        }

    public int getHeadY() //получить координату головы по у
        {
            return piece[0].getY();
        }

    public int getDirection_x() //получить направление змейки по х
        {
            return Direction_x;
        }

    public int getDirection_y() //получить направление змейки по у
        {
            return Direction_y;
        }

        public void event_wall(Events mes) //проверка на то, врезались ли в стену
        {
            switch (getDirection_x())
            {
                case 1: if (getHeadX() == mes.width) mes.code = 1; break; //если вправо, то не врезались ли в правую стенку
                case -1: if (getHeadX() == -1) mes.code = 1; break; //если влево, то не врезались ли в левую стенку
                case 0:
                    {
                        switch (getDirection_y())
                        {
                            case 1: if (getHeadY() == mes.height) mes.code = 1; break; //если вниз, то не врезались ли в нижнюю стенку
                            case -1: if (getHeadY() == -1) mes.code = 1; break; //если вверх, то не врезались ли в верхнюю стенку
                        }
                    }
                    break;
            }
        }

 

    public void prov_xy(Events mes) //проверка на то, чтобы животное не появилось в теле змейки
        {
            int x = mes.x;
            int y = mes.y;
            bool ok = true; //вспомогательная переменная, чтобы зря условия не проверять, если уже не подходит
            foreach(Link it in piece) //провереям само теле змейки
            {
                if (it.getX() == x)
                    foreach (Link it1 in piece)
                        if (it1.getY() == y)
                        {
                            mes.boolCoord = 1;
                            ok = false;
                        }
            }
            if (ok) //если в тело змейки не попало, то смотрим, чтобы не появилось прямо перед головой
            {
                Link it_one = piece[0];
                if (it_one.getX() + Direction_x == x)
                    if (it_one.getY() + Direction_y == y)
                    {
                        mes.boolCoord = 1;
                        ok = false;
                    }
            }
             if (ok) //если прямо перед головой не попало, то смотрим, чтобы не появилось через одну клетку после этого
            {
                    Link it_two = piece[0];
                    if (it_two.getX() + Direction_x + Direction_x == x)
                        if (it_two.getY() + Direction_y + Direction_y == y)
                            mes.boolCoord = 1;
                        else mes.boolCoord = 0;
                    else mes.boolCoord = 0;
            }
            
        }
    public void event_prov_body(Events mes) //проверка на то, врезалась ли змейка сама в себя
        {
            Link it = piece[0];
            Link it1;
            for (int i=1; i<len; i++)
            {
                it1 = piece[i];
                if (it1.getX() == it.getX() && it1.getY() == it.getY())
                {
                    mes.code = 1;
                    break;
                }
            }
        }
    }
}
