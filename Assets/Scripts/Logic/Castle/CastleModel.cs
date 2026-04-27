using System;
using System.Collections.Generic;

namespace Logic.Castle
{
    public class CastleModel
    {
        public int Hp { get; set; }
        public int Gold { get; set; }
        public int Food { get; set; }
        
        public List<BuildingInstance> Buildings { get; private set; } = new();

        // Событие теперь передает саму модель, чтобы UI знал, что обновилось
        public event Action OnChanged;
        
        public CastleModel(int initialHp, int initialGold, int initialFood)
        {
            Hp = initialHp;
            Gold = initialGold;
            Food = initialFood;
        }

        public void Changed() => OnChanged?.Invoke();
    }
}