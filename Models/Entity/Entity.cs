namespace Models.Entity
{
    public class Entity
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public List<EntityField> Fields { get; set; }
        public List<Dictionary<string, string>> DefaultValues { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public short Status { get; set; }
    }
}
