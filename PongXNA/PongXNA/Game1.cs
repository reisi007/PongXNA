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
        Random Rand;
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
# if DEBUG
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1800;
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
        int sec = 1000;
        int next = 2000;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (sec >= next)
            {
                Balls.Add(new Ball(new Vector2(Rand.Next(-5, 5), Rand.Next(-5, 5)), spawn_pos, ball_img, spriteBatch, 5, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height)));
                sec -= next;
            }
            sec += gameTime.ElapsedGameTime.Milliseconds;
            foreach (Ball b in Balls)
                b.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
