using System.Collections;
using DG.Tweening;
using RocketDestruction.Core;
using RocketDestruction.SignalSystem;
using RocketDestruction.Ui.UiManager;
using RocketDestruction.Utility;
using Supyrb;
using UnityEngine;
using UnityEngine.UI;

namespace RocketDestruction.Ui.ViewControllers
{
    public class GameplayMenuViewController : UiMenu
    {
        [SerializeField] private Button     launchButton;
        [SerializeField] private Button     settingsButton;
        [SerializeField] private Slider     targetSlider;
        [SerializeField] private Slider     movingSlider;
        [SerializeField] private float      sliderTolerance;
        [SerializeField] private float      slideSpeed;
        [SerializeField] private float      slideSpeedMultiplier;
        [SerializeField] private GameObject sliders;
        [SerializeField] private GameObject targetSliderHandle;
        [SerializeField] private GameObject movingSliderHandle;

        private SliderDirection _sliderDirection;
        private bool            _launchInitiated;
        private float           _defaultSlideSpeed;

        private void Awake()
        {
            _defaultSlideSpeed = slideSpeed;
        }

        private void OnEnable()
        {
            launchButton.onClick.AddListener(OnLaunchButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);

            Signals.Get<RocketBarrageFinished>()
                .AddListener(OnRocketBarrageFinshed);

            Signals.Get<TargetRekt>()
                .AddListener(OnTargetRekt);

            SetSlideSpeed();
            SetRandomTargetSliderValue();
        }

        private void Update()
        {
            if (_launchInitiated) return;

            if (movingSlider.value >= movingSlider.maxValue || movingSlider.value <= movingSlider.minValue)
                _sliderDirection = _sliderDirection == SliderDirection.Forward
                    ? SliderDirection.Backward
                    : SliderDirection.Forward;

            if (_sliderDirection == SliderDirection.Forward)
            {
                movingSlider.value += Time.deltaTime * slideSpeed;
            }
            else
            {
                movingSlider.value -= Time.deltaTime * slideSpeed;
            }
        }

        private void OnDisable()
        {
            launchButton.onClick.RemoveListener(OnLaunchButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);

            Signals.Get<RocketBarrageFinished>()
                .RemoveListener(OnRocketBarrageFinshed);

            Signals.Get<TargetRekt>()
                .RemoveListener(OnTargetRekt);
        }

        private void OnTargetRekt(int rektTargetsCount)
        {
            GameMode gameMode = GameManager.Instance.GameMode;
            if (gameMode == GameMode.TripleBarrage || rektTargetsCount == AtConstants.MaxRocketsCount) return;

            slideSpeed       *= slideSpeedMultiplier;
            _launchInitiated =  false;
            SetRandomTargetSliderValue();
            sliders.SetActive(true);
            launchButton.gameObject.SetActive(true);
        }

        private void OnLaunchButtonClicked()
        {
            LaunchInitiated();
        }

        private void LaunchInitiated()
        {
            _launchInitiated = true;
            launchButton.gameObject.SetActive(false);
            StartCoroutine(CheckSliderAccuracy());
        }

        private IEnumerator CheckSliderAccuracy()
        {
            float currentValue = movingSlider.value;
            float targetValue  = targetSlider.value;

            if (currentValue <= targetValue + sliderTolerance && currentValue >= targetValue - sliderTolerance)
            {
                Signals.Get<RocketHitStatus>()
                    .Dispatch(true);

                targetSliderHandle.transform.DOPunchScale(Vector3.one * 1.1f, 0.5f, 2)
                    .SetEase(Ease.InOutQuad);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                movingSliderHandle.transform.DOShakePosition(1f, new Vector3(5f, 5f, 0));
                yield return new WaitForSeconds(1f);
                Signals.Get<RocketHitStatus>()
                    .Dispatch(false);
            }

            Signals.Get<LaunchRocket>()
                .Dispatch();
            sliders.SetActive(false);
        }

        private void SetRandomTargetSliderValue()
        {
            targetSlider.value = Random.Range(0.1f, 0.9f);
            movingSlider.value = 0f;
        }

        private void SetSlideSpeed()
        {
            GameMode gameMode = GameManager.Instance.GameMode;
            if (gameMode == GameMode.TripleBarrage)
            {
                slideSpeed = _defaultSlideSpeed * 3f;
            }
            else
            {
                slideSpeed = _defaultSlideSpeed;
            }
        }

        private void OnSettingsButtonClicked()
        {
            Signals.Get<ShowMenu>()
                .Dispatch(MenuType.SettingsMenu);
        }

        private void OnRocketBarrageFinshed()
        {
            launchButton.gameObject.SetActive(false);
        }

        public override void ResetMenu()
        {
            base.ResetMenu();
            SetSlideSpeed();
            SetRandomTargetSliderValue();
            launchButton.gameObject.SetActive(true);
            sliders.SetActive(true);
            _launchInitiated = false;
        }
    }

    public enum SliderDirection
    {
        Forward,
        Backward
    }
}