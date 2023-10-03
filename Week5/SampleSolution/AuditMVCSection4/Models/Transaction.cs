namespace AuditMVCSection4.Models
{
    public class Transaction : AuditableEntity
    {
        public int Id { get; set; }

        public decimal Total { get; set; }


    }
}
