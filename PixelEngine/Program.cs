

using var game = new XnaGameEngine(
    new GameEngine((s, b, l, p) => new XnaRenderService(s, b, l, p)));
game.Run();
