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
    public class DraggableEventTrigger  : EventTriggerBase<IDraggable>
    {

        #region NESTES CLASSES
        [Serializable] public class DraggableEvent : UnityEvent<IDraggable> { }
        #endregion

        #region FIELDS AND PROPERTIES

        [SerializeField] DraggableEvent onDragStart = null;
        [SerializeField] DraggableEvent onDrag = null;
        [SerializeField] DraggableEvent onDragEnd = null;

        #endregion


        #region UNITY EVENTS

        void Start() {
            EventSource.dragStartEvent += onDragStartHandler;
            EventSource.dragEvent += onDragHandler;
            EventSource.dragEndEvent += onDragEndHandler;
        }

        private void OnDestroy() {
            EventSource.dragStartEvent -= onDragStartHandler;
            EventSource.dragEvent -= onDragHandler;
            EventSource.dragEndEvent -= onDragEndHandler;
        }

        #endregion


        #region PRIVATE METHODS

        private void onDragStartHandler(IDraggable draggable) {
            onDragStart?.Invoke(draggable);
        }

        private void onDragHandler(IDraggable draggable) {
            onDrag?.Invoke(draggable);
        }

        private void onDragEndHandler(IDraggable draggable) {
            onDragEnd?.Invoke(draggable);
        }
        #endregion
    }
}
