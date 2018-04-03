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
        LevelSelect,
        InGame,
        LevelComplete,
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
        private MouseState prevMouseState;
        //keyboard States
        private KeyboardState kbState;
        private KeyboardState prevKbsState;

        //random
        Random rng;

        //font
        private SpriteFont spriteFont;
        private Vector2 fontVector = new Vector2(350, 0);

        //menu buttons
        private Button menuStart;
        private Texture2D menuStartTexture;

        //enemy bullets
        private List<EnemyBullet> enemies;
        private Texture2D enemyTexture;
        public float timer;

        //player
        Player player;
        public Texture2D character;
        private List<PlayerProjectile> pProjects;

        FileReader reader;
        string currentFile;
        //collectables
        Collectable collectable;
        private Texture2D collectTexture;
        private List<Collectable> collectables;
        private int collectRNG;
        //score
        private int bonusScore;
        //powerups
        PowerUps powerUp;
        private Texture2D powerUpTexture;
        private List<PowerUps> powerUps;
        private int powerRNG;
        private string altfire;

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

            //window size
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            this.IsMouseVisible = true; 
            rng = new Random();
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 75, 100, 5.0f);
            pProjects = new List<PlayerProjectile>();
            enemies = new List<EnemyBullet>();
            collectables = new List<Collectable>();
            powerUps = new List<PowerUps>();
            kbState = Keyboard.GetState();
            mouse = Mouse.GetState();
            currentFile = string.Format("Content/test.txt");
            reader = new FileReader(currentFile);
            reader.ReadLine();
            gameState = GameState.MainMenu;
            bonusScore = 0;
            menuStart = new Button(new Rectangle(400, 400, 400, 160));
            base.Initialize();
            // Drew Donovan
            // Quinn Hopwood
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

            spriteFont = Content.Load<SpriteFont>("FontFile");

            character = Content.Load<Texture2D>("arrow");
            player.Texture = character;

            enemyTexture = Content.Load<Texture2D>("enemybullet");

            collectTexture = Content.Load<Texture2D>("coin");

            menuStartTexture = Content.Load<Texture2D>("menuStart");
            
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
            
            prevKbsState = kbState;//sets previous kb state
            kbState = Keyboard.GetState();

            prevMouseState = mouse;
            mouse = Mouse.GetState();
            
            
            //gameState switch
            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMenu(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.LevelSelect:
                    break;
                case GameState.InGame:
                    UpdateInGame(gameTime);
                    if (player.Health <= 0) //Ends current game if player health is equal to or below 0
                    {
                        gameState = GameState.GameOver;
                    }
                    break;
                case GameState.LevelComplete:
                    break;
                case GameState.GameOver:
                    break;
            }

            base.Update(gameTime);
        }

        protected void UpdateMenu(GameTime gameTime)
        {
            //menu logic
            if ((mouse.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released) && menuStart.rectangle.Contains(mouse.Position))
            {
                gameState = GameState.InGame;
            }
        }

        protected void UpdateInGame(GameTime gameTime)
        {
            //angle calculations for player
            angle = (float)(Math.Atan2(mouse.Y - player.PositionY, mouse.X - player.PositionX));


            if ((mouse.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released))//checks if player pressed space and fires a bullet
            {
                if (altfire == "spread")
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = enemyTexture;
                    p.Angle = (float)angle + 1;
                    pProjects.Add(p);
                    p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = enemyTexture;
                    p.Angle = (float)angle - 1;
                    pProjects.Add(p);
                    p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    pProjects.Add(p); p.Texture = enemyTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                    Console.WriteLine("SHOT FIRED");
                }
                else if (altfire == "big")
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 50, 50, 7.0f);
                    p.Texture = enemyTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                }
                else
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = enemyTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                    Console.WriteLine("SHOT FIRED");
                }
            }


            //creates bullets
            
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            foreach(EnemyBullet en in enemies)
            {
                if(en.AttackName == "homing")
                {
                    en.Angle = (en.FindAngle(en, player));
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
                        player.Health--;
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
            //collectable spawn
            rng = new Random();
            collectRNG = rng.Next(1000);
            if(collectRNG == 10)
            {
                collectable = new Collectable(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0);
                collectable.Texture = collectTexture;
                collectables.Add(collectable);
            }
            //collectable collision
            for (int i = 0; i < collectables.Count; i++)
            {
                if (collectables[i].Position.Intersects(player.Position))
                {
                    collectables.RemoveAt(i);
                    bonusScore += 100;
                    i--;
                }
            }
            //powerup spawn
            rng = new Random();
            powerRNG = rng.Next(1000);
            if(powerRNG == 10)
            {
                powerRNG = rng.Next(4);
                //shield
                if(powerRNG == 0)
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0, "shield");
                    powerUp.Texture = collectTexture;//shield texture
                    powerUps.Add(powerUp);
                }
                //heal
                else if (powerRNG == 1)
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0, "heal");
                    powerUp.Texture = collectTexture;//heal texture
                    powerUps.Add(powerUp);
                }
                //speedup
                else if (powerRNG == 2)
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0, "speedup");
                    powerUp.Texture = collectTexture;//speedUp texture
                    powerUps.Add(powerUp);
                }
                //altfirespread
                else if (powerRNG == 3)
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0, "altfirespread");
                    powerUp.Texture = collectTexture;//altfirespread texture
                    powerUps.Add(powerUp);
                }
                //altfirebig
                else
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 25, 25, 0, "altfirebig");
                    powerUp.Texture = collectTexture;//altfirebig texture
                    powerUps.Add(powerUp);
                }
            }

            //powerUp collision
            for (int i = 0; i < powerUps.Count; i++)
            {
                if (powerUps[i].Position.Intersects(player.Position))
                {
                    if (powerUps[i].Type == "shield")
                    {
                        //do what it needs to do
                        collectables.RemoveAt(i);
                        i--;
                    }
                    else if (powerUps[i].Type == "heal")
                    {
                        player.Health++;
                        collectables.RemoveAt(i);
                        i--;
                    }
                    else if (powerUps[i].Type == "speedup")
                    {
                        if (player.Speed == 5f)
                        {
                            player.Speed = 10f;
                        }
                        collectables.RemoveAt(i);
                        i--;
                    }
                    else if (powerUps[i].Type == "altfirespread")
                    {
                        altfire = "spread";
                        collectables.RemoveAt(i);
                        i--;
                    }
                    else if (powerUps[i].Type == "altfirebig")
                    {
                        altfire = "big";
                        collectables.RemoveAt(i);
                        i--;
                    }
                }
            }

            //testing filereader

            if (timer >= reader.TimeStamp && reader.LevelComplete == false)
            {
                Console.WriteLine(reader.TimeStamp + " " + reader.AttackName
                    + " " + reader.NumberOfAttacks + " " + reader.xPosition + " " + reader.yPosition);
                if (reader.AttackName == " basic")//spawns basic enemy bullets
                {
                    for(int i = 0; i < reader.NumberOfAttacks; i++)
                    {
                        EnemyBullet bullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 6);
                        Console.WriteLine("Bullet created at: " + bullet.PositionX + ", " + bullet.PositionY);
                        Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                        reader.xPosition += rng.Next(25, 100);//spaces bullets out
                        reader.yPosition += rng.Next(25, 100);
                        bullet.Texture = enemyTexture;
                        bullet.Angle = bullet.FindAngle(bullet, player);
                        bullet.AttackName = "basic";
                        enemies.Add(bullet);
                    }
                }
                else if (reader.AttackName == " homing")//spawns homing bullets
                {
                    EnemyBullet Hbullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 4);
                    Console.WriteLine("Bullet created at: " + Hbullet.PositionX + ", " + Hbullet.PositionY);
                    Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                    reader.xPosition += rng.Next(25, 100);//spaces bullets out
                    reader.yPosition += rng.Next(25, 100);
                    Hbullet.Texture = enemyTexture;
                    Hbullet.Angle = Hbullet.FindAngle(Hbullet, player);
                    Hbullet.AttackName = "homing";
                    enemies.Add(Hbullet);
                }
                else
                {
                    Console.WriteLine("Invalid bullet name, line will not spawn");//if attackname is recognized
                }
                reader.ReadLine();
            }
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

            //gameState switch
            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMenu(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.InGame:
                    DrawInGame(gameTime);
                    if (player.Health <= 0) //Ends current game if player health is equal to or below 0
                    {
                        gameState = GameState.GameOver;
                    }
                    break;
                case GameState.GameOver:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Draw(menuStartTexture, menuStart.rectangle, Color.White);
        }

        protected void DrawInGame(GameTime gameTime)
        {
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
            foreach (Collectable c in collectables)
            {
                spriteBatch.Draw(c.Texture, c.Position, Color.White);
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
            spriteBatch.DrawString(spriteFont, "Score: " + (Math.Round(timer + bonusScore, 0)), fontVector, Color.Black);
        }

        //bullet spawner
        /*
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
         */
    }
}
