using UnityEngine;
using static CodeBase.Scripts.Utils.Enums;

namespace CodeBase.Scripts.UI.Menus.Base
{
    public abstract class UiMenu : UiHider
    {
        [field: Header("Menu Parameters")]
        [field: SerializeField] public GameState State { get; private set; }
    }
}
