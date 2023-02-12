using System;

namespace Game.Core
{
    public interface IHaveHealth
    {
        public event Action HealthChanged;

        public float CurrentHealth { get; }
        public float MaxHealth { get; }
    }
}
