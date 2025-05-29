using UnityEngine;

namespace CodeBase.Scripts.Weapons
{
    public class Aim : MonoBehaviour
    {
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);
    }
}
