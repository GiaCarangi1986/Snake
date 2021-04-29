using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace WindowsFormsApp1
{
    [Serializable] //нужно для сохранения игры
    public partial class Form2 : Form
    {
        int width; //ширина окна
        int height; //высота окна
        int score; //счет
        int constanta;// = 20;
        bool downlong; //чтобы если загружена сохраненная игра, то сначала поставить ее на паузу
        bool ok; //чтобы не было недоразумений, если резко одновременно нажали несколько клавиш (поворот и пауза
        //осуществляется после того, как змейка сходила на 1 клетку после изменения ее движения, или, если 
        //направление прежнее, то в любой момент)
        Events ev;
        Form1 f1;
        Graphics g;
        Random rand = new Random();
        Animals a;
        Anaconda snake;
        int count = 0; //кол-во шагов, которое должна пройти змейка, не съев мангуста, чтобы он исчез и скорость игры увеличилась
        int monEat; //кол-во шагов для змейки, при этом оно увеличивается с увеличением скорости игры
        int speed;
        public Form2(int width, int height, Form1 f1) //конструктор для новой игры
        {
            InitializeComponent();
            downlong = false;
            score =0;
            monEat = 8; 
            speed = 250;
            this.width = width;
            this.height = height;
            constanta = 20;
            ev = new Events();
            this.f1=f1;
            g = panel1.CreateGraphics();
            snake = new Anaconda(3);
            start();
            timer1.Enabled = true; //запускаем таймер
        }
        public Form2(int width, int height, Form1 f1, Events ev) //конструктор для загруженной игры
        {
            InitializeComponent();
            //восстанавливаем сохраненную игру
                FileStream myStream = File.OpenRead("..\\downlong.dat");
                BinaryFormatter myBinaryFormat = new BinaryFormatter();
                this.snake = (Anaconda)myBinaryFormat.Deserialize(myStream);
                this.a = (Animals)myBinaryFormat.Deserialize(myStream);
                this.score=(int)myBinaryFormat.Deserialize(myStream);
                this.speed = (int)myBinaryFormat.Deserialize(myStream);
                this.monEat = (int)myBinaryFormat.Deserialize(myStream);
                myStream.Close();
            snake.Inizilizaziy(); //принудительно подписываем змейку на события
            this.width = width;
            this.height = height;
            constanta = 20;
            this.ev = ev;
            ev.x = a.getX(); //обновляем данные в структуре
            ev.y = a.getY();
            ev.id = a.ID();
            this.f1 = f1;
            g = panel1.CreateGraphics();
            label2.Text = score.ToString(); //сразу выставляем счет
            button3.Enabled = true; //делаем выход в главное меню активным, ибо сейчас стоим на паузе и мб, увидев картину
            //и вспомнив ситуацию, пользователю не захочется играть и он сразу выйдет
            downlong = true;
            timer1.Enabled = true; //запускаем таймер
        }

        // события///////////////////////////////////////////////////////////(проверка попадания координаты животного в тело змейки)

        public delegate void ProvBodySnake(Events ev);
        public static event ProvBodySnake Event_ProvBodySnake;

        public void ProvBodySnake_event()
        {
            if (Event_ProvBodySnake != null)
                Event_ProvBodySnake(ev);
        }
        //события/////////////////////////////////////////////////////////////(проверка попадания координаты животного в тело змейки)

        

        // события///////////////////////////////////////////////////////////(проверка попадания змейки в стену)

        public delegate void ProvSnakeWall(Events ev);
        public static event ProvSnakeWall Event_ProvSnakeWall;

        public void ProvSnakeWall_event()
        {
            if (Event_ProvSnakeWall != null)
                Event_ProvSnakeWall(ev);
        }
        //события/////////////////////////////////////////////////////////////(проверка попадания змейки в стену)

        void f_record() //запись в таблицу рекордов
        {
            int[] rec = new int[10];
           string path = "..\\game.dat";
            if (!File.Exists(path)) //если файла нет, то создаем его (всего 10 записей), в нулевое значение
            {                       //вставляем нынешний счет, в остальные - 0
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    rec[0] = score;
                    writer.Write(rec[0]);
                    for (int i = 1; i < 10; i++)
                    {
                        rec[i] = 0;
                        writer.Write(rec[i]);
                    }
                }
            }
            else //если файл есть, то выбираем мин значение, сравниваем его с текущим и смотрим: если текущее значение
            {    //больше мин значения, то мин заменяем на текущее и переписываем массив в файл, иначе ничего не делаем
                int i = 0, j = 0;
                int min = 200;
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.OpenOrCreate)))
                {
                    // пока не достигнут конец файла
                    // считываем каждое значение из файла
                    while (reader.PeekChar() > -1)
                    {
                        rec[i] = reader.ReadInt32();
                        if (rec[i] < min)
                        {
                            min = rec[i];
                            j = i;
                        }
                        i++;
                    }
                    if (min < score)
                    {
                        rec[j] = score;
                        
                    }
                }
                if (min < score)
                    using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                    {
                        for (i = 0; i < 10; i++)
                            writer.Write(rec[i]);
                    }
            }
            //f1.button5.Enabled = true; //активируем таблицу рекордов
        }

        public void start() //сначала координаты ищем прям тут, потом отправляем событием в змейку, там проверяем и если
                     // все норм, то в конструктор животного координаты присваиваем
        { //то есть тут получаем координаты животного
            int x = 0, y = 0;
            ev.boolCoord = 1;
            while (ev.boolCoord == 1)
            {
                x = rand.Next(width);
                y = rand.Next(height);
                ev.x = x ;
                ev.y = y ;
                ProvBodySnake_event();
            }
            int random = rand.Next(2);
            if (random == 1)
                a = new Rabbit(ev.x, ev.y, ev);
            else
                a = new Mongoose(ev.x, ev.y, ev);
            a.show(ev, g); //рисуем животное
        }

        bool once = true; //нужна для того, чтобы в начале игры отобразилось все

        public void game() //логика самой игры
        {
            {
                snake.move(ev, g); //двигаем змейку
                ok = true; //змейка сдвинулась после возможного изменения направления => можно ставить на паузу/менять снова направление

                if ((ev.code == 2) || (count == monEat && ev.id == 0))
                { //если съели кролика или прошли нужное кол-во шагов и не попали на мангуста
                    score++; //присваивается очко, если увернулся от мангуста или съел кролика
                    label2.Text = score.ToString(); //очко высвечиваем
                    monEat += 2; //прибавляем время пребывания мангуста на поле
                    if (speed > 65 && ev.code == 2) //чтобы уж не умереть от скорости, 
                        //то увеличиваем ее (уменьшаем время таймера) постепенно для предела (И ЕСЛИ СЪЕЛИ КРОЛИКА)
                        speed -= 20;
                    count = 0;
                    ev.code = 0;
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb(128, 255, 128)); //теперь зарисовываем животное
                    g.FillRectangle(solidBrush, new Rectangle(ev.x * ev.constanta + 20, ev.y * ev.constanta + 20, ev.constanta, ev.constanta));
                    start();
                }

                if (ev.x == snake.getHeadX() && ev.y == snake.getHeadY()) //если наткнулись на животное
                    if (ev.id != 0) //если это кролик
                        ev.code = 2;
                    else ev.code = 1; //если это мангуст

                if (ev.code != 1) //если не мангуст
                    {
                    snake.event_wall(ev);
                        if (ev.code != 1) //если не врезалась в стену
                        {
                            snake.event_prov_body(ev);
                            if (ev.code != 1) //если не врезалась в себя
                            {
                                count++; //увеличиваем очки (счет игры)
                            if (once && ev.newGame_const) //если метод запущен впервые, и игра новая (а не загруженная)
                            {
                                a.show(ev, g); //рисуем животное
                                snake.draw_head(ev, g);
                                snake.draw_body(ev, g);
                                once = false; //уже будет выполнен этот метод не впервые
                            }
                            else 
                            {
                                snake.draw_head(ev, g);
                                snake.draw_second(ev, g);
                            }
                        }
                            else
                            {
                                //отрисуем сначала тело, потом голову, чтобы было видно, что она
                                snake.draw_second(ev, g);
                                snake.draw_head(ev, g); //врезалась в себя
                                end();
                            }
                        }
                        else
                        {
                        snake.draw_second(ev, g);
                        end();
                        }   
                   }
                    else 
                    {
                    snake.draw_second(ev, g);
                    end();
                    }
            } 
        }
        bool end_game = false; //чтобы деактивация сработала норм при проигрыше
        public void end() //окончание игры
        {
            end_game = true;
           Thread thread = new Thread(new ThreadStart(f_record)); //включаем дополнительный поток
           thread.Start();
            f1.button5.Enabled = true; //активируем таблицу рекордов
            //f_record(); //запись рекорда
            timer1.Enabled = false; //останавливаем таймер
            button3.Enabled = true; //делаем активным выход в меню, а остальные кнопки (продолжить и сохранить) блокируем
            button2.Enabled = false;  
            button1.Enabled = false;
            MessageBox.Show("ИГРА ОКОНЧЕНА"); //вывод сообщения о том, что игра окончена
            if (!ev.newGame) //если игра была загружена, то удаляем ее, ибо мы ее закончили
            {
                File.Delete("..\\downlong.dat");
                f1.button2.Enabled = false; //соответственно кнопку загрузить сохраненную игру делаем неактивной
            }
        }

        private void button1_Click_1(object sender, EventArgs e) //пауза/продолжить
        {
            if (ok) //если нет заедания клавиш и змейка адекватно поменяла свое положение
            {
                if (ev.go) //если она идет
                {
                    timer1.Enabled = false; //оставливаем таймер
                    ev.go = false; //она больше не идет
                    button2.Enabled = true; //делаем активным сохранение
                    button3.Enabled = true; //делаем активным выход в меню
                }
                else //если она стоит
                {
                    timer1.Enabled = true; //включаем таймер
                    ev.go = true; //теперь она идет
                    button2.Enabled = false; //делаем неактивным сохранение
                    button3.Enabled = false; //делаем активным выход в меню
                }
            }
        }

        public void Form2_KeyDown(object sender, KeyEventArgs e) //событие нажатия на клавишу управления
        {
            if (ok && ev.go) //если клавиши не зажаты и змейка в движении
                switch (e.KeyCode.ToString())
                {
                    case "W":/*UP*/
                        snake.change_direction(0, -1); ok = false; break;
                    case "S":/*DOWN*/
                        snake.change_direction(0, 1); ok = false; break;
                    case "D":/*Right*/
                        snake.change_direction(1, 0); ok = false; break;
                    case "A":/*Left*/
                        snake.change_direction(-1, 0); ok = false; break;
                }
        }

        private void timer1_Tick(object sender, EventArgs e) //работа таймера
        {
            //мб как то по другому можно?
            if (downlong) //сработает 1 раз ток при загрузке сохраненной игры 
            {//если загружена сохраненная игра, то рисуем все и ставим на паузу
                snake.draw_head(ev, g);
                snake.draw_body(ev, g);
                a.show(ev, g);
                timer1.Enabled = false; //пауза
                ev.go = false; //мы не идем
                downlong = false; //чтобы больше не сработало
                ok = true; //можно продолжить игру
            }
            else
            {
                timer1.Interval = speed; //время берем у скорости игры
                game(); //запускаем игру
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e) //закрытие формы на красный крестик
        {
            if (button2.Enabled)
            {
                f1.button2.Enabled = true; //загрузить активно
                ev.newGame = true; //надо для того, чтобы можно было: сохранить - проиграть - загрузить - сохранить - проиграть - загрузить
            }
            f1.Show(); //показываем и активируем форму 1, где главное меню
            f1.Activate();
        }

        private void button3_Click(object sender, EventArgs e) //выйти в главное меню
        {
            if (button2.Enabled) //если кнопка сохранения активна, т. е. мы не сохранили последние действия, то напомнить об этом
            {
                DialogResult result = MessageBox.Show(
               "Выйти без сохранения?",
               "Вы уверены?",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button2,
               MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                    f1.Show();
                    f1.Activate();
                }
                else
                    Activate();
            }
            else
            {
                this.Close();
                f1.Show();
                f1.Activate();
            }
          } 

        void Save() //сохранение игры
        {
            FileStream myStream = File.Create("..\\downlong.dat");
            // Помещаем объектный граф в поток в двоичной формате
            BinaryFormatter myBinaryFormat = new BinaryFormatter();
            myBinaryFormat.Serialize(myStream, snake);
            myBinaryFormat.Serialize(myStream, a);
            myBinaryFormat.Serialize(myStream, score);
            myBinaryFormat.Serialize(myStream, speed);
            myBinaryFormat.Serialize(myStream, monEat);
            myStream.Close();
           // MessageBox.Show("Сохранено");
        }

        private void button2_Click(object sender, EventArgs e) //сохранение игры (клик на эту кнопку)
        {
          Thread thread = new Thread(new ThreadStart(Save)); //вызываем дополнительный поток
          thread.Start();
           // Save();
            MessageBox.Show("Сохранено");
            button2.Enabled = false; //сохранение не активно
            button3.Enabled = true; //выход в меню активен
            f1.button2.Enabled = true; //загрузить активно
            ev.newGame = true; //надо для того, чтобы можно было: сохранить - проиграть - загрузить - сохранить - проиграть - загрузить
        }

        private void logic() //логика, которая ставит на паузу игру, перед сворачиваем/деактивацией формы, если игра не на стопе
        {
            if (ev.go) //если змейка шла
            {
                timer1.Enabled = false; //ставим на паузу
                ev.go = false; //она теперь стоит
                button2.Enabled = true; //можно будет продолжить
                button3.Enabled = true;//можно будет сохранить
            }
        }

        bool min = false; //нужно для корректного свертывания

        private void Form2_Resize(object sender, EventArgs e) //срабатывает при изменении размеров (свертывании и развертывании)
        {
            if (!end_game)
            switch (WindowState) //какой у нас размер окна?
            {
                case FormWindowState.Minimized: //свернуто
                    {
                        logic();
                        min = true; //размер минимальный
                    }
                    break;
                case FormWindowState.Normal:
                   if (min) { //если размер был минимальным, то все отрисовываем
                        //draw();
                        snake.draw_head(ev, g);
                        snake.draw_body(ev, g);
                        a.show(ev, g);
                        min = false; //размер больше не мин
                    }
                    break;
            }
        }

        private void Form2_Deactivate(object sender, EventArgs e) 
        {
            if (!end_game) //если игра не окончена (чтобы кнопки не активировались и не перерисовывалось поле с животными)
            {
                logic();
            }
        }

        private void Form2_Activated(object sender, EventArgs e)
        {

        }

    }
}
