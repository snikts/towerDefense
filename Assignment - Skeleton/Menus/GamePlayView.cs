using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace CS5410
{
    public class GamePlayView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "Isn't this game fun!";
        private List<Objects.AnimatedSprite> fighterIdle;
        private Objects.AnimatedSprite fighterSwipe;
        private Objects.AnimatedSprite fighterButton;
        private SpriteRenderer fBRenderer;
        private SpriteRenderer fIRenderer;
        private SpriteRenderer fSRenderer;

        private List<Objects.AnimatedSprite> wizardIdle;
        private Objects.AnimatedSprite wizardCast;
        private Objects.AnimatedSprite wizardButton;
        private SpriteRenderer wBRenderer;
        private SpriteRenderer wIRenderer;
        private SpriteRenderer wCRenderer;

        private List<Objects.AnimatedSprite> rangerIdle;
        private Objects.AnimatedSprite rangerShoot;
        private Objects.AnimatedSprite rangerButton;
        private SpriteRenderer rBRenderer;
        private SpriteRenderer rIRenderer;
        private SpriteRenderer rSRenderer;

        private List<Objects.AnimatedSprite> warlockIdle;
        private Objects.AnimatedSprite warlockCast;
        private Objects.AnimatedSprite warlockSwipe;
        private Objects.AnimatedSprite warlockButton;
        private SpriteRenderer waBRenderer;
        private SpriteRenderer waIRenderer;
        private SpriteRenderer waCRenderer;
        private SpriteRenderer waSRenderer;

        private Objects.AnimatedSprite goblinWalk;
        private Objects.AnimatedSprite goblinDeath;
        private SpriteRenderer gWRenderer;
        private SpriteRenderer gDRenderer;

        private Objects.AnimatedSprite hobgoblinWalk;
        private Objects.AnimatedSprite hobgoblinDeath;
        private SpriteRenderer hWRenderer;
        private SpriteRenderer hDRenderer;

        private Objects.AnimatedSprite dragonFly;
        private Objects.AnimatedSprite dragonDeath;
        private SpriteRenderer dFRenderer;
        private SpriteRenderer dDRenderer;

        private Texture2D grass;
        private Texture2D walls;
        private Texture2D pillars;
        private Texture2D coinIcon;
        private Texture2D wood;

        private Objects.GameCell[,] board;

        private Input.MouseInput m_inputMouse;

        private bool placingFighter;
        private bool placingWizard;
        private bool placingRanger;
        private bool placingWarlock;

        private Texture2D[] fighterLevels;
        private Texture2D[] wizardLevels;
        private Texture2D[] rangerLevels;
        private Texture2D[] warlockLevels;

        private Texture2D radius;
        private Texture2D errorRadius;

        private Vector2 placingPos;
        private int currLevel;
        private bool gridOn = false;
        private bool wasGDown = false;

        private Texture2D graph;
        private bool canPlace = true;
        private DateTime errorTime;

        private Objects.ParticleSystem particleSystem;
        private Texture2D[] particleTextures;

        private Keys[] controlKeys;
        public const string fileName = "Controls";
        private bool keysLoaded;

        private double[] highScores;
        private const string fileName2 = "HighScores";

        // TEMPORARY -- MOVE TO PLAYER CLASS
        //private double highScore = 10;
        private Objects.Player player;




        public override void loadContent(ContentManager contentManager)
        {
            fighterLevels = new Texture2D[3];
            wizardLevels = new Texture2D[3];
            rangerLevels = new Texture2D[3];
            warlockLevels = new Texture2D[3];
            errorTime = DateTime.Now;
            keysLoaded = false;

            currLevel = 0;

            m_font = contentManager.Load<SpriteFont>("Fonts/Alkhemikal");
            fighterIdle = new List<Objects.AnimatedSprite>();
            fighterSwipe = new Objects.Fighter(new Vector2(64, 64), new Vector2(128, 128), 1, 1, 5);
            fighterButton = new Objects.Fighter(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth-384+96, 64), 1, 1, 5);
            fIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterFixed3"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250 });
            fSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterSwipeFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            fBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterFixed3"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250 });

            wizardIdle = new List<Objects.AnimatedSprite>();
            wIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            wizardCast = new Objects.Wizard(new Vector2(64, 64), new Vector2(256, 128), 1, 1, 10);
            wCRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardCastFixed"), new int[] { 250, 250, 250, 250, 250, 250 });
            wizardButton = new Objects.Wizard(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, (m_graphics.PreferredBackBufferHeight/6) + 64), 1, 1, 5);
            wBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250});

            rangerIdle = new List<Objects.AnimatedSprite>();
            rIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            rangerShoot = new Objects.Ranger(new Vector2(84, 84), new Vector2(384, 128), 1, 1, 15);
            rSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerShootFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250, 250 });
            rangerButton = new Objects.Ranger(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6)*2) + 64), 1, 1, 5);
            rBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });

            warlockIdle = new List<Objects.AnimatedSprite>();
            waIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            warlockCast = new Objects.Warlock(new Vector2(64, 64), new Vector2(512, 128), 1, 1, 1, 15, 5);
            waCRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockCastFixed"), new int[] { 250, 250, 250, 250, 250, 250});
            warlockSwipe = new Objects.Warlock(new Vector2(64, 64), new Vector2(576, 128), 1, 1, 1, 15, 5);
            waSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockSwipeFixed"), new int[] { 250, 250, 250, 250, 250, 250 });
            warlockButton = new Objects.Warlock(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6) *3) + 64), 1, 1, 1, 15, 5);
            waBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });

            goblinWalk = new Objects.Goblin(new Vector2(64, 64), new Vector2(640, 128), 1, 1);
            gWRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/GoblinWalkFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            goblinDeath = new Objects.Goblin(new Vector2(64, 64), new Vector2(704, 128), 1, 1);
            gDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/GoblinDeathFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });

            hobgoblinWalk = new Objects.Hobgoblin(new Vector2(64, 64), new Vector2(768, 128), 1, 1);
            hWRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/HobGoblinWalkFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            hobgoblinDeath = new Objects.Hobgoblin(new Vector2(64, 64), new Vector2(832, 128), 1, 1);
            hDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/HobGoblinDeathFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250});

            dragonFly = new Objects.Dragon(new Vector2(64, 64), new Vector2(896, 128), 1, 1);
            dFRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/DragonFlyFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            dragonDeath = new Objects.Dragon(new Vector2(64, 64), new Vector2(960, 128), 1, 1);
            dDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/DragonDeathFixed"), new int[] { 250, 250, 250, 250, 250});

            grass = contentManager.Load<Texture2D>("Images/Grass");
            walls = contentManager.Load<Texture2D>("Images/walls");
            pillars = contentManager.Load<Texture2D>("Images/pillar");
            //coinIcon = contentManager.Load<Texture2D>("Images/coinIcon");
            wood = contentManager.Load<Texture2D>("Images/woodBack");
            fighterLevels[0] = contentManager.Load<Texture2D>("Images/fighter1");
            fighterLevels[1] = contentManager.Load<Texture2D>("Images/fighter2");
            fighterLevels[2] = contentManager.Load<Texture2D>("Images/fighter3");
            wizardLevels[0] = contentManager.Load<Texture2D>("Images/wizard1");
            wizardLevels[1] = contentManager.Load<Texture2D>("Images/wizard2");
            wizardLevels[2] = contentManager.Load<Texture2D>("Images/wizard3");
            rangerLevels[0] = contentManager.Load<Texture2D>("Images/ranger1");
            rangerLevels[1] = contentManager.Load<Texture2D>("Images/ranger2");
            rangerLevels[2] = contentManager.Load<Texture2D>("Images/ranger3");
            warlockLevels[0] = contentManager.Load<Texture2D>("Images/warlock1");
            warlockLevels[1] = contentManager.Load<Texture2D>("Images/warlock2");
            warlockLevels[2] = contentManager.Load<Texture2D>("Images/warlock3");

            radius = contentManager.Load<Texture2D>("Images/fighter1Radius");
            errorRadius = contentManager.Load<Texture2D>("Images/errorRadius");

            graph = contentManager.Load<Texture2D>("Images/gridpiece");



            placingFighter = false;

            board = new Objects.GameCell[m_graphics.PreferredBackBufferHeight / 64, (m_graphics.PreferredBackBufferWidth-384) / 64];
            for (int y = 0; y < (m_graphics.PreferredBackBufferHeight) / 64; y++)
            {
                for (int x = 0; x < (m_graphics.PreferredBackBufferWidth-384) / 64; x++)
                {
                    board[y, x] = new Objects.GameCell(x * 64, y * 64);
                }
            }

            m_inputMouse = new Input.MouseInput();
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseDown, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseDown));
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseUp, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseUp));
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseMove, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseMove));

            particleTextures = new Texture2D[] {contentManager.Load<Texture2D>("Images/deathParticles"), contentManager.Load<Texture2D>("Images/fireball"), contentManager.Load<Texture2D>("Images/explosion"), contentManager.Load<Texture2D>("Images/eldritchParticle"), contentManager.Load<Texture2D>("Images/eldritchHit"), contentManager.Load<Texture2D>("Images/coinIcon") };
            particleSystem = new Objects.ParticleSystem(contentManager, particleTextures);
            // particleSystem.creepDeath(new TimeSpan(0, 0, 0, 0, 100), 500, 500, 25, 2, new TimeSpan(0, 0, 1));

            // load content from controls file
            // deserialize it from json into keys array

            if (File.Exists(fileName2))
            {
                string jsonContents = File.ReadAllText(fileName2);
                highScores = JsonSerializer.Deserialize<double[]>(jsonContents);
            }
            else
            {
                highScores = new double[] { 0, 0, 0, 0};
            }

            player = new Objects.Player(0, 100, new Vector2(32, 32), new Vector2(64, 64));

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (!keysLoaded)
            {
                if (File.Exists(fileName))
                {
                    string jsonContents = File.ReadAllText(fileName);
                    controlKeys = JsonSerializer.Deserialize<Keys[]>(jsonContents);
                }
                else
                {
                    controlKeys = new Keys[] { Keys.U, Keys.S, Keys.G };
                    File.WriteAllText(fileName, JsonSerializer.Serialize(controlKeys));
                }
                keysLoaded = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                int lowest = 0;
                for (int i = 0; i < highScores.Length; i++)
                {
                    if(highScores[lowest] > highScores[i])
                    {
                        lowest = i;
                    }
                   
                }
                if (highScores[lowest] < player.getScore())
                {
                    highScores[lowest] = player.getScore();
                }
                File.WriteAllText(fileName2, JsonSerializer.Serialize(highScores));
                return GameStateEnum.MainMenu;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.G) && !wasGDown)
            {
                wasGDown = true;
                gridOn = !gridOn;
            }
            else if(wasGDown && !Keyboard.GetState().IsKeyDown(Keys.G))
            {
                wasGDown = false;
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            for (int i = 0; i < m_graphics.PreferredBackBufferWidth-384; i += 128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(grass, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            for (int i = m_graphics.PreferredBackBufferWidth - 384; i < m_graphics.PreferredBackBufferWidth; i+=128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(wood, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            //m_spriteBatch.Draw(wood, new Rectangle(m_graphics.PreferredBackBufferWidth - 384, 0, 128, 128), Color.White);
            //m_spriteBatch.Draw(wood, new Rectangle(m_graphics.PreferredBackBufferWidth - 384+128, 0, 128, 128), Color.White);

            for (int i = 0; i < m_graphics.PreferredBackBufferWidth-384-64; i+=62)
            {
                if(i < 868 || i > 1116)
                {
                    m_spriteBatch.Draw(walls, new Rectangle(i, 0, 64, 64), Color.White);
                }
                if(i >= 868 && i < 930)
                {
                    m_spriteBatch.Draw(pillars, new Rectangle(i-20, 0, 64, 64), Color.White);
                }
            }
            m_spriteBatch.Draw(pillars, new Rectangle(1116 + 22, 0, 64, 64), Color.White);

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y), Color.Yellow);

            for (int i = 0; i < fighterIdle.Count; i++)
            {
                fIRenderer.draw(m_spriteBatch, fighterIdle[i]);
            }

            for (int i = 0; i < wizardIdle.Count; i++)
            {
                wIRenderer.draw(m_spriteBatch, wizardIdle[i]);
            }

            for (int i = 0; i < rangerIdle.Count; i++)
            {
                rIRenderer.draw(m_spriteBatch, rangerIdle[i]);
            }

            for (int i = 0; i < warlockIdle.Count; i++)
            {
                waIRenderer.draw(m_spriteBatch, warlockIdle[i]);
            }

            if(gridOn)
            {
                for (int y = 0; y < m_graphics.PreferredBackBufferHeight; y+=64)
                {
                    for (int x = 0; x < m_graphics.PreferredBackBufferWidth-384; x+=64)
                    {
                        m_spriteBatch.Draw(graph, new Rectangle(x, y, 64, 64), Color.White);
                    }
                }
            }


            waCRenderer.draw(m_spriteBatch, warlockCast);
            waSRenderer.draw(m_spriteBatch, warlockSwipe);
            gWRenderer.draw(m_spriteBatch, goblinWalk);
            gDRenderer.draw(m_spriteBatch, goblinDeath);
            hWRenderer.draw(m_spriteBatch, hobgoblinWalk);
            hDRenderer.draw(m_spriteBatch, hobgoblinDeath);
            dFRenderer.draw(m_spriteBatch, dragonFly);
            dDRenderer.draw(m_spriteBatch, dragonDeath);
            fBRenderer.draw(m_spriteBatch, fighterButton);
            wBRenderer.draw(m_spriteBatch, wizardButton);
            rBRenderer.draw(m_spriteBatch, rangerButton);
            waBRenderer.draw(m_spriteBatch, warlockButton);

            //m_spriteBatch.Draw(coinIcon, new Rectangle((int)fighterButton.Center.X-64, (int)fighterButton.Center.Y + 32, 16, 16), Color.White);

            m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)fighterButton.Center.Y-100, 250, 250), Color.White);
            m_spriteBatch.Draw(wizardLevels[0], new Rectangle((int)wizardButton.Center.X, (int)wizardButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(rangerLevels[0], new Rectangle((int)rangerButton.Center.X, (int)rangerButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(warlockLevels[0], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y - 100, 250, 250), Color.White);

            if(placingFighter)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 64, (int)placingPos.Y - 64, 128, 128), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 64, (int)placingPos.Y - 64, 128, 128), Color.White);
                }
                //placingPos = null;
            }
            else if(placingWizard)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 96, (int)placingPos.Y - 96, 192, 192), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 96, (int)placingPos.Y - 96, 192, 192), Color.White);
                }
            }
            else if(placingRanger)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.White);
                }
            }
            else if(placingWarlock)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.Green);
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 64, (int)placingPos.Y - 64, 128, 128), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.White);
                }
            }


            if(currLevel == 1)
            {
                m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)warlockButton.Center.Y - 450, 250, 250), Color.White);
            }

            particleSystem.draw(m_spriteBatch);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            fIRenderer.update(gameTime);
            fSRenderer.update(gameTime);
            //fBRenderer.update(gameTime);
            wIRenderer.update(gameTime);
            wCRenderer.update(gameTime);
            rIRenderer.update(gameTime);
            rSRenderer.update(gameTime);
            waIRenderer.update(gameTime);
            waCRenderer.update(gameTime);
            waSRenderer.update(gameTime);
            gWRenderer.update(gameTime);
            gDRenderer.update(gameTime);
            hWRenderer.update(gameTime);
            hDRenderer.update(gameTime);
            dFRenderer.update(gameTime);
            dDRenderer.update(gameTime);
            m_inputMouse.Update(gameTime);

            particleSystem.update(gameTime);

            if((DateTime.Now-errorTime).TotalMilliseconds >= 100)
            {
                canPlace = true;
            }

        }

        private void onMouseDown(GameTime gameTime, int x, int y)
        {
            if (x < fighterButton.Center.X + 64 && x > fighterButton.Center.X - 64)
            {
                if (y < fighterButton.Center.Y + 64 && y > fighterButton.Center.Y - 64)
                {
                    placingFighter = true;
                }

            }
            if (x < wizardButton.Center.X + 64 && x > wizardButton.Center.X - 64)
            {
                if (y < wizardButton.Center.Y + 64 && y > wizardButton.Center.Y - 64)
                {
                    placingWizard = true;
                }
            }
            if (x < rangerButton.Center.X + 64 && x > rangerButton.Center.X - 64)
            {
                if (y < rangerButton.Center.Y + 64 && y > rangerButton.Center.Y - 64)
                {
                    placingRanger = true;
                }
            }
            if (x < warlockButton.Center.X + 64 && x > warlockButton.Center.X - 64)
            {
                if (y < warlockButton.Center.Y + 64 && y > warlockButton.Center.Y - 64)
                {
                    placingWarlock = true;
                }
            }
            if (x < m_graphics.PreferredBackBufferWidth - 384 && placingFighter)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() == null)
                {
                    fighterIdle.Add(new Objects.Fighter(new Vector2(64, 64), new Vector2(board[row, column].m_x + 64, board[row, column].m_y + 64), 1, 1, 5));
                    m_mouseCapture = true;

                    board[row, column].setObject(fighterIdle[fighterIdle.Count - 1]);

                    placingFighter = false;
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }
            else if (x < m_graphics.PreferredBackBufferWidth - 384 && placingWizard)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() == null)
                {
                    wizardIdle.Add(new Objects.Wizard(new Vector2(64, 64), new Vector2(board[row, column].m_x+64, board[row, column].m_y+64), 1, 1, 5));
                    m_mouseCapture = true;

                    board[row, column].setObject(wizardIdle[wizardIdle.Count - 1]);

                    placingWizard = false;
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }
            else if (x < m_graphics.PreferredBackBufferWidth - 384 && placingRanger)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() == null)
                {
                    rangerIdle.Add(new Objects.Ranger(new Vector2(64, 64), new Vector2(board[row, column].m_x + 64, board[row, column].m_y + 64), 1, 1, 5));
                    m_mouseCapture = true;

                    board[row, column].setObject(rangerIdle[rangerIdle.Count - 1]);

                    placingRanger = false;
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }
            else if (x < m_graphics.PreferredBackBufferWidth - 384 && placingWarlock)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() == null)
                {
                    warlockIdle.Add(new Objects.Warlock(new Vector2(64, 64), new Vector2(board[row, column].m_x + 64, board[row, column].m_y + 64), 1, 1, 1, 15, 5));
                    m_mouseCapture = true;

                    board[row, column].setObject(warlockIdle[warlockIdle.Count - 1]);

                    placingWarlock = false;
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }
        }

        private void onMouseUp(GameTime gameTime, int x, int y)
        {
            m_mouseCapture = false;
        }
        private bool m_mouseCapture = false;

        private void onMouseMove(GameTime gameTime, int x, int y)
        {
            if (m_mouseCapture && placingFighter)
            {
                //fighterIdle.setX(x);
                //fighterIdle.setY(y);
            }
            if(placingFighter || placingWizard || placingRanger || placingWarlock)
            {
                placingPos = new Vector2(x, y);
            }

        }
    }
}
