using UnityEngine;

namespace Game.Runtime.Views
{
    public class UnitView : MonoBehaviour
    {
        private static readonly int Up = Animator.StringToHash("up");
        private static readonly int Walk = Animator.StringToHash("walk");

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

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
    }
}
