using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class MainMenuView : GameStateView
    {
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;
        private Texture2D background;
        private Texture2D title;
        private Texture2D knightPoster;

        private enum MenuState
        {
            NewGame,
            HighScores,
            Help,
            About,
            Quit
        }

        private MenuState m_currentSelection = MenuState.NewGame;
        private bool m_waitForKeyRelease = false;

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/Quiapo_Free");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/Quiapo_Free");
            background = contentManager.Load<Texture2D>("Images/woodBackground");
            title = contentManager.Load<Texture2D>("Images/title");
            knightPoster = contentManager.Load<Texture2D>("Images/journal");
        }
        public override GameStateEnum processInput(GameTime gameTime)
        {
            // This is the technique I'm using to ensure one keypress makes one menu navigation move
            if (!m_waitForKeyRelease)
            {
                // Arrow keys to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    m_currentSelection = m_currentSelection + 1;
                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    m_currentSelection = m_currentSelection - 1;
                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.NewGame)
                {
                    return GameStateEnum.GamePlay;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.HighScores)
                {
                    return GameStateEnum.HighScores;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Help)
                {
                    return GameStateEnum.Help;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.About)
                {
                    return GameStateEnum.About;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Quit)
                {
                    return GameStateEnum.Exit;
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.MainMenu;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Rectangle backgroundRect = new Rectangle(0, 0, 512, 512);
            Rectangle backgroundRect2 = new Rectangle(0, 512, 512, 512);
            Rectangle backgroundRect3 = new Rectangle(512, 0, 512, 512);
            Rectangle backgroundRect4 = new Rectangle(512, 512, 512, 512);
            Rectangle backgroundRect5 = new Rectangle(1024, 0, 512, 512);
            Rectangle backgroundRect6 = new Rectangle(1024, 512, 512, 512);
            Rectangle backgroundRect7 = new Rectangle(1536, 0, 512, 512);
            Rectangle backgroundRect8 = new Rectangle(1536, 512, 512, 512);
            Rectangle backgroundRect9 = new Rectangle(0, 1024, 512, 512);
            Rectangle backgroundRect10 = new Rectangle(512, 1024, 512, 512);
            Rectangle backgroundRect11 = new Rectangle(1024, 1024, 512, 512);
            Rectangle backgroundRect12 = new Rectangle(1536, 1024, 512, 512);
            m_spriteBatch.Draw(background, backgroundRect, Color.White);
            m_spriteBatch.Draw(background, backgroundRect2, Color.White);
            m_spriteBatch.Draw(background, backgroundRect3, Color.White);
            m_spriteBatch.Draw(background, backgroundRect4, Color.White);
            m_spriteBatch.Draw(background, backgroundRect5, Color.White);
            m_spriteBatch.Draw(background, backgroundRect6, Color.White);
            m_spriteBatch.Draw(background, backgroundRect7, Color.White);
            m_spriteBatch.Draw(background, backgroundRect8, Color.White);
            m_spriteBatch.Draw(background, backgroundRect9, Color.White);
            m_spriteBatch.Draw(background, backgroundRect10, Color.White);
            m_spriteBatch.Draw(background, backgroundRect11, Color.White);
            m_spriteBatch.Draw(background, backgroundRect12, Color.White);

            Rectangle titleRect = new Rectangle(175, -50, 1584, 396);
            m_spriteBatch.Draw(title, titleRect, Color.White);

            //Vector2 stringSize = m_fontMenu.MeasureString("ADVENTURERS FOR HIRE");
            //m_spriteBatch.DrawString(m_fontMenu, "ADVENTURERS FOR HIRE", new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X*1.5f, 0 + stringSize.Y/2), Color.Black, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);

            // I split the first one's parameters on separate lines to help you see them better
            float bottom = drawMenuItem(
                m_currentSelection == MenuState.NewGame ? m_fontMenuSelect : m_fontMenu,
                "New Game",
                200,
                m_currentSelection == MenuState.NewGame ? Color.White : Color.Black);
            bottom = drawMenuItem(m_currentSelection == MenuState.HighScores ? m_fontMenuSelect : m_fontMenu, "High Scores", bottom, m_currentSelection == MenuState.HighScores ? Color.White : Color.Black);
            bottom = drawMenuItem(m_currentSelection == MenuState.Help ? m_fontMenuSelect : m_fontMenu, "Help", bottom, m_currentSelection == MenuState.Help ? Color.White : Color.Black);
            bottom = drawMenuItem(m_currentSelection == MenuState.About ? m_fontMenuSelect : m_fontMenu, "About", bottom, m_currentSelection == MenuState.About ? Color.White : Color.Black);
            drawMenuItem(m_currentSelection == MenuState.Quit ? m_fontMenuSelect : m_fontMenu, "Quit", bottom, m_currentSelection == MenuState.Quit ? Color.White : Color.Black);


            m_spriteBatch.End();
        }

        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color,
                0, 
                new Vector2(0, 0), 
                1.5f, 
                SpriteEffects.None, 
                0);

            return y + stringSize.Y+stringSize.Y*.5f;
        }
    }
}
