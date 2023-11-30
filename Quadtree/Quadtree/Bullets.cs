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
    class Bullets
    {
        public int posX, posY;
        public static int width = 10, height = 10;
        public Texture2D texture;
        public int speed;

        public Rectangle rect;

        public Color color = Color.White;
        public bool isColliding;

        public Bullets(int posX, int posY, Texture2D texture, int speed)
        {
            this.posX = posX;
            this.posY = posY;
            this.texture = texture;
            this.speed = speed;

            rect = new Rectangle(posX, posY, width, height);
        }

        public void Update(GameTime gameTime)
        {
            if ((posY + height + speed) < Game1.screenSizeHeight + 100)
                posY += speed;
            else
            {
                posY = -50;
                posX = new Random().Next(0, Game1.screenSizeWidth - width);
                //speed = new Random().Next(1, 3);
            }
            //posY = ((posY + height + speed) < Game1.screenSizeHeight+100) ? posY + speed : -100;
            rect = new Rectangle(posX, posY, width, height);
            isColliding = false;
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
            if (isColliding)
                color = Color.Red;
            else
                color = Color.White;
            spriteBatch.Draw(texture, rect, color);
        }
    }
}
