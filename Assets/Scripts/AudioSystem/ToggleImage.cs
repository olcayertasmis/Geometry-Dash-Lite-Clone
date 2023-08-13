using UnityEngine;

namespace AudioSystem
{
    public class ToggleImage : MonoBehaviour
    {
        [SerializeField] private GameObject mutedEffect, unMutedEffect, mutedMusic, unMutedMusic;

        public void ChangeEffectImage()
        {
            ChangeImage(mutedEffect, unMutedEffect);
        }

        public void ChangeMusicImage()
        {
            ChangeImage(mutedMusic, unMutedMusic);
        }

        private void ChangeImage(GameObject muted, GameObject unMuted)
        {
            if (muted.activeSelf)
            {
                muted.SetActive(false);
                unMuted.SetActive(true);
            }
            else if (unMuted.activeSelf)
            {
                muted.SetActive(true);
                unMuted.SetActive(false);
            }
        }
    }
}