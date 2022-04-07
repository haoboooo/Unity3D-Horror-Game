using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BigBlit.ActivePack
{
    /// <summary>
    ///  Pointer Drag Controller for circular gesture.
    /// NOTICE: The controller is based on Unity Event System. 
    /// For it to work please make sure that:
    /// - You have unity Event System configured.
    /// - Camera has PhysicsRaycaster component added.
    /// - PhysicsRaycaster EventMask layer and ActiveObjects layers are properly set.
    /// </summary>
    public class PointerCircularDragController : DragControllerBase,
        IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region FIELDS AND PROPERTIES
        public enum PointerId
        {
            All = -10,
            MouseLeft = -1, MouseRight = -2, MouseMiddle = -3,
            TouchFirst = 0, TouchSecond = 1, TouchThird = 2
        };

        /// <summary>The id of the mouse button or finger number that will trigger the events or all for all mouse button&touch events.</summary>
        [Tooltip("The id of the mouse button or finger number that will trigger the events.")]
        [SerializeField] PointerId _pointerId = PointerId.MouseLeft;

        /// <summary>Keyboard button that must be pressed in addition to the pointer.</summary>
        [Tooltip("Keyboard button that must be pressed in addition to the pointer.")]
        [SerializeField] KeyCode _modifier = KeyCode.None;

        /// <summary>Use drag threshold.</summary>
        [Tooltip("Use drag threshold.")]
        [SerializeField] bool _useDragThreshold = true;

        /// <summary>Pointer sensitivity in turns.More turns = less sensitivity</summary>
        [Tooltip("Pointer sensitivity expressed in turns. More turns = less sensitivity.")]
        [SerializeField] float _turns = 1.0f;

        /// <summary>Hide cursor while dragging.</summary>
        [Tooltip("Hide cursor on drag.")]
        [SerializeField] bool _hideCursor = false;

        /// <summary>Drag Center offset on game object local Z axis.</summary>
        [Tooltip(">Drag Center offset on game object local Z axis.")]
        [SerializeField] float _dragCenterZOffset = 0.0f;

        private Vector3 DragCenter => transform.TransformPoint(new Vector3(0.0f, 0.0f, _dragCenterZOffset));

        private Camera EventCamera => _eventCamera != null ? _eventCamera : Camera.main;

        private Camera _eventCamera = null;

        private bool _prevCursorVisible = false;

        private float _prevAngle = 0.0f;

        private bool _isDragging = false;

        #endregion

        #region UNITY EVENTS
        void OnValidate() {
            _turns = Mathf.Max(0.01f, _turns);
        }

        void OnDrawGizmosSelected() {
            Gizmos.DrawSphere(DragCenter, 0.01f);
        }

        #endregion

        #region EVENT SYSTEM EVENTS
        public void OnInitializePotentialDrag(PointerEventData eventData) {
            eventData.useDragThreshold = _useDragThreshold;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!checkPointer(eventData.pointerId, true))
                return;

            _isDragging = true;
            _eventCamera = eventData.pressEventCamera;
            _prevAngle = getMousePosAngle(eventData.position);

            _prevCursorVisible = Cursor.visible;
            if (_hideCursor)
                Cursor.visible = false;

            Target.DragStart();
        }

        public void OnDrag(PointerEventData eventData) {
            if (!_isDragging || !checkPointer(eventData.pointerId, false))
                return;

            float angle = getMousePosAngle(eventData.position);
            float angleDelta = Mathf.DeltaAngle(angle, _prevAngle);
            _prevAngle = angle;

            Target.Drag(new Vector2(angleDelta / (360.0f * _turns), 0.0f));
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!_isDragging || !checkPointer(eventData.pointerId, false))
                return;

            Target.DragEnd();

            _isDragging = false;
            _eventCamera = null;
            Cursor.visible = _prevCursorVisible;
        }


        #endregion

        #region PRIVATE METHODS

        private float getMousePosAngle(Vector2 mousePos) {

            Vector2 pivotPosition = EventCamera.WorldToScreenPoint(DragCenter);
            Vector2 vec = (mousePos - pivotPosition).normalized;
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        }

        private bool checkPointer(int pointerId, bool checkModifier) {
            return (int)_pointerId == (int)PointerId.All || pointerId == (int)_pointerId
                && (_modifier == KeyCode.None | Input.GetKey(_modifier) || !checkModifier);
        }

        #endregion


    }
}