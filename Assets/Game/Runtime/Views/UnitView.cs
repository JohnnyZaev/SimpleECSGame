using Game.Runtime.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Runtime.Views
{
    public class UnitView : MonoBehaviour
    {
        private static readonly int Up = Animator.StringToHash("up");
        private static readonly int Walk = Animator.StringToHash("walk");

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        
        private int _entity;
        private EcsWorld _world;
        
        public void Construct(int entity, EcsWorld world)
        {
            _entity = entity;
            _world = world;
        }
        
        private void OnCollisionEnter2D(Collision2D _)
        {
            var entity = _world.NewEntity();
            var pool = _world.GetPool<CollisionEvent>();
            ref var evt = ref pool.Add(entity);
            evt.CollidedEntity = _entity;
        }

        public void Move(Vector3 translation)
        {
            transform.Translate(translation);
        }

        public void SetDirection(Vector3 velocity)
        {
            spriteRenderer.flipX = velocity.x < 0;
            spriteRenderer.flipY = velocity.y < 0;
        }

        public void UpdateAnimationState(Vector3 velocity)
        {
            animator.SetBool(Up, velocity.y != 0);
            animator.SetBool(Walk, velocity.x != 0 && velocity.y == 0);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void RotateTo(Vector3 position)
        {
            var direction = position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = rotation;
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
