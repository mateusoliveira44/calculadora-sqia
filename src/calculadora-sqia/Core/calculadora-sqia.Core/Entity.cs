namespace calculadora_sqia.Core
{
    public abstract class Entity
    {
        public int Id { get; private set; }

        protected Entity()
        {

        }

        protected Entity(int id)
        {
            Id = id;
        }
    }
}
