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

namespace Quadtree
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static int screenSizeWidth = 400, screenSizeHeight = 400;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tQtree, tBullet;
        Random r = new Random();
        Rectangle boundary;

        int maxBullets = 1000;

        Qtree qTree;
        Bullets[] bullets = new Bullets[1000];

        Player player;
        Texture2D QuadTreeTexture;
        QuadtreeColision quadtree;

        //speed up
        int numberFramesWait = 60 * 7;
        int maxCount = 10;
        int count = 10;

        List<double> frameTime = new List<double>();
        List<double> frameTimeT = new List<double>();
        int endCount = 0;
        double lastTime = 0;
        double timer;
        bool once;

        public static bool useQuadtree = false;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenSizeWidth;
            graphics.PreferredBackBufferHeight = screenSizeHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            for (int i = 0; i < maxBullets; i++)
            {
                this.bullets[i] = new Bullets(r.Next(0, 390), r.Next(0, 390), tBullet, r.Next(1,4));
            }

            boundary = new Rectangle(0, 0, screenSizeWidth, screenSizeHeight);
            #region Qtree
            //boundary = new Rectangle(0, 0, 400, 400);
            //tQtree = Content.Load<Texture2D>(@"Texture\square");
            //qTree = new Qtree(boundary, bullets.ToList<Bullets>(), tQtree);
            #endregion

            #region Player QuadTree
            Texture2D texture = Content.Load<Texture2D>(@"Texture\player");
            player = new Player(175, 320, 50, 50, 1, 5, texture);
            QuadTreeTexture = Content.Load<Texture2D>(@"Texture\square");
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < maxBullets; i++){
                tBullet = Content.Load<Texture2D>(@"Texture\bullet");
                bullets[i].texture = tBullet;
            }
            

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i].Update(gameTime);
            }

            #region Player QuadTree
            player.Update(gameTime);
            if (useQuadtree)
            {
                quadtree = null;
                quadtree = new QuadtreeColision(boundary, bullets.ToList<Bullets>(), QuadTreeTexture, player);
            }
            else
            {
                foreach(Bullets b in bullets)
                {
                    if (player.rect.Intersects(b.rect))
                    {
                        b.isColliding = true;
                    }
                }
            }
            #endregion

            PreSpeedUp(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            
            #region Player QuadTree
            player.Draw(gameTime, spriteBatch);
            if (useQuadtree)
            {
                quadtree.Draw(gameTime, spriteBatch);
            }
            #endregion

            #region Qtree
            //qTree.Draw(gameTime, spriteBatch);
            #endregion

            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"> linear </param>
        /// <param name="b"> multi thread </param>
        public void SpeedUp(List<double> a, List<double> b)
        {
            double mediaA, mediaB;
            Console.WriteLine("___________________A___________________" + a.Count);
            a.Sort();   //ordenação crescente na lista
            a.RemoveAt(0);  //remover os outliers
            a.RemoveAt(a.Count - 1);
            for (int i = 0; i < a.Count; i++)
                Console.WriteLine(a[i] + "");

            Console.WriteLine("___________________B___________________" + b.Count);
            b.Sort();
            b.RemoveAt(0);
            b.RemoveAt(b.Count - 1);
            for (int i = 0; i < b.Count; i++)
                Console.WriteLine(b[i] + "");

            Console.WriteLine("_______________SPEED UP___________________");
            mediaA = a.Sum();//soma de todos os fatores da lista A
            Console.WriteLine("SOMA   A: " + mediaA);
            mediaB = b.Sum();
            Console.WriteLine("SOMA   B: " + mediaB);
            mediaA = mediaA / a.Count;
            Console.WriteLine("MEDIA  A: " + mediaA);
            mediaB = mediaB / b.Count;
            Console.WriteLine("MEDIA  B: " + mediaB);

            Console.WriteLine("SPEED UP: " + mediaA / mediaB);
            once = true;
        }

        public void PreSpeedUp(GameTime gametime)
        {
            if (endCount > 1)
            {
                if (!once)
                    SpeedUp(frameTime, frameTimeT);

                return;

            }

            if (numberFramesWait <= 0 && numberFramesWait > numberFramesWait - (maxCount) * 2)  //conferir se ja/ainda pode coletar dados
            {

                timer = gametime.TotalGameTime.TotalMilliseconds - lastTime; //diferença do tempo atual para o tempod o frame anterior
                if (count > 0)
                {
                    if (useQuadtree)//adiciona o tempo no list referente a thread
                    {
                        frameTimeT.Add(timer);
                        count--;
                    }
                    else//adiciona o tempo no list referente ao linear
                    {
                        frameTime.Add(timer);
                        count--;
                    }
                    Console.WriteLine("Time - " + useQuadtree + ":   " + timer);

                }
                else // validação dos controladores e troca do valor da booleana use thread
                {
                    if (endCount > 1)
                    {
                        return;
                    }
                    endCount++;
                    count = maxCount;
                    useQuadtree = !useQuadtree;
                }

            }
            else
            {
                numberFramesWait--;
            }

            lastTime = gametime.TotalGameTime.TotalMilliseconds;

        }
    }
}
