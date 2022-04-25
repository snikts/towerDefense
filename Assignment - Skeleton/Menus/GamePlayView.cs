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
        // declare non sprite assets
        private SpriteFont m_font;
        private Texture2D grass;
        private Texture2D walls;
        private Texture2D pillars;
        private Texture2D wood;
        private Texture2D[] fighterLevels;
        private Texture2D[] wizardLevels;
        private Texture2D[] rangerLevels;
        private Texture2D[] warlockLevels;
        private Texture2D radius;
        private Texture2D errorRadius;
        private Texture2D graph;
        private Texture2D[] particleTextures;
        private Texture2D selected;
        private Texture2D healthbar;
        private Texture2D greenBar;
        private Texture2D redBar;
        private Texture2D goblinScore;
        private Texture2D hobgoblinScore;
        private Texture2D dragonScore;

        // declare animated sprite objects
        private List<Objects.AnimatedSprite> fighterIdle;
        private Objects.AnimatedSprite fighterButton;
        private List<Objects.AnimatedSprite> wizardIdle;
        private Objects.AnimatedSprite wizardButton;
        private List<Objects.AnimatedSprite> rangerIdle;
        private Objects.AnimatedSprite rangerButton;
        private List<Objects.AnimatedSprite> warlockIdle;
        private Objects.AnimatedSprite warlockButton;
        private List<Objects.AnimatedSprite> goblins;
        private List<Objects.AnimatedSprite> hobgoblins;
        private List<Objects.AnimatedSprite> dragons;
        private Objects.AnimatedSprite clicked;
        private List<Objects.AnimatedSprite> scores;

        // declare sprite renderers
        private SpriteRenderer fBRenderer;
        private SpriteRenderer fIRenderer;
        private SpriteRenderer fSRenderer;
        private SpriteRenderer wBRenderer;
        private SpriteRenderer wIRenderer;
        private SpriteRenderer wCRenderer;
        private SpriteRenderer rBRenderer;
        private SpriteRenderer rIRenderer;
        private SpriteRenderer rSRenderer;
        private SpriteRenderer waBRenderer;
        private SpriteRenderer waIRenderer;
        private SpriteRenderer waCRenderer;
        private SpriteRenderer waSRenderer;
        private SpriteRenderer gWRenderer;
        private SpriteRenderer gDRenderer;
        private SpriteRenderer hWRenderer;
        private SpriteRenderer hDRenderer;
        private SpriteRenderer dFRenderer;
        private SpriteRenderer dDRenderer;
        private SpriteRenderer pointerRenderer;
        private SpriteRenderer renderer100;
        private SpriteRenderer renderer200;
        private SpriteRenderer renderer300;

        // declare booleans used during gameplay
        private bool placingFighter;
        private bool placingWizard;
        private bool placingRanger;
        private bool placingWarlock;
        private bool gridOn;
        private bool wasSpaceDown;
        private bool wasGDown;
        private bool canPlace;
        private bool keysLoaded;
        private bool towerPlaced;
        private bool levelStarted;
        private bool wasUDown;
        private bool upgrading;
        private bool wasSDown;
        private bool selling;
        private bool waveEnded;

        // declare gameboard
        private Objects.GameCell[,] board;

        // declare mouse input device
        private Input.MouseInput m_inputMouse;

        // declare particle system
        private Objects.ParticleSystem particleSystem;

        // declare controls array and file
        private Keys[] controlKeys;
        public const string fileName = "Controls";

        // declare high scores array and file
        private double[] highScores;
        private const string fileName2 = "HighScores";

        // declare player object
        private Objects.Player player;

        // random variables needed for gameplay
        private Vector2 placingPos; // position tower is being placed at
        private int currLevel; // level the selected object is
        private DateTime errorTime; // time in which a tower failed to place
        private int level; // level of game
        private List<Vector2> startingLocations; // an array of starting locations for creeps
        private List<Vector2> endingLocations; // an array of destinations for creeps
        private DateTime lastCreep; // time last creep entered arena
        private int wave;
        private List<int[]> creepsPerWave;
        private int totalGoblins;
        private int totalHobGoblins;
        private int totalDragons;
        private DateTime waveTime;


        public override void loadContent(ContentManager contentManager)
        {
            // load nonsprite texture assets
            fighterLevels = new Texture2D[3];
            wizardLevels = new Texture2D[3];
            rangerLevels = new Texture2D[3];
            warlockLevels = new Texture2D[3];
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
            m_font = contentManager.Load<SpriteFont>("Fonts/Alkhemikal");
            grass = contentManager.Load<Texture2D>("Images/Grass");
            walls = contentManager.Load<Texture2D>("Images/walls");
            pillars = contentManager.Load<Texture2D>("Images/pillar");
            wood = contentManager.Load<Texture2D>("Images/woodBack");
            radius = contentManager.Load<Texture2D>("Images/fighter1Radius");
            errorRadius = contentManager.Load<Texture2D>("Images/errorRadius");
            graph = contentManager.Load<Texture2D>("Images/gridpiece");
            particleTextures = new Texture2D[] { contentManager.Load<Texture2D>("Images/deathParticles"), contentManager.Load<Texture2D>("Images/fireball"), contentManager.Load<Texture2D>("Images/explosion"), contentManager.Load<Texture2D>("Images/eldritchParticle"), contentManager.Load<Texture2D>("Images/eldritchHit"), contentManager.Load<Texture2D>("Images/coinIcon") };
            selected = contentManager.Load<Texture2D>("Images/selected");
            healthbar = contentManager.Load<Texture2D>("Images/BarBackground");
            greenBar = contentManager.Load<Texture2D>("Images/GreenBar");
            redBar = contentManager.Load<Texture2D>("Images/RedBar");
            goblinScore = contentManager.Load<Texture2D>("Images/100");
            hobgoblinScore = contentManager.Load<Texture2D>("Images/200");
            dragonScore = contentManager.Load<Texture2D>("Images/300");

            // load animated sprite assets
            fighterIdle = new List<Objects.AnimatedSprite>();
            fighterButton = new Objects.Fighter(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, 64), 1, 1, 5);
            wizardIdle = new List<Objects.AnimatedSprite>();
            wizardButton = new Objects.Wizard(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, (m_graphics.PreferredBackBufferHeight / 6) + 64), 1, 1, 5);
            rangerIdle = new List<Objects.AnimatedSprite>();
            rangerButton = new Objects.Ranger(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6) * 2) + 64), 1, 1, 5);
            warlockIdle = new List<Objects.AnimatedSprite>();
            warlockButton = new Objects.Warlock(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6) * 3) + 64), 1, 1, 1, 15, 5);
            goblins = new List<Objects.AnimatedSprite>();
            hobgoblins = new List<Objects.AnimatedSprite>();
            dragons = new List<Objects.AnimatedSprite>();
            scores = new List<Objects.AnimatedSprite>();

            // load renderer assets
            fIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterFixed3"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250 });
            fSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterSwipeFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            fBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/FighterFixed3"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250 });
            wIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            wCRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardCastFixed"), new int[] { 250, 250, 250, 250, 250, 250 });
            wBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WizardIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            rIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            rSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerShootFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250, 250, 250 });
            rBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/RangerIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            waIRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            waCRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockCastFixed"), new int[] { 250, 250, 250, 250, 250, 250 });
            waSRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockSwipeFixed"), new int[] { 250, 250, 250, 250, 250, 250 });
            waBRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/WarlockIdleFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            gWRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/GoblinWalkFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            gDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/GoblinDeathFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            hWRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/HobGoblinWalkFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            hDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/HobGoblinDeathFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250 });
            dFRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/DragonFlyFixed"), new int[] { 250, 250, 250, 250, 250, 250, 250, 250 });
            dDRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/DragonDeathFixed"), new int[] { 250, 250, 250, 250, 250 });
            pointerRenderer = new SpriteRenderer(contentManager.Load<Texture2D>("Images/Pointer"), new int[] { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 });

            // initialize gameplay booleans
            keysLoaded = false;
            placingFighter = false;
            towerPlaced = true;
            wasUDown = false;
            gridOn = false;
            wasSpaceDown = false;
            wasGDown = false;
            canPlace = true;
            wasSDown = false;
            selling = false;
            waveEnded = false;

            // initialize gameboard
            board = new Objects.GameCell[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    int originx = x * 64;
                    int originy = y * 64;
                    int centerx = originx + 32;
                    int centery = originy + 32;
                    board[y, x] = new Objects.GameCell(originx, originy, centerx, centery, 0);
                }
            }

            // initialize input mouse and register commands
            m_inputMouse = new Input.MouseInput();
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseDown, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseDown));
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseUp, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseUp));
            m_inputMouse.registerCommand(Input.MouseInput.MouseEvent.MouseMove, new Input.InputDeviceHelper.CommandDelegatePosition(onMouseMove));

            // initialize particle system
            particleSystem = new Objects.ParticleSystem(contentManager, particleTextures);

            // initialize high score array from file if possible, otherwise initialie high score array to 0
            if (File.Exists(fileName2))
            {
                string jsonContents = File.ReadAllText(fileName2);
                highScores = JsonSerializer.Deserialize<double[]>(jsonContents);
            }
            else
            {
                highScores = new double[] { 0, 0, 0, 0 };
            }

            // initialize player object
            player = new Objects.Player(100, 100, new Vector2(32, 32), new Vector2(64, 64));

            //initialize other objects needed for gameplay
            errorTime = DateTime.Now; // initialize last time we got an error placing tower to now
            currLevel = 0; // initialize the current level of our clicked object to 0
            level = 0; // initialize the current level of our game to zero
            startingLocations = new List<Vector2>(); // initialize our starting locations array
            startingLocations.Add(new Vector2(5, 0)); // add vector for level 1 starting location
            startingLocations.Add(new Vector2(0, 5)); // add vector for level 2 starting location
            endingLocations = new List<Vector2>(); // initialize our ending locations array
            endingLocations.Add(new Vector2(5, 9)); // add vector for level 1 destination
            endingLocations.Add(new Vector2(9, 5)); // add vector for level 2 destination
            lastCreep = DateTime.Now; // initialize the last time a creep entered the stage to now
            clicked = null; // initialize our currently clicked object to nothing
            //creeps.Add(new Objects.Goblin(new Vector2(64, 64), new Vector2(board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centerx, board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centery + 64), 1, 50));
            wave = 0;
            creepsPerWave = new List<int[]>();
            creepsPerWave.Add(new int[] { 5, 2, 1 });
            creepsPerWave.Add(new int[] { 10, 5, 2});
            totalGoblins = 0;
            totalHobGoblins = 0;
            totalDragons = 0;
            waveTime = DateTime.Now;

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            // if we haven't loaded our control keys from our control key file, try to load key array, otherwise, initialize key array
            if (!keysLoaded)
            {
                if (File.Exists(fileName))
                {
                    string jsonContents = File.ReadAllText(fileName);
                    controlKeys = JsonSerializer.Deserialize<Keys[]>(jsonContents);
                }
                else
                {
                    controlKeys = new Keys[] { Keys.U, Keys.S, Keys.G, Keys.Space };
                    File.WriteAllText(fileName, JsonSerializer.Serialize(controlKeys));
                }
                keysLoaded = true;
                towerPlaced = true;
            }

            // when escape is pressed, write high scores to file and exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                int lowest = 0;
                for (int i = 0; i < highScores.Length; i++)
                {
                    if (highScores[lowest] > highScores[i])
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

            // if space is pressed and space was not previously down, indicate that grid should be on
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !wasSpaceDown)
            {
                wasSpaceDown = true;
                gridOn = !gridOn;
            }
            else if (wasSpaceDown && !Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                wasSpaceDown = false;
            }

            // if g is pressed and wasn't previously down, indicate that level should start
            if (Keyboard.GetState().IsKeyDown(Keys.G) && !wasGDown)
            {
                wasGDown = true;
                if (!levelStarted)
                {
                    levelStarted = true;
                    //level++;
                    wave = 0;
                    if(level >= 2)
                    {
                        creepsPerWave.Add(new int[] { creepsPerWave[level - 1][0] + 5, creepsPerWave[level - 1][1] + 3, creepsPerWave[level - 1][2] + 1 });

                    }
                    totalDragons = 0;
                    totalGoblins = 0;
                    totalHobGoblins = 0;
                }
            }
            else if (wasGDown && !Keyboard.GetState().IsKeyDown(Keys.G))
            {
                wasGDown = false;
            }

            // if u is pressed and wasn't previously down, indicate that we are upgrading a tower
            if (Keyboard.GetState().IsKeyDown(Keys.U) && !wasUDown)
            {
                wasUDown = true;
                upgrading = true;
            }
            else if (wasUDown && !Keyboard.GetState().IsKeyDown(Keys.U))
            {
                wasUDown = false;
            }

            // if s is pressed and wasn't previously down, indicate that we are upgrading a tower
            if (Keyboard.GetState().IsKeyDown(Keys.S) && !wasSDown)
            {
                wasSDown = true;
                selling = true;
            }
            else if (wasSDown && !Keyboard.GetState().IsKeyDown(Keys.S))
            {
                wasSDown = false;
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // draw grass background
            for (int i = 0; i < m_graphics.PreferredBackBufferWidth - 384; i += 128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(grass, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            // draw wood menu background
            for (int i = m_graphics.PreferredBackBufferWidth - 384; i < m_graphics.PreferredBackBufferWidth; i += 128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(wood, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            // draw walls
            for (int i = 0; i < m_graphics.PreferredBackBufferWidth - 384 - 64; i += 62)
            {
                if (i < 868 || i > 1116)
                {
                    m_spriteBatch.Draw(walls, new Rectangle(i, 0, 64, 64), Color.White);
                }
                if (i >= 868 && i < 930)
                {
                    m_spriteBatch.Draw(pillars, new Rectangle(i - 20, 0, 64, 64), Color.White);
                }
            }
            m_spriteBatch.Draw(pillars, new Rectangle(1116 + 22, 0, 64, 64), Color.White);

            // draw all fighter towers
            for (int i = 0; i < fighterIdle.Count; i++)
            {
                fIRenderer.draw(m_spriteBatch, fighterIdle[i]);
            }

            // draw all wizard towers
            for (int i = 0; i < wizardIdle.Count; i++)
            {
                wIRenderer.draw(m_spriteBatch, wizardIdle[i]);
            }

            // draw all ranger towers
            for (int i = 0; i < rangerIdle.Count; i++)
            {
                rIRenderer.draw(m_spriteBatch, rangerIdle[i]);
            }

            // draw all warlock towers
            for (int i = 0; i < warlockIdle.Count; i++)
            {
                waIRenderer.draw(m_spriteBatch, warlockIdle[i]);
            }

            // if our grid is supposed to be on, draw our grid squares
            if (gridOn)
            {
                for (int y = 0; y < 10 * 64; y += 64)
                {
                    for (int x = 0; x < 10 * 64; x += 64)
                    {
                        m_spriteBatch.Draw(graph, new Rectangle(x, y, 64, 64), Color.White);
                    }
                }
            }

            // draw the menu sprites
            fBRenderer.draw(m_spriteBatch, fighterButton);
            wBRenderer.draw(m_spriteBatch, wizardButton);
            rBRenderer.draw(m_spriteBatch, rangerButton);
            waBRenderer.draw(m_spriteBatch, warlockButton);

            // draw the menu descriptions
            m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)fighterButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(wizardLevels[0], new Rectangle((int)wizardButton.Center.X, (int)wizardButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(rangerLevels[0], new Rectangle((int)rangerButton.Center.X, (int)rangerButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(warlockLevels[0], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y - 100, 250, 250), Color.White);

            // if we are placing a tower, draw the radius of the tower
            if (placingFighter)
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
            else if (placingWizard)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 160, (int)placingPos.Y - 160, 320, 320), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 160, (int)placingPos.Y - 160, 320, 320), Color.White);
                }
            }
            else if (placingRanger)
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
            else if (placingWarlock)
            {
                if (canPlace)
                {
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.Green);
                    m_spriteBatch.Draw(radius, new Rectangle((int)placingPos.X - 96, (int)placingPos.Y - 96, 192, 192), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(errorRadius, new Rectangle((int)placingPos.X - 192, (int)placingPos.Y - 192, 384, 384), Color.White);
                }
            }

            // draw the description for the level of our current clicked item
            if (currLevel == 1)
            {
                m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)warlockButton.Center.Y - 450, 250, 250), Color.White);
            }

            // draw all our creeps
            for (int i = 0; i < goblins.Count; i++)
            {

                m_spriteBatch.Draw(healthbar, new Rectangle((int)goblins[i].Center.X - 70, (int)goblins[i].Center.Y - 96, 32, 16), Color.White);
                double percentage = (double)goblins[i].m_health / (double)50;
                if (percentage >= .5)
                {
                    m_spriteBatch.Draw(greenBar, new Rectangle((int)goblins[i].Center.X - 70, (int)goblins[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(redBar, new Rectangle((int)goblins[i].Center.X - 70, (int)goblins[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                gWRenderer.draw(m_spriteBatch, goblins[i]);
            }
            for (int i = 0; i < hobgoblins.Count; i++)
            {

                m_spriteBatch.Draw(healthbar, new Rectangle((int)hobgoblins[i].Center.X - 70, (int)hobgoblins[i].Center.Y - 96, 32, 16), Color.White);
                double percentage = (double)hobgoblins[i].m_health / (double)100;
                if (percentage >= .5)
                {
                    m_spriteBatch.Draw(greenBar, new Rectangle((int)hobgoblins[i].Center.X - 70, (int)hobgoblins[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(redBar, new Rectangle((int)hobgoblins[i].Center.X - 70, (int)hobgoblins[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                hWRenderer.draw(m_spriteBatch, hobgoblins[i]);
            }
            for (int i = 0; i < dragons.Count; i++)
            {

                m_spriteBatch.Draw(healthbar, new Rectangle((int)dragons[i].Center.X - 70, (int)dragons[i].Center.Y - 96, 32, 16), Color.White);
                double percentage = (double)dragons[i].m_health / (double)150;
                if (percentage >= .5)
                {
                    m_spriteBatch.Draw(greenBar, new Rectangle((int)dragons[i].Center.X - 70, (int)dragons[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(redBar, new Rectangle((int)dragons[i].Center.X - 70, (int)dragons[i].Center.Y - 96, (int)(32 * percentage), 16), Color.White);
                }
                dFRenderer.draw(m_spriteBatch, dragons[i]);
            }

            // if our clicked object is not null, highlight it's square
            if (clicked != null)
            {
                m_spriteBatch.Draw(selected, new Rectangle((int)clicked.Center.X - 64, (int)clicked.Center.Y - 96, 64, 64), Color.Red);
                if (clicked is Objects.Fighter)
                {
                    m_spriteBatch.Draw(fighterLevels[clicked.level - 1], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y + 100, 250, 250), Color.White);
                }
                else if (clicked is Objects.Wizard)
                {
                    m_spriteBatch.Draw(wizardLevels[clicked.level - 1], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y + 100, 250, 250), Color.White);
                }
                else if (clicked is Objects.Ranger)
                {
                    m_spriteBatch.Draw(rangerLevels[clicked.level - 1], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y + 100, 250, 250), Color.White);
                }
                else if (clicked is Objects.Warlock)
                {
                    m_spriteBatch.Draw(warlockLevels[clicked.level - 1], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y + 100, 250, 250), Color.White);
                }
            }

            if (!levelStarted)
            {
                if (startingLocations.Count > level)
                {
                    if (startingLocations[level].X == 0)
                    {
                        Objects.AnimatedSprite pointer = new Objects.AnimatedSprite(new Vector2(32, 32), new Vector2((startingLocations[level].X + 1) * 64, (startingLocations[level].Y * 64) + 48), 0, 0);
                        pointer.Rotation = (float)Math.PI;
                        pointerRenderer.draw(m_spriteBatch, pointer);
                    }
                    else if (startingLocations[level].Y == 0)
                    {
                        Objects.AnimatedSprite pointer = new Objects.AnimatedSprite(new Vector2(32, 32), new Vector2((startingLocations[level].X * 64) + 48, (startingLocations[level].Y + 1) * 64), 0, 0);
                        pointer.Rotation = (float)(Math.PI / 2);
                        pointerRenderer.draw(m_spriteBatch, pointer);
                    }
                }
                else
                {
                    startingLocations.Add(new Vector2(startingLocations[level - 2].X, startingLocations[level - 2].Y));
                    endingLocations.Add(new Vector2(endingLocations[level - 2].X, endingLocations[level - 2].Y));
                }
            }

            for (int i = 0; i < scores.Count; i++)
            {
                if (((Objects.Score)scores[i]).m_value == 100)
                {
                    m_spriteBatch.Draw(goblinScore, new Rectangle((int)scores[i].Center.X - 96, (int)scores[i].Center.Y - 96, 64, 64), Color.White);
                }
                if (((Objects.Score)scores[i]).m_value == 200)
                {
                    m_spriteBatch.Draw(hobgoblinScore, new Rectangle((int)scores[i].Center.X - 96, (int)scores[i].Center.Y - 96, 64, 64), Color.White);
                }
                if (((Objects.Score)scores[i]).m_value == 300)
                {
                    m_spriteBatch.Draw(dragonScore, new Rectangle((int)scores[i].Center.X - 96, (int)scores[i].Center.Y - 96, 64, 64), Color.White);
                }
                if (!((Objects.Score)scores[i]).move())
                {
                    scores.RemoveAt(i);
                }
            }

            Vector2 stringSize = m_font.MeasureString("SCORE: " + player.getScore().ToString());
            m_spriteBatch.DrawString(m_font, "SCORE: " + player.getScore().ToString(), new Vector2(m_graphics.PreferredBackBufferWidth - 384 - stringSize.X-100, 10), Color.Black);
            Vector2 stringSize2 = m_font.MeasureString("MONEY: " + player.getMoney().ToString());
            m_spriteBatch.DrawString(m_font, "MONEY: " + player.getMoney().ToString(), new Vector2(m_graphics.PreferredBackBufferWidth - 384 - stringSize.X - 200 - stringSize2.X, 10), Color.Black);
            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            // update all our sprite renderers
            fIRenderer.update(gameTime);
            wIRenderer.update(gameTime);
            rIRenderer.update(gameTime);
            waIRenderer.update(gameTime);
            gWRenderer.update(gameTime);
            pointerRenderer.update(gameTime);
            hWRenderer.update(gameTime);
            dFRenderer.update(gameTime);

            // update our mouse input
            m_inputMouse.Update(gameTime);

            // if our time since an error is greater than or equal to 100 ms, we can place a tower again
            if ((DateTime.Now - errorTime).TotalMilliseconds >= 100)
            {
                canPlace = true;
            }

            // if we have placed a tower, recompute shortest path for each creep
            if (towerPlaced)
            {
                for (int i = 0; i < goblins.Count; i++)
                {
                    findPath2(goblins[i]);
                }

                towerPlaced = false;

            }

            // if our level has started, move creeps
            if (levelStarted)
            {
                // if our last creep came in over a second ago, add a new creep
                if ((DateTime.Now - lastCreep).TotalSeconds >= 1)
                {
                    //if(waveEnded)
                    //{
                        //if((DateTime.Now-waveTime).TotalSeconds >= 10)
                        //{
                            //waveEnded = false;
                        //}
                    //}
                    if(wave == 0)
                    {
                        if (totalGoblins < creepsPerWave[level][wave])
                        {
                            totalGoblins++;
                            goblins.Add(new Objects.Goblin(new Vector2(64, 64), new Vector2(board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centerx, board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centery + 64), 1, 50));
                            findPath2(goblins.Last());
                        }
                        else
                        {
                            waveTime = DateTime.Now;
                            waveEnded = true;
                            wave++;
                        }
                    }
                    else if(wave == 1)
                    {
                        if (totalHobGoblins < creepsPerWave[level][wave])
                        {
                            totalHobGoblins++;
                            hobgoblins.Add(new Objects.Hobgoblin(new Vector2(64, 64), new Vector2(board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centerx, board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centery + 64), 1, 100));
                            findPath2(hobgoblins.Last());
                        }
                        else
                        {
                            waveTime = DateTime.Now;
                            waveEnded = true;
                            wave++;
                        }
                    }
                    else if(wave == 2)
                    {
                        if (totalDragons < creepsPerWave[level][wave])
                        {
                            totalDragons++;
                            dragons.Add(new Objects.Dragon(new Vector2(64, 64), new Vector2(board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centerx, board[(int)startingLocations[level].Y, (int)startingLocations[level].X].m_centery + 64), 1, 150));
                            findPath2(dragons.Last());
                        }
                        else
                        {
                            if (dragons.Count == 0)
                            {
                                waveTime = DateTime.Now;
                                waveEnded = true;
                                wave = 0;
                                levelStarted = false;
                                level++;
                            }
                        }
                    }
                    
                    lastCreep = DateTime.Now;
                }
                // for each of our creeps, move
                for (int i = 0; i < goblins.Count; i++)
                {
                    // if our creep has not reached the last element of the path
                    if (goblins[i].location != goblins[i].shortestPath.Count - 1)
                    {
                        // if the creep's location is greater than our shortest path, we have recomputed the path. break and set location to 0
                        if (goblins[i].location + 1 > goblins[i].shortestPath.Count)
                        {
                            goblins[i].location = 0;
                            break;
                        }
                        else
                        {
                            // get the creep's next cell to go to
                            Objects.GameCell next = goblins[i].shortestPath[goblins[i].location + 1];
                            // if our next cell has a greater x, increase our creep's x
                            if (next.m_centerx > goblins[i].Center.X)
                            {
                                double left = (2 * Math.PI) - goblins[i].Rotation;
                                if (goblins[i].Rotation >= 0 + .5 || goblins[i].Rotation <= 0 - .5)
                                {
                                    if (left < goblins[i].Rotation)
                                    {
                                        goblins[i].Rotation += .1f;
                                        if (goblins[i].Rotation >= (2 * Math.PI) + .5 || goblins[i].Rotation >= (2 * Math.PI) - .5)
                                        {
                                            goblins[i].Rotation = 0;
                                        }
                                    }
                                    else
                                    {
                                        goblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    goblins[i].increaseX(1);
                                }
                                //creeps[i].Rotation = 0;
                            }
                            // if our next cell has a smaller x, decrease our creeps x
                            if (next.m_centerx < goblins[i].Center.X)
                            {
                                double left = (Math.PI) - goblins[i].Rotation;
                                double right = goblins[i].Rotation - Math.PI;
                                if (goblins[i].Rotation >= Math.PI + .5 || goblins[i].Rotation <= Math.PI - .5)
                                {
                                    if (left < right)
                                    {
                                        goblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        goblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    goblins[i].increaseX(-1);
                                }
                            }
                            // if our next cell has a greater y, increase our creeps y
                            if (next.m_centery > goblins[i].Center.Y - 64)
                            {
                                double left = ((Math.PI) / 2) - goblins[i].Rotation;
                                double right = goblins[i].Rotation - ((Math.PI) / 2);
                                if (goblins[i].Rotation >= ((Math.PI) / 2) + .5 || goblins[i].Rotation <= ((Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        goblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        goblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    goblins[i].increaseY(1);
                                }
                            }
                            // if our next cell has a smaller y, decrease our creeps y
                            if (next.m_centery < goblins[i].Center.Y - 64)
                            {
                                if (goblins[i].Rotation == 0)
                                {
                                    goblins[i].Rotation = (float)(2 * Math.PI);
                                }
                                double left = ((3 * Math.PI) / 2) - goblins[i].Rotation;
                                double right = goblins[i].Rotation - ((3 * Math.PI) / 2);
                                if (goblins[i].Rotation >= ((3 * Math.PI) / 2) + .5 || goblins[i].Rotation <= ((3 * Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        goblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        goblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    goblins[i].increaseY(-1);
                                }
                            }
                            // otherwise, our creep has reached the next cell, update it's location in our path
                            if (next.m_centerx == goblins[i].Center.X && next.m_centery == goblins[i].Center.Y - 64)
                            {
                                goblins[i].location++;
                            }
                        }
                    }
                    else
                    {
                        goblins.RemoveAt(i);
                    }
                }
                for (int i = 0; i < hobgoblins.Count; i++)
                {
                    // if our creep has not reached the last element of the path
                    if (hobgoblins[i].location != hobgoblins[i].shortestPath.Count - 1)
                    {
                        // if the creep's location is greater than our shortest path, we have recomputed the path. break and set location to 0
                        if (hobgoblins[i].location + 1 > hobgoblins[i].shortestPath.Count)
                        {
                            hobgoblins[i].location = 0;
                            break;
                        }
                        else
                        {
                            // get the creep's next cell to go to
                            Objects.GameCell next = hobgoblins[i].shortestPath[hobgoblins[i].location + 1];
                            // if our next cell has a greater x, increase our creep's x
                            if (next.m_centerx > hobgoblins[i].Center.X)
                            {
                                double left = (2 * Math.PI) - hobgoblins[i].Rotation;
                                if (hobgoblins[i].Rotation >= 0 + .5 || hobgoblins[i].Rotation <= 0 - .5)
                                {
                                    if (left < hobgoblins[i].Rotation)
                                    {
                                        hobgoblins[i].Rotation += .1f;
                                        if (hobgoblins[i].Rotation >= (2 * Math.PI) + .5 || hobgoblins[i].Rotation >= (2 * Math.PI) - .5)
                                        {
                                            hobgoblins[i].Rotation = 0;
                                        }
                                    }
                                    else
                                    {
                                        hobgoblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    hobgoblins[i].increaseX(1);
                                }
                                //creeps[i].Rotation = 0;
                            }
                            // if our next cell has a smaller x, decrease our creeps x
                            if (next.m_centerx < hobgoblins[i].Center.X)
                            {
                                double left = (Math.PI) - hobgoblins[i].Rotation;
                                double right = hobgoblins[i].Rotation - Math.PI;
                                if (hobgoblins[i].Rotation >= Math.PI + .5 || hobgoblins[i].Rotation <= Math.PI - .5)
                                {
                                    if (left < right)
                                    {
                                        hobgoblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        hobgoblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    hobgoblins[i].increaseX(-1);
                                }
                            }
                            // if our next cell has a greater y, increase our creeps y
                            if (next.m_centery > hobgoblins[i].Center.Y - 64)
                            {
                                double left = ((Math.PI) / 2) - hobgoblins[i].Rotation;
                                double right = hobgoblins[i].Rotation - ((Math.PI) / 2);
                                if (hobgoblins[i].Rotation >= ((Math.PI) / 2) + .5 || hobgoblins[i].Rotation <= ((Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        hobgoblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        hobgoblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    hobgoblins[i].increaseY(1);
                                }
                            }
                            // if our next cell has a smaller y, decrease our creeps y
                            if (next.m_centery < hobgoblins[i].Center.Y - 64)
                            {
                                if (hobgoblins[i].Rotation == 0)
                                {
                                    hobgoblins[i].Rotation = (float)(2 * Math.PI);
                                }
                                double left = ((3 * Math.PI) / 2) - hobgoblins[i].Rotation;
                                double right = hobgoblins[i].Rotation - ((3 * Math.PI) / 2);
                                if (hobgoblins[i].Rotation >= ((3 * Math.PI) / 2) + .5 || hobgoblins[i].Rotation <= ((3 * Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        hobgoblins[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        hobgoblins[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    hobgoblins[i].increaseY(-1);
                                }
                            }
                            // otherwise, our creep has reached the next cell, update it's location in our path
                            if (next.m_centerx == hobgoblins[i].Center.X && next.m_centery == hobgoblins[i].Center.Y - 64)
                            {
                                hobgoblins[i].location++;
                            }
                        }
                    }
                    else
                    {
                        hobgoblins.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dragons.Count; i++)
                {
                    // if our creep has not reached the last element of the path
                    if (dragons[i].location != dragons[i].shortestPath.Count - 1)
                    {
                        // if the creep's location is greater than our shortest path, we have recomputed the path. break and set location to 0
                        if (dragons[i].location + 1 > dragons[i].shortestPath.Count)
                        {
                            dragons[i].location = 0;
                            break;
                        }
                        else
                        {
                            // get the creep's next cell to go to
                            Objects.GameCell next = dragons[i].shortestPath[dragons[i].location + 1];
                            // if our next cell has a greater x, increase our creep's x
                            if (next.m_centerx > dragons[i].Center.X)
                            {
                                double left = (2 * Math.PI) - dragons[i].Rotation;
                                if (dragons[i].Rotation >= 0 + .5 || dragons[i].Rotation <= 0 - .5)
                                {
                                    if (left < dragons[i].Rotation)
                                    {
                                        dragons[i].Rotation += .1f;
                                        if (dragons[i].Rotation >= (2 * Math.PI) + .5 || dragons[i].Rotation >= (2 * Math.PI) - .5)
                                        {
                                            dragons[i].Rotation = 0;
                                        }
                                    }
                                    else
                                    {
                                        dragons[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    dragons[i].increaseX(1);
                                }
                                //creeps[i].Rotation = 0;
                            }
                            // if our next cell has a smaller x, decrease our creeps x
                            if (next.m_centerx < dragons[i].Center.X)
                            {
                                double left = (Math.PI) - dragons[i].Rotation;
                                double right = dragons[i].Rotation - Math.PI;
                                if (dragons[i].Rotation >= Math.PI + .5 || dragons[i].Rotation <= Math.PI - .5)
                                {
                                    if (left < right)
                                    {
                                        dragons[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        dragons[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    dragons[i].increaseX(-1);
                                }
                            }
                            // if our next cell has a greater y, increase our creeps y
                            if (next.m_centery > dragons[i].Center.Y - 64)
                            {
                                double left = ((Math.PI) / 2) - dragons[i].Rotation;
                                double right = dragons[i].Rotation - ((Math.PI) / 2);
                                if (dragons[i].Rotation >= ((Math.PI) / 2) + .5 || dragons[i].Rotation <= ((Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        dragons[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        dragons[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    dragons[i].increaseY(1);
                                }
                            }
                            // if our next cell has a smaller y, decrease our creeps y
                            if (next.m_centery < dragons[i].Center.Y - 64)
                            {
                                if (dragons[i].Rotation == 0)
                                {
                                    dragons[i].Rotation = (float)(2 * Math.PI);
                                }
                                double left = ((3 * Math.PI) / 2) - dragons[i].Rotation;
                                double right = dragons[i].Rotation - ((3 * Math.PI) / 2);
                                if (dragons[i].Rotation >= ((3 * Math.PI) / 2) + .5 || dragons[i].Rotation <= ((3 * Math.PI) / 2) - .5)
                                {
                                    if (right < left)
                                    {
                                        dragons[i].Rotation += .1f;
                                    }
                                    else
                                    {
                                        dragons[i].Rotation -= .1f;
                                    }
                                }
                                else
                                {
                                    dragons[i].increaseY(-1);
                                }
                            }
                            // otherwise, our creep has reached the next cell, update it's location in our path
                            if (next.m_centerx == dragons[i].Center.X && next.m_centery == dragons[i].Center.Y - 64)
                            {
                                dragons[i].location++;
                            }
                        }
                    }
                    else
                    {
                        dragons.RemoveAt(i);
                    }
                }
                for (int i = 0; i < fighterIdle.Count; i++)
                {
                    if (findTarget(fighterIdle[i]))
                    {
                        double angle;
                        if (fighterIdle[i].target.Center.X < fighterIdle[i].Center.X)
                        {
                            angle = (float)(Math.PI);
                        }
                        else
                        {
                            angle = AngleBetween(fighterIdle[i].Center, fighterIdle[i].target.Center);
                        }
                        if (fighterIdle[i].Rotation >= angle + .5 || fighterIdle[i].Rotation <= angle - .5)
                        {
                            fighterIdle[i].Rotation += .1f;
                            if (fighterIdle[i].Rotation <= (2 * Math.PI) + .5 && fighterIdle[i].Rotation >= (2 * Math.PI) - .5)
                            {
                                fighterIdle[i].Rotation = 0;
                            }
                        }
                        if (((Objects.Fighter)fighterIdle[i]).canSwipe())
                        {
                            if (fighterIdle[i].Rotation <= angle + .5 && fighterIdle[i].Rotation >= angle - .5)
                            {
                                fighterIdle[i].target.m_health -= ((Objects.Fighter)fighterIdle[i]).damage;
                            }
                            if (fighterIdle[i].target.m_health <= 0)
                            {
                                
                                if (fighterIdle[i].target is Objects.Goblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), fighterIdle[i].target.Center, 100));
                                    player.addToScore(105);
                                    player.addMoney(5);
                                    goblins.Remove(fighterIdle[i].target);
                                }
                                else if(fighterIdle[i].target is Objects.Hobgoblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), fighterIdle[i].target.Center, 200));
                                    player.addToScore(210);
                                    player.addMoney(10);
                                    hobgoblins.Remove(fighterIdle[i].target);
                                }
                                fighterIdle[i].target = null;

                            }
                        }
                    }
                }
                for (int i = 0; i < wizardIdle.Count; i++)
                {
                    if (findTarget(wizardIdle[i]))
                    {
                        double angle;
                        if (wizardIdle[i].target.Center.X < wizardIdle[i].Center.X)
                        {
                            angle = (float)(Math.PI);
                        }
                        else
                        {
                            angle = AngleBetween(wizardIdle[i].Center, wizardIdle[i].target.Center);
                        }
                        if (wizardIdle[i].Rotation >= angle + .5 || wizardIdle[i].Rotation <= angle - .5)
                        {
                            wizardIdle[i].Rotation += .1f;
                            if (wizardIdle[i].Rotation <= (2 * Math.PI) + .5 && wizardIdle[i].Rotation >= (2 * Math.PI) - .5)
                            {
                                wizardIdle[i].Rotation = 0;
                            }
                        }
                        if (((Objects.Wizard)wizardIdle[i]).canFire())
                        {
                            if (wizardIdle[i].Rotation <= angle + .5 && wizardIdle[i].Rotation >= angle - .5)
                            {
                                wizardIdle[i].target.m_health -= ((Objects.Wizard)wizardIdle[i]).damage;
                            }
                            if (wizardIdle[i].target.m_health <= 0)
                            {
                                if (wizardIdle[i].target is Objects.Goblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), wizardIdle[i].target.Center, 100));
                                    player.addToScore(105);
                                    player.addMoney(5);
                                    goblins.Remove(wizardIdle[i].target);
                                }
                                else if (wizardIdle[i].target is Objects.Hobgoblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), wizardIdle[i].target.Center, 200));
                                    player.addToScore(210);
                                    player.addMoney(10);
                                    hobgoblins.Remove(wizardIdle[i].target);
                                }
                                wizardIdle[i].target = null;
                            }
                        }
                    }
                }
                for (int i = 0; i < rangerIdle.Count; i++)
                {
                    if (findTarget(rangerIdle[i]))
                    {
                        double angle;
                        if (rangerIdle[i].target.Center.X < rangerIdle[i].Center.X)
                        {
                            angle = (float)(Math.PI);
                        }
                        else
                        {
                            angle = AngleBetween(rangerIdle[i].Center, rangerIdle[i].target.Center);
                        }
                        if (rangerIdle[i].Rotation >= angle + .5 || rangerIdle[i].Rotation <= angle - .5)
                        {
                            rangerIdle[i].Rotation += .1f;
                            if (rangerIdle[i].Rotation <= (2 * Math.PI) + .5 && rangerIdle[i].Rotation >= (2 * Math.PI) - .5)
                            {
                                rangerIdle[i].Rotation = 0;
                            }
                        }
                        if (((Objects.Ranger)rangerIdle[i]).canFire())
                        {
                            if (rangerIdle[i].Rotation <= angle + .5 && rangerIdle[i].Rotation >= angle - .5)
                            {
                                rangerIdle[i].target.m_health -= ((Objects.Ranger)rangerIdle[i]).damage;
                            }
                            if (rangerIdle[i].target.m_health <= 0)
                            {
                                if (rangerIdle[i].target is Objects.Goblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), rangerIdle[i].target.Center, 100));
                                    player.addToScore(105);
                                    player.addMoney(5);
                                    goblins.Remove(rangerIdle[i].target);
                                }
                                else if (rangerIdle[i].target is Objects.Hobgoblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), rangerIdle[i].target.Center, 200));
                                    player.addToScore(210);
                                    player.addMoney(10);
                                    hobgoblins.Remove(rangerIdle[i].target);
                                }
                                else
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), rangerIdle[i].target.Center, 300));
                                    player.addToScore(315);
                                    player.addMoney(15);
                                    dragons.Remove(rangerIdle[i].target);
                                }
                                rangerIdle[i].target = null;
                            }
                        }
                    }
                }
                for (int i = 0; i < warlockIdle.Count; i++)
                {
                    if (findTarget(warlockIdle[i]))
                    {
                        double angle;
                        if (warlockIdle[i].target.Center.X < warlockIdle[i].Center.X)
                        {
                            angle = (float)(Math.PI);
                        }
                        else
                        {
                            angle = AngleBetween(warlockIdle[i].Center, warlockIdle[i].target.Center);
                        }
                        if (warlockIdle[i].Rotation >= angle + .5 || warlockIdle[i].Rotation <= angle - .5)
                        {
                            warlockIdle[i].Rotation += .1f;
                            if (warlockIdle[i].Rotation <= (2 * Math.PI) + .5 && warlockIdle[i].Rotation >= (2 * Math.PI) - .5)
                            {
                                warlockIdle[i].Rotation = 0;
                            }
                        }
                        if (((Objects.Warlock)warlockIdle[i]).canFire())
                        {

                            if (warlockIdle[i].Rotation <= angle + .5 && warlockIdle[i].Rotation >= angle - .5)
                            {
                                warlockIdle[i].target.m_health -= ((Objects.Warlock)warlockIdle[i]).damage;
                            }
                            if (warlockIdle[i].target.m_health <= 0)
                            {
                                if (warlockIdle[i].target is Objects.Goblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), warlockIdle[i].target.Center, 100));
                                    player.addToScore(105);
                                    player.addMoney(5);
                                    goblins.Remove(warlockIdle[i].target);
                                }
                                else if (warlockIdle[i].target is Objects.Hobgoblin)
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), warlockIdle[i].target.Center, 200));
                                    player.addToScore(210);
                                    player.addMoney(10);
                                    hobgoblins.Remove(warlockIdle[i].target);
                                }
                                else
                                {
                                    scores.Add(new Objects.Score(new Vector2(16, 16), warlockIdle[i].target.Center, 300));
                                    player.addToScore(315);
                                    player.addMoney(15);
                                    dragons.Remove(warlockIdle[i].target);
                                }
                                warlockIdle[i].target = null;
                            }
                        }
                    }
                    else if (findTarget(warlockIdle[i]))
                    {
                        double angle;
                        if (warlockIdle[i].target.Center.X < warlockIdle[i].Center.X)
                        {
                            angle = (float)(Math.PI);
                        }
                        else
                        {
                            angle = AngleBetween(warlockIdle[i].Center, warlockIdle[i].target.Center);
                        }
                        if (warlockIdle[i].Rotation >= angle + .5 || warlockIdle[i].Rotation <= angle - .5)
                        {
                            warlockIdle[i].Rotation += .1f;
                            if (warlockIdle[i].Rotation <= (2 * Math.PI) + .5 && warlockIdle[i].Rotation >= (2 * Math.PI) - .5)
                            {
                                warlockIdle[i].Rotation = 0;
                            }
                        }
                        if (((Objects.Warlock)warlockIdle[i]).canSwipe())
                        {
                            if (warlockIdle[i].Rotation <= angle + .5 && warlockIdle[i].Rotation >= angle - .5)
                            {
                                warlockIdle[i].target.m_health -= ((Objects.Warlock)warlockIdle[i]).damage;
                            }
                            if (warlockIdle[i].target.m_health <= 0)
                            {
                                scores.Add(new Objects.Score(new Vector2(16, 16), warlockIdle[i].target.Center, 100));
                                player.addToScore(105);
                                player.addMoney(5);
                                goblins.Remove(warlockIdle[i].target);
                                warlockIdle[i].target = null;
                            }
                        }
                    }
                }
            }

            // if we are trying to upgrade a tower
            if (upgrading)
            {
                // check that we have clicked an object
                if (clicked != null)
                {
                    // if our object is a fighter and we have enough money, update depending on level
                    if (clicked is Objects.Fighter && player.getMoney() - 5 >= 0)
                    {
                        if (clicked.level == 1)
                        {
                            ((Objects.Fighter)clicked).damage = 20;
                            ((Objects.Fighter)clicked).m_swipeRate = 12;
                            player.addMoney(-5);
                            player.addToScore(5);
                            clicked.level++;
                        }
                        else if (clicked.level == 2)
                        {
                            ((Objects.Fighter)clicked).damage = 25;
                            ((Objects.Fighter)clicked).m_swipeRate = 15;
                            ((Objects.Fighter)clicked).m_radius = 4;
                            player.addMoney(-5);
                            player.addToScore(5);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                    // if our object is a wizard and we have enough money, update depending on level
                    if (clicked is Objects.Wizard)
                    {
                        if (clicked.level == 1 && player.getMoney() - 2 >= 0)
                        {
                            ((Objects.Wizard)clicked).damage = 15;
                            ((Objects.Wizard)clicked).m_fireRate = 4;
                            player.addMoney(-2);
                            player.addToScore(2);
                            clicked.level++;
                        }
                        else if (clicked.level == 2 && player.getMoney() - 3 >= 0)
                        {
                            ((Objects.Wizard)clicked).damage = 20;
                            ((Objects.Wizard)clicked).m_fireRate = 10;
                            ((Objects.Wizard)clicked).m_radius = 6;
                            player.addMoney(-3);
                            player.addToScore(3);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                    // if our object is a ranger and we have enough money, update depending on level
                    if (clicked is Objects.Ranger)
                    {
                        if (clicked.level == 1 && player.getMoney() - 2 >= 0)
                        {
                            ((Objects.Ranger)clicked).damage = 12;
                            ((Objects.Ranger)clicked).m_fireRate = 7;
                            player.addMoney(-2);
                            player.addToScore(2);
                            clicked.level++;
                        }
                        else if (clicked.level == 2 && player.getMoney() - 3 >= 0)
                        {
                            ((Objects.Ranger)clicked).damage = 15;
                            ((Objects.Ranger)clicked).m_fireRate = 10;
                            ((Objects.Ranger)clicked).m_radius = 7;
                            player.addMoney(-3);
                            player.addToScore(3);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                    // if our object is a warlock and we have enough money, update depending on level
                    if (clicked is Objects.Warlock)
                    {
                        if (clicked.level == 1 && player.getMoney() - 2 >= 0)
                        {
                            ((Objects.Warlock)clicked).damage = 12;
                            ((Objects.Warlock)clicked).m_fireRate = 5;
                            player.addMoney(-2);
                            player.addToScore(2);
                            clicked.level++;
                        }
                        else if (clicked.level == 2 && player.getMoney() - 3 >= 0)
                        {
                            ((Objects.Warlock)clicked).damage = 15;
                            ((Objects.Warlock)clicked).m_fireRate = 10;
                            ((Objects.Warlock)clicked).m_mradius = 7;
                            ((Objects.Warlock)clicked).m_bradius = 4;
                            player.addMoney(-3);
                            player.addToScore(3);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                }
                upgrading = false;
            }

            if (selling)
            {
                if (clicked != null)
                {
                    if (clicked is Objects.Fighter)
                    {
                        fighterIdle.Remove(clicked);
                        int x = (int)(clicked.Center.X - 32) / 64;
                        int y = (int)(clicked.Center.Y - 32) / 64;
                        if(clicked.level == 1)
                        {
                            player.addMoney(5);
                            player.addToScore(-5);
                        }
                        else if(clicked.level == 2)
                        {
                            player.addMoney(10);
                            player.addToScore(-10);
                        }
                        else
                        {
                            player.addMoney(15);
                            player.addToScore(-15);
                        }
                        board[y, x].setObject(null);
                    }
                    else if (clicked is Objects.Wizard)
                    {
                        wizardIdle.Remove(clicked);
                        int x = (int)(clicked.Center.X - 32) / 64;
                        int y = (int)(clicked.Center.Y - 32) / 64;
                        if (clicked.level == 1)
                        {
                            player.addMoney(15);
                            player.addToScore(-15);
                        }
                        else if (clicked.level == 2)
                        {
                            player.addMoney(17);
                            player.addToScore(-17);
                        }
                        else
                        {
                            player.addMoney(20);
                            player.addToScore(-20);
                        }
                        board[y, x].setObject(null);
                    }
                    else if (clicked is Objects.Ranger)
                    {
                        rangerIdle.Remove(clicked);
                        int x = (int)(clicked.Center.X - 32) / 64;
                        int y = (int)(clicked.Center.Y - 32) / 64;
                        if (clicked.level == 1)
                        {
                            player.addMoney(15);
                            player.addToScore(-15);
                        }
                        else if (clicked.level == 2)
                        {
                            player.addMoney(17);
                            player.addToScore(-17);
                        }
                        else
                        {
                            player.addMoney(20);
                            player.addToScore(-20);
                        }
                        board[y, x].setObject(null);
                    }
                    else if (clicked is Objects.Warlock)
                    {
                        warlockIdle.Remove(clicked);
                        int x = (int)(clicked.Center.X - 32) / 64;
                        int y = (int)(clicked.Center.Y - 32) / 64;
                        if (clicked.level == 1)
                        {
                            player.addMoney(20);
                            player.addToScore(-20);
                        }
                        else if (clicked.level == 2)
                        {
                            player.addMoney(22);
                            player.addToScore(-22);
                        }
                        else
                        {
                            player.addMoney(25);
                            player.addToScore(-25);
                        }
                        board[y, x].setObject(null);
                    }
                    clicked = null;
                    for (int i = 0; i < goblins.Count; i++)
                    {
                        findPath2(goblins[i]);
                    }
                }
                selling = false;
            }

        }

        // on mouse down
        private void onMouseDown(GameTime gameTime, int x, int y)
        {
            // if we have clicked one of our menu buttons, check that we have enough money and indicate we are trying to place a tower
            if (x < fighterButton.Center.X + 64 && x > fighterButton.Center.X - 64)
            {
                if (y < fighterButton.Center.Y + 64 && y > fighterButton.Center.Y - 64)
                {
                    if (player.getMoney() - 5 >= 0)
                    {
                        placingFighter = true;
                        player.addMoney(-5);
                        player.addToScore(5);
                    }
                }

            }
            if (x < wizardButton.Center.X + 64 && x > wizardButton.Center.X - 64)
            {
                if (y < wizardButton.Center.Y + 64 && y > wizardButton.Center.Y - 64)
                {
                    if (player.getMoney() - 15 >= 0)
                    {
                        placingWizard = true;
                        player.addMoney(-15);
                        player.addToScore(15);
                    }
                }
            }
            if (x < rangerButton.Center.X + 64 && x > rangerButton.Center.X - 64)
            {
                if (y < rangerButton.Center.Y + 64 && y > rangerButton.Center.Y - 64)
                {
                    if (player.getMoney() - 15 > 0)
                    {
                        placingRanger = true;
                        player.addMoney(-15);
                        player.addToScore(15);
                    }
                }
            }
            if (x < warlockButton.Center.X + 64 && x > warlockButton.Center.X - 64)
            {
                if (y < warlockButton.Center.Y + 64 && y > warlockButton.Center.Y - 64)
                {
                    if (player.getMoney() - 20 >= 0)
                    {
                        placingWarlock = true;
                        player.addMoney(-20);
                        player.addToScore(20);
                    }
                }
            }

            // if we are trying to place a tower, check that we are in a valid area, and that the grid we are placing in is not occupied
            if (x < m_graphics.PreferredBackBufferWidth - 384 && placingFighter)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() == null)
                {
                    bool valid = true;
                    Objects.Fighter toPlace = new Objects.Fighter(new Vector2(64, 64), new Vector2(board[row, column].m_centerx + 32, board[row, column].m_centery + 64), 5, 1, 3);
                    board[row, column].setObject(toPlace);
                    for (int i = 0; i < goblins.Count; i++)
                    {
                        if (!isPath(goblins[i]))
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        fighterIdle.Add(toPlace);
                        m_mouseCapture = true;

                        board[row, column].setObject(fighterIdle[fighterIdle.Count - 1]);

                        placingFighter = false;
                        towerPlaced = true;
                    }
                    else
                    {
                        board[row, column].setObject(null);
                        canPlace = false;
                        errorTime = DateTime.Now;
                    }
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
                    bool valid = true;
                    Objects.Wizard toPlace = new Objects.Wizard(new Vector2(64, 64), new Vector2(board[row, column].m_centerx + 32, board[row, column].m_centery + 64), 2, 1, 5);
                    board[row, column].setObject(toPlace);
                    for (int i = 0; i < goblins.Count; i++)
                    {
                        if (!isPath(goblins[i]))
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        wizardIdle.Add(toPlace);
                        m_mouseCapture = true;

                        board[row, column].setObject(wizardIdle[wizardIdle.Count - 1]);

                        placingWizard = false;
                        towerPlaced = true;
                    }
                    else
                    {
                        board[row, column].setObject(null);
                        canPlace = false;
                        errorTime = DateTime.Now;
                    }
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
                    bool valid = true;
                    Objects.Ranger toPlace = new Objects.Ranger(new Vector2(64, 64), new Vector2(board[row, column].m_centerx + 32, board[row, column].m_centery + 64), 5, 1, 6);
                    board[row, column].setObject(toPlace);
                    for (int i = 0; i < goblins.Count; i++)
                    {
                        if (!isPath(goblins[i]))
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        rangerIdle.Add(toPlace);
                        m_mouseCapture = true;

                        board[row, column].setObject(rangerIdle[rangerIdle.Count - 1]);

                        placingRanger = false;
                        towerPlaced = true;
                    }
                    else
                    {
                        canPlace = false;
                        errorTime = DateTime.Now;
                    }
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
                    bool valid = true;
                    Objects.Warlock toPlace = new Objects.Warlock(new Vector2(64, 64), new Vector2(board[row, column].m_centerx + 32, board[row, column].m_centery + 64), 2, 2, 1, 6, 3);
                    board[row, column].setObject(toPlace);
                    for (int i = 0; i < goblins.Count; i++)
                    {
                        if (!isPath(goblins[i]))
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        warlockIdle.Add(toPlace);
                        m_mouseCapture = true;

                        board[row, column].setObject(warlockIdle[warlockIdle.Count - 1]);

                        placingWarlock = false;
                        towerPlaced = true;
                    }
                    else
                    {
                        canPlace = false;
                        errorTime = DateTime.Now;
                    }
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }
            // if we have clicked a valid square in the grid, if there is an object, set it as our clicked object
            else if (x < m_graphics.PreferredBackBufferWidth - 384)
            {
                int row = y / 64;
                int column = x / 64;
                if (board[row, column].getObject() != null)
                {
                    clicked = board[row, column].getObject();
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
            //if (m_mouseCapture && placingFighter)
            //{
            //fighterIdle.setX(x);
            //fighterIdle.setY(y);
            //}
            // get position mouse has moved to to render radius circle
            if (placingFighter || placingWizard || placingRanger || placingWarlock)
            {
                placingPos = new Vector2(x, y);
            }

        }

        private bool isPath(Objects.AnimatedSprite creep)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    board[y, x].visited = false;
                }
            }
            // get our destination and our creeps current location
            Vector2 dest = endingLocations[level];
            Vector2 start = new Vector2((int)(creep.Center.X - 32) / 64, (((int)creep.Center.Y - 64) / 64));

            // clear current creeps path
            creep.shortestPath.Clear();

            // get distance from current cell to destination
            int dist = (int)(dest.X - start.X) + (int)(dest.Y - start.Y);

            // set the distance of current cell to destination
            board[(int)start.Y, (int)start.X].m_distance = dist;

            // initialize a gamecell queue and add our current cell
            List<Objects.GameCell> q = new List<Objects.GameCell>();
            q.Add(board[(int)start.Y, (int)start.X]);

            // while our queue isn't empty
            while (q.Count != 0)
            {
                // get the first object of queue and remove it
                Objects.GameCell curr = q[0];
                q.RemoveAt(0);

                // if our distance from the current cell is greater than 1, we have not reached our destination
                if (curr != board[(int)endingLocations[level].Y, (int)endingLocations[level].X])
                {
                    // if our current cell has a right neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_originx + 64 < 10 * 64)
                    {
                        int x = (curr.m_originx + 64) / 64;
                        int y = curr.m_originy / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]) && !board[y, x].visited)
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a left neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_originx - 64 >= 0)
                    {
                        int x = (curr.m_originx - 64) / 64;
                        int y = curr.m_originy / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]) && !board[y, x].visited)
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a upper neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_originy - 64 >= 0)
                    {
                        int x = (curr.m_originx) / 64;
                        int y = (curr.m_originy - 64) / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]) && !board[y, x].visited)
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a lower neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_originy + 64 < 10 * 64)
                    {
                        int x = (curr.m_originx) / 64;
                        int y = (curr.m_originy + 64) / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]) && !board[y, x].visited)
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // add our current cell to the shortest path
                    curr.visited = true;

                    // reorder queue by distance
                    q = q.OrderBy(g => g.m_distance).ToList();
                }
                // if we have found our destination, add the destination to the path and break from the queue loop
                else
                {
                    return true;
                }

            }
            return false;
        }

        private void findPath2(Objects.AnimatedSprite creep)
        {
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    board[row, column].visited = false;
                    board[row, column].m_distance = int.MaxValue;
                }
            }

            // get our destination and our creeps current location
            Vector2 dest = endingLocations[level];
            Vector2 start = new Vector2((int)(creep.Center.X - 32) / 64, (((int)creep.Center.Y - 64) / 64));

            // clear current creeps path
            creep.shortestPath.Clear();

            // get distance from current cell to destination
            int dist = Math.Abs((int)(dest.X - start.X)) + Math.Abs((int)(dest.Y - start.Y));
            board[(int)start.Y, (int)start.X].m_distance = dist;

            List<Objects.GameCell> q = new List<Objects.GameCell>();
            q.Add(board[(int)start.Y, (int)start.X]);

            while (q.Count > 0)
            {
                Objects.GameCell current = q[0];
                q.RemoveAt(0);
                current.visited = true;

                int currx = current.m_originx / 64;
                int curry = current.m_originy / 64;

                if (current == board[(int)dest.Y, (int)dest.X])
                {
                    creep.shortestPath.Add(current);
                    break;
                }
                else
                {
                    creep.shortestPath.Add(current);
                    if (current.m_originx + 64 < 10 * 64)
                    {
                        int x = (current.m_originx + 64) / 64;
                        int y = current.m_originy / 64;
                        if (!board[y, x].visited && board[y, x].getObject() == null)
                        {
                            int tempDist = Math.Abs((int)(dest.X - x)) + Math.Abs((int)(dest.Y - y));
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    if (current.m_originx - 64 >= 0)
                    {
                        int x = (current.m_originx - 64) / 64;
                        int y = current.m_originy / 64;
                        if (!board[y, x].visited && board[y, x].getObject() == null)
                        {
                            int tempDist = Math.Abs((int)(dest.X - x)) + Math.Abs((int)(dest.Y - y));
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    if (current.m_originy - 64 >= 0)
                    {
                        int x = (current.m_originx) / 64;
                        int y = (current.m_originy - 64) / 64;
                        if (!board[y, x].visited && board[y, x].getObject() == null)
                        {
                            int tempDist = Math.Abs((int)(dest.X - x)) + Math.Abs((int)(dest.Y - y));
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    if (current.m_originy + 64 < 10 * 64)
                    {
                        int x = (current.m_originx) / 64;
                        int y = (current.m_originy + 64) / 64;
                        if (!board[y, x].visited && board[y, x].getObject() == null)
                        {
                            int tempDist = Math.Abs((int)(dest.X - x)) + Math.Abs((int)(dest.Y - y));
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    q = q.OrderBy(g => g.m_distance).ToList();
                }
            }
        }

        bool findTarget(Objects.AnimatedSprite tower)
        {
            for (int i = 0; i < goblins.Count; i++)
            {
                if (!(tower is Objects.Warlock))
                {
                    if (tower.doIntersect(new Vector2(goblins[i].Center.X - 32, goblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = goblins[i];
                        return true;
                    }
                }
                else
                {
                    Objects.Warlock warlockTower = (Objects.Warlock)tower;
                    if (warlockTower.doIntersectMissiles(new Vector2(goblins[i].Center.X - 32, goblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = goblins[i];
                        return true;
                    }
                    else if (warlockTower.doIntersectBombs(new Vector2(goblins[i].Center.X - 32, goblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = goblins[i];
                        return true;
                    }
                }
            }
            for (int i = 0; i < hobgoblins.Count; i++)
            {
                if (!(tower is Objects.Warlock))
                {
                    if (tower.doIntersect(new Vector2(hobgoblins[i].Center.X - 32, hobgoblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = hobgoblins[i];
                        return true;
                    }
                }
                else
                {
                    Objects.Warlock warlockTower = (Objects.Warlock)tower;
                    if (warlockTower.doIntersectMissiles(new Vector2(hobgoblins[i].Center.X - 32, hobgoblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = hobgoblins[i];
                        return true;
                    }
                    else if (warlockTower.doIntersectBombs(new Vector2(hobgoblins[i].Center.X - 32, hobgoblins[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = hobgoblins[i];
                        return true;
                    }
                }
            }
            for (int i = 0; i < dragons.Count; i++)
            {
                if (tower is Objects.Ranger)
                {
                    if (tower.doIntersect(new Vector2(dragons[i].Center.X - 32, dragons[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = dragons[i];
                        return true;
                    }
                }
                else if (tower is Objects.Warlock)
                {
                    Objects.Warlock warlockTower = (Objects.Warlock)tower;
                    if (warlockTower.doIntersectMissiles(new Vector2(dragons[i].Center.X - 32, dragons[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = dragons[i];
                        return true;
                    }
                    else if (warlockTower.doIntersectBombs(new Vector2(dragons[i].Center.X - 32, dragons[i].Center.Y - 32), new Vector2(64, 64)))
                    {
                        tower.target = dragons[i];
                        return true;
                    }
                }
            }
            return false;
        }

        public static double AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos); //* (180 / Math.PI);
        }
    }
}
