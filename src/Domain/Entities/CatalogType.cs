namespace CleanArchitecture.Domain.Entities;

public class CatalogType : BaseAuditableEntity
{
    public string Name { get;  set; } = string.Empty;
    public string Description { get;  set; } = string.Empty;
    //public CatalogType(string name, string description)
    //{
    //    Name = name;
    //    Description = description;
    //}
}
