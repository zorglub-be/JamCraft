using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string[] _withTags;    
    [SerializeField] private bool _ignoreFriends;    
    [SerializeField] private bool _ignoreFoes;
    [SerializeField] private bool _ignoreTriggers;    
    [SerializeField] private bool _ignoreColliders;
    public GameObjectEvent OnTriggerEntered;
    public GameObjectEvent OnTriggerStay;
    public GameObjectEvent OnTriggerExit;
    private HashSet<GameObject> _alreadyEntered = new HashSet<GameObject>();
    private HashSet<GameObject> _alreadyExited = new HashSet<GameObject>();
    private HashSet<GameObject> _alreadyStayed = new HashSet<GameObject>();

    private void Update()
    {
        _alreadyEntered.Clear();
        _alreadyExited.Clear();
        _alreadyStayed.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerEntered, _alreadyEntered);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerStay, _alreadyStayed);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        HandleTrigger(other, OnTriggerExit, _alreadyExited);
    }

    private void HandleTrigger(Collider2D other, GameObjectEvent callback, HashSet<GameObject> alreadyColliding)
    {
        if(ShouldIgnoreColliderType(other))
            return;
        var obj = other?.attachedRigidbody?.gameObject;
        if (obj == null)
            obj = other.gameObject;
        if (ShouldIgnore(obj, alreadyColliding) == false)
        {
            alreadyColliding.Add(obj);
            callback?.Invoke(obj);
        }
    }
    private bool ShouldIgnoreColliderType(Collider2D other)
    {
        
        if (_ignoreTriggers && other.isTrigger)
        {
            return true;
        }
        return _ignoreColliders && other.isTrigger == false;
    }
    private bool ShouldIgnore(GameObject other, HashSet<GameObject> alreadyColliding)
    {
        if (_ignoreFoes || _ignoreFriends)
        {
            var isFriend = GameState.Instance.AreFriendly(gameObject, other);
            if (isFriend && _ignoreFriends || !isFriend && _ignoreFoes)
                return true;
        }
        if (alreadyColliding.Contains(other))
            return true;
        if (_withTags.Length ==0 || _withTags.Contains(other.tag))
            return false;
        return true;
    }
}