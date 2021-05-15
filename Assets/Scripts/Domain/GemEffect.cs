using UnityEngine;

namespace Domain
{
    public class GemEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fx;

        public void Activate(Color color)
        {
            transform.SetParent(null);
            var module = _fx.main;
            module.startColor = color;
            
            _fx.gameObject.SetActive(true);
            
            //use particle lifetime value to avoid visual glitches
            Destroy(gameObject, module.startLifetime.constant);
        }
    }
}