using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public class Aim : MonoBehaviour
    {
        [SerializeField] private GameObject body;

        public void Hide() => body.SetActive(false);

        public void Show() => body.SetActive(true);
    }
}
