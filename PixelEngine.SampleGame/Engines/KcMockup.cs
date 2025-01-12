public class KcMockup : Engine
{
    private TileAnimator? _frameAnimator;
    private TileFont? _hudFont;
    private SpriteString? _clockString;
    private Clock? _clock;
    private WaterWaver? _waterWaver;
    private Camera? _camera;
    private CoordinateTranslator? _coordinateTranslator;
    private StreamScroller? _streamScroller;
    private PlayerController _playerController;

    public KcMockup(RenderService renderService, InputManager inputManager, Specs specs) : base(renderService, inputManager, specs)
    {
    }

    public override void Load()
    {
        Workbench.LoadKcScene(_renderService.Layers, _renderService, _specs);

        _frameAnimator = new TileAnimator(_renderService);
        _frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("gem", 0x6ea, 0x6fd, [6, 6, 6]));
        _frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("ankh", 0x231, 0x233, [120, 6, 6, 6]));

        _hudFont = Workbench.CreateHudFont();
        _clockString = Workbench.SetupClockSprites(_hudFont, _renderService);
        _clock = new Clock(2, 55, _clockString);

        _waterWaver = new WaterWaver(_renderService.Layers.Background, 128, 200);

        var playerAnimations = Workbench.LoadPlayerAnimations();
        _frameAnimator.AddAnimation(playerAnimations);

        _clockString.Text = "1:23";

        _camera = new Camera();
        //todo - create map
        _coordinateTranslator = new CoordinateTranslator(_renderService.Layers.Foreground, null, _camera, _specs);
        var collisionMap = Workbench.CreateCollisionMap();
        var collisionManager = new CollisionManager(collisionMap, _coordinateTranslator, _specs);

        var player = new MovingSprite(_renderService.Sprites[11], _coordinateTranslator, _specs);
        _playerController = new PlayerController(
            _inputManager,
            playerAnimations,
            player,
            collisionManager);

        var bgHScrollLayers = new BackgroundScrollLayer[]
        {
            new BackgroundScrollLayer(56, 0.2),
            new BackgroundScrollLayer(148, 0.4),
        };

        var bgVScrollLayers = new BackgroundScrollLayer[]
        {
            new BackgroundScrollLayer(1, 0.5),
        };

        _streamScroller = new StreamScroller(_renderService.Layers.Background, _renderService.Layers.Foreground, null, player,
            _coordinateTranslator, _camera, bgHScrollLayers, bgVScrollLayers, _specs);
    }

    public override void Update(ulong frameNumber)
    {
        _frameAnimator!.Update(frameNumber);
        _clock!.Update(frameNumber);
        _streamScroller!.Update();
        _playerController!.Update();
        _waterWaver.Update();
    }
}