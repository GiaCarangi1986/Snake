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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists("..\\downlong.dat"))
                button2.Enabled = false; //если нет сохраненной игры, то загрузка игры не активна
            if (!File.Exists("..\\game.dat"))
                button5.Enabled = false; //если нет таблицы рекордов, то соответствующая кнопка не активна
        }

        private void button1_Click(object sender, EventArgs e) //начать игру
        { //если есть сохраненная игра, то нужно предупредить об этом
            if (File.Exists("..\\downlong.dat"))
            { 
                DialogResult result = MessageBox.Show(
              "Если вы начнете новую игру, то старая будет утеряна",
              "Вы уверены?",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Warning,
              MessageBoxDefaultButton.Button2,
              MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes) //если все же начинаем заново, то удаляем сохраненную игру и начинаем новую
                {
                    File.Delete("..\\downlong.dat");
                    button2.Enabled = false;
                    Hide(); //надо?
                    Form2 f2 = new Form2(20, 20, this);
                    f2.Show();
                    f2.Activate();
                }
                else
                    Activate();
            }
            else //если сохраненной игры нет, то загружаем новую
           {
                Hide(); //надо?
                Form2 f2 = new Form2(20, 20, this);
                f2.Show();
                f2.Activate();
            }
        }

        private void button2_Click(object sender, EventArgs e) //загрузка игры
        {
                Events ev = new Events();
                ev.newGame = false;
                ev.newGame_const = false;
                Hide();
                Form2 f2 = new Form2(20, 20, this, ev); //вызываем конструктор для загрузки
                f2.Show();
                f2.Activate();
        }

        private void button4_Click(object sender, EventArgs e) //выход (закрываем форму)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e) //таблица рекордов
        {
            int[] rec = new int[10]; 
            string path = "..\\game.dat";
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    // пока не достигнут конец файла
                    // считываем каждое значение из файла
                    int i = 0;
                    while (reader.PeekChar() > -1)
                    {
                        rec[i] = reader.ReadInt32();
                        i++;
                    }
                }
                Array.Sort(rec);
                Form3 f3 = new Form3();
                f3.textBox1.Text += "\r\nТаблица рекордов\r\n";
                for (int i = rec.Length-1; i>=0; i--)
                    f3.textBox1.Text += "\r\n" + (rec[i]).ToString();
                f3.Text = "Таблица рекордов";
                f3.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e) //правила 
        {
              Form3 f3 = new Form3();
            f3.textBox1.Text = "\r\nПравила игры";
            f3.textBox2.Visible = true;
            f3.Text = "Правила игры";
            f3.textBox2.Text += "\r\nСУТЬ ИГРЫ:";
            f3.textBox2.Text += "\r\n   Управляя змейкой, нужно набрать как можно больше очков. Для этого необходимо " +
                "есть кроликов и уворачиваться от мангустов. За каждое их этих действий прибавляется 1 очко. " +
                "С каждым новым очком скорость игры увеличивается до некоторого конечного значения. Игра заканчивается," +
                " если Вы проиграли, то есть врезались в стенку, в себя или попали на мангуста.";
            f3.textBox2.Text += "\r\n";
            f3.textBox2.Text += "\r\nФУНКЦИОНАЛЬНЫЕ ВОЗМОЖНОСТИ ИГРЫ:";
            f3.textBox2.Text += "\r\n   В главном меню существуют две основные возможности: начать новую игру или загрузить" +
                " предыдущую. Если загруженной игры нет, то нужно начать новую. Если же у вас была загруженная игра, то вы " +
                "можете либо поиграть в нее, либо начать новую, при этом старая игра сотрется.";
            f3.textBox2.Text += "\r\n   В самой игре у вас также существуют возможности: поставить на паузу/продолжить, " +
                "сохранить или выйти в главное меню.";
            f3.textBox2.Text += "\r\n   В загрузке в главном меню хранится Ваша сохраненная игра (если такая имеется). Если " +
                "Вы решите доиграть ее и проиграете, то игра будет считаться оконченной и сотрется из загрузки (чтобы " +
                "поиграть, нужно будет начать новую игру).";
            f3.textBox2.Text += "\r\n   Набранные Вами очки после каждой оконченной игры попадают в таблицу рекордов. " +
                "В ней хранятся 10 самых лучших результатов. Логично, что, если вы сыграли меньше 10 раз или не набрали " +
                "очков, то результат несыгранной игры равен 0.";
            f3.textBox2.Text += "\r\n";
            f3.textBox2.Text += "\r\n\nУПРАВЛЕНИЕ:";
            f3.textBox2.Text += "\r\n   Для передвижения змейкой используйте клавиши WASD";
            f3.textBox2.Text += "\r\n   Для навигации в меню можно использовать мышку или стрелки на клавиатуре " +
                "и клавишу ENTER";
            f3.textBox2.Text += "\r\n";
            f3.textBox2.Text += "\r\n\nТЕХНИЧЕСКИЕ ОСОБЕННОСТИ:";
            f3.textBox2.Text += "\r\n   Игровое поле и главное меню разрешается сворачивать и разворачивать. Игра при этом" +
                " автоматически поставится на паузу. Можно также перемещать форму, но нельзя изменять ее размеры и " +
                "разворачивать на весь экран.";
            f3.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.textBox1.Text = "\r\nИнформация о продукте";
            f3.Text = "О продукте";
            f3.textBox1.Text += "\r\n";
            f3.textBox1.Text += "\r\n   Система состоит из одного основного проекта — “ANACONDA”";
            f3.textBox1.Text += "\r\n   Игра предназначена для развлечения, скоротания времени";
            f3.textBox1.Text += "\r\n   Основанием для разработки является «Учебный план по программе бакалавриата";
            f3.textBox1.Text += "\r\n   Направление 09.03.04 Программная инженерия. Профиль: Разработка программно -информационных систем»";
            f3.textBox1.Text += "\r\n   Copyright © Курочкина Елизавета";
            f3.textBox1.Text += "\r\n   Все права защищены. Играйте с удовольствием! 2020 г.";
            f3.ShowDialog();
        }
    }
    }
