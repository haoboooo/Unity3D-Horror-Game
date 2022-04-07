// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;

namespace BigBlit.ActivePack
{
    /// <summary>
    /// Converts native IToggleable interface events to Unity Events.
    /// Add this component to the gameObject that implements IToggleable if you want to receive IToggleable events as UnityEvents
    /// </summary>
    public class PositionableEventTrigger : EventTriggerBase< IPositionable>
    {
        #region NESTES CLASSES

        [Serializable] public class PositionableEvent : UnityEvent<IPositionable> { }

        [Serializable] public class PositionableValueEvent : UnityEvent<float> { }

        #endregion

        #region FIELDS AND PROPERTIES

        [SerializeField] PositionableEvent onPositionChanged = null;

        [Tooltip("Multiplies value sent by onPositionValueChanged event.")]
        [SerializeField] float valueMultiplier = 1.0f;

        [SerializeField] PositionableValueEvent onPositionValueChanged = null;

        #endregion

        #region UNITY EVENTS


        void Start() {
            EventSource.positionChangedEvent += onPositionChangedHandler;
        }

        private void OnDestroy() {
            EventSource.positionChangedEvent -= onPositionChangedHandler;
        }

        #endregion


        #region PRIVATE METHODS

        private void onPositionChangedHandler(IPositionable positionable) {
            onPositionChanged?.Invoke( positionable);
            onPositionValueChanged?.Invoke(positionable.Position * valueMultiplier);
        }


        #endregion
    }
}
