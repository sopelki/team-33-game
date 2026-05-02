using Interfaces;

namespace Logic.Castle
{
    public class CastleSystem: ITickable
    {
        private readonly CastleModel model;

        public CastleSystem(CastleModel model)
        {
            this.model = model;
        }

        public void Tick()
        {
            ProduceResources();
        }

        private void ProduceResources()
        {
            var changed = false;
            foreach (var building in model.Buildings)
            {
                if (building.Data.type == BuildingType.Farm)
                {
                    model.Food += building.Production;
                    changed = true;
                }
                // TODO: Добавить логику для других BuildingType
            }

            if (changed)
                model.Changed();
        }

        public bool TryBuyBuilding(BuildingData data)
        {
            if (!TrySpendGold(data.baseCost))
                return false;

            var instance = new BuildingModel(data);
            model.Buildings.Add(instance);

            return true;
        }
        
        public bool TrySpendGold(int price)
        {
            if (model.Gold < price)
                return false;

            model.Gold -= price;
            model.Changed();
            return true;
        }
    }
}