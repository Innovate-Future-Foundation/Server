public class QueryProfilesRequest
{
    public QueryProfileFilters? Filters { get; set; }
    public string? OrderBy { get; set; }
    public bool? IsAscending { get; set; }
    
    // Offset-based pagination
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}

public class QueryProfileFilters
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public Guid? OrgId { get; set; }
    public Guid? RoleId { get; set; }
    public bool? IsActive { get; set; }
}
