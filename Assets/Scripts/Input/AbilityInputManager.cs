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