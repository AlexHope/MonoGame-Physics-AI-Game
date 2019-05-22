using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace General
{
    public class PhysicsAIGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        SpriteFont _font;
        Camera _camera;
        KeyboardState _oldKeyboardState;
        ButtonState _oldMouseStateLeft;
        ButtonState _oldMouseStateMiddle;
        ButtonState _oldMouseStateRight;
        World _world;

        // Cursor
        Texture2D _cursorTexture;
        Vector2 _cursorPos;

        // Set some more textures
        Texture2D _ballTexture;
        Texture2D _bulletTexture;
        Texture2D _pendulumTexture;
        Texture2D _playerTexture;
        Texture2D _enemyTexture;
        Texture2D _targetTexture;
        Texture2D _gridTexture;
        Texture2D _debugPathTexture;
        Texture2D _debugNodeTexture;

        private const int _delayBeforeQuit = 5;
        private float _delayRemaining = _delayBeforeQuit;
        private bool _drawGrid = false;
        private bool _drawNodes = false;
        private bool _drawPath = false;
        private bool _allowDebugControls = false;

        public PhysicsAIGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set the desired FPS to 60
            IsFixedTimeStep = true;
            TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 16);

            // Set the default resolution
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {
            _world = new World(_graphics.GraphicsDevice);
            _camera = new Camera(GraphicsDevice.Viewport);

            // Set each static block's texture
            foreach (Physics.PhysObject po in _world.PhysicsManager.StaticObjects)
            {
                po.Texture = TextureSetting.SetTexture(_graphics.GraphicsDevice,
                                                po.RigidBody.Width,
                                                po.RigidBody.Height,
                                                Color.Gainsboro);

                po.TexturePos = new Vector2(po.RigidBody.Position.X - (po.RigidBody.Width / 2), po.RigidBody.Position.Y - (po.RigidBody.Height / 2));
            }

            _debugPathTexture = TextureSetting.SetTexture(_graphics.GraphicsDevice,
                                                          32,
                                                          32,
                                                          Color.Red);

            _debugNodeTexture = TextureSetting.SetTexture(_graphics.GraphicsDevice,
                                                          32,
                                                          32,
                                                          Color.Blue);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the custom images & fonts
            _cursorTexture = Content.Load<Texture2D>("cursor");
            _ballTexture = Content.Load<Texture2D>("ballprojsmall_b_16");
            _bulletTexture = Content.Load<Texture2D>("bulletprojsmall_b_10");
            _pendulumTexture = Content.Load<Texture2D>("pendulumball");
            _playerTexture = Content.Load<Texture2D>("playermed");
            _enemyTexture = Content.Load<Texture2D>("enemymed");
            _targetTexture = Content.Load<Texture2D>("target");
            _gridTexture = Content.Load<Texture2D>("grid32");
            _font = Content.Load<SpriteFont>("Arial");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Is the window active?
            if (IsActive)
            {
                // Press escape to close the game
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                // Grab the cursor position relative to the game, not the window
                _cursorPos = new Vector2(Mouse.GetState().X + _camera.CameraPosition.X, Mouse.GetState().Y + _camera.CameraPosition.Y);

                // As long as we haven't lost the game, keep updating inputs
                if (_world.PortalHealth != 0)
                {
                    UpdateInput();
                }
                // Otherwise close the game after a given number of seconds (currently 10)
                else
                {
                    _world.GameOver = true;

                    _delayRemaining -= 0.01f;

                    if (_delayRemaining <= 0)
                    {
                        Exit();
                    }
                }

                // Update the world
                _world.Update(gameTime);

                // Set the camera to follow the player
                _camera.CameraPosition = new Vector2(_world.Player.RigidBody.Position.X - _graphics.GraphicsDevice.Viewport.Width / 2,
                                                     _world.Player.RigidBody.Position.Y - _graphics.GraphicsDevice.Viewport.Height / 2 - 128);

                base.Update(gameTime);
            }
        }

        // Check for user input
        private void UpdateInput()
        {
            KeyboardState state = Keyboard.GetState();
            ButtonState mouseStateLeft = Mouse.GetState().LeftButton;
            ButtonState mouseStateMiddle = Mouse.GetState().MiddleButton;
            ButtonState mouseStateRight = Mouse.GetState().RightButton;

            // Player movement
            if (state.IsKeyDown(Keys.A))
            {
                _world.Player.MoveLeft();
            }
            if (state.IsKeyDown(Keys.D))
            {
                _world.Player.MoveRight();
            }
            if (state.IsKeyDown(Keys.S) && !_oldKeyboardState.IsKeyDown(Keys.S))
            {
                _world.Player.Stop();
            }
            if (state.IsKeyDown(Keys.Space) && !_oldKeyboardState.IsKeyDown(Keys.Space))
            {
                _world.Player.Jump();
            }

            // Player attack
            if (mouseStateLeft == ButtonState.Pressed && _oldMouseStateLeft != ButtonState.Pressed)
            {
                _world.PhysicsManager.Player.Attack(_world.PhysicsManager, _cursorPos, Physics.Projectile.ProjectileType.Bullet);
            }
            if (mouseStateRight == ButtonState.Pressed && _oldMouseStateRight != ButtonState.Pressed)
            {
                _world.Player.Attack(_world.PhysicsManager, _cursorPos, Physics.Projectile.ProjectileType.Ball);
            }

            // Toggle full screen
            if (state.IsKeyDown(Keys.F1) && !_oldKeyboardState.IsKeyDown(Keys.F1))
            {
                _graphics.ToggleFullScreen();
            }

            // Changes the maximum number of enemies with keys 1-5
            if (state.IsKeyDown(Keys.D1) && !_oldKeyboardState.IsKeyDown(Keys.D1))
            {
                _world.MaximumEnemies = 1;
            }
            if (state.IsKeyDown(Keys.D2) && !_oldKeyboardState.IsKeyDown(Keys.D2))
            {
                _world.MaximumEnemies = 2;
            }
            if (state.IsKeyDown(Keys.D3) && !_oldKeyboardState.IsKeyDown(Keys.D3))
            {
                _world.MaximumEnemies = 3;
            }
            if (state.IsKeyDown(Keys.D4) && !_oldKeyboardState.IsKeyDown(Keys.D4))
            {
                _world.MaximumEnemies = 4;
            }
            if (state.IsKeyDown(Keys.D5) && !_oldKeyboardState.IsKeyDown(Keys.D5))
            {
                _world.MaximumEnemies = 5;
            }

            // Toggles use of debug commands
            if (state.IsKeyDown(Keys.T) && !_oldKeyboardState.IsKeyDown(Keys.T))
            {
                if (!_allowDebugControls) { _allowDebugControls = true; }
                else { _allowDebugControls = false; }
            }
            // - Debug Commands -
            if (_allowDebugControls)
            {
                // Spawns an enemy at the cursor position
                if (mouseStateMiddle == ButtonState.Pressed && _oldMouseStateMiddle != ButtonState.Pressed)
                {
                    _world.DebugSpawnEnemy(_cursorPos);
                }
                // Toggles collisions between characters
                if (state.IsKeyDown(Keys.C) && !_oldKeyboardState.IsKeyDown(Keys.C))
                {
                    _world.PhysicsManager.ToggleCharacterCollision();
                }
                // Draws a visualisation of the node grid
                if (state.IsKeyDown(Keys.G) && !_oldKeyboardState.IsKeyDown(Keys.G))
                {
                    if (!_drawGrid) { _drawGrid = true; }
                    else { _drawGrid = false; }
                }
                // Toggles AI behaviour
                if (state.IsKeyDown(Keys.I) && !_oldKeyboardState.IsKeyDown(Keys.I))
                {
                    if (!_world.AllowAIBehaviour) { _world.AllowAIBehaviour = true; }
                    else { _world.AllowAIBehaviour = false; }
                }
                // Toggles the drawing of the connection nodes used by the AI to navigate
                if (state.IsKeyDown(Keys.O) && !_oldKeyboardState.IsKeyDown(Keys.O))
                {
                    if (!_drawNodes) { _drawNodes = true; }
                    else { _drawNodes = false; }
                }
                // Toggles the drawing of the current path through the nodes of the first enemy
                if (state.IsKeyDown(Keys.P) && !_oldKeyboardState.IsKeyDown(Keys.P))
                {
                    if (!_drawPath) { _drawPath = true; }
                    else { _drawPath = false; }
                }
                // Toggles whether or not the AI can be 'killed' by projectiles
                if (state.IsKeyDown(Keys.M) && !_oldKeyboardState.IsKeyDown(Keys.M))
                {
                    if (!_world.ProjectileCharacterCollision) { _world.ProjectileCharacterCollision = true; }
                    else { _world.ProjectileCharacterCollision = false; }
                }
            }

            // Stores the previous states of the mouse/keyboard
            _oldMouseStateLeft = mouseStateLeft;
            _oldMouseStateMiddle = mouseStateMiddle;
            _oldMouseStateRight = mouseStateRight;
            _oldKeyboardState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            // Sets the background colour
            GraphicsDevice.Clear(Color.DimGray);

            // Set the sprite drawing order, and also the view matrix for the camera
            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: _camera.ViewMatrix());

            // Draw the objects in the scene
            foreach (Physics.PhysObject po in _world.PhysicsManager.AllPhysObjects)
            {
                // Draw projectiles
                if (po is Physics.Projectile)
                {
                    Physics.Projectile p = (Physics.Projectile)po;

                    if (p.Type == Physics.Projectile.ProjectileType.Ball)
                    {
                        _spriteBatch.Draw(_ballTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.7f);
                    }
                    else if (p.Type == Physics.Projectile.ProjectileType.Bullet)
                    {
                        _spriteBatch.Draw(_bulletTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.9f);
                    }
                }
                // Draw characters
                else if (po is Physics.Character)
                {
                    Physics.Character c = (Physics.Character)po;

                    if (c.IsPlayer)
                    {
                        _spriteBatch.Draw(_playerTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.8f);
                    }
                    else
                    {
                        _spriteBatch.Draw(_enemyTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.9f);
                    }
                }
                // Draw target
                else if (po.IsTarget)
                {
                    _spriteBatch.Draw(_targetTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.5f);
                }
                // Draw pendulums
                else if (po is Physics.Pendulum)
                {
                    _spriteBatch.Draw(_pendulumTexture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.5f);
                }

                // Draw everything else
                else
                {
                    _spriteBatch.Draw(po.Texture, po.TexturePos, null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.9f);
                }
            }

            // DEBUG - Draws the node grid
            if (_drawGrid)
            {
                _spriteBatch.Draw(_gridTexture, new Vector2(0, 0), null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.6f);
            }

            // DEBUG - Draws the connection nodes
            if (_drawNodes)
            {
                foreach (AI.Node n in _world.AIManager.DebugNodeDisplay)
                {
                    _spriteBatch.Draw(_debugNodeTexture, new Vector2(n.Position.X - 16, n.Position.Y - 16), null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.7f);
                }
            }

            // DEBUG - Draws the path for the first enemy
            if (_drawPath)
            {
                foreach (AI.Node n in _world.AIManager.DebugPathCheck)
                {
                    _spriteBatch.Draw(_debugPathTexture, new Vector2(n.Position.X - 16, n.Position.Y - 16), null, null, Vector2.Zero, 0, null, null, SpriteEffects.None, 0.65f);
                }
            }

            // Draw the UI text
            _spriteBatch.DrawString(_font, "Controls in README", _camera.CameraPosition + new Vector2(16, 16), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);

            if (_world.PortalHealth > 20)
            {
                _spriteBatch.DrawString(_font, "Portal Health: " + _world.PortalHealth + "%", _camera.CameraPosition + new Vector2(16, 48), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            }
            else
            {
                _spriteBatch.DrawString(_font, "Portal Health: " + _world.PortalHealth + "%", _camera.CameraPosition + new Vector2(16, 48), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            }
            _spriteBatch.DrawString(_font, "Score: " + (int)_world.Score, _camera.CameraPosition + new Vector2(16, 80), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            _spriteBatch.DrawString(_font, "Ball Ammo: " + _world.Player.BallAmmo, _camera.CameraPosition + new Vector2(16, 112), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);


            // Display loss message if portal health at 0
            if (_world.PortalHealth == 0)
            {
                _spriteBatch.DrawString(_font, "Oh no! You lost! Your score is: " + (int)_world.Score + "\n\n        Game closing in " + (int)_delayRemaining + "...", _camera.CameraPosition + new Vector2(620, 350), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            }

            // Draw the cursor last so it appears in the front
            _spriteBatch.Draw(_cursorTexture, _cursorPos);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
