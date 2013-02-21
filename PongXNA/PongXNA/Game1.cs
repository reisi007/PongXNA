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

namespace PongXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
     public enum Player { Red = 1, Blue = 0 }
     public class Game1 : Microsoft.Xna.Framework.Game
     {
         GraphicsDeviceManager graphics;
         SpriteBatch spriteBatch;
         Texture2D background;
         List<Ball> Balls = new List<Ball>();
         Texture2D ball_img;
         Vector2 spawn_pos;
         SpriteFont Font;
         public Random Rand;
         public Game1()
         {
             graphics = new GraphicsDeviceManager(this);
             Content.RootDirectory = "Content";
             Rand = new Random();
         }

         /// <summary>
         /// Allows the game to perform any initialization it needs to before starting to run.
         /// This is where it can query for any required services and load any non-graphic
         /// related content.  Calling base.Initialize will enumerate through any components
         /// and initialize them as well.
         /// </summary>
         protected override void Initialize()
         {
             // TODO: Add your initialization logic here
             background = Content.Load<Texture2D>("grass");
             ball_img = Content.Load<Texture2D>("ball");
             Font = Content.Load<SpriteFont>("BigFont");
# if DEBUG
             graphics.PreferredBackBufferHeight = 800;
             graphics.PreferredBackBufferWidth = 1300;
#else

            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ToggleFullScreen();
#endif
             graphics.ApplyChanges();
             spawn_pos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
             base.Initialize();
         }

         /// <summary>
         /// LoadContent will be called once per game and is the place to load
         /// all of your content.
         /// </summary>
         protected override void LoadContent()
         {
             // Create a new SpriteBatch, which can be used to draw textures.
             spriteBatch = new SpriteBatch(GraphicsDevice);
             // TODO: use this.Content to load your game content here
         }

         /// <summary>
         /// UnloadContent will be called once per game and is the place to unload
         /// all content.
         /// </summary>
         protected override void UnloadContent()
         {
             // TODO: Unload any non ContentManager content here
         }

         /// <summary>
         /// Allows the game to run logic such as updating the world,
         /// checking for collisions, gathering input, and playing audio.
         /// </summary>
         /// <param name="gameTime">Provides a snapshot of timing values.</param>
         int sec = 0;
         const int next_min = 1000;
         int next = 1700;
         protected override void Update(GameTime gameTime)
         {
             // Allows the game to exit
             if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                 this.Exit();
             if (gameTime.TotalGameTime.Seconds % 10 == 0)
             {
                 if (next > next_min)
                     next -= 50;
             }
             if (sec >= next)
             {
                 Balls.Add(new Ball(new Vector2(Rand.Next(-5, 5), Rand.Next(-5, 5)), spawn_pos, ball_img, spriteBatch, 5, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height)));
                 sec -= next;
             }
             sec += gameTime.ElapsedGameTime.Milliseconds;
             for (int i = 0; i < Balls.Count; i++)
             {
                 if (Balls[i].LifeTime < 0)
                 {
                     Balls.RemoveAt(i);
                     i--;
                 }
             }
             // Collision detection
             for (int i = 0; i < Balls.Count; i++)
             {
                 for (int y = 0; y < Balls.Count; y++)
                 {
                     if ((y != i) /*&& !((Balls[y].Fluent > 0) || (Balls[i].Fluent > 0))*/)
                     {
                         tmp = (float)(Math.Pow(Balls[i].Position.X - Balls[y].Position.X, 2) + Math.Pow(Balls[i].Position.Y - Balls[y].Position.Y, 2));
                         if (tmp <= (d * d))
                         {
                             if(!(Balls[i].Collision && Balls[y].Collision))
                                Ball_Collide( Balls[i].Direction,  Balls[y].Direction, Balls[i].Position, Balls[y].Position, ref Balls[i].Reflection,ref Balls[y].Reflection);
                             Balls[i].Collision = true;
                             Balls[y].Collision = true;
                         }
                     }
                 }
             }
             foreach (Ball b in Balls)
             {
                 b.Update(gameTime);
             }
             base.Update(gameTime);
         }
         // Variables for Collision detection
         int d = 50;
         float tmp;
         /// <summary>
         /// This is called when the game should draw itself.
         /// </summary>
         /// <param name="gameTime">Provides a snapshot of timing values.</param>
         protected override void Draw(GameTime gameTime)
         {
             GraphicsDevice.Clear(Color.CornflowerBlue);
             spriteBatch.Begin();
             // Draw background
             for (int x = 0; x < Window.ClientBounds.Width; x += background.Width)
                 for (int y = 0; y < Window.ClientBounds.Height; y += background.Height)
                     spriteBatch.Draw(background, new Vector2(x, y), Color.White);
             foreach (Ball b in Balls)
                 b.Draw();
             /* spriteBatch.Draw(ball_img,new Rectangle(5,5, (int)Font.MeasureString(p2).X + 5,(int)Font.MeasureString(p2).Y + 5),Color.DarkBlue);
              spriteBatch.Draw(ball_img, new Rectangle(Window.ClientBounds.Width - (int)Font.MeasureString(p1).X -15,5,(int)Font.MeasureString(p1).X +5,(int)Font.MeasureString(p1).Y +5),Color.DarkRed);*/
             spriteBatch.DrawString(Font, p2, new Vector2(10), Color.DarkRed);
             spriteBatch.DrawString(Font, p1, new Vector2(Window.ClientBounds.Width - (int)Font.MeasureString(p1).X - 10, 10), Color.DarkBlue);
             spriteBatch.DrawString(Font, Convert.ToString(p2s), new Vector2(5, Window.ClientBounds.Height - Font.MeasureString(Convert.ToString(p1s)).Y), Color.DarkRed);
             spriteBatch.DrawString(Font, Convert.ToString(p1s), new Vector2(Window.ClientBounds.Width - Font.MeasureString(Convert.ToString(p1s)).X - 5, Window.ClientBounds.Height - Font.MeasureString(Convert.ToString(p1s)).Y), Color.DarkBlue);
             spriteBatch.End();

             base.Draw(gameTime);
         }
         string p1 = "Player 1 ";
         string p2 = "Player 2 ";
         int p1s = 0, p2s = 0;
         Vector2 AB;
         private void Ball_Collide( Vector2 A_Dir,  Vector2 B_Dir, Vector2 A_Pos, Vector2 B_Pos, ref Vector2 A_Ref , ref Vector2 B_Ref)
         {
             AB = B_Pos - A_Pos;
             AB.Normalize();
             A_Ref = Vector2.Reflect(A_Dir, AB);
             B_Ref = Vector2.Reflect(B_Dir, -AB);
             A_Ref.Normalize();
             B_Ref.Normalize();
         }
     }
}
