using UnityEngine;

namespace Controllers.Sound
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public void Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}
