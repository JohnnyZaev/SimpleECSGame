using Game.Runtime.Views;
using UnityEngine;

namespace Game.Runtime.Services
{
    public class SceneService : MonoBehaviour
    {
        [field: SerializeField] public UnitView PlayerView { get; private set; }
        [field: SerializeField] public float PlayerMoveSpeed { get; private set; } = 10;
        [field: SerializeField] public CounterView CounterView { get; private set; }
        [field: SerializeField] public PopupView PopupView { get; private set; }
        public bool GameIsOver { get; set; }
    }
}
