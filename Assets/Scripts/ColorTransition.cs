using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransition : MonoBehaviour
{
    [Header("BackGround")]
    [SerializeField] private Image backgroundSprite;
    [SerializeField] private Color[] colors;
    [SerializeField] private float transitionDuration;
    [SerializeField] private float pauseDuration;

    private int _currentIndex = 0;
    private Coroutine _colorTransitionCoroutine;

    private void Start()
    {
        backgroundSprite.color = colors[0];
        _currentIndex = 0;

        _colorTransitionCoroutine = StartCoroutine(ColorTransitionCoroutine());
    }

    private IEnumerator ColorTransitionCoroutine()
    {
        while (true)
        {
            Color currentColor = backgroundSprite.color;
            Color targetColor = colors[(_currentIndex + 1) % colors.Length];
            float elapsedTime = 0.0f;

            while (elapsedTime < transitionDuration)
            {
                float t = elapsedTime / transitionDuration;
                backgroundSprite.color = Color.Lerp(currentColor, targetColor, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            backgroundSprite.color = targetColor;
            _currentIndex = (_currentIndex + 1) % colors.Length;

            yield return new WaitForSeconds(pauseDuration);
        }
    }
}