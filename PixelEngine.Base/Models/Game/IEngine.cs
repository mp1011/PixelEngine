public abstract class Engine
{
    protected Specs _specs;
    protected RenderService _renderService;   
    protected InputManager _inputManager;

    protected Engine(RenderService renderService, InputManager inputManager, Specs specs)
    {
        _inputManager = inputManager;
        _renderService = renderService;
        _specs = specs;
    }

    public abstract void Load();
    public abstract void Update(ulong frameNumber);
}