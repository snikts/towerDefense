using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class HelpView : GameStateView
    {
        private SpriteFont m_font;
        private const string up = "UP CONTROL:";
        private const string down = "DOWN CONTROL:";
        private const string left = "LEFT CONTROL:";
        private const string right = "RIGHT CONTROL:";

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/Alkhemikal");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Help;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_font.MeasureString(up);
            m_spriteBatch.DrawString(m_font, up,
                new Vector2(m_graphics.PreferredBackBufferWidth * (1 / 5) - stringSize.X / 2, m_graphics.PreferredBackBufferHeight * (1 / 5) - stringSize.Y), Color.Yellow);

            Vector2 stringSize2 = m_font.MeasureString(down);
            m_spriteBatch.DrawString(m_font, down,
                new Vector2(m_graphics.PreferredBackBufferWidth * (2 / 5) - stringSize2.X / 2, m_graphics.PreferredBackBufferHeight * (2 / 5) - stringSize2.Y), Color.Yellow);

            Vector2 stringSize3 = m_font.MeasureString(left);
            m_spriteBatch.DrawString(m_font, left,
                new Vector2(m_graphics.PreferredBackBufferWidth * (3 / 5) - stringSize3.X / 3, m_graphics.PreferredBackBufferHeight * (3 / 5) - stringSize3.Y), Color.Yellow);

            Vector2 stringSize4 = m_font.MeasureString(right);
            m_spriteBatch.DrawString(m_font, right,
                new Vector2(m_graphics.PreferredBackBufferWidth * (4 / 5) - stringSize4.X / 2, m_graphics.PreferredBackBufferHeight * (4 / 5) - stringSize4.Y), Color.Yellow);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
