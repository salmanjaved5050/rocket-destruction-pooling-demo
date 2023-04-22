using RocketDestruction.SignalSystem;
using RocketDestruction.Utility;
using Supyrb;

namespace RocketDestruction.Core
{
    public class GameManager : AtSingletonNonPersistent<GameManager>
    {
        private IRdManager[] _managers;

        public GameMode GameMode { get; private set; }

        private void Awake()
        {
            _managers = GetComponentsInChildren<IRdManager>();
            InitializeGame();
        }

        private void OnEnable()
        {
            Signals.Get<ResetLoop>()
                .AddListener(OnTargetsDestroyed);

            Signals.Get<SetGameMode>()
                .AddListener(OnSetGameMode);
        }

        private void OnDisable()
        {
            Signals.Get<ResetLoop>()
                .RemoveListener(OnTargetsDestroyed);

            Signals.Get<SetGameMode>()
                .RemoveListener(OnSetGameMode);
        }

        private void InitializeGame()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i]
                    .Init();
            }
        }

        private void OnResetLoop()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i]
                    .Reset();
            }
        }

        private void OnTargetsDestroyed()
        {
            Invoke(nameof(OnResetLoop), AtConstants.ResetLoopDelay);
        }

        private void OnSetGameMode(GameMode gameMode)
        {
            Instance.GameMode = gameMode;
        }
    }
}