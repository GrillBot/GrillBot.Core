using GrillBot.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace GrillBot.Core.Services.UserMeasures.Models.Measures;

public class DeleteMeasuresRequest : IValidatableObject, IDictionaryObject
{
    public Guid? Id { get; set; }
    public long? ExternalId { get; set; }

    public DeleteMeasuresRequest()
    {
    }

    public DeleteMeasuresRequest(Guid? id, long? externalId)
    {
        Id = id;
        ExternalId = externalId;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Id is null && ExternalId is null)
            yield return new ValidationResult("Missing measure ID", new[] { nameof(Id), nameof(ExternalId) });
    }

    public Dictionary<string, string?> ToDictionary()
    {
        return new Dictionary<string, string?>
        {
            { nameof(Id), Id?.ToString() },
            { nameof(ExternalId), ExternalId?.ToString() }
        };
    }
}
