using System;

using var game = new RetroDemo.Game();
game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 59.0);
game.Run();
