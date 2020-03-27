using UnityEngine;

public class Burner : MonoBehaviour
{
        [SerializeField] private int _tickDamage;
        [SerializeField] private int _duration;
    
        public void Burn(GameObject target)
        {
            if (target == null)
                return;
            target.GetComponentInChildren<Flamable>()?.Burn(_tickDamage, _duration);
        }
}
