using Game.Runtime.Components;
using Game.Runtime.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Runtime.Systems {
    internal sealed class EnemiesSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<EnemiesSpawnerService> _enemiesSpawnerService;
        private EcsCustomInject<SceneService> _sceneService;
        private EcsPoolInject<UnitComponent> _unitCmpPool;
        private EcsPoolInject<LifetimeComponent> _lifetimeComponentPool;
        private EcsFilterInject<Inc<LifetimeComponent>> _lifetimeFilter;

        private float _spawnInterval;
        private Camera _camera;

        public void Init(IEcsSystems systems)
        {
            _spawnInterval = _enemiesSpawnerService.Value.EnemySpawnInterval;
            _camera = _enemiesSpawnerService.Value.Camera;
        }

        public void Run(IEcsSystems systems)
        {
            if (_sceneService.Value.GameIsOver)
                return;
            
            CreateEnemy();
            CheckEnemyLifetime();
        }

        private void CreateEnemy()
        {
            if ((_spawnInterval -= Time.deltaTime) > 0)
                return;

            _spawnInterval = _enemiesSpawnerService.Value.EnemySpawnInterval;

            var enemyView = _enemiesSpawnerService.Value.GetEnemy();
            var enemyPosition = GetOutOfScreenPosition();
            enemyView.SetPosition(enemyPosition);
            enemyView.RotateTo(_sceneService.Value.PlayerView.transform.position);

            var enemyEntity = _world.Value.NewEntity();
            ref var unitCmp = ref _unitCmpPool.Value.Add(enemyEntity);
            unitCmp.View = enemyView;
            unitCmp.View.Construct(enemyEntity, _world.Value);
            unitCmp.Velocity = Vector3.up * _enemiesSpawnerService.Value.EnemyMoveSpeed;
            
            ref var lifetimeComponent = ref _lifetimeComponentPool.Value.Add(enemyEntity);
            lifetimeComponent.Value = 3f;
        }

        private Vector3 GetOutOfScreenPosition()
        {
            var randomX = Random.Range(-1000, 1000);
            var randomY = Random.Range(-1000, 1000);
            var randomPosition = new Vector3(randomX, randomY);
            var randomDirection = (_camera.transform.position - randomPosition).normalized;
            var cameraHeight = _camera.orthographicSize * 2;
            var cameraWith = cameraHeight * _camera.aspect;
            return new Vector3(randomDirection.x * cameraHeight, randomDirection.y * cameraWith);
        }
        
        private void CheckEnemyLifetime()
        {
            foreach (var entity in _lifetimeFilter.Value)
            {
                ref var lifetimeCmp = ref _lifetimeComponentPool.Value.Get(entity);
                lifetimeCmp.Value -= Time.deltaTime;

                if (lifetimeCmp.Value > 0)
                    continue;

                ref var unitCmp = ref _unitCmpPool.Value.Get(entity);
                _enemiesSpawnerService.Value.ReleaseEnemy(unitCmp.View);
				
                _world.Value.DelEntity(entity);
            }
        }
    }
}