using TMPro;
using UnityEngine;

namespace Game.Runtime.Views
{
    public class CounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp;

        public void SetText(string text)
        {
            tmp.text = text;
        }
    }
}
