using CodeBase.Scripts.Damageable;
using UnityEngine;

namespace CodeBase.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private Transform mainFiller;
        [SerializeField] private Transform delayedFiller;
        [SerializeField] private GameObject container;

        [Header("Parameters")]
        [SerializeField] private bool hideOnEnable = true;
        [SerializeField, Range(-1f, 1f)] private float localScale = 1f;
        [SerializeField, Min(1f)] private float delaySpeed = 10f;

        private bool IsHidden => !container.activeSelf;

        private DamageableObject _damageableObject;

        private float _step;
        #endregion

        private void Awake()
        {
            _damageableObject = GetComponentInParent<DamageableObject>();
        }

        private void OnEnable()
        {
            if (hideOnEnable) Hide();

            Subscribe();
            Init();
        }

        private void Subscribe()
        {
            _damageableObject.OnHealthRestored += Init;
            _damageableObject.OnHealthChanged += OnHealthChanged;
            _damageableObject.OnDied += Hide;
        }

        private void Unsubscribe()
        {
            _damageableObject.OnHealthRestored -= Init;
            _damageableObject.OnHealthChanged -= OnHealthChanged;
            _damageableObject.OnDied -= Hide;
        }

        private void Update()
        {
            SmoothFill();
        }

        private void Init()
        {
            mainFiller.localScale = Vector3.one;
            delayedFiller.localScale = Vector3.one;
            _step = 1f / _damageableObject.MaxHealth;

            ForceLocalScale(localScale);
        }

        private void OnHealthChanged(float health)
        {
            if (IsHidden) Show();

            mainFiller.localScale = new Vector2(health * _step, 1f);
        }

        private void Hide() => container.SetActive(false);

        private void Show() => container.SetActive(true);

        private void SmoothFill()
        {
            if (Mathf.Abs(delayedFiller.localScale.x - mainFiller.localScale.x) > 0.001f)
            {
                var delayedScaleX = Mathf.Lerp(delayedFiller.localScale.x, mainFiller.localScale.x, delaySpeed * Time.deltaTime);
                delayedFiller.localScale = new Vector2(delayedScaleX, 1f);
            }
            else
            {
                delayedFiller.localScale = mainFiller.localScale;
            }
        }

        private void ForceLocalScale(float scale)
        {
            var localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
