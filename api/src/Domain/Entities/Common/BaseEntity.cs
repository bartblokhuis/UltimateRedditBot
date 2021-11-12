namespace Domain.Entities.Common
{
    public abstract class BaseEntity : IEntity
    {
        #region Properties

        public Guid Id { get; set; } = Guid.NewGuid();

        #endregion
    }
}
