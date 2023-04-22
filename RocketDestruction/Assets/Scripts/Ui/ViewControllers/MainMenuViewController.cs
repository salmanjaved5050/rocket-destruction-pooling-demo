using RocketDestruction.SignalSystem;
using RocketDestruction.Ui.UiManager;
using Supyrb;
using UnityEngine;
using UnityEngine.UI;

namespace RocketDestruction.Ui.ViewControllers
{
    public class MainMenuViewController : UiMenu
    {
        [SerializeField] private Button playButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            Signals.Get<ShowMenu>()
                .Dispatch(MenuType.GameplayMenu);
        }
    }
}