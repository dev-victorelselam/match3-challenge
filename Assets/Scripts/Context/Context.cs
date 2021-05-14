using Controllers;
using Controllers.Game;
using Controllers.Input;
using UnityEngine;

namespace Context
{
    public class Context : IContext
    {
        public Environment Environment => Environment.Game;
        public GameSettings GameSettings { get; }
        public GameController GameController { get; }
        public InputController InputController { get; }

        public Context()
        {
            ContextProvider.Subscribe(this);

            InputController = new InputController();
            GameSettings = Load<GameSettings>($"Settings/{nameof(GameSettings)}");
            
            GameController = LoadAndInstantiate<GameController>($"Controllers/{nameof(GameController)}");
            GameController.Initialize(GameSettings, InputController);
        }

        private T Load<T>(string path) where T : Object => Resources.Load<T>(path);

        private T LoadAndInstantiate<T>(string path) where T : Object
        {
            var prefab = Load<GameObject>(path);
            return Object.Instantiate(prefab).GetComponent<T>();
        }
    }
}