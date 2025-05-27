using CodeBase.Scripts.Utils;
using DG.Tweening;
using System;
using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        public GameState State { get; private set; }

        private void Start()
        {
            Application.targetFrameRate = 60;
            DOTween.SetTweensCapacity(500, 250);

            ChangeState(GameState.Loading);
        }

        public void ChangeState(GameState newState)
        {
            OnBeforeStateChanged?.Invoke(State);
            State = newState;
            OnAfterStateChanged?.Invoke(newState);

            Print.Log($"<color=green>New state: {StringHelper.GetFormattedString(newState)}</color>");
        }
    }
}
