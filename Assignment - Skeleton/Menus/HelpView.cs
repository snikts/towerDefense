using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json;
using System;
using System.Linq;

namespace CS5410
{
    public class HelpView : GameStateView
    {
        private SpriteFont m_font;
        private string UPGRADEMESSAGE = "UPGRADE TOWER: ";
        private string SELLMESSAGE = "SELL TOWER: ";
        private string STARTLEVEL = "START LEVEL: ";
        private const string CHANGING = "Changing Key! Select a new key to use.";
        private Keys[] controlKeys = new Keys[] {Keys.U, Keys.S, Keys.G};
        public const string fileName = "Controls";
        private bool m_waitForKeyRelease = false;
        private bool listening = false;
        private KeyboardState prevState;
        private KeyboardState currState;

        private enum MenuState
        {
            Upgrade = 0,
            Sell = 1,
            Start = 2
        }
        private MenuState m_currentSelection = MenuState.Upgrade;

        public override void loadContent(ContentManager contentManager)
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(controlKeys));
            m_font = contentManager.Load<SpriteFont>("Fonts/Alkhemikal");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            currState = Keyboard.GetState();   
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }
            if(!m_waitForKeyRelease)
            {
                if (listening)
                {
                    Keys[] allKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToArray();
                    foreach (var keys in allKeys)
                    {
                        if(currState.IsKeyDown(keys))
                        {
                            controlKeys[(int)m_currentSelection] = keys;
                            File.WriteAllText(fileName, JsonSerializer.Serialize(controlKeys));
                            listening = false;
                            // set prompt to false
                            break;
                        }
                    }
                }
                else
                {
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

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        // write that key can be changed
                        listening = true;
                        m_waitForKeyRelease = true;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.Help;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            if(listening)
            {
                Vector2 stringSize = m_font.MeasureString(CHANGING);
                m_spriteBatch.DrawString(m_font, CHANGING, new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, stringSize.Y / 2), Color.Yellow);
            }

            float bottom = drawMenuItem(
                m_currentSelection == MenuState.Upgrade ? m_font : m_font,
                UPGRADEMESSAGE + controlKeys[0].ToString(),
                200,
                m_currentSelection == MenuState.Upgrade ? Color.Yellow : Color.Blue);
            bottom = drawMenuItem(
                m_currentSelection == MenuState.Sell ? m_font : m_font,
                SELLMESSAGE + controlKeys[1].ToString(),
                bottom,
                m_currentSelection == MenuState.Sell ? Color.Yellow : Color.Blue);
            //bottom = drawMenuItem(m_currentSelection == MenuState.Upgrade ? m_font : m_font, UPGRADEMESSAGE, bottom, m_currentSelection == MenuState.Upgrade ? Color.White : Color.Black);
            bottom = drawMenuItem(m_currentSelection == MenuState.Start ? m_font : m_font, STARTLEVEL + controlKeys[2].ToString(), bottom, m_currentSelection == MenuState.Start ? Color.Yellow : Color.Blue);
            //bottom = drawMenuItem(m_currentSelection == MenuState.About ? m_fontMenuSelect : m_fontMenu, "About", bottom, m_currentSelection == MenuState.About ? Color.White : Color.Black);
            //drawMenuItem(m_currentSelection == MenuState.Quit ? m_fontMenuSelect : m_fontMenu, "Quit", bottom, m_currentSelection == MenuState.Quit ? Color.White : Color.Black);

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

            return y + stringSize.Y + stringSize.Y * .5f;
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
