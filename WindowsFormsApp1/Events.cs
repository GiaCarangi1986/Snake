using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public class Events: EventArgs
    {
        public int code = 0; //0-все норм, 1-проигрыш, 2-съеден кролик
        public int width = 20;
        public int height = 20;
        public int constanta = 20;
        public int x; //координаты животного
        public int y; 
        public int id; //id животного для идентификации (0 - мангуст, 1 - кролик)
        public int boolCoord=1; //чтобы искал подходящие координаты животного до тех пор, пока не найдет подходящие
        public bool go = true; //идет ли змейка, а не стоит (нужно для регулировки пауза-продолжить)
        public bool newGame = true; //новая игра или сохраненная
        public bool one = true; //первый ход
        public bool newGame_const = true; //новая игра или сохраненная (надо для отрисовки)
    }
}
