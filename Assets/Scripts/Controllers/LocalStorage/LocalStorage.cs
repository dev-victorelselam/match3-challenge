using UnityEngine;

namespace Controllers.LocalStorage
{
    public class LocalStorage
    {
        private const string LevelKey = "level_key";
        
        public void LevelPass()
        {
            var currentLevel = GetLevel();
            SetLevel(currentLevel + 1);
        }

        public int GetLevel() => PlayerPrefs.GetInt(LevelKey);
        public void SetLevel(int newLevel) => PlayerPrefs.SetInt(LevelKey, newLevel);

        public void ClearData() => PlayerPrefs.DeleteAll();
    }
}