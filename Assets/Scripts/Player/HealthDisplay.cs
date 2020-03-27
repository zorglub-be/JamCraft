using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthDisplay : MonoBehaviour
{
    private Health _playerHealth;
    private Heart[] _hearts;
    [SerializeField] private int _healthPerHeart = 2;
    

    private void Start()
    {
        _hearts = GetComponentsInChildren<Heart>();
        _playerHealth = GameState.Instance.Player.GetComponent<Health>();
        UpdateHealth();
        _playerHealth.OnChanged.AddListener(UpdateHealth);
    }

    private void UpdateHealth()
    {
        var max = _playerHealth.MaximumHealth;
        var current = _playerHealth.CurrentHealth;
        for (int i = _hearts.Length -1; i >=0; i--)
        {
            var remainingEmptyHeart = max - i * _healthPerHeart;
            if (remainingEmptyHeart <= 0)
            {
                _hearts[i].gameObject.SetActive(false);
                continue;
            }
            _hearts[i].gameObject.SetActive(true);
            _hearts[i].SetBackgroundFill(Mathf.Clamp((float) remainingEmptyHeart / _healthPerHeart, 0f, 1f));
            var remainingHeart = current - i * _healthPerHeart;
            if (remainingHeart <= 0)
            {
                _hearts[i].SetFill(0);
                continue;
            }

            if (remainingHeart < _healthPerHeart)
            {
                _hearts[i].SetFill((float) remainingHeart / _healthPerHeart);
                continue;
            }
            _hearts[i].SetFill(1);
        }
    }
}
