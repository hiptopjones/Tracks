using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    internal class Game
    {
        private InputManager InputManager { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private WindowManager WindowManager { get; set; }
        private TimeManager TimeManager { get; set; }
        private SceneManager SceneManager { get; set; }
        private CoroutineManager CoroutineManager { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        private TileRenderer TileRenderer { get; set; }
        private FontAtlas FontAtlas { get; set; }

        public bool IsRunning => WindowManager.IsOpen;

        public Game()
        {
            Initialize();
        }

        public void Initialize()
        {
            InputManager = new InputManager();
            ServiceLocator.Instance.ProvideService(InputManager);

            ResourceManager = new ResourceManager();
            ServiceLocator.Instance.ProvideService(ResourceManager);

            TimeManager = new TimeManager();
            ServiceLocator.Instance.ProvideService(TimeManager);

            SceneManager = new SceneManager();
            ServiceLocator.Instance.ProvideService(SceneManager);

            WindowManager = new WindowManager(GameSettings.GameName, GameSettings.WindowWidth, GameSettings.WindowHeight);
            WindowManager.FocusChanged += InputManager.OnWindowFocusChanged;
            WindowManager.KeyDown += InputManager.OnKeyDown;
            WindowManager.KeyUp += InputManager.OnKeyUp;
            WindowManager.MouseDown += InputManager.OnMouseDown;
            WindowManager.MouseUp += InputManager.OnMouseUp;
            WindowManager.MouseMove += InputManager.OnMouseMove;
            WindowManager.MouseWheel += InputManager.OnMouseWheel;
            ServiceLocator.Instance.ProvideService(WindowManager);

            CoroutineManager = new CoroutineManager();
            ServiceLocator.Instance.ProvideService(CoroutineManager);

            SpriteRenderer = new SpriteRenderer();
            ServiceLocator.Instance.ProvideService(SpriteRenderer);

            TileRenderer = new TileRenderer();
            ServiceLocator.Instance.ProvideService(TileRenderer);

            FontAtlas = new FontAtlas();
            ServiceLocator.Instance.ProvideService(FontAtlas);

            InitializeScenes();
        }

        private void InitializeScenes()
        {
            GameScene gameScene = new GameScene();
            int gameSceneId = SceneManager.AddScene(gameScene);

            SplashScreenScene splashScene = new SplashScreenScene();
            splashScene.TextureId = (int)GameSettings.TextureId.SplashScreen;
            splashScene.TransitionSceneId = gameSceneId;
            int splashSceneId = SceneManager.AddScene(splashScene);

            //SceneManager.SwitchTo(splashSceneId);
            SceneManager.SwitchTo(gameSceneId);
        }

        public void StartFrame()
        {
            TimeManager.OnFrameStarted();
            InputManager.OnFrameStarted();
        }

        public void ProcessEvents()
        {
            WindowManager.ProcessEvents();

            if (InputManager.IsKeyPressed(Keys.Escape))
            {
                WindowManager.Close();
            }
        }

        public void Update()
        {
            float deltaTime = TimeManager.DeltaTime;

            SceneManager.Update(deltaTime);
            CoroutineManager.Update(deltaTime);
        }

        public void LateUpdate()
        {
            float deltaTime = TimeManager.DeltaTime;

            SceneManager.LateUpdate(deltaTime);
        }

        public void Draw()
        {
            WindowManager.BeginDraw();

            SceneManager.Draw();

            WindowManager.EndDraw();
        }
    }
}
