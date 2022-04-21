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

        // declare animated sprite objects
        private List<Objects.AnimatedSprite> fighterIdle;
        private Objects.AnimatedSprite fighterButton;
        private List<Objects.AnimatedSprite> wizardIdle;
        private Objects.AnimatedSprite wizardButton;
        private List<Objects.AnimatedSprite> rangerIdle;
        private Objects.AnimatedSprite rangerButton;
        private List<Objects.AnimatedSprite> warlockIdle;
        private Objects.AnimatedSprite warlockButton;
        private List<Objects.AnimatedSprite> creeps;
        private Objects.AnimatedSprite clicked;

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

            // load animated sprite assets
            fighterIdle = new List<Objects.AnimatedSprite>();
            fighterButton = new Objects.Fighter(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, 64), 1, 1, 5);
            wizardIdle = new List<Objects.AnimatedSprite>();
            wizardButton = new Objects.Wizard(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, (m_graphics.PreferredBackBufferHeight / 6) + 64), 1, 1, 5);
            rangerIdle = new List<Objects.AnimatedSprite>();
            rangerButton = new Objects.Ranger(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6) * 2) + 64), 1, 1, 5);
            warlockIdle = new List<Objects.AnimatedSprite>();
            warlockButton = new Objects.Warlock(new Vector2(64, 64), new Vector2(m_graphics.PreferredBackBufferWidth - 384 + 96, ((m_graphics.PreferredBackBufferHeight / 6) * 3) + 64), 1, 1, 1, 15, 5);
            creeps = new List<Objects.AnimatedSprite>();

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

            // initialize gameplay booleans
            keysLoaded = false;
            placingFighter = false;
            towerPlaced = true;
            wasUDown = false;
            gridOn = false;
            wasSpaceDown = false;
            wasGDown = false;
            canPlace = true;

            // initialize gameboard
            board = new Objects.GameCell[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    board[y, x] = new Objects.GameCell(x * 64, y * 64, 0);
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
            player = new Objects.Player(0, 100, new Vector2(32, 32), new Vector2(64, 64));

            //initialize other objects needed for gameplay
            errorTime = DateTime.Now; // initialize last time we got an error placing tower to now
            currLevel = 0; // initialize the current level of our clicked object to 0
            level = 0; // initialize the current level of our game to zero
            startingLocations = new List<Vector2>(); // initialize our starting locations array
            startingLocations.Add(new Vector2(0, 5)); // add vector for level 1 starting location
            startingLocations.Add(new Vector2(5, 0)); // add vector for level 2 starting location
            endingLocations = new List<Vector2>(); // initialize our ending locations array
            endingLocations.Add(new Vector2(9, 5)); // add vector for level 1 destination
            endingLocations.Add(new Vector2(5, 9)); // add vector for level 2 destination
            lastCreep = DateTime.Now; // initialize the last time a creep entered the stage to now
            clicked = null; // initialize our currently clicked object to nothing

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

            // if space is pressed and space was not previously down, indicate that grid should be on
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && !wasSpaceDown)
            {
                wasSpaceDown = true;
                gridOn = !gridOn;
            }
            else if(wasSpaceDown && !Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                wasSpaceDown = false;
            }

            // if g is pressed and wasn't previously down, indicate that level should start
            if(Keyboard.GetState().IsKeyDown(Keys.G) && !wasGDown)
            {
                wasGDown = true;
                levelStarted = true;
            }
            else if(wasGDown && !Keyboard.GetState().IsKeyDown(Keys.G))
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

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // draw grass background
            for (int i = 0; i < m_graphics.PreferredBackBufferWidth-384; i += 128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(grass, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            // draw wood menu background
            for (int i = m_graphics.PreferredBackBufferWidth - 384; i < m_graphics.PreferredBackBufferWidth; i+=128)
            {
                for (int j = 0; j < m_graphics.PreferredBackBufferHeight; j += 128)
                {
                    m_spriteBatch.Draw(wood, new Rectangle(i, j, 128, 128), Color.White);
                }
            }

            // draw walls
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
            if(gridOn)
            {
                for (int y = 0; y < 10*64; y+=64)
                {
                    for (int x = 0; x < 10*64; x+=64)
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
            m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)fighterButton.Center.Y-100, 250, 250), Color.White);
            m_spriteBatch.Draw(wizardLevels[0], new Rectangle((int)wizardButton.Center.X, (int)wizardButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(rangerLevels[0], new Rectangle((int)rangerButton.Center.X, (int)rangerButton.Center.Y - 100, 250, 250), Color.White);
            m_spriteBatch.Draw(warlockLevels[0], new Rectangle((int)warlockButton.Center.X, (int)warlockButton.Center.Y - 100, 250, 250), Color.White);

            // if we are placing a tower, draw the radius of the tower
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

            // draw the description for the level of our current clicked item
            if(currLevel == 1)
            {
                m_spriteBatch.Draw(fighterLevels[0], new Rectangle((int)fighterButton.Center.X, (int)warlockButton.Center.Y - 450, 250, 250), Color.White);
            }

            // draw all our creeps
            for (int i = 0; i < creeps.Count; i++)
            {
                gWRenderer.draw(m_spriteBatch, creeps[i]);
            }

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

            // update our mouse input
            m_inputMouse.Update(gameTime);

            // if our time since an error is greater than or equal to 100 ms, we can place a tower again
            if((DateTime.Now-errorTime).TotalMilliseconds >= 100)
            {
                canPlace = true;
            }

            // if we have placed a tower, recompute shortest path for each creep
            if(towerPlaced)
            {
                for (int i = 0; i < creeps.Count; i++)
                {
                    findPath(creeps[i]);
                }

                towerPlaced = false;

            }

            // if our level has started, move creeps
            if (levelStarted)
            {
                // if our last creep came in over a second ago, add a new creep
                if ((DateTime.Now - lastCreep).TotalSeconds >= 1)
                {
                    creeps.Add(new Objects.Goblin(new Vector2(64, 64), new Vector2((startingLocations[level].X * 64) + 32, (startingLocations[level].Y * 64) + 32), 1, 1));
                    findPath(creeps.Last());
                    lastCreep = DateTime.Now;
                }
                // for each of our creeps, move
                for (int i = 0; i < creeps.Count; i++)
                {
                    // if our creep has not reached the last element of the path
                    if (creeps[i].location != creeps[i].shortestPath.Count - 1)
                    {
                        // if the creep's location is greater than our shortest path, we have recomputed the path. break and set location to 0
                        if (creeps[i].location + 1 > creeps[i].shortestPath.Count)
                        {
                            creeps[i].location = 0;
                            break;
                        }
                        else
                        {
                            // get the creep's next cell to go to
                            Objects.GameCell next = creeps[i].shortestPath[creeps[i].location + 1];
                            // if our next cell has a greater x, increase our creep's x
                            if (next.m_x > creeps[i].Center.X)
                            {
                                creeps[i].increaseX(1);
                            }
                            // if our next cell has a smaller x, decrease our creeps x
                            else if (next.m_x < creeps[i].Center.X)
                            {
                                creeps[i].increaseX(-1);
                            }
                            // if our next cell has a greater y, increase our creeps y
                            else if (next.m_y > creeps[i].Center.Y)
                            {
                                creeps[i].increaseY(1);
                            }
                            // if our next cell has a smaller y, decrease our creeps y
                            else if (next.m_y < creeps[i].Center.Y)
                            {
                                creeps[i].increaseY(-1);
                            }
                            // otherwise, our creep has reached the next cell, update it's location in our path
                            else
                            {
                                creeps[i].location++;
                            }
                        }
                    }
                }
            }

            // if we are trying to upgrade a tower
            if(upgrading)
            {
                // check that we have clicked an object
                if (clicked != null)
                {
                    // if our object is a fighter and we have enough money, update depending on level
                    if(clicked is Objects.Fighter && player.getMoney() - 5 >= 0)
                    {
                        if (clicked.level == 1)
                        {
                            ((Objects.Fighter)clicked).damage = 20;
                            ((Objects.Fighter)clicked).m_swipeRate = 12;
                            player.addMoney(-5);
                            clicked.level++;
                        }
                        if(clicked.level == 2)
                        {
                            ((Objects.Fighter)clicked).damage = 25;
                            ((Objects.Fighter)clicked).m_swipeRate = 15;
                            ((Objects.Fighter)clicked).m_radius = 10;
                            player.addMoney(-5);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                    // if our object is a wizard and we have enough money, update depending on level
                    if(clicked is Objects.Wizard)
                    {
                        if (clicked.level == 1)
                        {
                            ((Objects.Fighter)clicked).damage = 20;
                            ((Objects.Fighter)clicked).m_swipeRate = 12;
                            player.addMoney(-5);
                            clicked.level++;
                        }
                        clicked = null;
                    }
                }
                upgrading = false;
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
                    fighterIdle.Add(new Objects.Fighter(new Vector2(64, 64), new Vector2(board[row, column].m_x + 64, board[row, column].m_y + 64), 1, 1, 5));
                    m_mouseCapture = true;

                    board[row, column].setObject(fighterIdle[fighterIdle.Count - 1]);

                    placingFighter = false;
                    towerPlaced = true;
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
                    towerPlaced = true;
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
                    towerPlaced = true;
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
                    towerPlaced = true;
                }
                else
                {
                    canPlace = false;
                    errorTime = DateTime.Now;
                }
            }

            // if we have clicked a valid square in the grid, if there is an object, set it as our clicked object
            if(x < m_graphics.PreferredBackBufferWidth-384)
            {
                int row = y / 64;
                int column = x / 64;
                if(board[row, column].getObject() != null)
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
            if(placingFighter || placingWizard || placingRanger || placingWarlock)
            {
                placingPos = new Vector2(x, y);
            }

        }

        // find shortest path for creep
        private void findPath(Objects.AnimatedSprite creep)
        {
            // clear current creeps path
            creep.shortestPath.Clear();

            // get our destination and our creeps current location
            Vector2 dest = endingLocations[level];
            Vector2 start = new Vector2((int)creep.Center.X / 64, (((int)creep.Center.Y+32) / 64));

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
                if (curr.m_distance > 1)
                {
                    // if our current cell has a right neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_x + 64 < 10*64)
                    {
                        int x = (curr.m_x + 64) / 64;
                        int y = curr.m_y / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]))
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a left neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_x - 64 >= 0)
                    {
                        int x = (curr.m_x - 64) / 64;
                        int y = curr.m_y / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]))
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a upper neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_y - 64 >= 0)
                    {
                        int x = (curr.m_x) / 64;
                        int y = (curr.m_y - 64) / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]))
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // if our current cell has a lower neighbor, that neighbor is empty, and not in the queue, calculate distance and add it to queue
                    if (curr.m_y + 64 < 10*64)
                    {
                        int x = (curr.m_x) / 64;
                        int y = (curr.m_y + 64) / 64;
                        if (board[y, x].getObject() == null && !q.Contains(board[y, x]))
                        {
                            int tempDist = (int)(dest.X - x) + (int)(dest.Y - y);
                            board[y, x].m_distance = tempDist;
                            q.Add(board[y, x]);
                        }
                    }
                    // add our current cell to the shortest path
                    creep.shortestPath.Add(curr);

                    // reorder queue by distance
                    q = q.OrderBy(g => g.m_distance).ToList();
                }
                // if we have found our destination, add the destination to the path and break from the queue loop
                else
                {
                    creep.shortestPath.Add(curr);
                    break;
                }

            }
        }
    }
}
