using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SelectControlOnStart : MonoBehaviour
{
 
        [FormerlySerializedAs("defaultButton")] [SerializeField] private GameObject _defaultButton;
        [SerializeField] private Canvas _canvas;
        private GameObject _previousObject;
        private Canvas _previousCanvas;
 
        private void Start()
        {
            _previousObject = EventSystem.current.currentSelectedGameObject;
            _previousCanvas = _previousObject?.GetComponentInParent<Canvas>();
            if (_previousCanvas && _previousCanvas != _canvas)
                _previousCanvas.enabled = false;
            if (_defaultButton)
            {
                EventSystem.current.SetSelectedGameObject(_defaultButton);
            }
        }

        private void OnDisable()
        {
            if(_previousObject)
                EventSystem.current.SetSelectedGameObject(_previousObject);
            if (_previousCanvas && _previousCanvas != _canvas)
                _previousCanvas.enabled = true;
        }
}
