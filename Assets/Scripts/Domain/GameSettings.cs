using System;
using UnityEngine;

namespace Domain
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Match3/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Match Settings")]
        public GemElement[] Gems;
        public TextMesh PointsUpPrefab;
        public GridSettings GridSettings;
        [Tooltip("Time in Seconds")] [Range(0, 1000)] public int MatchTime;
        
        [Header("Goal Settings")]
        [Space(10)]
        public int StartGoal;
        public int GoalStep;
        
        [Header("Sequence Settings")]
        [Space(10)] 
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