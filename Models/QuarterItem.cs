namespace GCBQuotationSystem.Models
{
    public class QuarterItem
    {
        public string Text { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj is QuarterItem other)
            {
                return StartDate.Equals(other.StartDate);
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return StartDate.GetHashCode();
        }
    }
} 