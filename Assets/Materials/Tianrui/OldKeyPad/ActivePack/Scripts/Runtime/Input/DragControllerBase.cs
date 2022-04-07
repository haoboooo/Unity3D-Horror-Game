// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;
using UnityEngine.Assertions;

namespace BigBlit.ActivePack
{
    /// <summary>
    /// Base for all IDraggable objects input controllers.
    /// </summary>
    public abstract class DragControllerBase : MonoBehaviour
    {
        #region FIELDS AND PROPERTIES
        protected IDraggable Target => _target;

        /// <summary>Explicitly set the game object that implements IDraggable interface. If set to null try to find the proper object.</summary>
        [Tooltip("Explicitly set the game object that implements IDraggable interface. If set to null try to find the proper object.")]
        [SerializeField] private GameObject _targetObject = null;

        private IDraggable _target;

        #endregion

        #region UNITY EVENTS

        void Awake() {
            if (_targetObject == null) {
                _target = GetComponent<IDraggable>();
                if (_target == null)
                    _target = GetComponentInChildren<IDraggable>();
            }
            else {
                _target = _targetObject.GetComponent<IDraggable>();
            }

            Assert.IsNotNull(_target);
        }

        #endregion
    }
}
