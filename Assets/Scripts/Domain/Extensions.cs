using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Domain
{
    public static class Extensions
    {
        public static bool IsNeighborFrom(this GemElement gemElement, GemElement otherGemElement)
        {
            if (Mathf.Abs(gemElement.X - otherGemElement.X) <= 1 && gemElement.Y - otherGemElement.Y == 0)
                return true;
            if (Mathf.Abs(gemElement.Y - otherGemElement.Y) <= 1 && gemElement.X - otherGemElement.X == 0)
                return true;

            return false;
        }

        public static IEnumerator RunAndWait(this MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            //run
            var coroutinesList = coroutines.Select(monoBehaviour.StartCoroutine).ToList();

            //wait
            foreach (var coroutine in coroutinesList)
                yield return coroutine;
        }

        public static bool IsNullOrEmpty(this ICollection collection) 
            => collection == null || collection.Count == 0;

        public static T GetRandom<T>(this ICollection<T> collection)
            => collection.ToList()[Random.Range(0, collection.Count)];

        public static string FormatTime(int timeInSeconds)
        {
            if (timeInSeconds < 60)
                return $"00:{timeInSeconds:00}";

            var result = Math.DivRem(timeInSeconds, 60, out var reminder);
            return $"{result:00}:{reminder:00}";
        }

        public static float GetMultiplier(this GameSettings gameSettings, int count) => 
            gameSettings.StageMultipliers.First(sm => sm.Count >= count).Multiplier;
        
        public static GemElement GetRandomGem(this GameSettings gameSettings, Transform container) 
            => Object.Instantiate(gameSettings.Gems[Random.Range(0, gameSettings.Gems.Length)], container);
        public static GemElement GetGemOfType(this GameSettings gameSettings, Transform container, GemType gemType) => 
            Object.Instantiate(gameSettings.Gems.First(g => g.GemType == gemType), container);
        public static GemElement GetGemOfType(this GameSettings gameSettings, Transform container, int id) => 
            Object.Instantiate(gameSettings.Gems.First(g => g.GemType == (GemType)id), container);

        public static IGridPosition[] GetNeighbors(this IGridPosition[,] grid, int x, int y)
        {
            return new[]
            {
                grid.GetAt(x + 1, y),
                grid.GetAt(x - 1, y),
                grid.GetAt(x, y + 1),
                grid.GetAt(x, y - 1),
            };
        }

        public static IGridPosition GetAt(this IGridPosition[,] grid, int x, int y)
        {
            try
            {
                return grid[x, y];
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    public class GemSnapshot
    {
        public Vector3 Position;
        public Vector3 EulerAngles;

        public int X;
        public int Y;
        
        public GemSnapshot(GemElement gemElement)
        {
            Position = gemElement.transform.position;
            EulerAngles = gemElement.transform.eulerAngles;

            X = gemElement.X;
            Y = gemElement.Y;
        }
    }
}