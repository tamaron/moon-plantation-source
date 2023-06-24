namespace U1W.MoonPlant
{
    public class PlantRange
    {
        public (RangeType type, int x, int y) type;
    }

    public enum RangeType
    {
        Single,
        Xs,
        Ys
    }
}