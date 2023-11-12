namespace CardsLand_Web.Entities
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int Code { get; set; }
    }

    public class ApiResourceList<T>
    {
        public List<T> Results { get; set; }
    }
}
