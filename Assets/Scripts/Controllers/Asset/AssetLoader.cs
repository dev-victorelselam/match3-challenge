using UnityEngine;

namespace Controllers.Asset
{
    public class AssetLoader
    {
        public T Load<T>(string path) where T : Object => Resources.Load<T>(path);

        public T LoadAndInstantiate<T>(string path) where T : Object
        {
            var prefab = Load<GameObject>(path);
            return Object.Instantiate(prefab).GetComponent<T>();
        }
    }
}