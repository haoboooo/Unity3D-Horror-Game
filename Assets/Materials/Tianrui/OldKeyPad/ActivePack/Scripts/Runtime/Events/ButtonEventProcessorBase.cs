// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BigBlit.ActivePack
{
    /// <summary> 
    /// Base class for all buttons events processors. 
    /// </summary>
    public abstract class ButtonEventProcessorBase : MonoBehaviour
    {

        #region FIELDS AND PROPERTIES
        private ActiveObject _target;

        /// <summary> Events target. </summary>
        public ActiveObject Target => _target;
        #endregion

        #region UNITY EVENTS

        protected void Awake() {
            _target = GetComponent<ActiveObject>();
            Assert.IsNotNull(_target);
        }

        protected void Start() {
            if (_target is IPositionable positionable)
                positionable.positionChangedEvent += onPositionChangeHandler;

            if (_target is IPressable pressable) {
                pressable.pressedEvent += onPressHandler;
                pressable.normalEvent += onNormalHandler;
            }

            if (_target is IClickable clickable) {
                clickable.clickEvent += onClickHandler;
                clickable.longClickEvent += onLongClickHandler;
            }

            if (_target is IToggleable toggleable) {
                toggleable.toggleOnEvent += onToggleOnHandler;
                toggleable.toggleOffEvent += onToggleOffHandler;
            }

            if (_target is IDisable disable) {
                disable.disableChangedEvent += disableChangedHandler;
            }
        }

        protected void OnDestroy() {
            Assert.IsNotNull(_target);

            if (_target is IPositionable positionable)
                positionable.positionChangedEvent -= onPositionChangeHandler;

            if (_target is IPressable pressable) {
                pressable.pressedEvent -= onPressHandler;
                pressable.normalEvent -= onNormalHandler;
            }

            if (_target is IClickable clickable) {
                clickable.clickEvent -= onClickHandler;
                clickable.longClickEvent -= onLongClickHandler;
            }

            if (_target is IToggleable toggleable) {
                toggleable.toggleOnEvent -= onToggleOnHandler;
                toggleable.toggleOffEvent -= onToggleOffHandler;
            }

            if (_target is IDisable disable) {
                disable.disableChangedEvent -= disableChangedHandler;
            }
        }

        #endregion

        #region PROTECTED FUNCTIONS

        protected virtual void onPositionChangeHandler(IPositionable positionable) {
            
        }

        protected virtual void onPressHandler(IPressable pressable) {
          
        }

        protected virtual void onNormalHandler(IPressable pressable) {
           
        }

        protected virtual void onLongClickHandler(IClickable clickable) {
          
        }

        protected virtual void onClickHandler(IClickable clickable) {
     
        }

        protected virtual void onToggleOffHandler(IToggleable toggleable) {

        }

        protected virtual void onToggleOnHandler(IToggleable toggleable) {

        }

        protected virtual void disableChangedHandler(IDisable disable) {

        }


        #endregion
    }
}
