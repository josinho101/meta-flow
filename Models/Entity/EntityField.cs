namespace Models.Entity
{
    public class EntityField
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public bool Nullable { get; set; } = true;
        public bool Unique { get; set; } = false;
        public int Length { get; set; }
        public PrimaryKey? PrimaryKey { get; set; }
        public ForeignKey? ForeignKey { get; set; }
        public string DefaultValue { get; set; }
    }
}
