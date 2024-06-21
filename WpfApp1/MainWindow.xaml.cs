using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool goLeft, goRight, start = false;
        bool grow = true;
        int points, prevSide, yetPrevSide = 0;
        int side = -1;
        int lifes = 3;
        double x = 504;
        double y = 691;
        double go;
        DispatcherTimer gameTimer = new DispatcherTimer();
        Random random = new Random();

        Rect[,] blocks = new Rect[6, 11];

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            go = random.NextDouble();

            Title = "Arkanoid Punkty: 0  Życia: 3";

            int k = 2;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    blocks[i, j] = new Rect(Canvas.GetLeft(GameArea.Children[k]), Canvas.GetTop(GameArea.Children[k]), 85, 23);
                    k++;
                }
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
            if (e.Key == Key.Up)
            {
                start = false;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.Up)
            {
                start = true;
            }
        }

        private void GameLogic()
        {
            Rect player = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            Rect ball = new Rect(x, y, Ball.Width, Ball.Height);
            if (side == -1 && go < 0.5)
            {
                x -= 10;
                y -= 10;
                if (x < 0)
                {
                    side = 1;
                }
                if (y <= 0)
                {
                    side = 2;
                    grow = false;
                    yetPrevSide = 3;
                }
            }
            else if (side == -1 && go > 0.5)
            {
                x += 10;
                y -= 10;
                if (x >= 990)
                {
                    side = 3;
                }
                if (y <= 0)
                {
                    side = 2;
                    grow = false;
                    yetPrevSide = 1;
                }
            }
            if (side == 0)
            {
                if (prevSide == 1)
                {
                    x += 10;
                    y -= 10;
                    yetPrevSide = 1;
                }
                else if (prevSide == 2 && yetPrevSide == 1)
                {
                    x += 10;
                    y -= 10;
                }
                else if (prevSide == 2 && yetPrevSide == 3)
                {
                    x -= 10;
                    y -= 10;
                }
                else if (prevSide == 3)
                {
                    x -= 10;
                    y -= 10;
                    yetPrevSide = 3;
                }
                if (x < 0)
                {
                    side = 1;
                    grow = true;
                    prevSide = 0;
                }
                if (y <= 0)
                {
                    side = 2;
                    grow = false;
                    prevSide = 0;
                }
                else if (x >= 990)
                {
                    side = 3;
                    grow = true;
                    prevSide = 0;
                }
            }
            if (side == 1 && grow)
            {
                x += 10;
                y -= 10;
                if (y <= 0)
                {
                    side = 2;
                    grow = false;
                    prevSide = 1;
                }
                else if (x >= 990)
                {
                    side = 3;
                    prevSide = 1;
                }
            }
            if (side == 1 && !grow)
            {
                x += 10;
                y += 10;
                if (x >= 990)
                {
                    side = 3;
                    prevSide = 1;
                }
            }
            if (side == 2)
            {
                if (prevSide == 0 && yetPrevSide == 1)
                {
                    x += 10;
                    y += 10;
                }
                if (prevSide == 0 && yetPrevSide == 3)
                {
                    x -= 10;
                    y += 10;
                }
                else if (prevSide == 1)
                {
                    x += 10;
                    y += 10;
                    yetPrevSide = 1;
                }
                else if (prevSide == 3)
                {
                    x -= 10;
                    y += 10;
                    yetPrevSide = 3;
                }
                if (x <= 0)
                {
                    side = 1;
                    prevSide = 2;
                }
                if (x >= 990)
                {
                    side = 3;
                    prevSide = 2;
                }
            }
            if (side == 3 && grow)
            {
                x -= 10;
                y -= 10;
                if (x < 0)
                {
                    side = 1;
                    prevSide = 3;
                }
                if (y <= 0)
                {
                    side = 2;
                    grow = false;
                    prevSide = 3;
                }
            }
            if (side == 3 && !grow)
            {
                x -= 10;
                y += 10;
                if (x < 0)
                {
                    side = 1;
                    prevSide = 3;
                }
            }
            if (player.IntersectsWith(ball) && side == 1)
            {
                side = 0;
                prevSide = 1;
            }
            if (player.IntersectsWith(ball) && side == 2)
            {
                side = 0;
                prevSide = 2;
            }
            if (player.IntersectsWith(ball) && side == 3)
            {
                side = 0;
                prevSide = 3;
            }

            if (y > 750 && !player.IntersectsWith(ball))
            {
                start = false;
                grow = true;
                side = -1;
                prevSide = 0;
                x = 504;
                y = 691;
                go = random.NextDouble();
                Canvas.SetLeft(Player, 470);
                lifes--;
                Title = "Arkanoid Punkty: " + points + "  Życia: " + lifes;
            }

            if (lifes == 0 || points == 330)
            {
                gameTimer.Stop();
                MessageBox.Show("Koniec gry\nZdobyte punkty: " + points);
            }
        }

        private void BlockLogic(Rectangle block)
        {
            Rect block0 = new Rect(Canvas.GetLeft(block), Canvas.GetTop(block) + block.Height, block.Width, 5);
            Rect block1 = new Rect(Canvas.GetLeft(block), Canvas.GetTop(block), 5, block.Height);
            Rect block2 = new Rect(Canvas.GetLeft(block), Canvas.GetTop(block), block.Width, 5);
            Rect block3 = new Rect(Canvas.GetLeft(block) + block.Width - 5, Canvas.GetTop(block), 5, block.Height);

            Rect ball = new Rect(x, y, Ball.Width, Ball.Height);
            if (ball.IntersectsWith(block0) && side == -1 && go > 0.5)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 1;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == -1 && go < 0.5)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 3;
                grow = false;
            }
            if (ball.IntersectsWith(block1) && side == -1 && go > 0.5)
            {
                side = 3;
                prevSide = 0;
                yetPrevSide = 1;
            }
            if (ball.IntersectsWith(block3) && side == -1 && go < 0.5)
            {
                side = 1;
                prevSide = 0;
                yetPrevSide = 3;
            }
            if (ball.IntersectsWith(block0) && side == 0 && prevSide == 1)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 1;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == 0 && prevSide == 2 && yetPrevSide == 1)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 1;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == 0 && prevSide == 2 && yetPrevSide == 3)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 3;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == 0 && prevSide == 3)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 3;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == 1)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 1;
                grow = false;
            }
            if (ball.IntersectsWith(block0) && side == 3)
            {
                side = 2;
                prevSide = 0;
                yetPrevSide = 3;
                grow = false;
            }
            if (ball.IntersectsWith(block1) && side == 1)
            {
                side = 3;
                prevSide = 1;
            }
            if (ball.IntersectsWith(block1) && side == 0 && prevSide == 1)
            {
                side = 3;
                prevSide = 0;
                yetPrevSide = 1;
            }
            if (ball.IntersectsWith(block2) && side == 2 && prevSide == 1)
            {
                side = 0;
                prevSide = 2;
                yetPrevSide = 1;
                grow = true;
            }
            if (ball.IntersectsWith(block2) && side == 2 && prevSide == 3)
            {
                side = 0;
                prevSide = 2;
                yetPrevSide = 3;
                grow = true;
            }
            if (ball.IntersectsWith(block2) && side == 2 && prevSide == 0 && yetPrevSide == 1)
            {
                side = 0;
                prevSide = 2;
                yetPrevSide = 1;
                grow = true;
            }
            if (ball.IntersectsWith(block2) && side == 2 && prevSide == 0 && yetPrevSide == 3)
            {
                side = 0;
                prevSide = 2;
                yetPrevSide = 3;
                grow = true;
            }
            if (ball.IntersectsWith(block2) && side == 1)
            {
                side = 0;
                prevSide = 1;
                grow = true;
            }
            if (ball.IntersectsWith(block2) && side == 3)
            {
                side = 0;
                prevSide = 3;
                grow = true;
            }
            if (ball.IntersectsWith(block3) && side == 3)
            {
                side = 1;
                prevSide = 3;
            }
            if (ball.IntersectsWith(block3) && side == 0 && prevSide == 3)
            {
                side = 1;
                prevSide = 0;
                yetPrevSide = 3;
            }
        }

        private void HitBlock(Rectangle block, int i, int j)
        {
            BlockLogic(block);
            GameArea.Children.Remove(block);
            blocks[i, j] = Rect.Empty;
            points += 5;
            Title = "Arkanoid Punkty: " + points + "  Życia: " + lifes;
        }

        private void GameEngine(object sender, EventArgs e)
        {
            if (goLeft && Canvas.GetLeft(Player) > 0)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - 30);
                if (side == -1 && !start)
                {
                    Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) - 30);
                    x -= 30;
                }
            }
            else if (goRight && Canvas.GetLeft(Player) < 920)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + 30);
                if (side == -1 && !start)
                {
                    Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + 30);
                    x += 30;
                }
            }
            if (start)
            {
                Rect ball = new Rect(x, y, Ball.Width, Ball.Height);
                GameLogic();

                if (ball.IntersectsWith(blocks[0, 0])) HitBlock(Block00, 0, 0);
                if (ball.IntersectsWith(blocks[0, 1])) HitBlock(Block01, 0, 1);
                if (ball.IntersectsWith(blocks[0, 2])) HitBlock(Block02, 0, 2);
                if (ball.IntersectsWith(blocks[0, 3])) HitBlock(Block03, 0, 3);
                if (ball.IntersectsWith(blocks[0, 4])) HitBlock(Block04, 0, 4);
                if (ball.IntersectsWith(blocks[0, 5])) HitBlock(Block05, 0, 5);
                if (ball.IntersectsWith(blocks[0, 6])) HitBlock(Block06, 0, 6);
                if (ball.IntersectsWith(blocks[0, 7])) HitBlock(Block07, 0, 7);
                if (ball.IntersectsWith(blocks[0, 8])) HitBlock(Block08, 0, 8);
                if (ball.IntersectsWith(blocks[0, 9])) HitBlock(Block09, 0, 9);
                if (ball.IntersectsWith(blocks[0, 10])) HitBlock(Block010, 0, 10);

                if (ball.IntersectsWith(blocks[1, 0])) HitBlock(Block10, 1, 0);
                if (ball.IntersectsWith(blocks[1, 1])) HitBlock(Block11, 1, 1);
                if (ball.IntersectsWith(blocks[1, 2])) HitBlock(Block12, 1, 2);
                if (ball.IntersectsWith(blocks[1, 3])) HitBlock(Block13, 1, 3);
                if (ball.IntersectsWith(blocks[1, 4])) HitBlock(Block14, 1, 4);
                if (ball.IntersectsWith(blocks[1, 5])) HitBlock(Block15, 1, 5);
                if (ball.IntersectsWith(blocks[1, 6])) HitBlock(Block16, 1, 6);
                if (ball.IntersectsWith(blocks[1, 7])) HitBlock(Block17, 1, 7);
                if (ball.IntersectsWith(blocks[1, 8])) HitBlock(Block18, 1, 8);
                if (ball.IntersectsWith(blocks[1, 9])) HitBlock(Block19, 1, 9);
                if (ball.IntersectsWith(blocks[1, 10])) HitBlock(Block110, 1, 10);

                if (ball.IntersectsWith(blocks[2, 0])) HitBlock(Block20, 2, 0);
                if (ball.IntersectsWith(blocks[2, 1])) HitBlock(Block21, 2, 1);
                if (ball.IntersectsWith(blocks[2, 2])) HitBlock(Block22, 2, 2);
                if (ball.IntersectsWith(blocks[2, 3])) HitBlock(Block23, 2, 3);
                if (ball.IntersectsWith(blocks[2, 4])) HitBlock(Block24, 2, 4);
                if (ball.IntersectsWith(blocks[2, 5])) HitBlock(Block25, 2, 5);
                if (ball.IntersectsWith(blocks[2, 6])) HitBlock(Block26, 2, 6);
                if (ball.IntersectsWith(blocks[2, 7])) HitBlock(Block27, 2, 7);
                if (ball.IntersectsWith(blocks[2, 8])) HitBlock(Block28, 2, 8);
                if (ball.IntersectsWith(blocks[2, 9])) HitBlock(Block29, 2, 9);
                if (ball.IntersectsWith(blocks[2, 10])) HitBlock(Block210, 2, 10);

                if (ball.IntersectsWith(blocks[3, 0])) HitBlock(Block30, 3, 0);
                if (ball.IntersectsWith(blocks[3, 1])) HitBlock(Block31, 3, 1);
                if (ball.IntersectsWith(blocks[3, 2])) HitBlock(Block32, 3, 2);
                if (ball.IntersectsWith(blocks[3, 3])) HitBlock(Block33, 3, 3);
                if (ball.IntersectsWith(blocks[3, 4])) HitBlock(Block34, 3, 4);
                if (ball.IntersectsWith(blocks[3, 5])) HitBlock(Block35, 3, 5);
                if (ball.IntersectsWith(blocks[3, 6])) HitBlock(Block36, 3, 6);
                if (ball.IntersectsWith(blocks[3, 7])) HitBlock(Block37, 3, 7);
                if (ball.IntersectsWith(blocks[3, 8])) HitBlock(Block38, 3, 8);
                if (ball.IntersectsWith(blocks[3, 9])) HitBlock(Block39, 3, 9);
                if (ball.IntersectsWith(blocks[3, 10])) HitBlock(Block310, 3, 10);

                if (ball.IntersectsWith(blocks[4, 0])) HitBlock(Block40, 4, 0);
                if (ball.IntersectsWith(blocks[4, 1])) HitBlock(Block41, 4, 1);
                if (ball.IntersectsWith(blocks[4, 2])) HitBlock(Block42, 4, 2);
                if (ball.IntersectsWith(blocks[4, 3])) HitBlock(Block43, 4, 3);
                if (ball.IntersectsWith(blocks[4, 4])) HitBlock(Block44, 4, 4);
                if (ball.IntersectsWith(blocks[4, 5])) HitBlock(Block45, 4, 5);
                if (ball.IntersectsWith(blocks[4, 6])) HitBlock(Block46, 4, 6);
                if (ball.IntersectsWith(blocks[4, 7])) HitBlock(Block47, 4, 7);
                if (ball.IntersectsWith(blocks[4, 8])) HitBlock(Block48, 4, 8);
                if (ball.IntersectsWith(blocks[4, 9])) HitBlock(Block49, 4, 9);
                if (ball.IntersectsWith(blocks[4, 10])) HitBlock(Block410, 4, 10);

                if (ball.IntersectsWith(blocks[5, 0])) HitBlock(Block50, 5, 0);
                if (ball.IntersectsWith(blocks[5, 1])) HitBlock(Block51, 5, 1);
                if (ball.IntersectsWith(blocks[5, 2])) HitBlock(Block52, 5, 2);
                if (ball.IntersectsWith(blocks[5, 3])) HitBlock(Block53, 5, 3);
                if (ball.IntersectsWith(blocks[5, 4])) HitBlock(Block54, 5, 4);
                if (ball.IntersectsWith(blocks[5, 5])) HitBlock(Block55, 5, 5);
                if (ball.IntersectsWith(blocks[5, 6])) HitBlock(Block56, 5, 6);
                if (ball.IntersectsWith(blocks[5, 7])) HitBlock(Block57, 5, 7);
                if (ball.IntersectsWith(blocks[5, 8])) HitBlock(Block58, 5, 8);
                if (ball.IntersectsWith(blocks[5, 9])) HitBlock(Block59, 5, 9);
                if (ball.IntersectsWith(blocks[5, 10])) HitBlock(Block510, 5, 10);

                Canvas.SetLeft(Ball, x);
                Canvas.SetTop(Ball, y);
            }
        }
    }
}
