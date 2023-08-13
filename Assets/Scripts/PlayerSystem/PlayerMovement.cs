using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Speed")]
        private List<float> _speedValues;
        private readonly SpeedState _currentSpeedState;

        [Header("Components")]
        private readonly Player _player;
        private readonly Rigidbody2D _rigidBody2D;
        private readonly Transform _playerSprite;
        private readonly PlayerData _playerData;


        #region Generate

        public PlayerMovement(Player player, SpeedState speedStateState, PlayerData playerData, Transform playerSprite)
        {
            _player = player;
            _rigidBody2D = player.GetComponent<Rigidbody2D>();
            _currentSpeedState = speedStateState;
            _playerData = playerData;
            _playerSprite = playerSprite;
        }

        #endregion


        #region MoveSystem

        public void MoveToRight()
        {
            _rigidBody2D.velocity = new Vector2(_playerData.speedValues[(int)_currentSpeedState], _rigidBody2D.velocity.y);
        }

        public void ShipMovement()
        {
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
            _rigidBody2D.AddForce(Vector2.up * (_playerData.flyPower * Time.deltaTime), ForceMode2D.Impulse);
        }

        #endregion

        #region ShipRotateSystem

        public void ShipRotate()
        {
            var velocityY = _rigidBody2D.velocity.y;
            var limitedVelocityY = Mathf.Clamp(velocityY, -_playerData.maxVelocityY, _playerData.maxVelocityY);
            _playerSprite.rotation = Quaternion.Euler(0, 0, limitedVelocityY * 2);
        }

        #endregion


        #region JumpSystem

        public void Jump()
        {
            _rigidBody2D.velocity = Vector2.zero;
            _rigidBody2D.AddForce(Vector2.up * _playerData.jumpPower, ForceMode2D.Impulse);

            AudioManager.Instance.PlayEffectSound(AudioManager.SoundType.Jump);

            JumpRotate();
        }

        private void JumpRotate()
        {
            var playerRot = _playerSprite.eulerAngles.z;
            var mainTargetRot = (playerRot) + (-180f);
            var otherTargetRot = playerRot - 90f;

            _playerSprite.DORotate(new Vector3(0, 0, mainTargetRot), _playerData.jumpRotateDuration)
                .OnUpdate(() =>
                {
                    if (_player.IsGround())
                    {
                        var myRot = _playerSprite.eulerAngles.z;
                        var mainTargetDiff = myRot - mainTargetRot;
                        var otherTargetDiff = myRot - otherTargetRot;

                        if (otherTargetDiff < mainTargetDiff)
                        {
                            _playerSprite.DOKill();
                            _playerSprite.DORotate(new Vector3(0, 0, otherTargetRot), 0.1f);
                        }
                        else
                        {
                            _playerSprite.DOKill();
                            _playerSprite.DORotate(new Vector3(0, 0, mainTargetRot), 0.1f);
                        }
                    }
                });
        }

        #endregion
    } //CLASS END
} // NAMESPACE END