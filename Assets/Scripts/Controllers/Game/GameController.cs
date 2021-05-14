using System.Collections;
using Context;
using Controllers.Input;
using Domain;
using UnityEngine;

namespace Controllers.Game
{
    public class GameController : MonoBehaviour
    {
        private GameSettings _gameSettings;
        private InputController _inputController;
    
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GameHud _gameHud;
        private GemElement[,] _gems;

        private GemSelectionController _gemSelectionController;

        public void Initialize(GameSettings gameSettings, InputController inputController)
        {
            _gameSettings = gameSettings;
            _inputController = inputController;
            _gemSelectionController = new GemSelectionController(_inputController);
            _gemSelectionController.OnSelectionComplete.AddListener(
                (first, second) => StartCoroutine(ChangeGemsPosition(first, second)));
        }
        
        public void StartGame()
        {
            _gameHud.StartGame(_gameSettings.MatchTime);
            
            ResetMatch();
            PopulateGrid(_gameSettings.GridSettings);
        }

        private void PopulateGrid(GridSettings gridSettings)
        {
            _gems = new GemElement[gridSettings.Horizontal, gridSettings.Vertical];
            for (var i = 0; i < gridSettings.Horizontal; i++)
            for (var j = 0; j < gridSettings.Vertical; j++)
            {
                var gem = GetRandomGem(i, j);
                gem.SetPosition(i, j);
                _gems[i, j] = gem;
            }
            
            _gameGrid.transform.position = new Vector3(-gridSettings.Horizontal + 1, gridSettings.Vertical);
        }
        
        private IEnumerator ChangeGemsPosition(GemElement first, GemElement second)
        {
            var firstSnapshot = new GemSnapshot(first);
            var secondSnapshot = new GemSnapshot(second);

            _inputController.Enable(false);
            yield return this.RunAndWait(first.Move(secondSnapshot), second.Move(firstSnapshot));
            _inputController.Enable(true);
        }

        private void ResetMatch()
        {
            if (_gems.IsNullOrEmpty())
                return;
            
            foreach (var gemElement in _gems)
                Destroy(gemElement.gameObject);
            _gems = new GemElement[0, 0];
        }

        private GemElement GetRandomGem(int i, int j)
        {
            return Instantiate(_gameSettings.Gems[Random.Range(0, _gameSettings.Gems.Length)], _gameGrid);
        }
    }
}