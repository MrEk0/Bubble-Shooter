namespace Game.Windows
{
    public abstract class AWindow<T> : ASimpleWindow where T : AWindowSetup, new()
    {
        public abstract void Init(T windowSetup);
    }
}