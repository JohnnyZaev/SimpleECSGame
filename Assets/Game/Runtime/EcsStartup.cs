using Game.Runtime.Services;
using Game.Runtime.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Runtime 
{
    internal sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SceneService sceneService;
        
        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start () {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _systems
                .Add(new PlayerInputSystem())
                .Add(new MovementSystem())
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Inject(sceneService)
                .Init ();
        }

        private void Update () {
            _systems?.Run ();
        }

        private void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
            }
            
            if (_world == null) return;
            _world.Destroy ();
            _world = null;
        }
    }
}