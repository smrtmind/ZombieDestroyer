using CodeBase.Scripts.Damageable;
using CodeBase.Scripts.UI;
using Unavinar.Pooling;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Scripts.Spawners
{
    public class PopupSpawner : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private PopupElement popupElement;

        [Header("Parameters")]
        [SerializeField, Min(0f)] private float positionOffsetY = 1f;
        [SerializeField] private Vector2 randomRangeMultiplier = Vector2.one;

        [Space]
        [SerializeField] private Color popupColor = Color.white;

        private ObjectPool _objectPool;
        #endregion

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            DamageableObject.OnAnyDamaged += OnAnyDamagedHandler;
        }

        private void Unsubscribe()
        {
            DamageableObject.OnAnyDamaged -= OnAnyDamagedHandler;
        }

        private void OnAnyDamagedHandler(DamageResult result)
        {
            var popUp = GetPopUp(result.Victim.GetTransform.position);

            var damageIsNegative = result.IsNegative;
            var actualDamageValue = Mathf.RoundToInt(Mathf.Abs(result.DealtDamage));
            var text = damageIsNegative ? $"{actualDamageValue}" : $"+{actualDamageValue}";

            popUp.Setup(text, popupColor);
            popUp.Run();
        }

        private PopupElement GetPopUp(Vector3 position)
        {
            var popUp = _objectPool.Get(popupElement);
            popUp.transform.SetParent(transform);
            popUp.transform.position = GetRandomPosition(position);
            popUp.transform.localScale = Vector3.one;

            return popUp;
        }

        private Vector3 GetRandomPosition(Vector3 victimPosition)
        {
            return new Vector3(victimPosition.x, victimPosition.y + positionOffsetY, victimPosition.z)
                + (Vector3)Vector2.Scale(Random.insideUnitCircle, randomRangeMultiplier);
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
