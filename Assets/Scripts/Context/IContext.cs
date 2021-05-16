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
    public interface IContext
    {
        UnityEvent<bool> OnPause { get; }
        
        Environment Environment { get; }
        AssetLoader AssetLoader { get; }
        GameSettings GameSettings { get; }
        GameController GameController { get; }
        InputController InputController { get; }
        ISequenceChecker SequenceChecker { get; }
        IPointsCalculator PointsCalculator { get; }
        LocalStorage LocalStorage { get; }
        SoundController SoundController { get; }

        void Pause();
    }
}