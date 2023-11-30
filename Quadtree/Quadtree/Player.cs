using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Quadtree
{
    class Player : GameObject
    {
        int life;
        int speed;
        Texture2D texture;
        public Rectangle rect;

        public Player(int posX, int posY, int width, int height, int life, int speed, Texture2D texture) : base()
        {
            this.posX = posX;
            this.posY = posY;
            this.width = width;
            this.height = height;
            this.life = life;
            this.speed = speed;
            this.texture = texture;
            rect = new Rectangle(posX, posY, width, height);
        }

        public void Update(GameTime gameTime)
        {
            Move();
        }

        void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                posX = ((posX - speed) > 0) ? posX - speed : 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                posX = ((posX + width + speed) < Game1.screenSizeWidth) ? posX + speed : Game1.screenSizeWidth - width;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                posY = ((posY - speed) > 0) ? posY - speed : 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                posY = ((posY + height + speed) < Game1.screenSizeHeight) ? posY + speed : Game1.screenSizeHeight - height;
            }
            rect = new Rectangle(posX, posY, width, height);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
        
    }
}
