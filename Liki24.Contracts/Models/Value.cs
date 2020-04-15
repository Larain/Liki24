namespace Liki24.Contracts.Models
{
    public class Value
    {
        public Value(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name + Id;
        }
    }
}