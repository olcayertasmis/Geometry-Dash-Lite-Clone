using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("BackGround")]
        [SerializeField] private Image bg;
        [SerializeField] private List<Sprite> bgSprites;
        [SerializeField] private float transitionDuration;
        [SerializeField] private float autoTransitionInterval;

        private int _currentIndex;
        private Coroutine _autoTransitionCoroutine;

        private void Start()
        {
            GameManager.Instance.SetGameState(GameState.Menu);

            AudioManager.Instance.PlayMusicSound(AudioManager.SoundType.BackgroundMusic);

            bg.sprite = bgSprites[0];
            _currentIndex = 0;

            _autoTransitionCoroutine = StartCoroutine(AutoTransition());
        }

        private void abc()
        {
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Level_1");
            GameManager.Instance.SetGameState(GameState.Play);
        }

        #region Transition

        private IEnumerator AutoTransition()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoTransitionInterval);

                int nextIndex = (_currentIndex + 1) % bgSprites.Count;
                yield return StartCoroutine(TransitionToNextImage(nextIndex));
            }
        }

        private IEnumerator TransitionToNextImage(int nextIndex)
        {
            float elapsedTime = 0.0f;
            Sprite currentSprite = bg.sprite;
            Sprite nextSprite = bgSprites[nextIndex];

            while (elapsedTime < transitionDuration)
            {
                float t = elapsedTime / transitionDuration;
                SetAlpha(bg, 1.0f - t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetAlpha(bg, 0.0f);
            bg.sprite = nextSprite;
            _currentIndex = nextIndex;

            while (elapsedTime < transitionDuration * 2)
            {
                float t = (elapsedTime - transitionDuration) / transitionDuration;
                SetAlpha(bg, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetAlpha(bg, 1.0f);
        }

        private static void SetAlpha(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        #endregion
    }
}