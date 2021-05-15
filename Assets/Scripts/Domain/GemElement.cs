using System.Collections;
using Context;
using Controllers.Input;
using DG.Tweening;
using UnityEngine;

namespace Domain
{
    public class GemElement : MonoBehaviour, IClickable, IGridPosition
    {
        public int Y { get; private set; }
        public int X { get; private set; }
        public int Id => (int) GemType;
        public Transform Transform => transform;

        public GemType GemType;
        public Color Color;
        [SerializeField] private GemEffect _fx;
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private SpriteRenderer _additive;
        [SerializeField] private AudioClip _select;

        public IEnumerator Move(GridPositionSnapshot gemSnapshot)
        {
            yield return transform.DOMove(gemSnapshot.Position, 0.3f).SetEase(Ease.Linear).WaitForCompletion();
            SetPosition(gemSnapshot.X, gemSnapshot.Y);
            Unselect();
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            
            transform.position = new Vector3(x * 2, y * 2);
        }
        
        public void Remove()
        {
            Unselect();
            _fx.Activate(Color);
            Destroy(gameObject);
        }

        public void OnClick()
        {
            //play some animation
        }

        public void Select()
        {
            ContextProvider.Context.SoundController.Play(_select);
            
            transform.DOScale(1.1f, 0.2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            _additive.DOFade(1, 0.2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        }

        public void Unselect()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            
            _additive.DOKill();
            _additive.DOFade(0, 0f);
        }

        public async void InvalidMove()
        {
            Unselect();

            await _image.DOColor(Color.red, 0.2f).AsyncWaitForCompletion();
            await _image.DOColor(Color.white, 0.2f).AsyncWaitForCompletion();
        }
    }
}