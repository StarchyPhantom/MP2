//Author: Benjamin Huynh
//File Name: Game1.cs
//Project Name: MP2
//Creation Date: Apr. 06, 2022
//Modified Date: Apr. 17, 2022
//Description: Sticky bomb diffuser game with ammo pickups and pause
//TODO:ADD A TRANSITION EFFECT, PREFERABLY A SINGLE TIMER AND BLACK/BLUE IMAGE THAT FADES

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Helper;

namespace MP2
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		static Random rng = new Random();

		const byte MENU = 0;
		const byte INSTRUCTIONS = 1;
		const byte PREGAME = 2;
		const byte GAMEPLAY = 3;
		const byte PAUSE = 4;
		const byte ENDGAME = 5;

		const int MAX_AMMO = 10;
		const int MAX_HEALTH = 10;
		const int SETUP_TIME = 5000;
		const int GAME_TIME = 30000;
		const int STICKY_TIME = 3000;
		const int AMMO_TIME = 1500;

		byte gameState = MENU;

		int currentScore;
		int currentAmmo;
		int currentHealth;
		int highScore = 2500;

		float buttonFade = 0;

		string gameTimerDisplay;
		string highScoreDisplay;

		bool ammoExists;
		bool sticky2FirstTime;
		bool sticky3FirstTime;
		bool gameStarting;
		bool usingAltBack = true;
		bool fadeReverse = false;
		bool songIsPlaying = false;

		Texture2D startImg;
		Texture2D instructionsImg;
		Texture2D exitImg;
		Texture2D continueImg;
		Texture2D fiveImg;
		Texture2D fourImg;
		Texture2D threeImg;
		Texture2D twoImg;
		Texture2D oneImg;
		Texture2D menuBackImg;
		Texture2D demoBackImg;
		Texture2D transitionBackImg;
		Texture2D gameBackImg;
		Texture2D pauseBackImg;
		Texture2D endBackImg;
		Texture2D stickyImg;
		Texture2D ammoImg;
		Texture2D titleImg;
		Texture2D logoImg;

		//For animations
		Texture2D explosionImg;
		Texture2D bigExplosionImg;
		Texture2D altMenuBackImg;

		Vector2 startLoc;
		Vector2 instructionsLoc;
		Vector2 exitLoc;
		Vector2 continueLoc;
		Vector2 preCountLoc;
		Vector2 backgroundLoc;
		Vector2 gameTimerLoc;
		Vector2 gameHealthLoc;
		Vector2 gameAmmoCountLoc;
		Vector2 gameScoreLoc;
		Vector2 stickyHideLoc;
		Vector2 highScoreLoc;
		Vector2 logoLoc;
		Vector2 menuTitleLoc;
		Vector2 generalTitleLoc;
		Vector2 demoStickyLoc;
		Vector2 demoAmmoLoc;
		Vector2 demoTextLLoc;
		Vector2 demoTextRLoc;
		Vector2 altBackgroundLoc;

		Animation explosion1Anim;
		Animation explosion2Anim;
		Animation explosion3Anim;
		Animation bigExplosion1Anim;
		Animation bigExplosion2Anim;
		Animation bigExplosion3Anim;
		Animation altMenuAnim;

		Rectangle startRec;
		Rectangle instructionsRec;
		Rectangle exitRec;
		Rectangle continueRec;
		Rectangle preCountRec;
		Rectangle backgroundRec;
		Rectangle sticky1Rec;
		Rectangle sticky2Rec;
		Rectangle sticky3Rec;
		Rectangle ammoRec;
		Rectangle titleRec;
		Rectangle logoRec;
		Rectangle demoStickyRec;
		Rectangle demoAmmoRec;

		Timer setupTimer;
		Timer gameTimer;
		Timer sticky1Timer;
		Timer sticky2Timer;
		Timer sticky3Timer;
		Timer ammoTimer;

		SpriteFont gameFont;
		SpriteFont titleFont;

		Song menuSong;
		Song gameSong;
		Song endSong;

		SoundEffect buttonSnd;
		SoundEffect countdownSnd;
		SoundEffect hitSnd;
		SoundEffect boomSnd;
		SoundEffect newAmmoSnd;
		SoundEffect pickUpAmmoSnd;
		SoundEffect missedAmmoSnd;
		SoundEffect noAmmoSnd;
		SoundEffect missSnd;

		MouseState mouse;
		MouseState prevMouse;

		KeyboardState kb;
		KeyboardState prevKb;


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
			IsMouseVisible = true;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			int screenWidth = this.graphics.GraphicsDevice.Viewport.Width;
			int screenHeight = this.graphics.GraphicsDevice.Viewport.Height;

			startImg = Content.Load<Texture2D>("Images/Sprites/MP2Start");
			instructionsImg = Content.Load<Texture2D>("Images/Sprites/MP2Instructions");
			exitImg = Content.Load<Texture2D>("Images/Sprites/MP2Exit");
			continueImg = Content.Load<Texture2D>("Images/Sprites/MP2Continue");
			oneImg = Content.Load<Texture2D>("Images/Sprites/MP2One");
			twoImg = Content.Load<Texture2D>("Images/Sprites/MP2Two");
			threeImg = Content.Load<Texture2D>("Images/Sprites/MP2Three");
			fourImg = Content.Load<Texture2D>("Images/Sprites/MP2Four");
			fiveImg = Content.Load<Texture2D>("Images/Sprites/MP2Five");
			menuBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2MenuBack");
			demoBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2DemoBack");
			transitionBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2TransitionBack");
			gameBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2GameBack");
			pauseBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2PauseBack");
			endBackImg = Content.Load<Texture2D>("Images/Backgrounds/MP2EndBack");
			stickyImg = Content.Load<Texture2D>("Images/Sprites/MP2Sticky");
			titleImg = Content.Load<Texture2D>("Images/Sprites/MP2Title");
			logoImg = Content.Load<Texture2D>("Images/Sprites/MP2Logo");
			ammoImg = Content.Load<Texture2D>("Images/Sprites/MP2Ammo");

			explosionImg = Content.Load<Texture2D>("Animations/MP2Explode");
			bigExplosionImg = Content.Load<Texture2D>("Animations/MP2BigExplode");
			altMenuBackImg = Content.Load<Texture2D>("Animations/MP2AltMenuBack");

			startLoc = new Vector2(0, screenHeight / 2 - 80);
			instructionsLoc = new Vector2(0, screenHeight / 2 - 20);
			exitLoc = new Vector2(0, screenHeight / 2 + 80);
			continueLoc = new Vector2((screenWidth / 5) * 2, screenHeight / 1.1f);
			preCountLoc = new Vector2(screenWidth / 2 - (fiveImg.Width / 2), screenHeight / 2 - (fiveImg.Height / 2));
			backgroundLoc = new Vector2(0, 0);
			gameTimerLoc = new Vector2((screenWidth / 3) * 1.2f, 0);
			gameHealthLoc = new Vector2(0, 0);
			gameAmmoCountLoc = new Vector2(screenWidth / 6, 0);
			gameScoreLoc = new Vector2((screenWidth / 4) * 3, 0);
			stickyHideLoc = new Vector2(-400, -400);
			highScoreLoc = new Vector2(screenWidth / 5, screenHeight / 2);
			logoLoc = new Vector2(screenWidth / 2, screenHeight / 4);
			menuTitleLoc = new Vector2(screenWidth / 2, screenHeight / 8);
			generalTitleLoc = new Vector2(screenWidth / 5, screenHeight / 16);
			demoAmmoLoc = new Vector2(screenWidth / 1.2f, screenHeight / 5);
			demoStickyLoc = new Vector2(screenWidth / 20, (screenHeight / 5) * 3);
			demoTextLLoc = new Vector2(screenWidth / 16, screenHeight / 4);
			demoTextRLoc = new Vector2(screenWidth / 5, screenHeight / 1.6f);
			altBackgroundLoc = new Vector2(0, (screenHeight / 3) * -1);

			startRec = new Rectangle((int)startLoc.X, (int)startLoc.Y, startImg.Width, startImg.Height);
			instructionsRec = new Rectangle((int)instructionsLoc.X, (int)instructionsLoc.Y, (int)(instructionsImg.Width*0.5), (int)(instructionsImg.Height*0.5));
			exitRec = new Rectangle((int)exitLoc.X, (int)exitLoc.Y, (int)(exitImg.Width*1.5), (int)(exitImg.Height*1.5));
			continueRec = new Rectangle((int)continueLoc.X, (int)continueLoc.Y, (int)(continueImg.Width * 0.5), (int)(continueImg.Height * 0.5));
			preCountRec = new Rectangle((int)preCountLoc.X, (int)preCountLoc.Y, (int)(fiveImg.Width * 1.5), (int)(fiveImg.Height * 1.5));
			backgroundRec = new Rectangle((int)backgroundLoc.X, (int)backgroundLoc.Y, screenWidth, screenHeight);
			sticky1Rec = new Rectangle((int)stickyHideLoc.X, (int)stickyHideLoc.Y, (int)(stickyImg.Width * 0.2), (int)(stickyImg.Height * 0.2));
			sticky2Rec = new Rectangle((int)stickyHideLoc.X, (int)stickyHideLoc.Y, (int)(stickyImg.Width * 0.2), (int)(stickyImg.Height * 0.2));
			sticky3Rec = new Rectangle((int)stickyHideLoc.X, (int)stickyHideLoc.Y, (int)(stickyImg.Width * 0.2), (int)(stickyImg.Height * 0.2));
			ammoRec = new Rectangle((int)stickyHideLoc.X, (int)stickyHideLoc.Y, (int)(ammoImg.Width * 0.1), (int)(ammoImg.Height * 0.1));
			logoRec = new Rectangle((int)logoLoc.X, (int)logoLoc.Y, (int)(logoImg.Width * 0.65), (int)(logoImg.Height * 0.65));
			titleRec = new Rectangle((int)menuTitleLoc.X, (int)menuTitleLoc.Y, (int)(titleImg.Width * 0.75), (int)(titleImg.Height * 0.75));
			demoAmmoRec = new Rectangle((int)demoAmmoLoc.X, (int)demoAmmoLoc.Y, (int)(ammoImg.Width* 0.15), (int)(ammoImg.Height * 0.15));
			demoStickyRec = new Rectangle((int)demoStickyLoc.X, (int)demoStickyLoc.Y, (int)(stickyImg.Width * 0.2), (int)(stickyImg.Height * 0.2));

			setupTimer = new Timer(SETUP_TIME, false);
			gameTimer = new Timer(GAME_TIME, false);
			sticky1Timer = new Timer(STICKY_TIME, false);
			sticky2Timer = new Timer(STICKY_TIME, false);
			sticky3Timer = new Timer(STICKY_TIME, false);
			ammoTimer = new Timer(AMMO_TIME, false);

			gameFont = Content.Load<SpriteFont>("Fonts/GameFont");
			titleFont = Content.Load<SpriteFont>("Fonts/SubTitleFont");

			explosion1Anim = new Animation(explosionImg, 4, 4, 16, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1, false);
			explosion2Anim = new Animation(explosionImg, 4, 4, 16, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1, false);
			explosion3Anim = new Animation(explosionImg, 4, 4, 16, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1, false);
			bigExplosion1Anim = new Animation(bigExplosionImg, 5, 5, 23, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1.5f, false);
			bigExplosion2Anim = new Animation(bigExplosionImg, 5, 5, 23, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1.5f, false);
			bigExplosion3Anim = new Animation(bigExplosionImg, 5, 5, 23, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 3, stickyHideLoc, 1.5f, false);
			altMenuAnim = new Animation(altMenuBackImg, 5, 4, 17, 0, 0, Animation.ANIMATE_FOREVER, 6, altBackgroundLoc, 1.6f, true);

			menuSong = Content.Load<Song>("Audio/Music/MP2SongWaiting");
			gameSong = Content.Load<Song>("Audio/Music/MP2SongRudeBuster");
			endSong = Content.Load<Song>("Audio/Music/MP2SongGameOver");

			buttonSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioButton");
			countdownSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioCountdown");
			hitSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioCrit");
			boomSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioKaboom");
			newAmmoSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioNewAmmo");
			pickUpAmmoSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioPickUpAmmo");
			missedAmmoSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioNo");
			noAmmoSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioNoShots");
			missSnd = Content.Load<SoundEffect>("Audio/Sounds/MP2AudioShoot");

			MediaPlayer.Volume = 0.5f;
			MediaPlayer.IsRepeating = true;
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
			// TODO: Add your update logic here
			int screenWidth = this.graphics.GraphicsDevice.Viewport.Width;
			int screenHeight = this.graphics.GraphicsDevice.Viewport.Height;

			switch (gameState)
			{
				case MENU:
					altMenuAnim.Update(gameTime);

					buttonFade = fadeMaker(fadeReverse, buttonFade);
					fadeReverse = fadeReverser(fadeReverse, buttonFade);

					prevKb = kb;
					kb = Keyboard.GetState();

					mouse = Mouse.GetState();

					if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
					{
						if (usingAltBack == false)
						{
							usingAltBack = true;
						}
						else
						{
							usingAltBack = false;
						}
					}

					if (MediaPlayer.State != MediaState.Playing && songIsPlaying==false)
					{
						MediaPlayer.Play(menuSong);
						songIsPlaying = true;
					}

					if (mouse.LeftButton == ButtonState.Pressed)
					{
						if (mouse.X <= (startRec.X+ startRec.Width) && mouse.Y <= (startRec.Y+ startRec.Height) && mouse.X >= startRec.X && mouse.Y >= startRec.Y)
						{
							buttonSnd.CreateInstance().Play();
							gameStarting = false;
							gameState = PREGAME;
						}
						else if (mouse.X <= (instructionsRec.X + instructionsRec.Width) && mouse.Y <= (instructionsRec.Y + instructionsRec.Height) && mouse.X >= instructionsRec.X && mouse.Y >= instructionsRec.Y)
						{
							buttonSnd.CreateInstance().Play();
							gameState =INSTRUCTIONS;
						}
						else if (mouse.X <= (exitRec.X + exitRec.Width) && mouse.Y <= (exitRec.Y + exitRec.Height) && mouse.X >= exitRec.X && mouse.Y >= exitRec.Y)
						{
							buttonSnd.CreateInstance().Play();
							Exit();
						}
					}
					break;
				case INSTRUCTIONS:
					buttonFade = fadeMaker(fadeReverse, buttonFade);
					fadeReverse = fadeReverser(fadeReverse, buttonFade);

					mouse = Mouse.GetState();

					if (mouse.LeftButton == ButtonState.Pressed)
					{
						if (mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y)
						{
							buttonSnd.CreateInstance().Play();
							gameState = MENU;
						}
					}
					break;
				case PREGAME:
					buttonFade = fadeMaker(fadeReverse, buttonFade);
					fadeReverse = fadeReverser(fadeReverse, buttonFade);

					mouse = Mouse.GetState();

					setupTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

					if (mouse.LeftButton == ButtonState.Pressed && mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y && gameStarting == false)
					{
						buttonSnd.CreateInstance().Play();
						countdownSnd.CreateInstance().Play();
						setupTimer.Activate();
						gameStarting = true;
					}

					gameTimer.ResetTimer(false);
					sticky1Timer.ResetTimer(false);
					sticky2Timer.ResetTimer(false);
					sticky3Timer.ResetTimer(false);
					ammoTimer.ResetTimer(false);

					bigExplosion1Anim.isAnimating = false;
					bigExplosion2Anim.isAnimating = false;
					bigExplosion3Anim.isAnimating = false;
					explosion1Anim.isAnimating = false;
					explosion2Anim.isAnimating = false;
					explosion3Anim.isAnimating = false;

					currentAmmo = MAX_AMMO;
					currentHealth = MAX_HEALTH;
					currentScore = 0;
					ammoExists = false;
					sticky2FirstTime = true;
					sticky3FirstTime = true;

					sticky1Rec.X = rng.Next(51, screenWidth - 50);
					sticky1Rec.Y = rng.Next(51, screenHeight - 50);
					sticky2Rec.X = (int)stickyHideLoc.X;
					sticky2Rec.Y = (int)stickyHideLoc.Y;
					sticky3Rec.X = (int)stickyHideLoc.X;
					sticky3Rec.Y = (int)stickyHideLoc.Y;
					ammoRec.X = (int)stickyHideLoc.X;
					ammoRec.Y = (int)stickyHideLoc.Y;

					if (setupTimer.GetTimeRemaining() < 0)
					{
						MediaPlayer.Pause();
						gameTimer.Activate();
						sticky1Timer.Activate();
						songIsPlaying = false;
						gameState = GAMEPLAY;
					}
					break;
				case GAMEPLAY:
					if (MediaPlayer.State != MediaState.Playing && songIsPlaying == false)
					{
						MediaPlayer.Play(gameSong);
						songIsPlaying = true;
					}

					gameTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
					gameTimerDisplay = gameTimer.GetTimeRemainingAsString(Timer.FORMAT_SEC_MIL);
					sticky1Timer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
					sticky2Timer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
					sticky3Timer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
					ammoTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

					bigExplosion1Anim.Update(gameTime);
					bigExplosion2Anim.Update(gameTime);
					bigExplosion3Anim.Update(gameTime);
					explosion1Anim.Update(gameTime);
					explosion2Anim.Update(gameTime);
					explosion3Anim.Update(gameTime);

					prevKb = kb;
					kb = Keyboard.GetState();

					prevMouse = mouse;
					mouse = Mouse.GetState();

					if (currentScore >= 1000 && sticky2FirstTime == true)
					{
						sticky2Timer.Activate();
						sticky2Rec.X = rng.Next(51, screenWidth - 50);
						sticky2Rec.Y = rng.Next(51, screenHeight - 50);
						sticky2FirstTime = false;
					}

					if (currentScore >= 2000 && sticky3FirstTime == true)
					{
						sticky3Timer.Activate();
						sticky3Rec.X = rng.Next(51, screenWidth - 50);
						sticky3Rec.Y = rng.Next(51, screenHeight - 50);
						sticky3FirstTime = false;
					}

					if (sticky1Timer.GetTimeRemaining() < 0)
					{
						boomSnd.CreateInstance().Play();
						bigExplosion1Anim.isAnimating = true;
						bigExplosion1Anim.destRec.X = sticky1Rec.X;
						bigExplosion1Anim.destRec.Y = sticky1Rec.Y;
						currentScore -= 100;
						currentHealth -= 5;
						sticky1Rec.X = (int)stickyHideLoc.X;
						sticky1Rec.Y = (int)stickyHideLoc.Y;
						sticky1Timer.ResetTimer(false);
					}

					if (sticky2Timer.GetTimeRemaining() < 0)
					{
						boomSnd.CreateInstance().Play();
						bigExplosion2Anim.isAnimating = true;
						bigExplosion2Anim.destRec.X = sticky2Rec.X;
						bigExplosion2Anim.destRec.Y = sticky2Rec.Y;
						currentScore -= 100;
						currentHealth -= 5;
						sticky2Rec.X = (int)stickyHideLoc.X;
						sticky2Rec.Y = (int)stickyHideLoc.Y;
						sticky2Timer.ResetTimer(false);
					}

					if (sticky3Timer.GetTimeRemaining() < 0)
					{
						boomSnd.CreateInstance().Play();
						bigExplosion3Anim.isAnimating = true;
						bigExplosion3Anim.destRec.X = sticky3Rec.X;
						bigExplosion3Anim.destRec.Y = sticky3Rec.Y;
						currentScore -= 100;
						currentHealth -= 5;
						sticky3Rec.X = (int)stickyHideLoc.X;
						sticky3Rec.Y = (int)stickyHideLoc.Y;
						sticky3Timer.ResetTimer(false);
					}

					if (ammoTimer.GetTimeRemaining() < 0)
					{
						missedAmmoSnd.CreateInstance().Play();
						ammoExists = false;
						ammoRec.X = (int)stickyHideLoc.X;
						ammoRec.Y = (int)stickyHideLoc.Y;
						ammoTimer.ResetTimer(false);
					}

					if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed && (currentAmmo > 0 || ammoExists == true))
					{
						if (mouse.X <= (sticky1Rec.X + sticky1Rec.Width) && mouse.Y <= (sticky1Rec.Y + sticky1Rec.Height) && mouse.X >= sticky1Rec.X && mouse.Y >= sticky1Rec.Y && currentAmmo > 0)
						{
							hitSnd.CreateInstance().Play();
							currentAmmo -= 1;
							currentScore += 100;
							explosion1Anim.isAnimating = true;
							explosion1Anim.destRec.X = sticky1Rec.X;
							explosion1Anim.destRec.Y = sticky1Rec.Y;
							sticky1Rec.X = (int)stickyHideLoc.X;
							sticky1Rec.Y = (int)stickyHideLoc.Y;
							sticky1Timer.ResetTimer(false);

							if (rng.Next(1, 101) >= 50 && ammoExists == false)
							{
								newAmmoSnd.CreateInstance().Play();
								ammoRec.X = rng.Next(100, screenWidth - 99);
								ammoRec.Y = rng.Next(100, screenHeight - 99);
								ammoTimer.Activate();
								ammoExists = true;
							}
						}
						else if (mouse.X <= (sticky2Rec.X + sticky2Rec.Width) && mouse.Y <= (sticky2Rec.Y + sticky2Rec.Height) && mouse.X >= sticky2Rec.X && mouse.Y >= sticky2Rec.Y && currentAmmo > 0)
						{
							hitSnd.CreateInstance().Play();
							currentAmmo -= 1;
							currentScore += 100;
							explosion2Anim.isAnimating = true;
							explosion2Anim.destRec.X = sticky2Rec.X;
							explosion2Anim.destRec.Y = sticky2Rec.Y;
							sticky2Rec.X = (int)stickyHideLoc.X;
							sticky2Rec.Y = (int)stickyHideLoc.Y;
							sticky2Timer.ResetTimer(false);

							if (rng.Next(1, 101) >= 50 && ammoExists == false)
							{
								newAmmoSnd.CreateInstance().Play();
								ammoRec.X = rng.Next(100, screenWidth - 99);
								ammoRec.Y = rng.Next(100, screenHeight - 99);
								ammoTimer.Activate();
								ammoExists = true;
							}
						}
						else if (mouse.X <= (sticky3Rec.X + sticky3Rec.Width) && mouse.Y <= (sticky3Rec.Y + sticky3Rec.Height) && mouse.X >= sticky3Rec.X && mouse.Y >= sticky3Rec.Y && currentAmmo > 0)
						{
							hitSnd.CreateInstance().Play();
							currentAmmo -= 1;
							currentScore += 100;
							explosion3Anim.isAnimating = true;
							explosion3Anim.destRec.X = sticky3Rec.X;
							explosion3Anim.destRec.Y = sticky3Rec.Y;
							sticky3Rec.X = (int)stickyHideLoc.X;
							sticky3Rec.Y = (int)stickyHideLoc.Y;
							sticky3Timer.ResetTimer(false);

							if (rng.Next(1, 101) >= 50 && ammoExists == false)
							{
								newAmmoSnd.CreateInstance().Play();
								ammoRec.X = rng.Next(100, screenWidth - 99);
								ammoRec.Y = rng.Next(100, screenHeight - 99);
								ammoTimer.Activate();
								ammoExists = true;
							}
						}
						else if (mouse.X <= (ammoRec.X + ammoRec.Width) && mouse.Y <= (ammoRec.Y + ammoRec.Height) && mouse.X >= ammoRec.X && mouse.Y >= ammoRec.Y)
						{
							pickUpAmmoSnd.CreateInstance().Play();
							currentAmmo += 2;

							if (currentAmmo>MAX_AMMO)
							{
								currentAmmo = MAX_AMMO;
							}

							ammoExists = false;
							ammoRec.X = (int)stickyHideLoc.X;
							ammoRec.Y = (int)stickyHideLoc.Y;
							ammoTimer.ResetTimer(false);
						}
						else if (currentAmmo > 0)
						{
							missSnd.CreateInstance().Play();
							currentAmmo -=1;
							currentScore -= 50;
						}
						else
						{
							noAmmoSnd.CreateInstance().Play();
						}
					}

					if (bigExplosion1Anim.isAnimating == false && explosion1Anim.isAnimating == false && sticky1Rec.X == stickyHideLoc.X)
					{
						sticky1Rec.X = rng.Next(100, screenWidth - 99);
						sticky1Rec.Y = rng.Next(100, screenHeight - 99);
						sticky1Timer.Activate();
					}

					if (bigExplosion2Anim.isAnimating == false && explosion2Anim.isAnimating == false && sticky2FirstTime==false && sticky2Rec.X == stickyHideLoc.X)
					{
						sticky2Rec.X = rng.Next(100, screenWidth - 99);
						sticky2Rec.Y = rng.Next(100, screenHeight - 99);
						sticky2Timer.Activate();
					}

					if (bigExplosion3Anim.isAnimating == false && explosion3Anim.isAnimating == false && sticky3FirstTime == false && sticky3Rec.X == stickyHideLoc.X)
					{
						sticky3Rec.X = rng.Next(100, screenWidth - 99);
						sticky3Rec.Y = rng.Next(100, screenHeight - 99);
						sticky3Timer.Activate();
					}

					if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
					{
						MediaPlayer.Volume = 0.1f;
						gameState = PAUSE;
					}

					if (gameTimer.GetTimeRemaining() < 0 || currentHealth <= 0 || (currentAmmo <= 0 && ammoExists == false))
					{
						MediaPlayer.Pause();
						songIsPlaying = false;
						gameState = ENDGAME;
					}
					break;
				case PAUSE:
					prevKb = kb;
					kb = Keyboard.GetState();

					if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
					{
						MediaPlayer.Volume = 0.5f;
						gameState = GAMEPLAY;
					}
					break;
				case ENDGAME:
					buttonFade = fadeMaker(fadeReverse, buttonFade);
					fadeReverse = fadeReverser(fadeReverse, buttonFade);

					if (MediaPlayer.State != MediaState.Playing && songIsPlaying == false)
					{
						MediaPlayer.Play(endSong);
						songIsPlaying = true;
					}

					mouse = Mouse.GetState();

					setupTimer.ResetTimer(false);

					if (currentScore > highScore)
					{
						highScore = currentScore;
						highScoreDisplay = "New high score! Congrats! New high score is: ";
					}
					else if (currentScore < highScore)
					{
						highScoreDisplay = "Current high score is: ";
					}

					if (mouse.LeftButton == ButtonState.Pressed && mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y)
					{
						buttonSnd.CreateInstance().Play();
						MediaPlayer.Pause();
						songIsPlaying = false;
						gameState = MENU;
					}
					break;
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

			switch (gameState)
			{
				case MENU:
					if (usingAltBack == true)
					{
						altMenuAnim.Draw(spriteBatch, Color.White * 0.5f, Animation.FLIP_NONE);
					}
					else
					{
						spriteBatch.Draw(menuBackImg, backgroundRec, Color.White * 0.5f);
					}


					if (mouse.X <= (startRec.X + startRec.Width) && mouse.Y <= (startRec.Y + startRec.Height) && mouse.X >= startRec.X && mouse.Y >= startRec.Y)
					{
						spriteBatch.Draw(startImg, startRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(startImg, startRec, Color.White);
					}

					if (mouse.X <= (instructionsRec.X + instructionsRec.Width) && mouse.Y <= (instructionsRec.Y + instructionsRec.Height) && mouse.X >= instructionsRec.X && mouse.Y >= instructionsRec.Y)
					{
						spriteBatch.Draw(instructionsImg, instructionsRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(instructionsImg, instructionsRec, Color.White);
					}

					if (mouse.X <= (exitRec.X + exitRec.Width) && mouse.Y <= (exitRec.Y + exitRec.Height) && mouse.X >= exitRec.X && mouse.Y >= exitRec.Y)
					{
						spriteBatch.Draw(exitImg, exitRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(exitImg, exitRec, Color.White);
					}
					
					spriteBatch.Draw(logoImg, logoRec, Color.White);
					spriteBatch.Draw(titleImg, titleRec, Color.White);
					break;
				case INSTRUCTIONS:
					spriteBatch.Draw(demoBackImg, backgroundRec, Color.White * 0.5f);
					spriteBatch.DrawString(titleFont, "Instructions", generalTitleLoc, Color.Cornsilk);

					if (mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y)
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White);
					}

					spriteBatch.Draw(stickyImg, demoStickyRec, Color.White);
					spriteBatch.Draw(ammoImg, demoAmmoRec, Color.White);
					spriteBatch.DrawString(gameFont, "The game is over if you have no health or ammo left, \nor if the time has run out \nYou start with 10 health & ammo & 30 seconds \nClick ammo packs to get your ammo back, \nthey only last 1.5 seconds", demoTextLLoc, Color.Plum);
					spriteBatch.DrawString(gameFont, "Stickybombs will blow up in 3 seconds after they appear \nClick them to diffuse them \nPress ESC to pause during the game \nPress ESC to change background in the menu", demoTextRLoc, Color.LemonChiffon);
					break;
				case PREGAME:
					spriteBatch.Draw(transitionBackImg, backgroundRec, Color.White * 0.5f);

					if (mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y)
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White);
					}

					if (setupTimer.GetTimeRemaining()>4000)
					{
						spriteBatch.Draw(fiveImg, preCountRec, Color.White);
					}
					else if (setupTimer.GetTimeRemaining() > 3000)
					{
						spriteBatch.Draw(fourImg, preCountRec, Color.White);
					}
					else if (setupTimer.GetTimeRemaining() > 2000)
					{
						spriteBatch.Draw(threeImg, preCountRec, Color.White);
					}
					else if (setupTimer.GetTimeRemaining() > 1000)
					{
						spriteBatch.Draw(twoImg, preCountRec, Color.White);
					}
					else if (setupTimer.GetTimeRemaining() > 0)
					{
						spriteBatch.Draw(oneImg, preCountRec, Color.White);
					}
					break;
				case GAMEPLAY:
					spriteBatch.Draw(gameBackImg, backgroundRec, Color.White * 0.5f);
					spriteBatch.DrawString(gameFont, "Time left: " + gameTimerDisplay, gameTimerLoc, Color.Bisque);
					spriteBatch.DrawString(gameFont, "Health: " + currentHealth, gameHealthLoc, Color.Bisque);
					spriteBatch.DrawString(gameFont, "Ammo: " + currentAmmo, gameAmmoCountLoc, Color.Bisque);
					spriteBatch.DrawString(gameFont, "Score: " + currentScore, gameScoreLoc, Color.Bisque);
					spriteBatch.Draw(stickyImg, sticky1Rec, Color.White);
					spriteBatch.Draw(stickyImg, sticky2Rec, Color.White);
					spriteBatch.Draw(stickyImg, sticky3Rec, Color.White);
					explosion1Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					explosion2Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					explosion3Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					bigExplosion1Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					bigExplosion2Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					bigExplosion3Anim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
					spriteBatch.Draw(ammoImg, ammoRec, Color.White);
					break;
				case PAUSE:
					spriteBatch.Draw(pauseBackImg, backgroundRec, Color.White * 0.5f);
					spriteBatch.DrawString(titleFont, "Paused", generalTitleLoc, Color.Orchid);
					spriteBatch.DrawString(gameFont, "Press ESC to unpause", continueLoc, Color.Orange);
					break;
				case ENDGAME:
					spriteBatch.Draw(endBackImg, backgroundRec, Color.White * 0.5f);
					spriteBatch.DrawString(titleFont, "Game Over!", generalTitleLoc, Color.Wheat);
					spriteBatch.DrawString(gameFont, "Your score is: "+currentScore+"\n"+highScoreDisplay+highScore, highScoreLoc, Color.Fuchsia);

					if (mouse.X <= (continueRec.X + continueRec.Width) && mouse.Y <= (continueRec.Y + continueRec.Height) && mouse.X >= continueRec.X && mouse.Y >= continueRec.Y)
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White * buttonFade);
					}
					else
					{
						spriteBatch.Draw(continueImg, continueRec, Color.White);
					}
					break;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		private static float fadeMaker(bool fadeReverse, float buttonFade)
		{
			if (fadeReverse == false)
			{
				buttonFade += 0.01f;
			}
			else
			{
				buttonFade -= 0.01f;
			}

			return buttonFade;
		}

		private static bool fadeReverser(bool fadeReverse, float buttonFade)
		{
			if (buttonFade > 1)
			{
				fadeReverse = true;
			}
			else if (buttonFade < 0)
			{
				fadeReverse = false;
			}

			return fadeReverse;
		}
	}
}
