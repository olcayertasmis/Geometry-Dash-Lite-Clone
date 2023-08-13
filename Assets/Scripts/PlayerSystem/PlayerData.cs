using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "New Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Speed")]
        public List<float> speedValues;

        [Header("Jump")]
        public float jumpPower;
        public float jumpRotateDuration;

        [Header("ShipMode")]
        public float flyPower;
        public float shipGravityScale;
        public float maxVelocityY;
        public Vector2 shipBoxColliderSize;
        public Sprite shipSprite;

        [Header("CubeMode")]
        public float cubeGravityScale;
        public Vector2 cubeBoxColliderSize;
        public Sprite cubeSprite;
    }
}