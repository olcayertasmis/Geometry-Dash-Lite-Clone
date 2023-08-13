using Managers;
using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "Level_", menuName = "New Level Data")]
    public class LevelData : ScriptableObject
    {
        public int levelNumber;
        public SpeedState speedState;
        public MotionState motionState;
    }
}