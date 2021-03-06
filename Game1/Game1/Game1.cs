﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Credits,
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
        Texture2D hurtOverlay;
        //font
        private SpriteFont spriteFont;
        private SpriteFont finalScoreSpriteFont;
        private Vector2 fontVector = new Vector2(350, 0);

        //menu buttons
        //main menu
        private Texture2D menuTexture;
        private Button menuStart;
        private Texture2D menuStartTexture;
        private Button menuQuit;
        private Texture2D menuQuitTexture;
        private Button menuCredits;
        private Texture2D menuCreditsTexture;

        //level select
        private Texture2D levelSelectTexture;
        private Texture2D level1Texture;
        private Button level1Button;
        private Texture2D level2Texture;
        private Button level2Button;
        private Texture2D level3Texture;
        private Button level3Button;
        private Texture2D level4Texture;
        private Button level4Button;

        //credits
        private Button back;
        private Texture2D backTexture;
        private Texture2D creditsTexture;

        //pause menu
        private Texture2D pauseTexture;
        private Texture2D pauseMenuTexture;
        private Texture2D pauseQuitTexture;
        private Button pauseMenuButton;
        private Button pauseQuitButton;

        //enemy bullets
        private List<EnemyBullet> enemies;
        private List<DestroyedBullet> destroyedEnemies;
        private Texture2D enemyTexture;
        private Texture2D homingEnemyTexture;
        private Texture2D enemyExplosionTexture;
        public float timer;
        public float hurtTimer;
      
        //game over
        private Texture2D levelLost;
        private Texture2D returnToMenu;
        private Texture2D retry;
        private Button returnToMenuButton;
        private Button retryButton;

        //level complete
        private Texture2D levelCompleteTexture;
        private Texture2D levelCompleteBTexture;
        private Button levelCompleteButton;

        //player
        Player player;
        private List<PlayerProjectile> pProjects;
        private Texture2D pProjectileTexture;
        private Texture2D playerShield;
        private Texture2D character;
        //level information
        FileReader reader;
        string currentLevel;
        int bpm;
        string level1;
        string level2;
        string level3;
        string level4;
        private bool l1;
        private bool l2;
        private bool l3;
        private bool l4;

        //collectables
        Collectable collectable;
        private Texture2D collectTexture;
        private List<Collectable> collectables;
        private int collectRNG;
        //score
        private int bonusScore;
        private double finalSongTime;
        //powerups
        PowerUps powerUp;
        private Texture2D shieldTexture;
        private Texture2D healTexture;
        private Texture2D speedTexture;
        private Texture2D spreadTexture;
        private Texture2D bigShotTexture;
        private List<PowerUps> powerUps;
        private int powerRNG;
        private bool shield = false;
        private bool speed = false;
        private bool spread = false;
        private bool bigShot = false;
        public float shieldTimer;
        public float speedTimer;
        public float spreadTimer;
        public float bigShotTimer;

        public bool Shield { get { return shield; }  set { shield = value; } }
        public bool Speed { get { return speed; }  set { speed = value; } }
        public bool Spread { get { return spread; }  set { spread = value; } }
        public bool BigShot { get { return bigShot; }  set { bigShot = value; } }

        //timer after getting shot
        private float shotTimer = 0f;



        //sounds
        SoundEffect playerHit;
        SoundEffect collectableGotten;
        SoundEffect playerShot;
        SoundEffect menuPress;
        SoundEffect bulletHit;

        Song music;


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
            destroyedEnemies = new List<DestroyedBullet>();
            collectables = new List<Collectable>();
            powerUps = new List<PowerUps>();
            kbState = Keyboard.GetState();
            mouse = Mouse.GetState();
            level1 = string.Format("Content/Level 1.txt");
            level2 = string.Format("Content/Level 2.txt");
            level3 = string.Format("Content/Level 3.txt");
            level4 = string.Format("Content/Level 4.txt");
            
            gameState = GameState.MainMenu;
            bonusScore = 0;
            
            
            //menu buttons
            menuStart = new Button(new Rectangle(186, 400, 426, 68));
            menuCredits = new Button(new Rectangle(186, 538, 426, 68));
            menuQuit = new Button(new Rectangle(186, 676, 426, 68));

            //level select
            level1Button = new Button(new Rectangle(0, 112, 509, 66));
            level2Button = new Button(new Rectangle(0, 238, 509, 66));
            level3Button = new Button(new Rectangle(0, 353, 509, 66));
            level4Button = new Button(new Rectangle(0, 479, 509, 66));
            
            //credits button
            back = new Button(new Rectangle(575, 710, 225, 100));
            creditsTexture = Content.Load<Texture2D>("creditsv2");
            //Game over - level lost
            retryButton = new Button(new Rectangle(71, 570, 263, 134));
            returnToMenuButton = new Button(new Rectangle(464, 570, 263, 134));

            //level complete
            levelCompleteButton = new Button(new Rectangle(116, 509, 568, 171));

            //pause menu
            pauseMenuButton = new Button(new Rectangle(268, 382, 263, 58));
            pauseQuitButton = new Button(new Rectangle(268, 469, 263, 58));


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
            finalScoreSpriteFont = Content.Load<SpriteFont>("FinalScoreFont");
            //sounds
            playerHit = Content.Load<SoundEffect>("hurt");
            collectableGotten = Content.Load<SoundEffect>("powerup");
            menuPress = Content.Load<SoundEffect>("button");
            playerShot = Content.Load<SoundEffect>("shot");
            bulletHit = Content.Load<SoundEffect>("hit");

            //player and enemy sprites
            player.Texture = Content.Load<Texture2D>("PlayerSprite1");
            playerShield = Content.Load<Texture2D>("playerShield");
            hurtOverlay = Content.Load<Texture2D>("playerHurtOverlayFinal");
            character = Content.Load<Texture2D>("PlayerSprite1");
            enemyTexture = Content.Load<Texture2D>("EnemySprite");
            homingEnemyTexture = Content.Load<Texture2D>("HomingEnemySprite");
            enemyExplosionTexture = Content.Load<Texture2D>("BulletExplosion");
            pProjectileTexture = Content.Load<Texture2D>("PlayerProjectileSprite");

            collectTexture = Content.Load<Texture2D>("CoinSprite");
            shieldTexture = Content.Load<Texture2D>("ShieldPowerupSprite");
            healTexture = Content.Load<Texture2D>("HealthPowerupSprite");
            speedTexture = Content.Load<Texture2D>("SpeedPowerupSprite");
            spreadTexture = Content.Load<Texture2D>("MultiPowerupSprite");
            bigShotTexture = Content.Load<Texture2D>("BigPowerupSprite");

            //main menu
            menuTexture = Content.Load<Texture2D>("menuFinalVersion");
            menuStartTexture = Content.Load<Texture2D>("PLAY");
            menuCreditsTexture = Content.Load<Texture2D>("CREDITS");
            menuQuitTexture = Content.Load<Texture2D>("QUIT");
            //level select
            levelSelectTexture = Content.Load<Texture2D>("levleSelectFinal");
            level1Texture = Content.Load<Texture2D>("SONG 1");
            level2Texture = Content.Load<Texture2D>("SONG 2");
            level3Texture = Content.Load<Texture2D>("SONG 3");
            level4Texture = Content.Load<Texture2D>("SONG 4");
            //credits/level select
            backTexture = Content.Load<Texture2D>("back button");
            //game over - level lost
            levelLost = Content.Load<Texture2D>("GameOverScreen");
            retry = Content.Load<Texture2D>("RETRY");
            returnToMenu = Content.Load<Texture2D>("MENU");
            //level complete
            levelCompleteTexture = Content.Load<Texture2D>("LevelComplete");
            levelCompleteBTexture = Content.Load<Texture2D>("RETURN TO MENU");
            //pause menu
            pauseTexture = Content.Load<Texture2D>("PauseScreen");
            pauseQuitTexture = Content.Load<Texture2D>("PauseQUIT");
            pauseMenuTexture = Content.Load<Texture2D>("PauseMENU");
            
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
                    UpdateMenu(gameTime);//updates main menu screen
                    break;
                case GameState.Credits:
                    UpdateCredits(gameTime);//updates credits screen
                    break;
                case GameState.Paused:
                    UpdatePaused(gameTime);
                    if (kbState.IsKeyDown(Keys.Space) && prevKbsState.IsKeyUp(Keys.Space))//unpauses music and resumes game
                    {
                        gameState = GameState.InGame;
                        MediaPlayer.Resume();
                    }
                    break;
                case GameState.LevelSelect:
                    UpdateLevelSelect(gameTime);//updates level select screen
                    break;
                case GameState.InGame:
                    UpdateInGame(gameTime);
                    if (player.Health <= 0) //Ends current game if player health is equal to or below 0
                    {
                        MediaPlayer.Stop();
                        gameState = GameState.GameOver;
                        
                    }
                    if(kbState.IsKeyDown(Keys.Space)&& prevKbsState.IsKeyUp(Keys.Space))
                    {
                        MediaPlayer.Pause();
                        gameState = GameState.Paused;
                    }
                    if (reader.LevelComplete && enemies.Count == 0)
                    {
                        gameState = GameState.LevelComplete;
                    }
                    break;
                case GameState.LevelComplete:
                    UpdateLevelComplete(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected void UpdateMenu(GameTime gameTime)
        {
            //menu logic
            //updates player model in menu
            angle = (float)(Math.Atan2(mouse.Y - player.PositionY, mouse.X - player.PositionX));

            //changes menu screens
            
            if(menuStart.checkPressed(mouse) && menuStart.checkPressed(prevMouseState) == false)//when user hits play
            {
                menuPress.Play();
                gameState = GameState.LevelSelect;
            }
            if (menuCredits.checkPressed(mouse) && menuCredits.checkPressed(prevMouseState) == false)//when user hits credits
            {
                menuPress.Play();
                gameState = GameState.Credits;
            }
            if (menuQuit.checkPressed(mouse) && menuQuit.checkPressed(prevMouseState) == false)//when user hits quit
            {
                menuPress.Play();
                Exit();
            }
        }
        protected void UpdateLevelSelect(GameTime gameTime)
        {
            //currently only level one has any data, so trying to play any of the others will crash the program
            if (level1Button.checkPressed(mouse) && level1Button.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                currentLevel = level1;
                reader = new FileReader(level1);
                reader.ReadLine();
                music = Content.Load<Song>("Level1");
                MediaPlayer.Play(music);
                bpm = 110;
                l1 = true;
                l2 = false;
                l3 = false;
                l4 = false;
                gameState = GameState.InGame;
            }
            else if (level2Button.checkPressed(mouse) && level2Button.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                currentLevel = level2;
                reader = new FileReader(level2);
                reader.ReadLine();
                music = Content.Load<Song>("Level2");
                MediaPlayer.Play(music);
                bpm = 100;
                l1 = false;
                l2 = true;
                l3 = false;
                l4 = false;
                gameState = GameState.InGame;
            }
            else if (level3Button.checkPressed(mouse) && level3Button.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                currentLevel = level3;
                reader = new FileReader(level3);
                reader.ReadLine();
                l1 = false;
                l2 = false;
                l3 = true;
                l4 = false;
                gameState = GameState.InGame;
            }
            else if (level4Button.checkPressed(mouse) && level4Button.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                currentLevel = level4;
                reader = new FileReader(level4);
                reader.ReadLine();
                l1 = false;
                l2 = false;
                l3 = false;
                l4 = true;
                gameState = GameState.InGame;
            }
            else if (back.checkPressed(mouse))
            {
                menuPress.Play();
                gameState = GameState.MainMenu;
            }
        }
        protected void UpdatePaused(GameTime gameTime)
        {
            if(pauseMenuButton.checkPressed(mouse)&& pauseMenuButton.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                ResetLevel();
                gameState = GameState.MainMenu;
            }
            else if (pauseQuitButton.checkPressed(mouse))
            {
                menuPress.Play();
                Exit();
            }
        }
        protected void UpdateCredits(GameTime gameTime)
        {
            if (back.checkPressed( mouse))
            {
                menuPress.Play();
                gameState = GameState.MainMenu;
            }
        }
        protected void UpdateGameOver(GameTime gameTime)
        {
            if (returnToMenuButton.checkPressed(mouse) && returnToMenuButton.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                ResetLevel();
                
                gameState = GameState.MainMenu;
                
            }
            else if (retryButton.checkPressed(mouse) && retryButton.checkPressed(prevMouseState) == false)
            {
                menuPress.Play();
                ResetLevel();
                MediaPlayer.Play(music);
                reader = new FileReader(currentLevel);
                gameState = GameState.InGame;
                
            }
        }
        protected void UpdateLevelComplete(GameTime gameTime)
        {
            if (levelCompleteButton.checkPressed(mouse) && levelCompleteButton.checkPressed(prevMouseState))
            {
                menuPress.Play();
                gameState = GameState.MainMenu;
                ResetLevel();
            }
                
        }
        protected void UpdateInGame(GameTime gameTime)
        {
            //angle calculations for player
            angle = (float)(Math.Atan2(mouse.Y - player.PositionY, mouse.X - player.PositionX));
            finalSongTime = MediaPlayer.PlayPosition.TotalSeconds;//updates the player position in the song, ends when level ends

            if ((mouse.LeftButton == ButtonState.Pressed) && (prevMouseState.LeftButton == ButtonState.Released))//checks if player pressed space and fires a bullet
            {
                playerShot.Play();
                //bonusScore -= 10;//decreases score as penalty for shooting
                if (spread == true && spreadTimer > timer)
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = pProjectileTexture;
                    p.Angle = (float)angle + 1;
                    pProjects.Add(p);
                    p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = pProjectileTexture;
                    p.Angle = (float)angle - 1;
                    pProjects.Add(p);
                    p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    pProjects.Add(p); p.Texture = pProjectileTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                    Console.WriteLine("SHOT FIRED");
                }
                else if (bigShot == true && bigShotTimer > timer)
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 50, 50, 7.0f);
                    p.Texture = pProjectileTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                    Console.WriteLine("SHOT FIRED");
                }
                else
                {
                    PlayerProjectile p = new PlayerProjectile(player.PositionX, player.PositionY, 25, 25, 7.0f);
                    p.Texture = pProjectileTexture;
                    p.Angle = (float)angle;
                    pProjects.Add(p);
                    Console.WriteLine("SHOT FIRED");
                }
            }


            //creates bullets
            
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (EnemyBullet en in enemies)//moves enemy bullets
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
            for (int i = 0; i <= enemies.Count - 1; i++)
            {
                if (enemies[i].CheckDelete(enemies[i].PositionX, enemies[i].PositionY))
                {
                    enemies.RemoveAt(i);
                    i--;
                }
                //else if (enemies[i].SpawnTimer <= timer && enemies[i].AttackName == "homing")
                //{
                //    enemies.RemoveAt(i);
                //    i--;
                //    Console.WriteLine("REMOVED HOMING SHOT");
                //}
                //   --changed behaviour to stop homing and fly straight after time limit
            }
            //removes and damages player when enemy collides with it
            for (int i = 0; i <= enemies.Count - 1; i++)
            {
                if (enemies[i].CheckCollision(enemies[i], player))
                {
                    enemies.RemoveAt(i);
                    i--;
                    if(shield)
                    {
                        shield = false;
                    }
                    else if(shotTimer > timer)
                    {
                        //does nothing
                    }
                    else
                    {
                        player.Health--;
                        playerHit.Play();//plays hit sound effect
                        hurtTimer = timer + 1.05f;
                        shotTimer = timer + 1.05f;
                    }
                }
            }

            //Changes player sprite based on health
            if (player.Health >= 10)
            {
                player.Texture = Content.Load<Texture2D>("PlayerSprite1");
            }
            else if (player.Health < 10 && player.Health >= 7)
            {
                player.Texture = Content.Load<Texture2D>("PlayerSprite2");
            }
            else if (player.Health < 7 && player.Health >= 4)
            {
                player.Texture = Content.Load<Texture2D>("PlayerSprite3");
            }
            else if (player.Health < 4 && player.Health >= 1)
            {
                player.Texture = Content.Load<Texture2D>("PlayerSprite4");
            }

            foreach(EnemyBullet en in enemies)
            {
                if(en.AttackName == "homing" && en.SpawnTimer >= timer)
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
                        bulletHit.Play();
                        destroyedEnemies.Add(new DestroyedBullet(enemies[i].Position, enemyExplosionTexture));
                        destroyedEnemies[destroyedEnemies.Count - 1].AppearTimer =  this.timer + 1.05f;
                        enemies.RemoveAt(i);
                        i--;
                        
                    }
                }
            }
            //checks to remove player bullets out of bounds
            for (int i = 0; i < pProjects.Count - 1; i++)
            {
                if (pProjects[i].CheckDelete(pProjects[i].PositionX, pProjects[i].PositionY))
                {
                    pProjects.RemoveAt(i);
                    i--;
                }
            }
            //processes player movement
            if (kbState.IsKeyDown(Keys.D) && player.PositionX < GraphicsDevice.Viewport.Width -35)//moves player left
            {
                player.PositionX += (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.A) && player.PositionX > 0 +35)//moves player right
            {
                player.PositionX -= (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.S) && player.PositionY < GraphicsDevice.Viewport.Height -35)//moves player down
            {
                player.PositionY += (int)player.Speed;
            }
            if (kbState.IsKeyDown(Keys.W) && player.PositionY > 0 +35)//moves player up
            {
                player.PositionY -= (int)player.Speed;
            }
            //collectable spawn
            rng = new Random();
            collectRNG = rng.Next(600);
            if(collectRNG == 10)
            {
                collectable = new Collectable(rng.Next(35, GraphicsDevice.Viewport.Width-35), rng.Next(35, GraphicsDevice.Viewport.Height -35), 25, 25, 0);
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
            powerRNG = rng.Next(800);
            if(powerRNG == 10)
            {
                powerRNG = rng.Next(5);
                //shield
                if(powerRNG == 0)
                {
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 30, 30, 0, "shield");
                    powerUp.Texture = shieldTexture;                                                                          
                    powerUps.Add(powerUp);                                                                                    
                }                                                                                                             
                //heal                                                                                                        
                else if (powerRNG == 1)                                                                                       
                {                                                                                                             
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 30, 30, 0, "heal");
                    powerUp.Texture = healTexture;                                                                            
                    powerUps.Add(powerUp);                                                                                    
                }                                                                                                             
                //speedup                                                                                                     
                else if (powerRNG == 2)                                                                                       
                {                                                                                                             
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 30, 30, 0, "speedup");
                    powerUp.Texture = speedTexture;                                                                           
                    powerUps.Add(powerUp);                                                                                    
                }                                                                                                             
                //altfirespread                                                                                               
                else if (powerRNG == 3)                                                                                       
                {                                                                                                              
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 30, 30, 0, "altfirespread");
                    powerUp.Texture = spreadTexture;                                                                          
                    powerUps.Add(powerUp);                                                                                    
                }                                                                                                             
                //altfirebig                                                                                                  
                else                                                                                                          
                {                                                                                                             
                    powerUp = new PowerUps(rng.Next(GraphicsDevice.Viewport.Width), rng.Next(GraphicsDevice.Viewport.Height), 30, 30, 0, "altfirebig");
                    powerUp.Texture = bigShotTexture;
                    powerUps.Add(powerUp);
                }
            }

            //powerUp collision
            for (int i = 0; i < powerUps.Count; i++)
            {
                if (powerUps[i].Position.Intersects(player.Position))
                {
                    collectableGotten.Play();
                    if (powerUps[i].Type == "shield")
                    {
                        shield = true;
                        powerUps.RemoveAt(i);
                        shieldTimer = timer + 5;
                        Console.WriteLine("PICKED UP SHIELD");
                        i--;
                    }
                    else if (powerUps[i].Type == "heal")
                    {
                        player.Health++;
                        powerUps.RemoveAt(i);
                        Console.WriteLine("PICKED UP HEAL");
                        i--;
                    }
                    else if (powerUps[i].Type == "speedup")
                    {
                        speed = true;
                        powerUps.RemoveAt(i);
                        speedTimer = timer + 5;
                        Console.WriteLine("PICKED UP SPEED");
                        i--;
                    }
                    else if (powerUps[i].Type == "altfirespread")
                    {
                        spread = true;
                        powerUps.RemoveAt(i);
                        spreadTimer = timer + 7;
                        Console.WriteLine("PICKED UP SPREAD SHOT");
                        i--;
                    }
                    else if (powerUps[i].Type == "altfirebig")
                    {
                        bigShot = true;
                        powerUps.RemoveAt(i);
                        bigShotTimer = timer + 7;
                        Console.WriteLine("PICKED UP BIG SHOT");
                        i--;
                    }
                }
            }

            //speed change
            if (speed == true && speedTimer > timer)
            {
                player.Speed = 10f;
            }
            else
            {
                player.Speed = 5f;
            }

            //testing filereader

            if (timer >= (float)reader.TimeStamp * (float)(60.0 / bpm) && reader.LevelComplete == false)
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
                if (reader.AttackName == " random")//spawns basic enemy bullet randomly
                {
                    int dist = rng.Next(-20,820);
                    int side = rng.Next(1,4);
                    switch (side) {
                        case 1:
                            reader.xPosition = dist;
                            reader.yPosition = -20;
                            break;
                        case 2:
                            reader.xPosition = dist;
                            reader.yPosition = 820;
                            break;
                        case 3:
                            reader.xPosition = -20;
                            reader.yPosition = dist;
                            break;
                        case 4:
                            reader.xPosition = 820;
                            reader.yPosition = dist;
                            break;
                    }
                    for(int i = 0; i < reader.NumberOfAttacks; i++)
                    {
                        EnemyBullet bullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 6);
                        Console.WriteLine("Bullet created at: " + bullet.PositionX + ", " + bullet.PositionY);
                        Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                        reader.xPosition += rng.Next(-30, 30);//spaces bullets out
                        reader.yPosition += rng.Next(-30, 30);
                        bullet.Texture = enemyTexture;
                        bullet.Angle = bullet.FindAngle(bullet, player) + (float)((rng.NextDouble()-.5)*.25);
                        bullet.AttackName = "basic";
                        enemies.Add(bullet);
                    }
                }
                else if (reader.AttackName == " line")//spawns a line of bullets
                {
                    float dir;
                    int offsetY;
                    int offsetX;
                    if (reader.yPosition < 0) {
                        dir = (float)Math.PI * 1 / 2; //down
                        offsetX = -40;
                        offsetY = 0;
                    } else if (reader.xPosition < 0) {
                        dir = 0; //right
                        offsetX = 0;
                        offsetY = 40;
                    } else if (reader.yPosition > 800) {
                        dir = (float)Math.PI * 3 / 2; //up
                        offsetX = 40;
                        offsetY = 0;
                    } else {
                        dir = (float)Math.PI; //left
                        offsetX = 0;
                        offsetY = -40;
                    }
                    for(int i = 0; i < reader.NumberOfAttacks; i++)
                    {
                        EnemyBullet bullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 6);
                        Console.WriteLine("Bullet created at: " + bullet.PositionX + ", " + bullet.PositionY);
                        Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                        reader.xPosition += offsetX;//spaces bullets out
                        reader.yPosition += offsetY;
                        bullet.Angle = dir;
                        bullet.Texture = enemyTexture;
                        bullet.AttackName = "basic";
                        enemies.Add(bullet);
                    }
                }
                else if (reader.AttackName == " homing")//spawns homing bullet
                {
                    EnemyBullet Hbullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 4);
                    Console.WriteLine("Bullet created at: " + Hbullet.PositionX + ", " + Hbullet.PositionY);
                    Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                    Hbullet.SpawnTimer = timer + 4;//sets time limit for how long the homing bullets will last
                    Console.WriteLine("Current timer is: " + timer + " homing bullet should despawn at" + Hbullet.SpawnTimer);
                    Hbullet.Texture = homingEnemyTexture;
                    Hbullet.Angle = Hbullet.FindAngle(Hbullet, player);
                    Hbullet.AttackName = "homing";
                    enemies.Add(Hbullet);
                }
                else if (reader.AttackName == " randhoming")//spawns homing bullet
                {
                    int dist = rng.Next(-20,820);
                    int side = rng.Next(1,4);
                    switch (side) {
                        case 1:
                            reader.xPosition = dist;
                            reader.yPosition = -20;
                            break;
                        case 2:
                            reader.xPosition = dist;
                            reader.yPosition = 820;
                            break;
                        case 3:
                            reader.xPosition = -20;
                            reader.yPosition = dist;
                            break;
                        case 4:
                            reader.xPosition = 820;
                            reader.yPosition = dist;
                            break;
                    }
                    EnemyBullet Hbullet = new EnemyBullet((int)reader.xPosition, (int)reader.yPosition, 25, 25, 4);
                    Console.WriteLine("Bullet created at: " + Hbullet.PositionX + ", " + Hbullet.PositionY);
                    Console.WriteLine("Current reader time: " + reader.TimeStamp + ". Current Game time: " + timer);
                    Hbullet.SpawnTimer = timer + 4;//sets time limit for how long the homing bullets will last
                    Console.WriteLine("Current timer is: " + timer + " homing bullet should despawn at" + Hbullet.SpawnTimer);
                    Hbullet.Texture = homingEnemyTexture;
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
            GraphicsDevice.Clear(Color.GhostWhite);

            
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //gameState switch
            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMenu(gameTime);
                    break;
                case GameState.Credits:
                    DrawCredits(gameTime);
                    break;
                case GameState.LevelSelect:
                    DrawLevelSelect(gameTime);
                    break;
                case GameState.Paused:
                    DrawInGame(gameTime);
                    DrawPaused(gameTime);
                    break;
                case GameState.InGame:
                    DrawInGame(gameTime);
                    //draw background color
                    if (l1 == true)
                    {
                        GraphicsDevice.Clear(Color.Lavender);
                    }
                    else if (l2 == true)
                    {
                        GraphicsDevice.Clear(Color.SeaShell);
                    }
                    else if (l3 == true)
                    {
                        GraphicsDevice.Clear(Color.MediumTurquoise);
                    }
                    else if (l4 == true)
                    {
                        GraphicsDevice.Clear(Color.DarkOliveGreen);
                    }
                    if (player.Health <= 0) //Ends current game if player health is equal to or below 0
                    {
                        gameState = GameState.GameOver; 
                    }
                    break;
                case GameState.LevelComplete:
                    DrawLevelComplete(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Draw(menuTexture, new Rectangle(0, 0, 800, 800), Color.White);
            player.PositionY = 250;
            spriteBatch.Draw(player.Texture, (player.Position), null,
                Color.White, (float)(angle + Math.PI / 2), new Vector2(player.Texture.Width / 2,
                player.Texture.Height / 2), SpriteEffects.None, 0);

            if (menuStart.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(menuStartTexture, menuStart.rectangle, Color.Green);
            }
            else
            {
                spriteBatch.Draw(menuStartTexture, menuStart.rectangle, Color.White);
            }

            if (menuCredits.checkHover(mouse))//checks if mouse is over credits button
            {
                spriteBatch.Draw(menuCreditsTexture, menuCredits.rectangle, Color.SkyBlue);
            }
            else
            {
                spriteBatch.Draw(menuCreditsTexture, menuCredits.rectangle, Color.White);
            }

            if (menuQuit.checkHover(mouse))//checks if mouse is over the quit button
            {
                spriteBatch.Draw(menuQuitTexture, menuQuit.rectangle, Color.Crimson);
            }
            else
            {
                spriteBatch.Draw(menuQuitTexture, menuQuit.rectangle, Color.White);
            }
        }
        protected void DrawLevelSelect(GameTime gameTime)
        {
            spriteBatch.Draw(levelSelectTexture, new Rectangle(0, 0, 800, 800), Color.White);

            if (level1Button.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(level1Texture, level1Button.rectangle, Color.Teal);
                
            }
            else
            {
                spriteBatch.Draw(level1Texture, level1Button.rectangle, Color.White);
            }
            if (level2Button.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(level2Texture, level2Button.rectangle, Color.Green);
            }
            else
            {
                spriteBatch.Draw(level2Texture, level2Button.rectangle, Color.White);
            }
            if (level3Button.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(level3Texture, level3Button.rectangle, Color.Yellow);
            }
            else
            {
                spriteBatch.Draw(level3Texture, level3Button.rectangle, Color.White);
            }
            if (level4Button.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(level4Texture, level4Button.rectangle, Color.Crimson);
            }
            else
            {
                spriteBatch.Draw(level4Texture, level4Button.rectangle, Color.White);
            }
            spriteBatch.Draw(backTexture, back.rectangle, Color.White);
        }
        protected void DrawCredits(GameTime gameTime)
        {
            //draws credits to the screen
            spriteBatch.Draw(creditsTexture, new Rectangle(0, 0, 800, 800), Color.White);
            spriteBatch.Draw(backTexture, back.rectangle, Color.White);
        }
        protected void DrawGameOver(GameTime gameTime)
        {
            spriteBatch.Draw(levelLost, new Rectangle(0, 0, 800, 800), Color.White);
            spriteBatch.DrawString(spriteFont, Math.Round((music.Duration.TotalSeconds - finalSongTime)).ToString(), new Vector2(320, 150), Color.Black);
            spriteBatch.DrawString(spriteFont, (Math.Round(timer + bonusScore, 0)).ToString(), new Vector2(325, 90), Color.Black);
            if (retryButton.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(retry, retryButton.rectangle, Color.Green);
            }
            else
            {
                spriteBatch.Draw(retry, retryButton.rectangle, Color.White);
            }
            if (returnToMenuButton.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(returnToMenu, returnToMenuButton.rectangle, Color.Teal);
            }
            else
            {
                spriteBatch.Draw(returnToMenu, returnToMenuButton.rectangle, Color.White);
            }   
        }
        protected void DrawLevelComplete(GameTime gameTime)
        {
            spriteBatch.Draw(levelCompleteTexture, new Rectangle(0, 0, 800, 800), Color.White);
            if (levelCompleteButton.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(levelCompleteBTexture, levelCompleteButton.rectangle, Color.Teal);
            }
            else
            {
                spriteBatch.Draw(levelCompleteBTexture, levelCompleteButton.rectangle, Color.White);
            }
            spriteBatch.DrawString(finalScoreSpriteFont
                , (Math.Round(timer + bonusScore, 0)).ToString(), new Vector2(GraphicsDevice.Viewport.Width/2-75, 350), Color.Black);
        }
        protected void DrawPaused(GameTime gameTime)
        {
            spriteBatch.Draw(pauseTexture, new Rectangle(0, 0, 800, 800), Color.White);

            if (pauseMenuButton.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(pauseMenuTexture, pauseMenuButton.rectangle, Color.Teal);
            }
            else
            {
                spriteBatch.Draw(pauseMenuTexture, pauseMenuButton.rectangle, Color.White);
            }
            if (pauseQuitButton.checkHover(mouse))//checks if mouse is over start button
            {
                spriteBatch.Draw(pauseQuitTexture, pauseQuitButton.rectangle, Color.Teal);
            }
            else
            {
                spriteBatch.Draw(pauseQuitTexture, pauseQuitButton.rectangle, Color.White);
            }
        }
        protected void DrawInGame(GameTime gameTime)
        {
            
            if(hurtTimer > timer)
            {
                spriteBatch.Draw(hurtOverlay, new Rectangle(0, 0, 800, 800), Color.White);
            }
            //spriteBatch.DrawString(spriteFont, timer.ToString(), new Vector2(200, 40), Color.Black);
            foreach(EnemyBullet en in enemies)
            {
                spriteBatch.Draw(en.Texture, en.Position, null, Color.White, 
                    (float)(en.Angle + Math.PI), new Vector2(en.Width/2, en.Height/2),
                    SpriteEffects.None, 0);
            }
            foreach(DestroyedBullet b in destroyedEnemies)
            {
                if (b.AppearTimer > timer)
                {
                    spriteBatch.Draw(b.Texture, b.Rectangle, Color.White);
                }
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
            foreach (PowerUps p in powerUps)
            {
                spriteBatch.Draw(p.Texture, p.Position, Color.White);
            }
            if (shield && shieldTimer > timer)
            {
                spriteBatch.Draw(playerShield, new Rectangle(player.PositionX-50, player.PositionY-50, 128, 128), Color.White);
            }
            else { shield = false; }

            

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
        
        public void ResetLevel()
        {
            
            player.PositionX = 400;//resets player position
            player.PositionY = 200;
            bonusScore = 0;//resets score
            reader.FinishFile();//finishes reading file
            player.Health = 10;//resets player health
            //making sure powerups dont continue cross levels
            shield = false;
            speed = false;
            spread = false;
            bigShot = false;
            timer = 0;
            hurtTimer = 0;
            shotTimer = 0;
            player.Texture = character;
            while(destroyedEnemies.Count > 0)
            {
                destroyedEnemies.RemoveAt(0);
            }
            while (enemies.Count > 0)//deletes enemy projectiles
            {
                enemies.RemoveAt(0);
            }
            while(collectables.Count > 0)//deletes collectables
            {
                collectables.RemoveAt(0);
            }
            while (powerUps.Count > 0)//deletes powerups
            {
                powerUps.RemoveAt(0);
            }
            while (pProjects.Count > 0)//deletes player projectiles
            {
                pProjects.RemoveAt(0);
            }
            
            ResetElapsedTime();
            
        }
        
        
        
    }
}
