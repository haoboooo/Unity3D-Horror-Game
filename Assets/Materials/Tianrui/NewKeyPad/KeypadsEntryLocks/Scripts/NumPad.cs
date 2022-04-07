using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BigBlit.ActivePack;

namespace BigBlit.Keypads
{
    [Serializable]
    public class NumPadEvent : UnityEvent<INumPad> { }


    public class NumPad : ActiveObject, INumPad
    {

        #region FIELDS AND PROPERTIES

        [SerializeField] private NumPadEvent valueChanged;
        [SerializeField] private NumPadEvent enter;

        [SerializeField] private int _maxLength = 8;
        [SerializeField] private bool _autoEnter = true;

        public int MaxLength {
            get => _maxLength;
            set {
                if (value == _maxLength)
                    return;

                _maxLength = value;
            }
        }

        [SerializeField] private string _value = "";

        public string Value {
            get => _value;
            set {
                if (value == _value)
                    return;
                _value = value;
                trimText();
                valueChanged?.Invoke(this);
            }
        }

        #endregion

        #region INTERFACE INumPad


        public void RegisterValueChangedListener(UnityAction<INumPad> call) {
            valueChanged.AddListener(call);
        }

        public void RemoveValueChangedListener(UnityAction<INumPad> call) {
            valueChanged.RemoveListener(call);
        }

        public void RegisterEnterListener(UnityAction<INumPad> call) {
            enter.AddListener(call);
        }

        public void RemoveEnterListener(UnityAction<INumPad> call) {
            enter.RemoveListener(call);
        }

        public void PressKey(string key) {
         
            int len = Mathf.Min(_maxLength - _value.Length, key.Length);
            if (len <= 0)
                return;
            _value += key.Substring(0, len);

            valueChanged?.Invoke(this);

            if (_autoEnter && _value.Length == _maxLength)
                enter?.Invoke(this);
        }

        public void PressBack() {
            if (_value.Length <= 0)
                return;

            _value = _value.Remove(_value.Length- 1);
            valueChanged?.Invoke(this);
        }

        public void PressClear() {
            if (_value.Length == 0)
                return;

            _value = _value.Remove(0);

            valueChanged?.Invoke(this);

        }

        public void PressEnter() {
            enter?.Invoke(this);
        }

        #endregion

        #region UNITY EVENTS

        protected override void OnValidate() {
            base.OnValidate();
            _maxLength = Mathf.Max(1, _maxLength);
            trimText();
        }

        #endregion


        #region PRIVATE METHODS

        private void trimText() {
            if (_value.Length > _maxLength)
                _value = _value.Remove(_maxLength);
        }
        #endregion


    }
}