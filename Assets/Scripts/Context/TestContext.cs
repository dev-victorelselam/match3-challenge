using Controllers;
using Controllers.Game;
using UnityEngine;

namespace Context
{
    public class TestContext : IContext
    {
        public Environment Environment => Environment.Test;
        public GameSettings GameSettings { get; }
        public GameController GameController { get; }

        public TestContext()
        {
            ContextProvider.Subscribe(this);
            
            GameSettings = Resources.Load<GameSettings>($"Settings/{nameof(GameSettings)}");
            GameController = Resources.Load<GameObject>($"Controllers/{nameof(GameController)}")
                .GetComponent<GameController>();
        }
    }
}