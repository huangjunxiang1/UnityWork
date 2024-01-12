namespace Game
{
    public abstract class SAssetBaseLoader
    {
        public abstract UnityEngine.Object Load(string path);
        public abstract STask<UnityEngine.Object> LoadAsync(string path);
        public abstract void Release(UnityEngine.Object target);
    }
}
