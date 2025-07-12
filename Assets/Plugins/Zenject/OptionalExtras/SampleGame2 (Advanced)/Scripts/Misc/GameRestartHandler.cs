using System;
using Plugins.Zenject.OptionalExtras.SampleGame2__Advanced_.Scripts.Enemy;
using Plugins.Zenject.OptionalExtras.Signals.Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Plugins.Zenject.OptionalExtras.SampleGame2__Advanced_.Scripts.Misc
{
    public class GameRestartHandler : IInitializable, IDisposable, ITickable
    {
        readonly SignalBus _signalBus;
        readonly Settings _settings;

        bool _isDelaying;
        float _delayStartTime;

        public GameRestartHandler(
            Settings settings,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _settings = settings;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        public void Tick()
        {
            if (_isDelaying)
            {
                if (Time.realtimeSinceStartup - _delayStartTime > _settings.RestartDelay)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        void OnPlayerDied()
        {
            // Wait a bit before restarting the scene
            _delayStartTime = Time.realtimeSinceStartup;
            _isDelaying = true;
        }

        [Serializable]
        public class Settings
        {
            public float RestartDelay;
        }
    }
}
