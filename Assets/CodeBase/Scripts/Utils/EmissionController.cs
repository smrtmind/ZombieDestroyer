using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Scripts.Utils
{
    [DisallowMultipleComponent]
    public class EmissionController : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private Renderer[] renderers;

        [Header("Defaults")]
        [SerializeField] private Color defaultEmission = Color.black;
        [SerializeField, Min(0.1f)] private float fadeDuration = 1f;

        private static readonly string[] k_PropNames = { "_EmissionColor", "_EmissiveColor" };
        private const string k_EmissionKeyword = "_EMISSION";

        private int _emissionID;
        private bool _hasEmissionProp;
        private bool _initialized;

        private Dictionary<Renderer, MaterialPropertyBlock> _blocks;
        private Dictionary<Renderer, Tween> _tweens;
        private Dictionary<Renderer, Color> _current;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            if (!_initialized) return;
            
            ResetToDefault();
        }

        private void Init()
        {
            if (renderers == null || renderers.Length == 0)
            {
                enabled = false;
                return;
            }

            Renderer firstValid = null;
            foreach (var r in renderers)
                if (r != null)
                {
                    firstValid = r;
                    break;
                }

            if (firstValid == null)
            {
                enabled = false;
                return;
            }

            DetectCorrectPropertyName(firstValid);

            if (!_hasEmissionProp)
            {
                enabled = false;
                return;
            }

            InitData();
            ForceEnablePropertyBlock();

            _initialized = true;
        }

        private void DetectCorrectPropertyName(Renderer validRenderer)
        {
            foreach (var name in k_PropNames)
            {
                if (validRenderer.sharedMaterial.HasProperty(name))
                {
                    _emissionID = Shader.PropertyToID(name);
                    _hasEmissionProp = true;
                    break;
                }
            }
        }

        private void InitData()
        {
            _blocks = new Dictionary<Renderer, MaterialPropertyBlock>(renderers.Length);
            _tweens = new Dictionary<Renderer, Tween>(renderers.Length);
            _current = new Dictionary<Renderer, Color>(renderers.Length);
        }

        private void ForceEnablePropertyBlock()
        {
            foreach (var r in renderers)
            {
                if (r == null) continue;

                var mat = r.material;
                mat.EnableKeyword(k_EmissionKeyword);

                var block = new MaterialPropertyBlock();
                r.GetPropertyBlock(block);

                block.SetColor(_emissionID, defaultEmission);
                r.SetPropertyBlock(block);

                _blocks[r] = block;
                _current[r] = defaultEmission;
            }
        }

        public void ChangeEmission(Color c)
        {
            if (!_initialized || _blocks == null) return;

            foreach (var r in renderers)
            {
                if (r == null) continue;
                if (!_blocks.TryGetValue(r, out var block)) continue;

                block.SetColor(_emissionID, c);
                r.SetPropertyBlock(block);
                _current[r] = c;
            }
        }

        public void ChangeEmissionSmooth(Color target, float duration)
        {
            if (!_initialized || _blocks == null) return;

            foreach (var r in renderers)
            {
                if (r == null) continue;

                if (_tweens.TryGetValue(r, out var old)) old.Kill();

                _tweens[r] = DOTween.To(
                    () => _current[r],
                    x =>
                    {
                        _current[r] = x;
                        _blocks[r].SetColor(_emissionID, x);
                        r.SetPropertyBlock(_blocks[r]);
                    },
                    target,
                    duration
                );
            }
        }

        public void FadeEmissionOut(Action onComplete = null)
        {
            if (!_initialized || _blocks == null) return;

            int remaining = 0;
            foreach (var r in renderers)
                if (r != null) remaining++;

            if (remaining == 0)
            {
                onComplete?.Invoke();
                return;
            }

            foreach (var r in renderers)
            {
                if (r == null) continue;
                if (_tweens.TryGetValue(r, out var old)) old.Kill();

                _tweens[r] = DOTween.To(
                    () => _current[r],
                    x =>
                    {
                        _current[r] = x;
                        _blocks[r].SetColor(_emissionID, x);
                        r.SetPropertyBlock(_blocks[r]);
                    },
                    defaultEmission,
                    fadeDuration
                )
                .OnComplete(() =>
                {
                    remaining--;
                    if (remaining <= 0)
                        onComplete?.Invoke();
                });
            }
        }

        public void ResetToDefault()
        {
            if (!_initialized || _blocks == null) return;

            foreach (var r in renderers)
            {
                if (r == null) continue;

                if (_tweens != null && _tweens.TryGetValue(r, out var old)) old.Kill();

                _blocks[r].SetColor(_emissionID, defaultEmission);
                r.SetPropertyBlock(_blocks[r]);
                _current[r] = defaultEmission;
            }
        }

        private void ClearAllTweens()
        {
            if (_tweens == null) return;

            foreach (var tween in _tweens.Values)
                tween?.Kill();
        }

        private void OnDisable()
        {
            ClearAllTweens();
        }

        private void OnDestroy()
        {
            ClearAllTweens();

            _blocks?.Clear();
            _tweens?.Clear();
            _current?.Clear();
        }
    }
}
