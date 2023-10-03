using System.ComponentModel.DataAnnotations;

namespace AuditMVCSection4.Models
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
