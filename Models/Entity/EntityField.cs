namespace Models.Entity
{
    public class EntityField
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public bool NotNull { get; set; }
        public PrimaryKey? PrimaryKey { get; set; }
        public ForeignKey? ForeignKey { get; set; }
    }
}
