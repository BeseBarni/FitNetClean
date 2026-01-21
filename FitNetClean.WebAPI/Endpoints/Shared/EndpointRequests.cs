namespace FitNetClean.WebAPI.Endpoints.Shared;

public class IdRequest
{
    public long Id { get; set; }
}

public class ListRequest
{
    public bool IncludeDeleted { get; set; } = false;
}
