// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;
using UnityEngine.EventSystems;
namespace BigBlit.ActivePack
{
    /// <summary>
    /// Pointer controller for all pressable objects.
    /// NOTICE: The controller is based on Unity Event System. 
    /// For it to work please make sure that:
    /// - You have unity Event System configured.
    /// - Camera has PhysicsRaycaster component added.
    /// - PhysicsRaycaster EventMask layer and ActiveObjects layers are properly set.
    /// </summary>
    public class PointerPressController : PressControllerBase, IPointerDownHandler, IPointerUpHandler
    {
        #region FIELDS AND PROPERTIES
        public enum PointerId { All = -10,
            MouseLeft = -1, MouseRight = -2, MouseMiddle = -3,
            TouchFirst = 0, TouchSecond = 1, TouchThird = 2 };

        /// <summary>The id of the mouse button or finger number that will trigger the events or all for all mouse button&touch events.</summary>
        [Tooltip("The id of the mouse button or finger number that will trigger the events.")]
        [SerializeField] PointerId _pointerId = PointerId.MouseLeft;

        /// <summary>Keyboard button that must be pressed in addition to the pointer.</summary>
        [Tooltip("Keyboard button that must be pressed in addition to the pointer.")]
        [SerializeField] KeyCode _modifier = KeyCode.None;

        private IPressable _target;

        #endregion

        #region UNITY EVENTS
        protected void Reset() {
            _pointerId = PointerId.MouseLeft;
            _modifier = KeyCode.None;
        }
        #endregion

        #region ES INTERFACES IMPLEMENTATION
        public void OnPointerDown(PointerEventData eventData) {

            if ((int)_pointerId == (int)PointerId.All || eventData.pointerId == (int)_pointerId && (_modifier == KeyCode.None | Input.GetKey(_modifier))) {
                Target.Press();
            }
        }

        public void OnPointerUp(PointerEventData eventData) {

            if ((int)_pointerId == (int)PointerId.All || eventData.pointerId == (int)_pointerId) {
                Target.Normal();
            }
        }

        #endregion
    }
}