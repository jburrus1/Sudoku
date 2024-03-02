using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Board
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _boardTransform;

        [SerializeField] private float _animateDuration;

        private Image _image;
        private GameObject _visual;

        [SerializeField]
        private Timer _timer;

        private TextMeshProUGUI _timeText;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _visual = transform.Find("VictoryVisual").gameObject;

            _timeText = _visual.transform.Find("Time").GetComponent<TextMeshProUGUI>();

                     _image.enabled = false;
            _visual.SetActive(false);
        }

        public void NewGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        public void ChangeDifficulty()
        {
            GameManager.GoToDifficultySelect();
        }

        public void ReturnToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        
        

        public IEnumerator AnimateVictory(float elapsedTime)
        {
            var start = _boardTransform.anchoredPosition;
            var final = new Vector2(0, 584);
            
            var ts = TimeSpan.FromSeconds(elapsedTime);
            _timer.Hide();
            _timeText.text = $"Total time -- {ts.TotalMinutes:00}:{ts.Seconds:00}";
            
            GameManager.UpdateStats(elapsedTime);
            
            
            float timeElapsed = 0;

            while (timeElapsed < _animateDuration)
            {
                _boardTransform.anchoredPosition = Vector2.Lerp(start, final, timeElapsed / _animateDuration);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            _boardTransform.anchoredPosition = final;

            _image.enabled = true;
            _visual.SetActive(true);

        }
    }
}