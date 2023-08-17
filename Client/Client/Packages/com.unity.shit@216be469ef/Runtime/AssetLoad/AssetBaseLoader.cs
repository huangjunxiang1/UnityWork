namespace Main
{
    public abstract class AssetBaseLoader
    {
        public abstract UnityEngine.Object Load(string path);
        public abstract TaskAwaiter<UnityEngine.Object> LoadAsync(string path);
        public abstract void Release(UnityEngine.Object target);
    }
}
