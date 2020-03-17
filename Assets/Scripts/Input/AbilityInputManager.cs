using UnityEngine;

public class AbilityInputManager : MonoBehaviour
{
    [SerializeField] private AbilitiesManager _abilitiesManager;

    private void Awake()
    {
        _abilitiesManager = GetComponent<AbilitiesManager>();
    }

    private void Update()
    {
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.NextPrimary))
        {
            _abilitiesManager.NextPrimary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.NextSecondary))
        {
            _abilitiesManager.NextSecondary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.PreviousPrimary))
        {
            _abilitiesManager.PreviousPrimary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.PreviousSecondary))
        {
            _abilitiesManager.PreviousSecondary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.PrimaryAttack))
        {
            _abilitiesManager.UsePrimary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.SecondaryAttack))
        {
            _abilitiesManager.UseSecondary();
        }
        if (NeoInput.GetKeyDown(NeoInput.NeoKeyCode.SpecialAttack))
        {
            _abilitiesManager.UseSpecial();
        }
    }
}