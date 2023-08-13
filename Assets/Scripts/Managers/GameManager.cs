using System;
using System.Collections;
using SingletonSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("GameState")]
        private MotionState _currentMotionState;
        private SpeedState _currentSpeedStateState;
        private GameState _currentGameState;

        [Header("Stats")]
        [SerializeField] private float timeToRestart;
        public int attemptCount;


        #region GetStates

        public MotionState GetCurrentMotionState() => _currentMotionState;
        public SpeedState GetCurrentSpeedState() => _currentSpeedStateState;

        public GameState GetCurrentGameState() => _currentGameState;

        #endregion

        #region SetStates

        public void SetMoverStates(MotionState motionState, SpeedState speedState)
        {
            _currentMotionState = motionState;
            _currentSpeedStateState = speedState;

            CheckMotionState(motionState);
            CheckSpeedState(speedState);
        }

        public void SetGameState(GameState gameState)
        {
            _currentGameState = gameState;

            CheckGameState(_currentGameState);
        }

        #endregion

        #region CheckStates

        private void CheckMotionState(MotionState motionState)
        {
            switch (motionState)
            {
                case MotionState.Cube:
                    break;
                case MotionState.Ship:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(motionState), motionState, null);
            }
        }

        private void CheckSpeedState(SpeedState speedState)
        {
            switch (speedState)
            {
                case SpeedState.Slow:
                    break;
                case SpeedState.Normal:
                    break;
                case SpeedState.Fast:
                    break;
                case SpeedState.Faster:
                    break;
                case SpeedState.Fastest:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(speedState), speedState, null);
            }
        }

        private void CheckGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    //AudioManager.Instance.PlayMusicSound(AudioManager.SoundType.BackgroundMusic);
                    attemptCount = 0;
                    break;
                case GameState.Play:
                    //var levelManager = LevelManager.Instance;
                    //var activeLevel = levelManager.GetLevelData(levelManager.currentLevel);
                    //SetMoverStates(activeLevel.motionState, activeLevel.speedState);
                    break;
                case GameState.GameOver:
                    attemptCount += 1;
                    StartCoroutine(OnGameOver());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        #endregion

        private IEnumerator OnGameOver()
        {
            AudioManager.Instance.PlayEffectSound(AudioManager.SoundType.Fail);
            yield return new WaitForSeconds(timeToRestart);

            var activeScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeScene);

            yield return new WaitForSeconds(.25f);
            SetGameState(GameState.Play);
        }
    } // CLASS END

    #region Enums

    public enum SpeedState
    {
        Slow,
        Normal,
        Fast,
        Faster,
        Fastest
    }

    public enum MotionState
    {
        Cube,
        Ship
    }

    public enum GameState
    {
        Menu,
        Play,
        GameOver
    }

    #endregion
} // NAMESPACE END