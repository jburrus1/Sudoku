using System;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Board
{
    public class Timer : MonoBehaviour
    {
        private float _elapsedTime;

        private TextMeshProUGUI _text;

        private bool _initialized;
        private bool _adDone;

        public float ElapsedTime => _elapsedTime;

        private void Awake()
        {
            _initialized = false;
            _text = GetComponent<TextMeshProUGUI>();
            _text.enabled = false;
        }

        public void Initialize()
        {
            _elapsedTime = 0f;
            _initialized = true;
            _text.enabled = true;
        }

        private void Update()
        {
            if (_initialized &&  _adDone)
            {
                _elapsedTime += Time.deltaTime;
                UpdateText();
            }
        }

        public void Hide()
        {
            _text.enabled = false;
        }

        private void UpdateText()
        {
            var ts = TimeSpan.FromSeconds(_elapsedTime);
            _text.text = $"{ts.TotalMinutes:00}:{ts.Seconds:00}";
        }

        public void AdDone()
        {
            _adDone = true;
        }
    }
}