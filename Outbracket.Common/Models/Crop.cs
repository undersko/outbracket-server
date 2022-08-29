namespace Outbracket.Common.Models
{
    public class Crop
    {
        public decimal X { get; set; }
        
        public decimal Y { get; set; }
        
        public int Aspect { get; set; }

        public decimal Height { get; set; }
        
        public decimal Width { get; set; }

        public string Unit { get; set; }
        
        public int ImageHeight { get; set; }
        
        public int ImageWidth { get; set; }
    }
}