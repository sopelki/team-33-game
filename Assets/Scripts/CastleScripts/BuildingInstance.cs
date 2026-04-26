namespace CastleScripts
{
    public class BuildingInstance
    {
        public BuildingData Data { get; private set; }
        public int Level { get; private set; } = 1;

        public BuildingInstance(BuildingData data)
        {
            Data = data;
        }

        public int Production => Data.baseProduction * Level;
        public int UpgradeCost => Data.baseCost * (Level + 1);

        public void Upgrade()
        {
            Level++;
        }
    }
}