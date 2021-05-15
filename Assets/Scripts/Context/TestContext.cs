using Controllers.Game;
using Controllers.Input;
using Controllers.Sequence;
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

        public TestContext()
        {
            ContextProvider.Subscribe(this);
        }
    }
}