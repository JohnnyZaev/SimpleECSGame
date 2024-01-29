using Game.Runtime.Components;
using Game.Runtime.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime.Systems
{
    internal sealed class EndGameSystem : IEcsRunSystem
    {
        private EcsCustomInject<SceneService> _sceneService;
        private EcsFilterInject<Inc<CollisionEvent>> _collisionsEvtFilter;
        private EcsPoolInject<CollisionEvent> _collisionsEvtPool;
        private EcsPoolInject<PlayerTag> _playerTagPool;
        private EcsFilterInject<Inc<UnitComponent>> _unitCmpFilter;

        public void Run(IEcsSystems systems)
        {
            if (_sceneService.Value.GameIsOver)
                return;
            
            CheckLoseCondition();
            CheckWinCondition();
        }
        
        private void CheckWinCondition()
        {
            if (Time.timeSinceLevelLoad <= 10)
                return;

            ShowEndGamePopup("You win!");
            _sceneService.Value.GameIsOver = true;
            StopAllUnits();
        }

        private void CheckLoseCondition()
        {
            foreach (var entity in _collisionsEvtFilter.Value)
            {
                ref var collisionEvt = ref _collisionsEvtPool.Value.Get(entity);
                var collidedEntity = collisionEvt.CollidedEntity;

                if (!_playerTagPool.Value.Has(collidedEntity))
                    continue;

                ShowEndGamePopup("You lose!");
                _sceneService.Value.GameIsOver = true;
                StopAllUnits();
            }
        }
        
        private void StopAllUnits()
        {
            foreach (var entity in _unitCmpFilter.Value)
            {
                _unitCmpFilter.Pools.Inc1.Del(entity);
            }
        }
        
        private void ShowEndGamePopup(string message)
        {
            var popupWindow = _sceneService.Value.PopupView;

            popupWindow.SetActive(true);
            popupWindow.SetDescription(message);
            popupWindow.SetButtonText("Restart");
            popupWindow.Button.onClick.RemoveAllListeners();
            popupWindow.Button.onClick.AddListener(RestartGame);
        }
        
        private static void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
