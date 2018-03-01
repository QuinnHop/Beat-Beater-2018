using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //fields

        //mouse and angle
        private double angle;
        private MouseState mouse;

        //keyboard States
        private KeyboardState kbState;

        //random
        Random rng;

        //player
        Player player;
        public Texture2D character;
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
            player = new Player(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 75, 100);
                
            base.Initialize();
            // Drew Donovan
            // Quinn Hopwod
            // Gabriel Schugardt
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
            kbState = Keyboard.GetState();
            mouse = Mouse.GetState();

            // TODO: Add your update logic here

            //angle calculations
            angle = (float)(Math.Atan2(mouse.Y - player.PositionY, mouse.X - player.PositionX));


            //processes player movement
            if (kbState.IsKeyDown(Keys.D) && player.PositionX < GraphicsDevice.Viewport.Width - 100)//moves player left
            {
                player.PositionX += 5;
            }
            if (kbState.IsKeyDown(Keys.A) && player.PositionX > 0)//moves player right
            {
                player.PositionX -= 5;
            }
            if (kbState.IsKeyDown(Keys.S) && player.PositionY < GraphicsDevice.Viewport.Height - 75)//moves player down
            {
                player.PositionY += 5;
            }
            if (kbState.IsKeyDown(Keys.W) && player.PositionY > 0)//moves player up
            {
                player.PositionY -= 5;
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
    }
}
