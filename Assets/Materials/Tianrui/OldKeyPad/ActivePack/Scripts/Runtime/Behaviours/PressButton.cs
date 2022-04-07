// Customizable Buttons, Toggles And Switches Pack
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;

namespace BigBlit.ActivePack.Buttons
{
    /// <summary> 
    /// Basic pressable Button. Implements button press behaviour. 
    /// </summary>
    public class PressButton : ActiveObject, IPositionable, IPressable, IDisable
    {
        #region FIELDS AND PROPERTIES

        /// <summary>Is button pressable.</summary>
        [Tooltip("Is button pressable.")]
        [SerializeField] bool _isPressable = true;

        [SerializeField] bool _isDisabled;


        public bool IsPressable {
            get => _isPressable;
            set { _isPressable = value; }
        }

        public virtual bool IsPressed => _isPressed;
        public virtual bool IsNormal => !_isPressed;

        public static readonly float PressedInValue = 1.0f;
        public static readonly float PressedOutValue = 0.0f;

        public float PressTime => Time.time - _pressStateTime;

     
        public virtual float Position {
            get => _isPressed ? PressedInValue : PressedOutValue;
            set {
                if (!_isPressable)
                    return;

                if (value < 0.5f) {
                    if (_isPressed)
                        Normal();
                }
                else if (!_isPressed)
                    Press();
            }
        }

        private float _pressStateTime;
        private bool _isPressed;


#if UNITY_EDITOR
        private bool _prevIsDisabled;
#endif

        #endregion


        #region UNITY EVENTS
        protected override void OnValidate() {
            base.OnValidate();
            ResetButton();
#if UNITY_EDITOR
            if(_prevIsDisabled != _isDisabled) {
                _prevIsDisabled = _isDisabled;
                disableChangedEvent?.Invoke(this);
            }
#endif
        }

        #endregion

        #region INTERFACE IDisable

        public event DisableEvent disableChangedEvent;

        public bool IsDisabled {
            get => _isDisabled;
            set {
                if (_isDisabled == value)
                    return;
                _isDisabled = true;
                disableChangedEvent?.Invoke(this);
                ResetButton();
            }
        }

        #endregion

        #region INTERFACE IPressable
        public event PressEvent pressedEvent;
        public event PressEvent normalEvent;
        public event PositionChangeEvent positionChangedEvent;

        public void Press() {
            if (_isDisabled || _isPressed || !_isPressable)
                return;
            setPressed();
        }

        public void Normal() {
            if (_isDisabled || !_isPressed || !_isPressable)
                return;
            setNormal();
        }
        #endregion

        #region PROTECTED FUNCTIONS

        protected virtual void OnPress() {

        }

        protected virtual void OnNormal() {

        }

        protected void RaisePositionChangedEvent() {
            if (_isDisabled)
                return;
            positionChangedEvent?.Invoke(this);
        }


        protected virtual void ResetButton() {
            _pressStateTime = 0.0f;
            _isPressed = false;
        }
        #endregion

        #region PRIVATE FUNCTIONS
        private void setPressed() {
            _pressStateTime = Time.time;
            _isPressed = true;
            OnPress();
            pressedEvent?.Invoke(this);
            RaisePositionChangedEvent();
        }

        private void setNormal() {
            _pressStateTime = Time.time;
            _isPressed = false;
            OnNormal();
            normalEvent?.Invoke(this);
            RaisePositionChangedEvent();
        }

        #endregion
    }
}
