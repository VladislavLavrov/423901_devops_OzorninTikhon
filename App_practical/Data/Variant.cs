using System.ComponentModel.DataAnnotations;

namespace App_practical.Data
{
    public class Variant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Value1 { get; set; }
        public char Operation { get; set; }
        public double Value2 { get; set; }
        public double? Result { get; set; }
    }
}
