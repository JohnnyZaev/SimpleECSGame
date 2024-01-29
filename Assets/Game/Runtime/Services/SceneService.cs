using Game.Runtime.Views;
using UnityEngine;

namespace Game.Runtime.Services
{
    public class SceneService : MonoBehaviour
    {
        [field: SerializeField] public UnitView PlayerView { get; private set; }
        [field: SerializeField] public float PlayerMoveSpeed { get; private set; } = 10;
    }
}
