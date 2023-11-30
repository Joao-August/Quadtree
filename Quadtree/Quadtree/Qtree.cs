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
    class Qtree
    {
        //limits of tree && children
        public Rectangle boundary;
        public Rectangle nw, ne, sw, se;
        public Qtree qNw, qNe, qSw, qSe;

        public Texture2D texture;

        // max bullets in each node
        public const int capacity = 1;
        public int container;

        public List<Bullets> bullets;

        bool minimumBondary = true, maximumSubdivide = false;
        public static int maxSubdivide = 5, minboundaryWidth, minBoundaryHeight;
        Rectangle minBondary;

        public Qtree(Rectangle boundary, List<Bullets> bullets, Texture2D texture)
        {
            maxSubdivide *= 4;
            minBondary = new Rectangle(0, 0, Bullets.width, Bullets.height);
            minboundaryWidth = Bullets.width;
            minBoundaryHeight = Bullets.height;

            this.texture = texture;
            this.boundary = boundary;
            this.bullets = bullets;

            if (maximumSubdivide)
                if (Qtree.maxSubdivide-- <= 0)
                    return;

            if (minimumBondary)
                if (this.boundary.Width/2 < minboundaryWidth || this.boundary.Height/2 < minBoundaryHeight)
                    return;
            

            foreach (Bullets b in bullets)
            {
                if (this.boundary.Intersects(b.rect))
                {
                    //Console.WriteLine(this.container);
                    if (++container > capacity)
                    {
                        Subdivide();
                        break;
                    }
                }
            }


        }

        public void Update(GameTime gameTime)
        {
            //check if divided, if not then Subdivide();
        }

        public void Subdivide()
        {
            int x = this.boundary.X;
            int w = this.boundary.Width;
            int y = this.boundary.Y;
            int h = this.boundary.Height;

            nw = new Rectangle(x, y, w / 2, h / 2);
            ne = new Rectangle(x + (w / 2), y, w / 2, h / 2);
            sw = new Rectangle(x, y + (h / 2), w / 2, h / 2);
            se = new Rectangle(x + (w / 2), y + (h / 2), w / 2, h / 2);

            List<Bullets> temp = new List<Bullets>();
            foreach (Bullets b in bullets)
            {
                if (nw.Intersects(b.rect))
                {
                    temp.Add(b);
                }
            }
            qNw = new Qtree(nw, temp,texture);

            temp.Clear();
            foreach (Bullets b in bullets)
            {
                if (ne.Intersects(b.rect))
                {
                    temp.Add(b);
                }
            }
            qNe = new Qtree(ne, temp, texture);

            temp.Clear();
            foreach (Bullets b in bullets)
            {
                if (sw.Intersects(b.rect))
                {
                    temp.Add(b);
                }
            }
            qSw = new Qtree(sw, temp, texture);

            temp.Clear();
            foreach (Bullets b in bullets)
            {
                if (se.Intersects(b.rect))
                {
                    temp.Add(b);
                }
            }
            qSe = new Qtree(se, temp, texture);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, boundary, Color.Purple);

            spriteBatch.Draw(texture, nw, Color.Purple);
            spriteBatch.Draw(texture, ne, Color.Purple);
            spriteBatch.Draw(texture, sw, Color.Purple);
            spriteBatch.Draw(texture, se, Color.Purple);
            if (qNw!=null)
                qNw.Draw(gameTime, spriteBatch); 
            if (qNe != null)
                qNe.Draw(gameTime, spriteBatch); 
            if (qSw != null)
                qSw.Draw(gameTime, spriteBatch);
            if (qSe != null)
                qSe.Draw(gameTime, spriteBatch);
        }

    }
}
