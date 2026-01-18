namespace Models.Entity
{
    public class Entity
    {
        public string Name { get; set; }
        public List<EntityField> Fields { get; set; }
        public List<Dictionary<string, string>> DefaultValues { get; set; }
    }
}
