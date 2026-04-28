namespace Logic.Castle
{
    public class BuildingInstance
    {
        public BuildingData Data { get; private set; }
        public int Level { get; private set; }

        public BuildingInstance(BuildingData data)
        {
            Data = data;
            Level = 1;
        }

        // TODO: поменять Production на что-то сложнее
        public int Production => Data.baseProduction * Level;
        public int GetUpgradeCost() => Data.baseCost * (Level + 1);
        public void Upgrade() => Level++;
    }
}