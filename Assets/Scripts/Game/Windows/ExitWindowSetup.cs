using System;

namespace Game.Windows
{
    public class ExitWindowSetup : AWindowSetup
    {
        public WindowSystem WindowSystem;
        public Action ExitCallback;
    }
}