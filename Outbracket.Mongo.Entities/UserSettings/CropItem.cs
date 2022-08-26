namespace Outbracket.Mongo.Entities.UserSettings
{
    public class CropItem
    {
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Aspect { get; set; }

        public int Height { get; set; }
        
        public int Width { get; set; }

        public string Unit { get; set; }
    }
}