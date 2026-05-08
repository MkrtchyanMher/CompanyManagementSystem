namespace Company.Application.DTO.Company
{
    public record Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Website { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
