using Game.Runtime.Views;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Runtime.Services
{
    public class EnemiesSpawnerService : MonoBehaviour
    {
        [field: SerializeField] public UnitView EnemyViewPrefab { get; private set; }
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public float EnemyMoveSpeed { get; private set; } = 13;
        [field: SerializeField] public float EnemySpawnInterval { get; private set; } = 0.5f;
    
        private ObjectPool<UnitView> _unitsPool;

        private void Awake()
        {
            _unitsPool = new ObjectPool<UnitView>(() => Instantiate(EnemyViewPrefab));
        }

        public UnitView GetEnemy()
        {
            var view = _unitsPool.Get();
            view.SetActive(true);
            return view;
        }

        public void ReleaseEnemy(UnitView view)
        {
            view.SetActive(false);
            _unitsPool.Release(view);
        }
    }
}
