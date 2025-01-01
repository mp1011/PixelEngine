var specs = Specs.GensLike;

var layers = new LayerGroup(
    new ScrollingLayer(specs, 64, 32),
    new ScrollingLayer(specs, 64, 32),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var coreRenderService = new RenderService(specs, layers);

Workbench.LoadKcScene(layers, coreRenderService, specs);
var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var windowManager = new SfmlWindowManager(inputManager);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);

var frameAnimator = new TileAnimator(coreRenderService);
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("gem",0x6ea, 0x6fd, [6, 6, 6]));
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("ankh", 0x231, 0x233, [120, 6, 6, 6]));

var font = Workbench.CreateHudFont();
var clockString = Workbench.SetupClockSprites(font, coreRenderService);

var clock = new Clock(2, 55, clockString);
ulong frameNumber = 0;

var waterWaver = new WaterWaver(layers.Background, 128, 200);

//var debugAnimation = Workbench.CreateDebugAnimation("kid\\walk", 0x625);
//frameAnimator.AddAnimation(debugAnimation);
var playerAnimations = Workbench.LoadPlayerAnimations();
frameAnimator.AddAnimation(playerAnimations);

clockString.Text = "1:23";
coreRenderService.Sprites[11].HorizontalPos += 32;

var camera = new Camera();

var coordinateTranslator = new CoordinateTranslator(layers.Foreground, camera);
var collisionMap = Workbench.CreateCollisionMap();
var collisionManager = new CollisionManager(collisionMap, coordinateTranslator, specs);


var player = new MovingSprite(coreRenderService.Sprites[11], coordinateTranslator, specs);
var playerController = new PlayerController(
    inputManager,
    playerAnimations,
    player,
    collisionManager);

var scroller = new StreamScroller(layers.Background, layers.Foreground, player, coordinateTranslator, camera, specs);
while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    frameAnimator.Update(frameNumber);
    clock.Update(frameNumber);
    waterWaver.Update();

    windowManager.DispatchEvents();

    //if (inputManager.Player1.KeyPressed(GenesisPadButtons.Right))
    //{
    //    debugAnimation.GameFramesRemaining = 0;
    //}
    //else if (inputManager.Player1.KeyPressed(GenesisPadButtons.Left))
    //{
    //    debugAnimation.CurrentFrameNumber-=2;
    //    debugAnimation.GameFramesRemaining = 0;
    //}

    renderService.DebugString = Debug.Text1;

    playerController.Update();
    scroller.Update();

    renderService.DisplayFrame();
    frameNumber++;
}
