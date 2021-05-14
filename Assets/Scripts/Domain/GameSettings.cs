using System;
using Domain;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Match3/GameSettings")]
public class GameSettings : ScriptableObject
{
    public GemElement[] Gems;
    public GridSettings GridSettings;
    public int MatchTime;
    [Space(10)]
    public int StartGoal;
    public int GoalStep;
}

[Serializable]
public class GridSettings
{
    public int Horizontal = 6;
    public int Vertical = 6;
}