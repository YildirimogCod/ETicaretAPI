namespace ETicaretAPI.Application.RequestParam
{
    public record Pagination
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
