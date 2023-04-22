using RocketDestruction.Core;
using RocketDestruction.Ui.UiManager;
using Supyrb;

namespace RocketDestruction.SignalSystem
{
    public class ShowMenu : Signal<MenuType> { }

    public class HideMenu : Signal<MenuType> { }

    public class RocketSpeedMultiplierChanged : Signal<int> { }

    public class LaunchRocket : Signal { }

    public class RocketHitStatus : Signal<bool> { }

    public class TargetRekt : Signal<int> { }

    public class RocketMissedTarget : Signal { }

    public class RocketHitTarget : Signal { }

    public class ResetLoop : Signal { };

    public class RocketBarrageFinished : Signal { }

    public class SetGameMode : Signal<GameMode> { }
}