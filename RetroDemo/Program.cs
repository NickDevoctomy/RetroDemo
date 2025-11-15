using RetroDemo;

using var game = new Game(800, 600, false);
game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 59.0);
game.Run();
