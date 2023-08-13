using Managers;
using UnityEngine;

namespace PortalSystem
{
    public class Portal : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private MotionState motionState;
        [SerializeField] private SpeedState speedState;

        public MotionState GetPortalMotionState() => motionState;
        public SpeedState GetPortalSpeedState() => speedState;
    }
}