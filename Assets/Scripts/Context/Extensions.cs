using System;
using System.Collections;
using System.Linq;
using Domain;
using UnityEngine;

namespace Context
{
    public static class Extensions
    {
        public static bool IsNeighbor(this GemElement gemElement, GemElement otherGemElement)
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

        public static string FormatTime(int timeInSeconds)
        {
            if (timeInSeconds < 60)
                return $"00:{timeInSeconds:00}";

            var result = Math.DivRem(timeInSeconds, 60, out var reminder);
            return $"{result:00}:{reminder:00}";
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