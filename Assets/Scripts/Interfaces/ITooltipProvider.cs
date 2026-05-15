namespace Interfaces
{
    public struct TooltipContent
    {
        public string Title;
        public string Description;
        public string Cost;
        public string SpecialInfo; // "Дает +5 золота" или "Урон: 10"
    }

    public interface ITooltipProvider
    {
        TooltipContent GetTooltipContent();
    }
}