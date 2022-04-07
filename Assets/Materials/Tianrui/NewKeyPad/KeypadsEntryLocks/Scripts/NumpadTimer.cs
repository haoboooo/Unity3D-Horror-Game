using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BigBlit.ActivePack;

namespace BigBlit.Keypads
{
    public class NumpadTimer : MonoBehaviour
    {

        public enum TimeSource { Delta, System };

        [SerializeField] private string _initialTime;
        [SerializeField] private string _dateFormat = "HH:mm:ss";

        [SerializeField] private TimeSource _timeSource;
        [SerializeField] private NumpadText _timerText;
        [SerializeField] private float _updateInterval;

        [NonSerialized] private float _deltaTime = 0.0f;

        public float DeltaTime {
            get => _deltaTime;
            set => _deltaTime = value;
        }

        private NumpadText _curTimerText;
        NumpadText CurTimerText {
            get {
                if (_timerText != null)
                    return _timerText;
                if (_curTimerText == null)
                    _curTimerText = GetComponent<NumpadText>();
                return _curTimerText;
            }
        }
        float _totDeltaTime = 0.0f;

        private void OnValidate() {
            updateTime();
        }

        // Start is called before the first frame update
        void Start() {

            updateTime();
        }

        // Update is called once per frame
        void Update() {

            _totDeltaTime += Time.deltaTime;
            if (_totDeltaTime >= _updateInterval) {

                updateTime();
                _totDeltaTime = 0.0f;
            }
        }

        private void updateTime() {
            if (_timeSource == TimeSource.System)
                updateSystemTime();
            else
                updateDeltaTime();

        }

        private void updateSystemTime() {
            DateTime date = DateTime.Now;
            CurTimerText.Text = date.ToString(_dateFormat);
        }

        private void updateDeltaTime() {
            _deltaTime += _totDeltaTime;
            if (DateTime.TryParse(_initialTime, out var dateTime)) {
                dateTime = dateTime.AddSeconds(_deltaTime);
                CurTimerText.Text = dateTime.ToString(_dateFormat);
            }
            else {
                CurTimerText.Text = "INV DATE";
            }



        }
    }
}
