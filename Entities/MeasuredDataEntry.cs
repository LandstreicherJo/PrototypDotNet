namespace PrototypDotNet.Entities
{
    public class MeasuredDataEntry
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public float? Temperature { get; set; }
        public int? Humidity { get; set; }
    }
}
