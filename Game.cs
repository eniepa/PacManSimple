using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacManSimple
{
    public partial class Game : Form
    {
       private int heroStep = 5;
        int verVelocity = 0;
        int horVelocity = 0;

        int verEnemyVelocity = 0;
        int horEnemyVelocity = 0;
        int enemyStep = 3;

        int heroImageCount = 1;
        int score = 0;
        int enemyImageCount = 1;
        string enemyDirection = "left";
        bool gamePaused = false;
        string heroDirection = "right";
        Random Rand = new Random();
        
        public Game()
        {
            InitializeComponent();
            SetUpGame();
        }
        private void SetUpGame() {
            this.BackColor = Color.Blue;
            Hero.BackColor = Color.Transparent;
            Hero.SizeMode = PictureBoxSizeMode.StretchImage;
            Hero.Width = 50;
            Hero.Height = 50;

            Food.BackColor = Color.Transparent;
            Food.Image = Properties.Resources.food_1;
            RandomizeFood();
            Food.SizeMode = PictureBoxSizeMode.StretchImage;
           
            Enemy.BackColor = Color.Transparent;
            Enemy.SizeMode = PictureBoxSizeMode.StretchImage;
            Enemy.Height = 40;
            Enemy.Width = 40;
            // starting timer
            TimerMove.Start();
            TimerAnimate.Start();
            // score
            UpdateScoreLabel();

            randomChangeEnemyDirection();
        }

        private void HeroFoodCollision()
        {
            if (Hero.Bounds.IntersectsWith(Food.Bounds))
            {
                score += 100;
                RandomizeFood();
                UpdateScoreLabel();
            }
        }
        private void RandomizeFood()
        {
            Food.Left = Rand.Next(0, ClientRectangle.Width - Food.Width);
            Food.Top= Rand.Next(0, ClientRectangle.Height - Food.Height);
        }
        private void UpdateScoreLabel()
        {
            ScoreLabel.Text = "Score: " + score;
        }
        private void HeroBorderCollision() {
            if (Hero.Top + Hero.Height < 0) {
                Hero.Top = ClientRectangle.Height;
            }
            else if (Hero.Top > ClientRectangle.Height)
            {
                Hero.Top = 0 - Hero.Height;
            }
            if (Hero.Left + Hero.Width < 0)
            {
                Hero.Left = ClientRectangle.Width;

            }
            else if (Hero.Left > ClientRectangle.Width)
            {
                Hero.Left = 0 - Hero.Width;
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                verVelocity = -heroStep;
                horVelocity = 0;
                heroDirection = "up";
            }
            else if (e.KeyCode == Keys.Down)
            {
                verVelocity = heroStep;
                horVelocity = 0;
                heroDirection = "down";
            }
            else if (e.KeyCode == Keys.Left)
            {
                horVelocity = -heroStep;
                verVelocity = 0;
                heroDirection = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                horVelocity = heroStep;
                verVelocity = 0;
                heroDirection = "right";
            }
            else if (e.KeyCode == Keys.P) // noepiezot P ir pauze
            {
                if (!gamePaused)
                {
                    TimerAnimate.Stop();
                    TimerMove.Stop();
                    gamePaused = true;
                }
                else
                {
                    TimerAnimate.Start();
                    TimerMove.Start();
                    gamePaused = false;
                }
            }
            randomChangeEnemyDirection();
        }
        

        private void TimerMove_Tick(object sender, EventArgs e)
        {
            HeroMove();
            enemyMove();
        }

        private void TimerAnimate_Tick(object sender, EventArgs e)
        {
            HeroAnimate();
            EnemyAnimate();
        }
        private void HeroMove()
        {
            Hero.Top += verVelocity;
            Hero.Left += horVelocity;
            HeroBorderCollision();
            HeroFoodCollision();
            HeroEnemyCollision();
        }
        private void enemyMove()
        {
            Enemy.Top += verEnemyVelocity;
            Enemy.Left += horEnemyVelocity;
            EnemyBorderCollision();
        }
        private void HeroAnimate()
        {
            string heroImageName;
            heroImageName = "pacman_" + heroDirection + "_" + heroImageCount;
            Hero.Image = (Image)Properties.Resources.ResourceManager.GetObject(heroImageName);
            heroImageCount += 1;
            if (heroImageCount>4)
            {
                heroImageCount = 1;
            }
        }
        private void EnemyAnimate()
        {
            string enemyImageName;
            enemyImageName = "enemy_" + enemyDirection + "_" + enemyImageCount;
            Enemy.Image = (Image)Properties.Resources.ResourceManager.GetObject(enemyImageName);
            enemyImageCount += 1;
            if (enemyImageCount > 2)
            {
                enemyImageCount = 1;
            }
        }

        private void randomChangeEnemyDirection()
        {
            int directionCode = Rand.Next(1, 5);
            if (directionCode == 1)
            {
                enemyDirection = "right";
                verEnemyVelocity = 0;
                horEnemyVelocity = enemyStep;
            }
            if (directionCode == 2)
            {
                enemyDirection = "down";
                verEnemyVelocity = enemyStep;
                horEnemyVelocity = 0;
            }
            if (directionCode == 3)
            {
                enemyDirection = "left";
                verEnemyVelocity = 0;
                horEnemyVelocity = -enemyStep;
            }
            if (directionCode == 4)
            {
                enemyDirection = "up";
                verEnemyVelocity = -enemyStep;
                horEnemyVelocity = 0;
            }
        }

        private void ScoreLabel_Click(object sender, EventArgs e)
        {

        }
        private void EnemyBorderCollision()
        {
            if (Enemy.Top < 0)
            {
                enemyDirection = "down";
                EnemyBorderBounce();
            }
            else if (Enemy.Top + Enemy.Height > ClientRectangle.Height)
            {
                enemyDirection = "up";
                EnemyBorderBounce();
            }
            if (Enemy.Left < 0)
            {
                enemyDirection = "right";
                EnemyBorderBounce();
            }
            else if (Enemy.Left + Enemy.Width > ClientRectangle.Width)
            {
                enemyDirection = "left";
                EnemyBorderBounce();
            }
        }

        private void EnemyBorderBounce()
        {
            verEnemyVelocity *= -1;
            horEnemyVelocity *= -1;
        }
        private void HeroEnemyCollision()
        {
            if (Hero.Bounds.IntersectsWith(Enemy.Bounds))
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            TimerAnimate.Stop();
            TimerMove.Stop();
            heroImageCount = 0;
            TimerHeroMelt.Start();
            

        }

        private void TimerHeroMelt_Tick(object sender, EventArgs e)
        {
            string heroImageName;
            heroImageName = "pacman_melt_" + heroImageCount;
            Hero.Image = (Image)Properties.Resources.ResourceManager.GetObject(heroImageName);
            heroImageCount += 1;
            if (heroImageCount > 14)
            {
                TimerHeroMelt.Stop();
                LabelGameOver.Visible = true;
            }
        }
    }
}
