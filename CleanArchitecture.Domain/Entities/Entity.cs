namespace CleanArchitecture.Domain.Entities
{
    public abstract class Entity
    {
        public int Id { get; private set; }
        public Guid Guid { get; private set; }
        public Guid CreatedBy { get; private set; }
        public Guid UpdatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set;} = DateTime.UtcNow;

        public Entity(Guid guid)
        {
            Guid = guid;
        }
        public Entity()
        {
            
        }
    }
}
