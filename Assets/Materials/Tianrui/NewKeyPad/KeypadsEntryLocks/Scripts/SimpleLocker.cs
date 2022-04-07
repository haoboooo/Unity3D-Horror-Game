using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BigBlit.ActivePack;

namespace BigBlit.Keypads
{
    public interface ILocker
    {
        bool IsLocked { get; }
    }

    [Serializable]
    public class LockEvent : UnityEvent<ILocker> { }

    public class SimpleLocker : ActiveObject, ILocker
    {

        [SerializeField] string _password;

        [SerializeField] bool _autoLock;

        [SerializeField] float _autolockDelay;

        [SerializeField] bool _isLocked;

        private float _autolockAge;

        [SerializeField] LockEvent locked;

        [SerializeField] LockEvent unlocked;

        [SerializeField] LockEvent accessGranted; 

        [SerializeField] LockEvent accessDeny;
   
        public bool IsLocked {
            get => _isLocked;
        }

        public INumPad Pad => _pad != null ? _pad : (_pad = GetComponent<INumPad>());

        private INumPad _pad;

        protected override void OnEnable() {
            Pad?.RegisterValueChangedListener(onVaueChanged);
            Pad?.RegisterEnterListener(onEnter);
        }

        protected override void OnDisable() {
            Pad?.RemoveValueChangedListener(onVaueChanged);
            Pad?.RemoveEnterListener(onEnter);
        }

        void Update() {
            if(_autoLock && !_isLocked) {
                _autolockAge += Time.deltaTime;
                if(_autolockAge >= _autolockDelay) {              
                    doLock () ;
                }
                 
            }
        }

        void onVaueChanged(INumPad numPad) {
            
        }

        void onEnter(INumPad numPad) {
            tryUnlock();
        }

        private void tryUnlock() {
            if (!IsLocked)
                return;

            if (Pad == null)
                return;

            if(_password == Pad.Value) {
                doUnlock();
                accessGranted.Invoke(this);
                Pad.Value = "";
            } else {
                accessDeny.Invoke(this);
            }
        }

        void doUnlock() {
            if(_autoLock) {
                _autolockAge = 0.0f;
            }

            _isLocked = false;
            unlocked?.Invoke(this);
        }

        void doLock() {
            _isLocked = true;
            locked?.Invoke(this);
        }
         
    }
}
