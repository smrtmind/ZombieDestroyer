using CodeBase.Scripts.Utils;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace CodeBase.Scripts.Damageable
{
    [DisallowMultipleComponent]
    public class DamageFeedback : MonoBehaviour
    {
        [SerializeField] private DamageableObject damageableObject;
        [SerializeField] private EmissionController emissionController;

        [Space]
        [SerializeField] private Color damageColor;
        [SerializeField, Min(0.1f)] private float blinkDuration;

        private CancellationTokenSource _blinkCts;

        private void OnEnable()
        {
            damageableObject.OnDamaged += OnDamagedHandler;
        }

        private void OnDisable()
        {
            damageableObject.OnDamaged -= OnDamagedHandler;

            _blinkCts?.Cancel();
            _blinkCts?.Dispose();
            _blinkCts = null;
        }

        private void OnDamagedHandler()
        {
            _blinkCts?.Cancel();
            _blinkCts?.Dispose();
            _blinkCts = new CancellationTokenSource();

            BlinkAsync(_blinkCts.Token).Forget();
        }

        private async UniTaskVoid BlinkAsync(CancellationToken token)
        {
            emissionController.ChangeEmission(damageColor);
            try
            {
                await UniTask.Delay(
                    (int)(blinkDuration * 1000),
                    cancellationToken: token);
            }
            catch (OperationCanceledException) { }

            emissionController.ResetToDefault();
        }
    }
}
