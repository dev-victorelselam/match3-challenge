using Controllers.Asset;
using Controllers.Game;
using Controllers.Input;
using Controllers.Points;
using Controllers.Sequence;
using Domain;

namespace Context
{
    public class Context : IContext
    {
        public Environment Environment => Environment.Game;
        public AssetLoader AssetLoader { get; }
        public GameSettings GameSettings { get; }
        public GameController GameController { get; }
        public InputController InputController { get; }
        public ISequenceChecker SequenceChecker { get; }
        public IPointsCalculator PointsCalculator { get; }

        public Context()
        {
            ContextProvider.Subscribe(this);
            
            AssetLoader = new AssetLoader();
            GameSettings = AssetLoader.Load<GameSettings>($"Settings/{nameof(GameSettings)}");
            
            InputController = AssetLoader.LoadAndInstantiate<InputController>($"Controllers/{nameof(InputController)}");
            PointsCalculator = new DefaultPointsCalculator();
            SequenceChecker = new DefaultSequenceChecker(GameSettings.MinItemsCount);

            GameController = AssetLoader.LoadAndInstantiate<GameController>($"Controllers/{nameof(GameController)}");
            GameController.Initialize(GameSettings, SequenceChecker, InputController, PointsCalculator);
        }
    }
}