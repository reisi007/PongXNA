using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongXNA
{
   public class Ball
    {
        Player Player;
        public bool Collision;
        int current_index = 0;
        public Vector2 Position, Direction;
        Color[][] Spherecol = new Color[2][];
        Texture2D Texture;
        int m_sec_change_graphic = 120;
        int m_sec_passed = 0;
        Rectangle ClientBound;
        Random rand;
        private Rectangle BounceRectangle { get { return new Rectangle((int)(Position.X - 25), (int)(Position.Y - 25), 50, 50); } }
        SpriteBatch batch;
        float Speed;
        public int LifeTime = 15000;
        public int Fluent = 600;
        public Ball(Vector2 dir,Vector2 pos, Texture2D ball, SpriteBatch spritebatch, float speed, Rectangle client)
        {
            rand = new Random();
            Speed = speed;
            ClientBound = client;
            dir.Normalize();
            Texture = ball;
            batch = spritebatch;
            Direction = dir;
            Position = pos;
            if( dir.X > 0)
                Player = Player.Red;
            else
                Player = Player.Blue;
            # region SphereColorArray Init
            Spherecol[0] = new Color[6];
            Spherecol[1] = new Color[6];

            Spherecol[0][0] = new Color(3, 17, 172);
            Spherecol[0][1] = new Color(4, 20, 197);
            Spherecol[0][2] = new Color(4, 22, 222);
            Spherecol[0][3] = new Color(5, 25, 247);
            Spherecol[0][5] = new Color(4, 20, 197);
            Spherecol[0][4] = new Color(4, 22, 222);
            
            Spherecol[1][0] = new Color(172, 3, 17);
            Spherecol[1][1] = new Color(197, 4, 20);
            Spherecol[1][2] = new Color(222, 4, 22);
            Spherecol[1][3] = new Color(247, 5, 25);
            Spherecol[1][5] = new Color(197, 4, 20);
            Spherecol[1][4] = new Color(222, 4, 22);
            #endregion

            
        }
        public void Update(GameTime gametime)
        {
            m_sec_passed += gametime.ElapsedGameTime.Milliseconds;
            if (m_sec_passed > m_sec_change_graphic)
            {
                m_sec_passed -= m_sec_change_graphic;
                current_index++;
                current_index %= 6;
            }
            // Collision detection here:
            // Ball
                // Top & Bottom
                if (BounceRectangle.Top < ClientBound.Top || BounceRectangle.Bottom > ClientBound.Bottom)
                    Direction.Y *= -1;
                // Left & Right
                if (BounceRectangle.Left < ClientBound.Left)
                {
                    Direction.X *= -1;
                    Player = PongXNA.Player.Red;
                }
                if (BounceRectangle.Right > ClientBound.Right)
                {
                    Direction.X *= -1;
                    Player = PongXNA.Player.Blue;
                }
            
            LifeTime -= gametime.ElapsedGameTime.Milliseconds;
            Fluent -= gametime.ElapsedGameTime.Milliseconds;
            if (Collision)
            {
                Direction *= -1;
                Collision = false;
                Fluent = 600;
                Direction.X += rand.Next(-1, 1);
                Direction.Normalize();
            }
            Position += (Direction * Speed);
           
        }
        public void Draw()
        {
            batch.Draw(Texture, BounceRectangle , Spherecol[(int)Player][current_index]);
        }
    }
}
