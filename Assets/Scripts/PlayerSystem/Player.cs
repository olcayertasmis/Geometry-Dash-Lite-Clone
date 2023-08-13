using System;
using Managers;
using PortalSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [Header("Jump Controller")]
        private bool _isJumping;
        private bool _canJump;
        private bool _isGround;

        [Header("Components")]
        [SerializeField] private Transform playerSpriteTransform;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private GameObject tailParticle;
        [SerializeField] private GameObject startParticle;
        [SerializeField] private GameObject gameOverParticle;
        private GameManager _gameManager;
        private PlayerMovement _playerMovement;
        private SpeedState _currentSpeedState;
        private SpriteRenderer _playerSpriteRenderer;

        [Header("Speed")]
        private float _currentSpeed;

        [Header("Ship Controller")]
        private bool _isFly;

        [Header("Control")]
        private bool _canMove;

        #region UnityFonctions

        private void Awake()
        {
            _gameManager = GameManager.Instance;

            _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            var levelManager = LevelManager.Instance;
            var activeLevel = levelManager.GetLevelData(levelManager.currentLevel);

            ChangeMotionState(activeLevel.motionState, activeLevel.speedState);

            Instantiate(startParticle, transform);
        }

        private void FixedUpdate()
        {
            if (_canMove) _playerMovement.MoveToRight();

            if (_canJump && !_isJumping)
            {
                _playerMovement.Jump();
                _isJumping = true;
            }

            if (_isFly) _playerMovement.ShipMovement();
        }

        private void Update()
        {
            if (_gameManager.GetCurrentGameState() == GameState.Play) Invoke(nameof(SetMover), .25f);

            switch (_gameManager.GetCurrentMotionState())
            {
                case MotionState.Cube:
                    _canJump = Input.GetMouseButton(0);
                    break;

                case MotionState.Ship:
                    if (_gameManager.GetCurrentMotionState() == MotionState.Ship)
                    {
                        _playerMovement.ShipRotate();

                        if (Input.GetMouseButton(0))
                        {
                            _isFly = true;
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            _isFly = false;
                        }
                    }

                    break;
                default:
                    Debug.Log("MotionState is empty");
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("ObstacleWall"))
            {
                _isJumping = false;

                var contactInfo = other.contacts[0].point;
                var contactInfoInverse = transform.InverseTransformPoint(contactInfo);

                if (contactInfoInverse.y > -.50)
                {
                    PlayGameOverParticle();
                    _gameManager.SetGameState(GameState.GameOver);
                }
            }

            if (other.gameObject.CompareTag("ObstacleSpine"))
            {
                PlayGameOverParticle();
                _gameManager.SetGameState(GameState.GameOver);
            }

            if (other.gameObject.CompareTag("Ground"))
            {
                tailParticle.SetActive(true);
                _isGround = true;
                _isJumping = false;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGround = false;
                tailParticle.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Portal"))
            {
                var portal = other.gameObject.GetComponent<Portal>();
                ChangeMotionState(portal.GetPortalMotionState(), portal.GetPortalSpeedState());
            }

            if (other.gameObject.CompareTag("Finish")) SceneManager.LoadScene("Finish");
        }

        #endregion

        #region MyFonctions

        private PlayerMovement UpdatePlayerMovement()
        {
            return new PlayerMovement(this, _currentSpeedState, playerData, playerSpriteTransform);
        }

        private void ChangeMotionState(MotionState motionState, SpeedState speedState)
        {
            _gameManager.SetMoverStates(motionState, speedState);

            _currentSpeedState = _gameManager.GetCurrentSpeedState();

            SetRbGravity(motionState);
            SetBoxColliderSize(motionState);
            SetSprite(motionState);

            _playerMovement = UpdatePlayerMovement();
        }

        private void SetRbGravity(MotionState motionState)
        {
            rigidBody2D.gravityScale = motionState switch
            {
                MotionState.Cube => playerData.cubeGravityScale,
                MotionState.Ship => playerData.shipGravityScale,
                _ => throw new ArgumentOutOfRangeException(nameof(motionState), motionState, null)
            };
        }

        private void SetBoxColliderSize(MotionState motionState)
        {
            boxCollider2D.size = motionState switch
            {
                MotionState.Cube => playerData.cubeBoxColliderSize,
                MotionState.Ship => playerData.shipBoxColliderSize,
                _ => throw new ArgumentOutOfRangeException(nameof(motionState), motionState, null)
            };
        }

        private void SetSprite(MotionState motionState)
        {
            switch (motionState)
            {
                case MotionState.Cube:
                    _playerSpriteRenderer.sprite = playerData.cubeSprite;
                    break;
                case MotionState.Ship:
                    _playerSpriteRenderer.sprite = playerData.shipSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(motionState), motionState, null);
            }
        }

        private void SetMover()
        {
            _canMove = true;
        }

        private void PlayGameOverParticle()
        {
            Instantiate(gameOverParticle, transform);
        }

        #endregion

        #region Helpers

        public bool IsGround() => _isGround;

        #endregion
    }
}