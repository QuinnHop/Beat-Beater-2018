using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public enum GameState
    {
        MainMenu,
        Paused,
        InGame,
        GameOver
    }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //fields

        //game state
        private GameState gameState;

        //mouse and angle
        private double angle;
        private MouseState mouse;

        //keyboard States
        private KeyboardState kbState;
        private KeyboardState prevKbsState;

        //random
        Random rng;

        //enemy bullets
        private List<EnemyBullet> enemies;
        private int spawnDirection;
        private Texture2D enemyTexture;
        public float timer;

        //player
        Player player;
        public Texture2D character;
        private List<PlayerProjectile> pProjects;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            //initialization
            this.IsMouseVisible = true; 
            rng = new Random();
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 75, 100, 5.0f);
            pProjects = new List<PlayerProjectile>();
            enemies = new List<EnemyBullet>();
            kbState = Keyboard.GetState();
            gameState = GameState.MainMenu;
            base.Initialize();
            // Drew Donovan
            // Quinn Hopwod
            // Gabriel Schugardt
            // Fisher Michaels
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            character = Content.Load<Texture2D>("arrow");
            player.Texture = character;

            enemyTexture = Content.Load<Texture2D>("enemybullet");
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //update keyboard and mouse states
            
            prevKbsState = kbState;
            kbState = Keyboard.GetState();
            mouse = Mouse.GetState();

            // TODO: Add your update logic here

            //angle calculations
            angle = (float)(Math.Atan2(mouse.Y - player.PositionY, mouse.X - player.PositionX));

            if (kbState.IsKeyDown(Keys.Space) && prevKbsState.IsKeyUp(Keys.Space))//checks if player pressed space and fires a bullet
            {
                PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                p.Texture = enemyTexture;
                p.Angle = (float)angle;
                pProjects.Add(p);
            }


            //creates bullets
            BulletSpawner();
            timer = gameTime.ElapsedGameTime.Seconds;
            foreach(EnemyBullet en in enemies)//moves enemy bullets
            {
                en.PositionX += (int)(en.Speed * Math.Cos(en.Angle));
                en.PositionY += (int)(en.Speed * Math.Sin(en.Angle));
            }

            foreach (PlayerProjectile p in pProjects)//moves player bullets
            {
                p.PositionX += (int)(p.Speed * Math.Cos(p.Angle));
                p.PositionY += (int)(p.Speed * Math.Sin(p.Angle));
            }
            //checks to enemy remove bullets out of bounds
            for (int i = 0; i < enemies.Count-1; i++)
            {
                if (enemies[i].CheckDelete(enemies[i].PositionX, enemies[i].PositionY)
                    || enemies[i].CheckCollision(enemies[i], player))
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            //checks playerProjectile collisions with enemyBullets
            foreach(PlayerProjectile p in pProjects)
            {
                for(int i = 0; i < enemies.Count; i++)
                {
                    if (p.CheckCollision(p, enemies[i]))
                    {
                        enemies.RemoveAt(i);
                        i--;
                    }
                }
            }
            //checks to  remove player bullets out of bounds
            for (int i = 0; i < pProjects.Count - 1; i++)
            {
                if (pProjects[i].CheckDelete(pProjects[i].PositionX, pProjects[i].PositionY))
                {
                    pProjects.RemoveAt(i);
                    i--;
                }
            }
            //processes player movement
            if (kbState.IsKeyDown(Keys.D) && player.PositionX < GraphicsDevice.Viewport.Width - 100)//moves player left
            {
                player.PositionX += (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.A) && player.PositionX > 0)//moves player right
            {
                player.PositionX -= (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.S) && player.PositionY < GraphicsDevice.Viewport.Height - 75)//moves player down
            {
                player.PositionY += (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.W) && player.PositionY > 0)//moves player up
            {
                player.PositionY -= (int)player.Speed;
            }

            //control gameState
            switch (gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Paused:
                    break;
                case GameState.InGame:
                    if (player.Health <= 0) //Ends current game if player health is equal to or below 0
                    {
                        gameState = GameState.GameOver;
                    }
                    break;
                case GameState.GameOver:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach(EnemyBullet en in enemies)
            {
                spriteBatch.Draw(en.Texture, en.Position, null, Color.White, 
                    (float)(en.Angle + Math.PI), new Vector2(en.Width/2, en.Height/2),
                    SpriteEffects.None, 0);
            }
            foreach (PlayerProjectile p in pProjects)
            {
                spriteBatch.Draw(p.Texture, p.Position, null, Color.White,
                    (float)(p.Angle + Math.PI), new Vector2(p.Width / 2, p.Height / 2),
                    SpriteEffects.None, 0);
            }

            //this is the drawing of the player overloads are as follows:
            //player.Texture: the texture to load
            //player.position: The rectangle it derives it's position from
            //null: SourceRectangle, not necissary in this case
            //Color.White: no tint needed, object is therefore white
            //(float)(angle): angle calculated earlier in the code, cast to a float
            //new Vector2: sets the origin to the center of the texture
            //spriteEffect.None: no flipping necissary
            //0: object is on layer 0
            spriteBatch.Draw(player.Texture, player.Position, null, 
                Color.White, (float)(angle + Math.PI/2), new Vector2(player.Texture.Width/2, 
                player.Texture.Height/2), SpriteEffects.None, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //bullet spawner
        public void BulletSpawner()
        {
            if(enemies.Count < 10)
            {
                //determine area to spawn in
                spawnDirection = rng.Next(1, 5);
                
                if(spawnDirection == 1)//spawns above the screen
                {
                    int randomX = rng.Next(10, GraphicsDevice.Viewport.Width);  //spawns above
                    EnemyBullet enemy = new EnemyBullet(randomX, -100, 20, 20, 6.0f);
                    enemy.Texture = enemyTexture;
                    enemy.Angle = enemy.FindAngle(enemy, player);
                    enemies.Add(enemy);
                }
                if(spawnDirection == 2)//spawns to the right of the screen
                {
                    int randomY = rng.Next(10, GraphicsDevice.Viewport.Height);//spawns to the right
                    EnemyBullet enemy = new EnemyBullet(GraphicsDevice.Viewport.Width +100, randomY, 20, 20, 6.0f);
                    enemy.Texture = enemyTexture;
                    enemy.Angle = enemy.FindAngle(enemy, player);
                    enemies.Add(enemy);
                }
                if(spawnDirection == 3)//spawns below the screen
                {
                    int randomX = rng.Next(10, GraphicsDevice.Viewport.Width);//spawns below
                    EnemyBullet enemy = new EnemyBullet(randomX, GraphicsDevice.Viewport.Height + 100, 20, 20, 6.0f);
                    enemy.Texture = enemyTexture;
                    enemy.Angle = enemy.FindAngle(enemy, player);
                    enemies.Add(enemy);
                }
                if(spawnDirection == 4)//spawns to the left of the screen
                {
                    int randomY = rng.Next(10, GraphicsDevice.Viewport.Height);//spawns to the left
                    EnemyBullet enemy = new EnemyBullet(-100, randomY, 20, 20, 6.0f);
                    enemy.Texture = enemyTexture;
                    enemy.Angle = enemy.FindAngle(enemy, player);
                    enemies.Add(enemy);
                }
                
            }
        }
    }
}
