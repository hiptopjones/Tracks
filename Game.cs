using static SFML.Window.Keyboard;

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
            WindowManager.WindowLostFocus += InputManager.OnWindowLostFocus;
            WindowManager.KeyPressed += InputManager.OnKeyPressed;
            WindowManager.KeyReleased += InputManager.OnKeyReleased;
            WindowManager.MouseMoved += InputManager.OnMouseMoved;
            WindowManager.MouseButtonPressed += InputManager.OnMouseButtonPressed;
            WindowManager.MouseButtonReleased += InputManager.OnMouseButtonReleased;
            ServiceLocator.Instance.ProvideService(WindowManager);

            CoroutineManager = new CoroutineManager();
            ServiceLocator.Instance.ProvideService(CoroutineManager);

            InitializeScenes();
        }

        private void InitializeScenes()
        {
            GameScene gameScene = new GameScene();
            int gameSceneId = SceneManager.AddScene(gameScene);

            SplashScreenScene splashScene = new SplashScreenScene();
            splashScene.TransitionSceneId = gameSceneId;
            int splashSceneId = SceneManager.AddScene(splashScene);

            SceneManager.SwitchTo(splashSceneId);
        }

        public void StartFrame()
        {
            TimeManager.OnFrameStarted();
            InputManager.OnFrameStarted();
        }

        public void ProcessEvents()
        {
            WindowManager.ProcessEvents();

            if (InputManager.IsKeyPressed(Key.Escape))
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

        public void Render()
        {
            WindowManager.BeginDraw();

            SceneManager.Render();

            WindowManager.EndDraw();
        }
    }
}
