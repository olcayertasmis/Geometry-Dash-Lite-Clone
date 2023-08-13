using SingletonSystem;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameUIManager : Singleton<GameUIManager>
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI attemptCountText;
        private const string AttemptText = "ATTEMPT ";

        private void Start()
        {
            attemptCountText.text = AttemptText + GameManager.Instance.attemptCount;
        }
    }
}