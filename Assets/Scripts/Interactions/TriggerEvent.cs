using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private bool _ignoreFriends;    
    [SerializeField] private bool _ignoreFoes;
    public GameObjectEvent OnTriggerEntered;
    public GameObjectEvent OnTriggerStay;
    public GameObjectEvent OnTriggerExit;
    private HashSet<GameObject> _alreadyColliding = new HashSet<GameObject>();

    private void Update()
    {
        _alreadyColliding.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var obj = other.attachedRigidbody.gameObject;
        if (ShouldIgnore(obj) == false)
        {
            _alreadyColliding.Add(obj);
            OnTriggerEntered?.Invoke(other.attachedRigidbody.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        var obj = other.attachedRigidbody.gameObject;
        if (ShouldIgnore(obj) == false)
        {
            _alreadyColliding.Add(obj);
            OnTriggerStay?.Invoke(other.attachedRigidbody.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var obj = other.attachedRigidbody.gameObject;
        if (ShouldIgnore(obj) == false)
        {
            _alreadyColliding.Add(obj);
            OnTriggerExit?.Invoke(other.attachedRigidbody.gameObject);
        }
    }

    private bool ShouldIgnore(GameObject other)
    {
        if (!_ignoreFoes && !_ignoreFriends)
            return false;
        var isFriend = GameState.Instance.AreFriendly(gameObject, other);
        if (isFriend && _ignoreFriends || !isFriend && _ignoreFoes)
            return true;
        return _alreadyColliding.Contains(other);
    }
}