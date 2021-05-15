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
        /// <summary>
        /// Check if one gem is neighbor from other
        /// </summary>
        /// <param name="gemElement"></param>
        /// <param name="otherGemElement"></param>
        /// <returns></returns>
        public static bool IsNeighborFrom(this GemElement gemElement, GemElement otherGemElement)
        {
            if (Mathf.Abs(gemElement.X - otherGemElement.X) <= 1 && gemElement.Y - otherGemElement.Y == 0)
                return true;
            if (Mathf.Abs(gemElement.Y - otherGemElement.Y) <= 1 && gemElement.X - otherGemElement.X == 0)
                return true;

            return false;
        }

        public static GridPositionSnapshot GetSnapshot(this IGridPosition gridPosition) 
            => new GridPositionSnapshot(gridPosition);

        public static IEnumerator RunAndWait(this MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            //run
            var coroutinesList = coroutines.Select(monoBehaviour.StartCoroutine).ToList();

            //wait
            foreach (var coroutine in coroutinesList)
                yield return coroutine;
        }

        public static bool IsNullOrEmpty(this ICollection collection) => collection == null || collection.Count == 0;
        public static T GetRandom<T>(this ICollection<T> collection) => collection.ToList()[Random.Range(0, collection.Count)];

        public static string FormatTime(int timeInSeconds)
        {
            if (timeInSeconds < 60)
                return $"00:{timeInSeconds:00}";

            var result = Math.DivRem(timeInSeconds, 60, out var reminder);
            return $"{result:00}:{reminder:00}";
        }

        public static float GetMultiplier(this GameSettings gameSettings, int count) => 
            gameSettings.StageMultipliers.First(sm => sm.Count >= count).Multiplier;
        
        /// <summary>
        /// Instantiate random gem
        /// </summary>
        /// <param name="gameSettings"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static GemElement GetRandomGem(this GameSettings gameSettings, Transform container) => 
            Object.Instantiate(gameSettings.Gems.GetRandom(), container);
        
        /// <summary>
        /// Instantiate gem based on type
        /// </summary>
        /// <param name="gameSettings"></param>
        /// <param name="container"></param>
        /// <param name="gemType"></param>
        /// <returns></returns>
        public static GemElement GetGemOfType(this GameSettings gameSettings, Transform container, GemType gemType) => 
            Object.Instantiate(gameSettings.Gems.First(g => g.GemType == gemType), container);
        
        /// <summary>
        /// Instantiate gem based on id
        /// </summary>
        /// <param name="gameSettings"></param>
        /// <param name="container"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GemElement GetGemOfType(this GameSettings gameSettings, Transform container, int id) => 
            Object.Instantiate(gameSettings.Gems.First(g => g.GemType == (GemType)id), container);

        /// <summary>
        /// Get neighbors from specified position.
        /// </summary>
        /// <param name="grid">grid</param>
        /// <param name="x">horizontal index</param>
        /// <param name="y">vertical index</param>
        /// <returns>
        /// 0 - right
        /// 1 - left
        /// 2 - up
        /// 3 - down</returns>
        public static IGridPosition[] GetNeighbors(this IGridPosition[,] grid, int x, int y)
        {
            return new[]
            {
                grid.SafeGetAt(x + 1, y), //right
                grid.SafeGetAt(x - 1, y), //left
                grid.SafeGetAt(x, y + 1), //up
                grid.SafeGetAt(x, y - 1), //down
            };
        }
        
        public static IGridPosition[] GetDiagonalNeighbors(this IGridPosition[,] grid, int x, int y)
        {
            return new[]
            {
                grid.SafeGetAt(x - 1, y + 1), //up left
                grid.SafeGetAt(x - 1 , y - 1), //down left
                
                grid.SafeGetAt(x + 1, y + 1), //up right
                grid.SafeGetAt(x + 1, y - 1), //down right
            };
        }

        /// <summary>
        /// Safe get in grid, returns null if the position is out of range
        /// </summary>
        /// <param name="grid">grid</param>
        /// <param name="x">horizontal index</param>
        /// <param name="y">vertical index</param>
        /// <returns></returns>
        public static IGridPosition SafeGetAt(this IGridPosition[,] grid, int x, int y)
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
}