using System;

namespace Plugins.Zenject.OptionalExtras.SampleGame2__Advanced_.Scripts.Enemy
{
    // These values are given as parameters into dynamically created
    // EnemyFacade instances
    [Serializable]
    public class EnemyTunables
    {
        public float Accuracy;
        public float Speed;
    }
}
