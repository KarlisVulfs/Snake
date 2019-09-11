using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
   public class Game
   {
        public enum Direction//parskaitījums
        {
            Left,
            Right,
            Up,
            Down
        };

        const int WIDTH = 60;// konstantus parasti definē ar lielajiem burtiem
        const int HEIGHT = 20;
        const int PAUSE_TIME = 40;// 1000 = 1 sekunde
        Position head;// snake galva
        Position fruit;// snake auglis kas jaapēd
        Direction direction; // virziens kāda parvietojas
        Direction previousDirection;
        List<Position> tail;
        int score;


        bool gameOver = false;
        public void Start()// Jaunas spēles uzsākšana
        {
            Console.SetWindowSize(WIDTH + 2, HEIGHT + 3);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorVisible = false;

            gameOver = false;
            direction = Direction.Right;
            head = new Position(WIDTH / 2, HEIGHT / 2);// JAUNS OBJEKTS, JAUNA STRUKTURA TAPEC LIEK NEW
            MoveFruit();
            tail = new List<Position>(); //jauns tukš saraksts
            score = 0;
        }

        public void Refresh()// atjauno lietotajam redzamo dalu
        {
            while (!gameOver)//! = nav
            //while (gameOver == false)
            {
                Input();
                Logic();
                Render();
                int speed = score / 10;

                Thread.Sleep(PAUSE_TIME - speed);// jo skaitlis lielaks jo snake lenak parvietosies
            }
            if(gameOver)
            {
                Lose();
            }
            else
            {
                Win();
            }
                
        }
        public void Input()// lietotāja ievades apstrāde
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);// ture tikai nolasa bet neetēlo
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        previousDirection = direction;
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        previousDirection = direction;
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        previousDirection = direction;
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        previousDirection = direction;
                        direction = Direction.Down;
                        break;
                }
            }
        }
        public void Logic()// spēles loģika kas veic aprēķinus
        {
            tail.Insert(0, new Position(head.X, head.Y));
            tail.RemoveAt(tail.Count - 1);


            switch(direction)
            {
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Up:
                    head.Y--;
                    break;
                case Direction.Left:
                    head.X--;
                    break;
                case Direction.Right:
                    head.X++;
                    break;
            }
            if(head.X == fruit.X && head.Y == fruit.Y)
            {
                tail.Add(new Position(fruit.X, fruit.Y));
                MoveFruit();
                score += 10;
            }
            // vai bija sadursme ar sienu
            else if(head.X == WIDTH || head.X == 0 || head.Y == HEIGHT || head.Y == 0)
            {
                gameOver = true;
            }
            else
            {
                if ((previousDirection == Direction.Down && direction == Direction.Up)
                        || (previousDirection == Direction.Up && direction == Direction.Down)
                        || (previousDirection == Direction.Left && direction == Direction.Right)
                        || (previousDirection == Direction.Right && direction == Direction.Left))
                {
                    // atļauta virziena maiņa
                }
                else
                {

                    foreach (Position p in tail)
                    {
                        if (p.X == head.X && p.Y == head.Y)
                        {
                            gameOver = true;
                            break;
                        }
                    }
                }
            }

        }
        public void MoveFruit()
        {
            Random rnd = new Random();
            fruit = new Position(rnd.Next(1, WIDTH), rnd.Next(1, HEIGHT));
        }


        public void Render()// atēlošana
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y <= HEIGHT; y++)
            {
                for (int x = 0; x <= WIDTH; x++)
                {
                    if (head.X == x && head.Y == y)
                    {
                        Console.Write("0");
                    }
                    else if(fruit.X == x && fruit.Y ==y)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("*");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if(x == WIDTH || y == HEIGHT || x == 0 || y == 0)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        // ja aste satur x,y koordinatas tad attēlo simbolu'o'
                        bool isTail = false;

                       foreach(Position p in tail)
                       {
                            if(p.X == x && p.Y ==y)
                            {
                                Console.Write("o");
                                isTail = true;
                            }
                       }
                        // attēlo tukšumu pozīcijā x,y
                        if (isTail == false) 
                        {
                            Console.Write(" ");
                        }
                            
                    }                                        
                }
                    Console.WriteLine();
            }
            Console.WriteLine("Rezultats: {0}", score);
        }
        public void Lose()
        {
            Console.Clear();
            //Console.CursorLeft = 30;
            Console.WriteLine("Tu zaudēji");
            Console.WriteLine("Rezultats: {0}", score);
            //1. vaicā vai sākt no jauma
            Console.WriteLine("Vai sāksi jaunu spēli(J/N)? ");
            string response = Console.ReadLine();
            //2.Uzsāk jaunu spēli vai aizver programmu
            if(response.ToLower() == "j")
            {
                Start();
                Refresh();
            }
            else
            {
                Environment.Exit(0);
            }
           
        }
        public void Win()
        {

        }
   }
}
