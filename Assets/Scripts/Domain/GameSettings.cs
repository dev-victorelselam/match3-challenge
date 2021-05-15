using System;
using UnityEngine;

namespace Domain
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Match3/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public GemElement[] Gems;
        public GridSettings GridSettings;
        [Tooltip("Time in Seconds")] [Range(0, 1000)] public int MatchTime;
        [Space(10)]
        public int StartGoal;
        public int GoalStep;
        [Space(10)] 
        [Header("Sequence Settings")]
        public int MinItemsCount;
        public float PointsPerItem;
        public StageMultiplier[] StageMultipliers;
    }

    [Serializable]
    public class StageMultiplier
    {
        public int Count;
        public float Multiplier;
    }

    [Serializable]
    public class GridSettings
    {
        [Range(3, 10)] public int Horizontal = 6;
        [Range(3, 10)] public int Vertical = 6;
    }
}