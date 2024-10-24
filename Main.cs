using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGamePixelTemplate {
    public class Main : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const int DEFAULT_SCREEN_WIDTH = 640;
        private const int DEFAULT_SCREEN_HEIGHT = 480;

        private const int TARGET_WIDTH = 64;
        private const int TARGET_HEIGHT = 64;


        private int _screenWidth;
        private int _screenHeight;

        private int _renderWidth;
        private int _renderHeight;

        Texture2D renderTarget;
        Rectangle renderTargetRect;

        Color[] render = new Color[TARGET_WIDTH * TARGET_HEIGHT];

        public Main() {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            _screenWidth = DEFAULT_SCREEN_WIDTH;
            _screenHeight = DEFAULT_SCREEN_HEIGHT;

            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.ClientSizeChanged += ClientChangedWindowSize;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize() {
            base.Window.Title = "MonoGame Pixel Template";

            renderTarget = new Texture2D(GraphicsDevice, TARGET_WIDTH, TARGET_HEIGHT);

            renderTarget.SetData(render);

            UpdateWindow();
            base.Initialize();
        }

        private void UpdateWindow() {
            float scale = Math.Min(_screenWidth / TARGET_WIDTH, _screenHeight / TARGET_HEIGHT);
            int newWidth = (int)(TARGET_WIDTH * scale);
            int newHeight = (int)(TARGET_HEIGHT * scale);

            _renderWidth = newWidth;
            _renderHeight = newHeight;

            renderTargetRect = new Rectangle(
                (_screenWidth - _renderWidth) / 2,
                (_screenHeight - _renderHeight) / 2,
                _renderWidth,
                _renderHeight);
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void ClientChangedWindowSize(object sender, EventArgs e) {
            if (GraphicsDevice.Viewport.Width != _graphics.PreferredBackBufferWidth ||
                GraphicsDevice.Viewport.Height != _graphics.PreferredBackBufferHeight) {
                if (Window.ClientBounds.Width == 0) return;
                _screenWidth = Window.ClientBounds.Width;
                _screenHeight = Window.ClientBounds.Height;

                _graphics.PreferredBackBufferWidth = _screenWidth;
                _graphics.PreferredBackBufferHeight = _screenHeight;
                _graphics.ApplyChanges();

                UpdateWindow();
            }
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // Update texture
            renderTarget.SetData(render);

            // Clear screen
            GraphicsDevice.Clear(Color.Black);

            // Draw render texture
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(renderTarget, renderTargetRect, Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}