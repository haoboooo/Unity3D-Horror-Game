// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;
using UnityEngine.Events;
using System;

namespace BigBlit.ActivePack
{
    /// <summary>
    /// Converts native IToggleable interface events to Unity Events.
    /// Add this component to the gameObject that implements IToggleable if you want to receive IToggleable events as UnityEvents
    /// </summary>
    public class ToggleableEventTrigger : EventTriggerBase<IToggleable>
    {
        #region NESTES CLASSES

        [Serializable] public class ToggleableEvent : UnityEvent<IToggleable> { }

        #endregion

        #region FIELDS AND PROPERTIES

        [SerializeField] ToggleableEvent onToggleOn = null;
        [SerializeField] ToggleableEvent onToggleOff = null;

        #endregion

        #region UNITY EVENTS

        void Start() {
            EventSource.toggleOnEvent += onToggleOnHandler;
            EventSource.toggleOffEvent += onToggleOffHandler;
        }

        void OnDestroy() {
            EventSource.toggleOnEvent -= onToggleOnHandler;
            EventSource.toggleOffEvent -= onToggleOffHandler;

        }

        #endregion

        #region PRIVATE METHODS

        private void onToggleOnHandler(IToggleable toggleable) {
            onToggleOn?.Invoke(toggleable);
        }

        private void onToggleOffHandler(IToggleable toggleable) {
            onToggleOff?.Invoke(toggleable);
        }

        #endregion
    }
}
