using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Game
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private Button _tryAgain;

        private void Awake()
        {
            _tryAgain.onClick.AddListener(() =>
            {
                ContextProvider.Context.GameController.StartGame();
                Hide();
            });
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            gameObject.transform.DOScale(0, 0.3f).SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}