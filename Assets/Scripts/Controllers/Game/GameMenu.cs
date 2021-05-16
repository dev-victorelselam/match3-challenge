using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Game
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _deleteProgress;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
            _restart.onClick.AddListener(() =>
            {
                ContextProvider.Context.GameController.StartGame();
                Hide();
            });
            _deleteProgress.onClick.AddListener(() =>
            {
                ContextProvider.Context.LocalStorage.ClearData();
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
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    ContextProvider.Context.Pause();
                });
        }
    }
}