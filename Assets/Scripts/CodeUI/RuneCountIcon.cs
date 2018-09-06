using UnityEngine;
using UnityEngine.UI;

namespace CodeUI
{
    public class RuneCountIcon : MonoBehaviour
    {
        [SerializeField] private Text text;

        public int Count
        {
            set { text.text = $"x{value}"; }
        }
    }
}