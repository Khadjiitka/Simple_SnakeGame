using System.Diagnostics;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int randomI, randomJ;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[400]; // Массив для хранения сегментов змейки
        private int length = 1; // начальная длина змейки (1 сегмент — голова)
        //private int score = 0;
        private int dirX, dirY;//движение по X и Y 
        private int prevDirX = 1, prevDirY = 0;//переменные для хранения предыдущего направления
        private int mapWidth = 811;
        private int mapHeight = 801;
        int sizeOfSides = 40;
        private bool isGameOver = false;
        public Form1()
        {
            InitializeComponent();
            this.Width = mapWidth ;
            this.Height = mapHeight;
            base.Text = "Snake";

            fruit = new PictureBox();
            fruit.Size = new Size(sizeOfSides, sizeOfSides);
            fruit.BackColor = Color.Yellow;
            _generateMap();
            _generateFruit();
            dirX = 1;
            dirY = 0;
            snake[0] = new PictureBox();
            snake[0].Location = new Point(200,200);
            snake[0].Size = new Size (sizeOfSides, sizeOfSides);
            snake[0].BackColor = Color.Green;
            this.Controls.Add(snake[0]);
            
            this.KeyDown += new KeyEventHandler(OKP);// Обработчик нажатий клавиатуры или this.KeyDown += OKP;
            timer1.Tick += new EventHandler(UpDate);//обработчик событий таймера
            timer1.Interval = 200;// миллисекунд между обновлениями
            timer1.Start();
        }

        private void _generateMap()
        {
            for (int i = 0; i<= mapWidth/sizeOfSides; i++) //горизонтальные линии
            {
                PictureBox line = new PictureBox();
                line.BackColor = Color.Black;
                line.Location = new Point(sizeOfSides*i,0);
                line.Size = new Size(1, mapHeight);
                this.Controls.Add(line);
            }
            for (int i = 0; i <= mapHeight/sizeOfSides; i++) //вертикальные линии
            {
                PictureBox line = new PictureBox();
                line.BackColor = Color.Black;
                line.Location = new Point(0, sizeOfSides * i);
                line.Size = new Size(mapWidth, 1);
                this.Controls.Add(line);
            }
        }
        private void _generateFruit()
        {
            Random rnd = new Random();
            randomI = rnd.Next(0, (Width - sizeOfSides) / sizeOfSides) * sizeOfSides;
            randomJ = rnd.Next(0, (Height - sizeOfSides) / sizeOfSides) * sizeOfSides;

            fruit.Location = new Point(randomI,randomJ);
            this.Controls.Add(fruit);
        }
        private void _eatFruit()
        {
            if (snake[0].Location == fruit.Location)
            {
                length++; // увеличиваем длину змейки
                snake[length - 1] = new PictureBox();
                snake[length - 1].Size = new Size(sizeOfSides, sizeOfSides);
                snake[length - 1].BackColor = Color.Green;
                snake[length - 1].Location = new Point(-100, -100); // временно за пределами экрана
                this.Controls.Add(snake[length - 1]);

                _generateFruit(); // сгенерировать новый фрукт
            }
        }
        private void OKP(object sender, KeyEventArgs e) // Обработка нажатий клавиш
        {
            if (isGameOver) return;
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    // cube.Location = new Point(cube.Location.X + sizeOfSides, cube.Location.Y);
                    if (prevDirX != -1)
                        { 
                        dirX = 1;
                        dirY = 0;
                        } 
                    break;
                case "Left":
                    //cube.Location = new Point(cube.Location.X- sizeOfSides, cube.Location.Y);
                    if (prevDirX != 1)
                    {
                        dirX = -1;
                        dirY = 0;
                    }
                    break;
                case "Up":
                    //cube.Location = new Point(cube.Location.X, cube.Location.Y-sizeOfSides);
                    if (prevDirY != 1)
                    {
                        dirY = -1;
                        dirX = 0;
                    }
                    break;
                case "Down":
                    //cube.Location = new Point(cube.Location.X, cube.Location.Y+sizeOfSides);
                    if (prevDirY != -1)
                    {
                        dirY = 1;
                        dirX = 0;
                    }
                    break;

            }
        }
        private void _moveSnake()
        {
            for (int i = length - 1; i > 0; i--)// Перемещаем тело: каждый сегмент становится на место предыдущего
            {
                snake[i].Location = snake[i - 1].Location;
            }
            // Двигаем голову вперёд
            snake[0].Location = new Point(snake[0].Location.X + dirX * sizeOfSides, snake[0].Location.Y + dirY * sizeOfSides);
            prevDirX = dirX;
            prevDirY = dirY;
            _eatItself();
        }
        private void _eatItself()
        {
            for (int i = 1; i < length; i++)
            {
                if (snake[i] != null && snake[i].Location == snake[0].Location)
                {
                    GameOver();
                }
            }
        }
        private void CheckCollisionWithWall()
        {
            // Выход за границы поля
            int x = snake[0].Location.X;
            int y = snake[0].Location.Y;

            if (x < 0 || x >= this.Width || y < 0 || y >= this.Height)
            {
                GameOver();
            }
        }
        private void UpDate (object sender, EventArgs e) // Основной игровой цикл
        {
            _moveSnake();
            _eatFruit();
            CheckCollisionWithWall();
            //cube.Location = new Point(cube.Location.X + dirX *sizeOfSides, cube.Location.Y+dirY*sizeOfSides);

        }
        private void GameOver()
        {
            isGameOver = true;
            timer1.Stop();

            Label gameOverLabel = new Label();
            gameOverLabel.Text = "Game Over";
            gameOverLabel.Font = new Font("Arial", 32, FontStyle.Bold);
            gameOverLabel.Size = new Size(300, 60);
            gameOverLabel.Location = new Point(this.Width / 2 - 150, this.Height / 2 - 100);
            gameOverLabel.TextAlign = ContentAlignment.MiddleCenter;
            gameOverLabel.BackColor = Color.Red;
            gameOverLabel.ForeColor = Color.White;
            gameOverLabel.BringToFront();
            this.Controls.Add(gameOverLabel);

            Button restartButton = new Button();
            restartButton.Text = "Restart";
            restartButton.Font = new Font("Arial", 14, FontStyle.Bold);
            restartButton.Size = new Size(100, 50);
            restartButton.Location = new Point(this.Width / 2 - 50, this.Height / 2);
            restartButton.Click += RestartButton_Click;
            this.Controls.Add(restartButton);
        }
        private void RestartButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
