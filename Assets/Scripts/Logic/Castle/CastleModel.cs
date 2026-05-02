using System;
using System.Collections.Generic;

namespace Logic.Castle
{
    public class CastleModel
    {
        public int Hp { get; set; }
        public int Gold { get; set; }
        public int Food { get; set; }
        
        public List<BuildingModel> Buildings { get; private set; } = new();

        public event Action OnChanged;
        public void Changed() => OnChanged?.Invoke();
        
        public CastleModel(int initialHp, int initialGold, int initialFood)
        {
            Hp = initialHp;
            Gold = initialGold;
            Food = initialFood;
        }
    }
}