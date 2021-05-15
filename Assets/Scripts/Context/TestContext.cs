using Controllers.Game;
using Controllers.Input;
using Controllers.LocalStorage;
using Controllers.Points;
using Controllers.Sequence;
using Controllers.Sound;
using Domain;

namespace Context
{
    public class TestContext : IContext
    {
        public Environment Environment => Environment.Test;
        public GameSettings GameSettings { get; }
        public GameController GameController { get; }
        public InputController InputController { get; }
        public ISequenceChecker SequenceChecker { get; }
        public IPointsCalculator PointsCalculator { get; }
        public LocalStorage LocalStorage { get; }
        public SoundController SoundController { get; }

        public TestContext()
        {
            ContextProvider.Subscribe(this);
        }
    }
}