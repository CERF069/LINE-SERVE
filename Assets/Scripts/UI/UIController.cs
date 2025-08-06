using System.Collections;
using UnityEngine;
using TMPro;
using Service;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _panelMenu;
        [SerializeField] private GameObject _panelPause;
        [SerializeField] private GameObject _panelResult;

        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _coinRewardText;
        [SerializeField] private TextMeshProUGUI _coinText;
        
        [SerializeField] private GameObject _pauseButton;
        public GameObject PauseButton => _pauseButton;
        
        [SerializeField] private TextMeshProUGUI _resultText;
        
        public TextMeshProUGUI ResultText => _resultText;
        
        public TextMeshProUGUI CoinRewardText => _coinRewardText;
        public TextMeshProUGUI CoinText => _coinText;

        private ITimerService _timerService;

        [Inject]
        public void Initialize(ITimerService timerService)
        {
            _timerService = timerService;
        }
        private void Update()
        {
            UpdateTimer();
            UpdateCoinBalance();
        }

        public void ShowMenu(float delay = 0f) => ShowPanelWithDelay(_panelMenu, delay);
        public void HideMenu() => SetActive(_panelMenu, false);

        public void ShowPause(float delay = 0f) => ShowPanelWithDelay(_panelPause, delay);
        public void HidePause() => SetActive(_panelPause, false);

        public void ShowResult(float delay = 0f) => ShowPanelWithDelay(_panelResult, delay);
        public void HideResult() => SetActive(_panelResult, false);

        public void HideAllUI()
        {
            SetActive(_panelMenu, false);
            SetActive(_panelPause, false);
            SetActive(_panelResult, false);
        }

        public void UpdateText(TextMeshProUGUI text, int value)
        {
            if (text != null)
                text.text = value.ToString();
        }
        
        public void UpdateText(TextMeshProUGUI text, string value)
        {
            if (text != null)
                text.text = value;
        }

        public void UpdateCoinReward(int reward)
        {
            UpdateText(_coinRewardText, reward);
        }

        private void UpdateTimer()
        {
            if (_timerService == null) return;

            float time = _timerService.GetTime();
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void UpdateCoinBalance()
        {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            UpdateText(_coinText, coins);
        }

        private void ShowPanelWithDelay(GameObject panel, float delay)
        {
            if (delay > 0f)
                StartCoroutine(ShowPanelCoroutine(panel, delay));
            else
                SetActive(panel, true);
        }

        private IEnumerator ShowPanelCoroutine(GameObject panel, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            SetActive(panel, true);
        }

        public void SetActive(GameObject panel, bool active)
        {
            if (panel != null && panel.activeSelf != active)
                panel.SetActive(active);
        }
    }
}
