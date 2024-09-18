using System;

namespace Game.Windows
{
    public class EndGameWindowSetup : AWindowSetup
    {
        public Action PlayAgainCallback;
        public int Score;
        public WindowSystem WindowSystem;
    }
}