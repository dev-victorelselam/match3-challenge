using System.Collections;
using System.Threading.Tasks;
using Context;
using Controllers.Input;
using DG.Tweening;
using UnityEngine;

namespace Domain
{
    public class GemElement : MonoBehaviour, IClickable
    {
        public int Y { get; private set; }
        public int X { get; private set; }
        
        public GemType GemType;

        public IEnumerator Move(GemSnapshot gemSnapshot)
        {
            yield return transform.DOMove(gemSnapshot.Position, 0.3f).SetEase(Ease.OutBack).WaitForCompletion();
            SetPosition(gemSnapshot.X, gemSnapshot.Y);
        }
        
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            
            transform.position = new Vector3(x * 2, -y * 2, 0);
        }

        public void OnClick()
        {
            throw new System.NotImplementedException();
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }

        public void Unselect()
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseDown()
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseUp()
        {
            throw new System.NotImplementedException();
        }
    }
}