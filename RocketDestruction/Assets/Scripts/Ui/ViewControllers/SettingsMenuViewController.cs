using RocketDestruction.Core;
using RocketDestruction.SignalSystem;
using RocketDestruction.Ui.UiManager;
using RocketDestruction.Utility;
using Supyrb;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RocketDestruction.Ui.ViewControllers
{
    public class SettingsMenuViewController : UiMenu
    {
        [SerializeField] private Slider   rocketSpeedSlider;
        [SerializeField] private TMP_Text sliderSpeedValueText;
        [SerializeField] private Button   closeButton;
        [SerializeField] private Toggle   singleTargetModeToggle;
        [SerializeField] private Toggle   tripelBarrageModeToggle;


        private void OnEnable()
        {
            rocketSpeedSlider.onValueChanged.AddListener(OnRocketSliderValueChanged);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            singleTargetModeToggle.onValueChanged.AddListener(OnSingleTargetToggleValueChanged);
            tripelBarrageModeToggle.onValueChanged.AddListener(OnTripleBarrageToggleValueChanged);
            
            SetSpeedMultiplier();
        }

        private void OnDisable()
        {
            rocketSpeedSlider.onValueChanged.RemoveListener(OnRocketSliderValueChanged);
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            
            singleTargetModeToggle.onValueChanged.RemoveListener(OnSingleTargetToggleValueChanged);
            tripelBarrageModeToggle.onValueChanged.RemoveListener(OnTripleBarrageToggleValueChanged);
        }

        private void SetSpeedMultiplier()
        {
            int speedMultiplierValue = (int)PlayerPrefs.GetFloat(AtConstants.RocketSpeedMultiplier, 1f);
            rocketSpeedSlider.value   = speedMultiplierValue;
            sliderSpeedValueText.text = speedMultiplierValue + "x";
        }

        private void OnCloseButtonClicked()
        {
            Signals.Get<SignalSystem.ShowMenu>()
                .Dispatch(MenuType.GameplayMenu);
        }

        private void OnRocketSliderValueChanged(float sliderValue)
        {
            int value = (int)sliderValue;
            sliderSpeedValueText.text = value + "x";
            PlayerPrefs.SetFloat(AtConstants.RocketSpeedMultiplier, value);
            Signals.Get<SignalSystem.RocketSpeedMultiplierChanged>()
                .Dispatch(value);
        }

        private void OnSingleTargetToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                Signals.Get<SetGameMode>().Dispatch(GameMode.SingleTarget);
            }
        }

        private void OnTripleBarrageToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                Signals.Get<SetGameMode>().Dispatch(GameMode.TripleBarrage);
            }
        }
    }
}