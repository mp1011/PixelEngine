public abstract class Engine
{
    protected RenderService _renderService;
    protected Specs _specs;

    protected Engine(RenderService renderService, Specs specs)
    {
        _renderService = renderService;
        _specs = specs;
    }

    public abstract void Load();
    public abstract void Update(ulong frameNumber);
}