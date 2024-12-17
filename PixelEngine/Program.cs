

using var game = new XnaGameEngine(
    new GameEngine((s, l, p) => new XnaRenderService(s, l, p)));
game.Run();
