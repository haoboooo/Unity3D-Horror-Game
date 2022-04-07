// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun


using UnityEngine;
using UnityEngine.Assertions;

namespace BigBlit.ActivePack
{    
    /// <summary>
     /// Base for all IPressable objects input controllers.
     /// </summary>
    public abstract class PressControllerBase : MonoBehaviour
    {
        #region FIELDS AND PROPERTIES

        protected IPressable Target => _target;

        /// <summary>Explicitly set the game object that implements IPressable interface. If set to null try to find the proper object.</summary>
        [Tooltip("Explicitly set the game object that implements IPressable interface. If set to null try to find the proper object.")]
        [SerializeField] private GameObject _targetObject = null;

        private IPressable _target;

        #endregion

        #region UNITY EVENTS

        void Awake() {
            if (_targetObject == null) {
                _target = GetComponent<IPressable>();
                if (_target == null)
                    _target = GetComponentInChildren<IPressable>();
            }
            else {
                _target = _targetObject.GetComponent<IPressable>();
            }

            Assert.IsNotNull(_target);
        }
        #endregion
    }
}
