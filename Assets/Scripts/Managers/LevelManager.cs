using System.Linq;
using LevelSystem;
using SingletonSystem;
using UnityEngine;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelData[] allLevels;

        public int currentLevel;

        public LevelData GetLevelData(int levelNumber)
        {
            return allLevels.FirstOrDefault(level => level.levelNumber == levelNumber);
        }
    }
}