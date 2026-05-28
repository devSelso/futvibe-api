namespace Futvibe.Domain.Entities;

public class Banner
{
    public Guid Id { get; private set; }
    public string ImageUrl { get; private set; } = default!;
    public string? Alt { get; private set; }
    public string? Href { get; private set; }
    public bool IsActive { get; private set; }
    public int DisplayOrder { get; private set; }

    private Banner() { }

    public static Banner Create(string imageUrl, string? alt, string? href, int displayOrder)
    {
        return new Banner
        {
            Id = Guid.NewGuid(),
            ImageUrl = imageUrl,
            Alt = alt,
            Href = href,
            IsActive = true,
            DisplayOrder = displayOrder
        };
    }
}
