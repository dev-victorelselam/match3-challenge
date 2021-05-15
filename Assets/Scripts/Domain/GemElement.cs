using System.Collections;
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

        public IEnumerator Move(GemSnapshot gemSnapshot)
        {
            
            yield return transform.DOMove(gemSnapshot.Position, 0.3f).SetEase(Ease.Linear).WaitForCompletion();
            X = gemSnapshot.X;
            Y = gemSnapshot.Y;
        }
        
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            
            transform.position = new Vector3(x * 2, -y * 2, 0);
        }
        
        public void Remove()
        {
            
        }

        public void OnClick()
        {
            
        }

        public void Select()
        {
            
        }

        public void Unselect()
        {
            
        }

        public void MouseDown()
        {
            
        }

        public void MouseUp()
        {
            
        }
    }
}