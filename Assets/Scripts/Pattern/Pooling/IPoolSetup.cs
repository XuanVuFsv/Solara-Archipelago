namespace VitsehLand.Scripts.Pattern.Pooling
{
    public interface IPoolSetup : IPool
    {
        public IPool InitPool();
        public int GetMaxPoolSize();
        public int GetPoolSize();
        public string GetName();
        public void Get();
        public void AddGameEvent();
        public void RemoveGameEvent();
    }
}