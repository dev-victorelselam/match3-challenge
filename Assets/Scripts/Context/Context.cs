using Controllers.Asset;
using Controllers.Game;
using Controllers.Input;
using Controllers.LocalStorage;
using Controllers.Points;
using Controllers.Sequence;
using Controllers.Sound;
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
        public LocalStorage LocalStorage { get; }
        public SoundController SoundController { get; }

        public Context()
        {
            ContextProvider.Subscribe(this);
            
            LocalStorage = new LocalStorage();
            AssetLoader = new AssetLoader();
            GameSettings = AssetLoader.Load<GameSettings>($"Settings/{nameof(GameSettings)}");
            SoundController = AssetLoader.LoadAndInstantiate<SoundController>($"Controllers/{nameof(SoundController)}");
            
            InputController = AssetLoader.LoadAndInstantiate<InputController>($"Controllers/{nameof(InputController)}");
            //since this creation is in context, we can easily change to another script with another formula
            PointsCalculator = new PointsCalculator();
            SequenceChecker = new DefaultSequenceChecker(GameSettings.MinItemsCount);

            GameController = AssetLoader.LoadAndInstantiate<GameController>($"Controllers/{nameof(GameController)}");
            GameController.Initialize(GameSettings, InputController, LocalStorage, 
                SoundController, SequenceChecker, PointsCalculator);
        }
    }
}