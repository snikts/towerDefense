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
    public class HighScoresView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "These are the high scores";
        private double[] highScores = new double[] { 0, 0, 0, 0};
        private bool scoresLoaded;
        public const string fileName = "HighScores";

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/Alkhemikal");
            //File.WriteAllText(fileName, JsonSerializer.Serialize(highScores));
            scoresLoaded = false;
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                scoresLoaded = false;
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.HighScores;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            float bottom = drawMenuItem(
                m_font,
                "High Score 1: " + highScores[0],
                200,
                Color.Blue);
            bottom = drawMenuItem(
                m_font,
                "High Score 2: " + highScores[1],
                bottom,
                Color.Blue);
            bottom = drawMenuItem(
                m_font,
                "High Score 3: " + highScores[2],
                bottom,
                Color.Blue);
            bottom = drawMenuItem(
                m_font,
                "High Score 4: " + highScores[3],
                bottom,
                Color.Blue);
            //bottom = drawMenuItem(
            //    m_font,
            //    "High Score 5: " + highScores[4],
            //    bottom,
            //    Color.Blue);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            if(!scoresLoaded)
            {
                if(File.Exists(fileName))
                {
                    string jsonContents = File.ReadAllText(fileName);
                    highScores = JsonSerializer.Deserialize<double[]>(jsonContents);
                }
                else
                {
                    //highScores = new double[] { 0, 0, 0, 0, 0};
                    File.WriteAllText(fileName, JsonSerializer.Serialize(highScores));
                }
                scoresLoaded = true;
            }
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
    }
}
