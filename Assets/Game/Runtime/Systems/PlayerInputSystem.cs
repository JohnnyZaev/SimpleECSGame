using Game.Runtime.Components;
using Game.Runtime.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Runtime.Systems 
{
    internal sealed class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem 
    {        
        private EcsWorldInject _world;
        private EcsPoolInject<UnitComponent> _unitCmpPool;
        private EcsPoolInject<PlayerTag> _playerTagPool;
        private EcsCustomInject<SceneService> _sceneData;

        private int _playerEntity;
        private Vector3 _direction;

        public void Init(IEcsSystems systems)
        {
            _playerEntity = _world.Value.NewEntity();

            _playerTagPool.Value.Add(_playerEntity);
            ref var playerCmp = ref _unitCmpPool.Value.Add(_playerEntity);

            playerCmp.View = _sceneData.Value.PlayerView;
        }

        public void Run(IEcsSystems systems)
        {
            var playerMoveSpeed = _sceneData.Value.PlayerMoveSpeed;
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            _direction.x = x;
            _direction.y = y;
            _direction.Normalize();
            var velocity = _direction * playerMoveSpeed;

            if (!_unitCmpPool.Value.Has(_playerEntity))
                return;

            ref var playerCmp = ref _unitCmpPool.Value.Get(_playerEntity);
            playerCmp.Velocity = velocity;
        }
    }
}