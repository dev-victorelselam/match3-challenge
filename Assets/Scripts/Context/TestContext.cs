using Controllers.Asset;
using Controllers.Game;
using Controllers.Input;
using Controllers.LocalStorage;
using Controllers.Points;
using Controllers.Sequence;
using Controllers.Sound;
using Domain;
using UnityEngine.Events;

namespace Context
{
    public class TestContext : IContext
    {
        public UnityEvent<bool> OnPause { get; }
        public Environment Environment => Environment.Test;
        public AssetLoader AssetLoader { get; }
        public GameSettings GameSettings { get; }
        public GameController GameController { get; }
        public InputController InputController { get; }
        public ISequenceChecker SequenceChecker { get; }
        public IPointsCalculator PointsCalculator { get; }
        public LocalStorage LocalStorage { get; }
        public SoundController SoundController { get; }
        public void Pause()
        {
            
        }

        public TestContext()
        {
            ContextProvider.Subscribe(this);
            
            OnPause = new UnityEvent<bool>();
            PointsCalculator = new PointsCalculator();
            AssetLoader = new AssetLoader();
            SequenceChecker = new DefaultSequenceChecker(3);
        }
    }
}