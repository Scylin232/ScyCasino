using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Domain.Models;

public enum RoomType
{
    RouletteRoom
}

public sealed class Room(Guid id) : Entity(id)
{
    [StringLength(20, MinimumLength = 5)]
    public string Name { get; set; }
    
    [EnumDataType(typeof(RoomType))]
    public RoomType RoomType { get; set; }
    
    public HashSet<Guid> Players { get; init; } = new();
}