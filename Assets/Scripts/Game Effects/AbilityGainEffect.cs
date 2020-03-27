using System;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Effects/Ability Gain")]
public class AbilityGainEffect : GameEffect
{
    [SerializeField] private Item _newAbility;
    [SerializeField] private bool _isSPecial;
    [SerializeField] private AbilitySlotType _slotType;
    public override void Execute(GameObject user, Action callback = null, CancellationTokenSource tokenSource = null)
    {
        //this should grant the player a new ability. What an ability is and how the player gets it is tbd
        var abilitiesManager = user.GetComponent<AbilitiesManager>();
        if (_isSPecial)
            abilitiesManager?.SetSpecial(_newAbility);
        else
            abilitiesManager?.Unlock(_newAbility, (int)_slotType);
        callback?.Invoke();
    }
    public enum AbilitySlotType
    {
        None = -1,
        Sword = 0,
        Bow = 1,
        Dash = 2,
        Flame = 3,
        Reflect = 4,
        Split = 5,
    }
}