using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BigBlit.ActivePack
{
    /// <summary>
    /// Pointer controller for all draggable objects.
    /// NOTICE: The controller is based on Unity Event System. 
    /// For it to work please make sure that:
    /// - You have unity Event System configured.
    /// - Camera has PhysicsRaycaster component added.
    /// - PhysicsRaycaster EventMask layer and ActiveObjects layers are properly set.
    /// </summary>
    public class PointerDragController : DragControllerBase,
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

        /// <summary>Pointer sensitivity</summary>
        [Tooltip("Pointer sensitivity.")]
        [SerializeField] float _sensitivity = 1.0f;

        /// <summary>Hide cursor while dragging.</summary>
        [Tooltip("Hide cursor on drag.")]
        [SerializeField] bool _hideCursor = false;

        public enum DragPlane { Right, Up, Forward };

        /// <summary>Local game object plane that should be used for drag delta calculation.</summary>
        [SerializeField] DragPlane _dragPlane = DragPlane.Forward;
        
        public enum DragAxis { Off, On, Inverted };

        /// <summary>Pointer delta on local X axis.</summary>
        [Tooltip("Pointer delta on local X axis.")]
        [SerializeField] DragAxis _xAxis = DragAxis.Off;

        /// <summary>Pointer delta on local Y axis.</summary>
        [Tooltip("Pointer delta on local Y axis.")]
        [SerializeField] DragAxis _yAxis = DragAxis.On;

        /// <summary>Pointer delta on local Z axis.</summary>
        [Tooltip("Pointer delta on local Z axis.")]
        [SerializeField] DragAxis _zAxis = DragAxis.Off;


        private Camera _eventCamera = null;

        private Camera EventCamera => _eventCamera != null ? _eventCamera : Camera.main;
   
        private bool _prevCursorVisible = false;

        private bool _isDragging;

        private Vector3 _dragCenter = Vector3.zero;

        private Plane _rayDirPlane = new Plane();

        private Vector3 _prevLocalPoint = Vector3.zero;

        private Vector3 _localDrag = Vector3.zero;

        #endregion

        #region UNITY EVENTS

        protected void Reset() {
            _pointerId = PointerId.MouseLeft;
            _modifier = KeyCode.None;
        }

        void OnDrawGizmosSelected() {
            if (!_isDragging)
                return;

            Vector3 _dragPoint = transform.TransformPoint(_localDrag);

            float dist = Vector3.Distance(_dragCenter, _dragPoint);

            if (_xAxis != DragAxis.Off) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_dragCenter + transform.TransformDirection(new Vector3(getValue(_xAxis, _localDrag.x), 0.0f, 0.0f)), 0.005f);
            }

            if (_yAxis != DragAxis.Off) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_dragCenter + transform.TransformDirection(new Vector3(0.0f, getValue(_yAxis, _localDrag.y), 0.0f)), 0.005f);
            }

            if (_zAxis != DragAxis.Off) {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_dragCenter + transform.TransformDirection(new Vector3(0.0f, 0.0f, getValue(_zAxis, _localDrag.z))), 0.005f);
            }
            Gizmos.color = Color.white;

            if (_dragPlane == DragPlane.Forward) {
                Vector3 a = _dragCenter + (-transform.right - transform.up) * dist;
                Vector3 b = _dragCenter + (-transform.right + transform.up) * dist;
                Vector3 c = _dragCenter + (transform.right + transform.up) * dist;
                Vector3 d = _dragCenter + (transform.right - transform.up) * dist;
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, d);
                Gizmos.DrawLine(d, a);
            }
            else if (_dragPlane == DragPlane.Right) {
                Vector3 a = _dragCenter + (-transform.forward - transform.up) * dist;
                Vector3 b = _dragCenter + (-transform.forward + transform.up) * dist;
                Vector3 c = _dragCenter + (transform.forward + transform.up) * dist;
                Vector3 d = _dragCenter + (transform.forward - transform.up) * dist;
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, d);
                Gizmos.DrawLine(d, a);
            }
            else if (_dragPlane == DragPlane.Up) {
                Vector3 a = _dragCenter + (-transform.forward - transform.forward) * dist;
                Vector3 b = _dragCenter + (-transform.right + transform.forward) * dist;
                Vector3 c = _dragCenter + (transform.right + transform.forward) * dist;
                Vector3 d = _dragCenter + (transform.right - transform.forward) * dist;
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, d);
                Gizmos.DrawLine(d, a);
            }
        }

        #endregion

        #region EVENT SYSTEM EVENTS
        public void OnInitializePotentialDrag(PointerEventData eventData) {         
            if (!checkPointer(eventData.pointerId, true))
                return;

            eventData.useDragThreshold = _useDragThreshold;
            _eventCamera = eventData.pressEventCamera;
            _dragCenter = eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!checkPointer(eventData.pointerId, true))
                return;

            _isDragging = true;

            Ray ray = EventCamera.ScreenPointToRay(eventData.position);
            _rayDirPlane = new Plane(-ray.direction, _dragCenter);
            _prevLocalPoint = transform.InverseTransformPoint(_dragCenter);
            _localDrag = Vector3.zero;

            _prevCursorVisible = Cursor.visible;
            if (_hideCursor)
                Cursor.visible = false;

            Target.DragStart();

        }

        public void OnDrag(PointerEventData eventData) {
            if (!_isDragging || !checkPointer(eventData.pointerId, false))
                return;

            Ray ray = EventCamera.ScreenPointToRay(eventData.position);
            _rayDirPlane.Raycast(ray, out var dist);
            Vector3 worldPoint = ray.GetPoint(dist);

            Vector3 planeNormal = transform.forward;
            if (_dragPlane == DragPlane.Right)
                planeNormal = transform.right;
            else if (_dragPlane == DragPlane.Up)
                planeNormal = transform.up;

            Plane dragPlane  = new Plane(planeNormal, _dragCenter);
            worldPoint = dragPlane.ClosestPointOnPlane(worldPoint);

            Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
            Vector3 localDelta = localPoint - _prevLocalPoint;
            _prevLocalPoint = localPoint;
            _localDrag += localDelta;

            Target.Drag(new Vector3(getValue(_xAxis, -localDelta.x * _sensitivity),
                getValue(_yAxis, localDelta.y) * _sensitivity,
                getValue(_zAxis, -localDelta.z) * _sensitivity));
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

        private float getValue(DragAxis axisMode, float val) {
            if (axisMode == DragAxis.Off)
                return 0.0f;
            if (axisMode == DragAxis.On)
                return val;
            return -val;
        }

        private bool checkPointer(int pointerId, bool checkModifier) {
            return (int)_pointerId == (int)PointerId.All || pointerId == (int)_pointerId
                && (_modifier == KeyCode.None | Input.GetKey(_modifier) || !checkModifier);
        }
    }

    #endregion

}