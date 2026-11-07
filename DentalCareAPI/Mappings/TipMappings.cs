using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class TipMappings
{
    public static TipResponse ToResponse(this Tip tip) =>
        new(tip.Id, tip.Title, tip.Content, tip.ImageUrl);
}
