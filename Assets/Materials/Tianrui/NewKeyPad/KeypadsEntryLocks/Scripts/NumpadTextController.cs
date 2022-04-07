using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using BigBlit.ActivePack;
using UnityEngine.UI;

namespace BigBlit.Keypads
{
    public class NumpadTextController : ActiveObject
    {

        #region FIELDS AND PROPERTIES

        public Text passwordText;

        [SerializeField] bool _showPrompt;
        [SerializeField] float _blinkInterval;

        [SerializeField] string _promptChar;

        [SerializeField] bool _showPassword;
        [SerializeField] string _maskChar;

        [SerializeField] NumPad _numPad;
        [SerializeField] NumpadText _numPadText;

        private NumPadLockControl padControl;

        private float _blinkAge;
        private bool _isPrompt;

        public NumPad NumPad {
            get => _numPad;
            set {
                if (_numPad == value)
                    return;
                if (_numPad != null)
                    _numPad.RemoveValueChangedListener(onNumPadValueChanged);
                _numPad = value;
                if (_numPad != null)
                    _numPad.RegisterValueChangedListener(onNumPadValueChanged);
            }
        }

        public void UpdatePassword(string text)
        {
            _numPadText.Text = text;
            //passwordText.text = text;
        }

        #endregion

        #region UNITY EVENTS

        protected override void OnValidate() {
            base.OnValidate();
            if (_maskChar.Length > 1)
                _maskChar = _maskChar.Substring(_maskChar.Length - 1, 1);
            if (_promptChar.Length > 1)
                _promptChar = _promptChar.Substring(_promptChar.Length - 1, 1);

        }

        protected override void Awake() {
            base.Awake();

            if (_numPad == null)
                _numPad = GetComponent<NumPad>();
            if (_numPadText == null)
                _numPadText = GetComponent<NumpadText>();

            padControl = GetComponent<NumPadLockControl>();
        }

        protected override void OnEnable() {
            base.OnEnable();

            if (_numPad != null)
                _numPad.RegisterValueChangedListener(onNumPadValueChanged);

            updateView();

        }

        protected override void OnDisable() {
            base.OnDisable();

            if (_numPad != null)
                _numPad.RemoveValueChangedListener(onNumPadValueChanged);
        }

        protected void Update() 
        {
            updatePrompt();
        }
        #endregion

        #region PRIVATE METHODS


        private void onNumPadValueChanged(INumPad numPad) {
            _blinkAge = 0.0f;
            _isPrompt = true;
            updateView();
        }

        private void updateView() {
            if (_numPad == null || _numPadText == null)
                return;

            _numPadText.CellsNum = padControl.passwordLimit;
            _numPadText.Text = padControl.passwordText;

            //passwordText.text = padControl.passwordText;

            //if (_showPassword)
            //{
            //    passwordText.text = _numPad.Value;
            //    //_numPadText.Text = _numPad.Value;
            //}
            //else
            //{
            //    if (string.IsNullOrEmpty(_maskChar))
            //    {
            //        _maskChar = "*";
            //    }
            //    passwordText.text = new string(_maskChar[0], _numPad.Value.Length);
            //    //_numPadText.Text = new string(_maskChar[0], _numPad.Value.Length);
            //}


            if (_isPrompt && _showPrompt && 4 > _numPadText.Text.Length)
            {
                _numPadText.Text += _promptChar;
                //passwordText.text += _promptChar;
            }
        }

        private void updatePrompt() {
            _blinkAge += Time.deltaTime;
            if (_blinkAge >= _blinkInterval) {
                _blinkAge = 0.0f;
                _isPrompt = !_isPrompt;
                updateView();
            }
        }
        #endregion


    }
}