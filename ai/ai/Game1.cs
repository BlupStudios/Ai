using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ai
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D rifle;
        Vector2 riflePos;
        float rifleAngle = 0.0f;
        SpriteEffects se = SpriteEffects.None;
        float speed = 1.5f;

        SpriteFont arial;

        Texture2D aim;
        Vector2 aimPos;

        Texture2D bullet;
        Vector2 bulletPos;
        float bulletAngel;
        bool shot = false;
        float rifleAngleToBullt;
        Rectangle bulletRe;

        Texture2D ball;
        Vector2 ballPos;
        Rectangle ballRe;
        Vector2 direction = new Vector2(1, 1);

        int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rifle = Content.Load<Texture2D>("m4a");
            riflePos = new Vector2(Window.ClientBounds.Width / 2 - rifle.Width / 2, Window.ClientBounds.Height / 2 - rifle.Height / 2);
            arial = Content.Load<SpriteFont>("Arial");

            aim = Content.Load<Texture2D>("aim");
            bullet = Content.Load<Texture2D>("bulletA");

            ball = Content.Load<Texture2D>("ball");
            Random rndBall = new Random();
            ballPos = new Vector2(rndBall.Next(Window.ClientBounds.Width - ball.Width),rndBall.Next(Window.ClientBounds.Height - ball.Height));

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyState.IsKeyDown(Keys.W))
                riflePos.Y -= speed;

            if (keyState.IsKeyDown(Keys.S))
                riflePos.Y += speed;

            if (keyState.IsKeyDown(Keys.A))
                riflePos.X -= speed;

            if (keyState.IsKeyDown(Keys.D))
                riflePos.X += speed;

            if (keyState.IsKeyDown(Keys.PageUp))
                speed += 0.1f;

            if (keyState.IsKeyDown(Keys.PageDown))
                speed -= 0.1f;

            if (riflePos.X + rifle.Width + rifle.Width / 2 > Window.ClientBounds.Width)
                riflePos.X = Window.ClientBounds.Width - rifle.Width - rifle.Width / 2;

            if (riflePos.Y + rifle.Height / 2 > Window.ClientBounds.Height)
                riflePos.Y = Window.ClientBounds.Height - rifle.Height / 2;

            if (riflePos.X - rifle.Width - rifle.Width / 2 < 0)
                riflePos.X = rifle.Width + rifle.Width / 2;

            if (riflePos.Y - rifle.Height / 2 < 0)
                riflePos.Y = rifle.Height / 2;

            if (speed < 1)
                speed = 1;

            aimPos = new Vector2(mouse.X, mouse.Y);


            Vector2 mouseLoc = new Vector2(mouse.X, mouse.Y); 
            rifleAngle = (float)Math.Atan2(riflePos.Y - mouse.Y, riflePos.X - mouse.X);
            Vector2 direction = (riflePos) - mouseLoc;
            rifleAngle = (float)(Math.Atan2(direction.Y, direction.X));
            rifleAngle += MathHelper.ToRadians(270);

            if (mouse.LeftButton == ButtonState.Pressed && shot == false)
            {
                shot = true;
                rifleAngleToBullt = rifleAngle;
                bulletAngel = rifleAngle;
            }

            if (shot == true)
            {
                bulletPos += new Vector2((float)Math.Sin(rifleAngleToBullt), -(float)Math.Cos(rifleAngleToBullt)) * 15;
            }
            else
                bulletPos = riflePos;

            if (bulletPos.X >= Window.ClientBounds.Width || bulletPos.X <= 0 || bulletPos.Y >= Window.ClientBounds.Height || bulletPos.Y <= 0)
            {
                shot = false;
                bulletPos = riflePos;
            }
            bulletRe = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bullet.Width, bullet.Height);

            ballMove();

            if (bulletRe.Intersects(ballRe))
            {
                shot = false;
                bulletPos = riflePos;
                score++;
            }

            base.Update(gameTime);
        }

        void ballMove()
        {
            if (ballPos.X > Window.ClientBounds.Width - ball.Width / 2)
                direction.X = -1;

            if (ballPos.X < 0)
                direction.X = 1;

            if (ballPos.Y > Window.ClientBounds.Height - ball.Height / 2)
                direction.Y = -1;

            if (ballPos.Y < 0)
                direction.Y = 1;

            ballPos += direction * speed;

            ballRe = new Rectangle((int)ballPos.X, (int)ballPos.Y, ball.Width, ball.Height);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(arial, "The movment speed is " + speed, new Vector2(20, 10), Color.White);
            spriteBatch.DrawString(arial, "Your score is " + score, new Vector2(20, 30), Color.White);
            if(shot == true)
                spriteBatch.Draw(bullet, bulletPos, null, Color.White, bulletAngel, new Vector2(bullet.Width / 2, bullet.Height / 2), 0.1f, SpriteEffects.None, 1);
            spriteBatch.Draw(rifle, riflePos, null, Color.White, rifleAngle, new Vector2(rifle.Width / 2, rifle.Height / 2), 1f, se, 0f);
            spriteBatch.Draw(ball, ballPos, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            spriteBatch.Draw(aim, aimPos, null, Color.White, 0, new Vector2(aim.Width / 2, aim.Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
