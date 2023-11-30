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
    class QuadtreeColision
    {
        Player player;
        public Rectangle boundary;
        public Rectangle nw, ne, sw, se;
        public QuadtreeColision qNw, qNe, qSw, qSe;

        public Texture2D texture;

        // max bullets in each node
        public const int capacity = 1;

        public List<Bullets> bullets;

        bool minimumBondary = true;
        int minboundaryWidth, minBoundaryHeight;
        //bool maximumSubdivide = false;
        //public static int maxSubdivide = 5;

        public QuadtreeColision(Rectangle boundary, List<Bullets> bullets, Texture2D texture, Player player)
        {
            this.texture = texture;
            this.boundary = boundary;
            this.bullets = bullets;
            this.player = player;

            //maxSubdivide *= 4;
            //if (maximumSubdivide)
            //    if (Qtree.maxSubdivide-- <= 0)
            //        return;

            minboundaryWidth = Bullets.width;
            minBoundaryHeight = Bullets.height;

            if (minimumBondary)
                if (this.boundary.Width/2 < minboundaryWidth || this.boundary.Height/2 < minBoundaryHeight)
                {
                    CheckColision();
                    return;
                }

            CheckSubdivide();
            
        }

        void CheckColision()
        {
            if (!boundary.Intersects(player.rect))
                return;
            foreach (Bullets b in bullets)
            {
                if (player.rect.Intersects(b.rect))
                {
                    //Console.WriteLine("COLIDIU!!!");
                    b.isColliding = true;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void CheckSubdivide()
        {
            if (!this.boundary.Intersects(player.rect))
                return;
            foreach (Bullets b in bullets)
            {
                if (this.boundary.Intersects(b.rect))
                {
                    Subdivide();
                    break;
                }
            }
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

            if (this.nw.Intersects(player.rect))
            {
                foreach (Bullets b in bullets)
                {
                    if (nw.Intersects(b.rect))
                        temp.Add(b);
                }
                qNw = new QuadtreeColision(nw, temp, texture,player);
            }

            if (this.ne.Intersects(player.rect))
            {
                temp.Clear();
                foreach (Bullets b in bullets)
                {
                    if (ne.Intersects(b.rect))
                        temp.Add(b);
                }
                qNe = new QuadtreeColision(ne, temp, texture, player);
            }

            if (this.sw.Intersects(player.rect))
            {
                temp.Clear();
                foreach (Bullets b in bullets)
                {
                    if (sw.Intersects(b.rect))
                        temp.Add(b);
                }
                qSw = new QuadtreeColision(sw, temp, texture, player);
            }

            if (this.se.Intersects(player.rect))
            {
                temp.Clear();
                foreach (Bullets b in bullets)
                {
                    if (se.Intersects(b.rect))
                    {
                        temp.Add(b);
                    }
                }
                qSe = new QuadtreeColision(se, temp, texture,player);
            }
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
