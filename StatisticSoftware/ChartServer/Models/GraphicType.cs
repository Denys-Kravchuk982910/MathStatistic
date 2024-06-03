namespace ChartServer.Models
{
    public class GraphicType
    {
        public double Middle { get; set; }
        public double Count { get; set; }
    }

    public class GraphicEnvelope
    {
        public List<GraphicType> graphics { get; set; }
    }
}
