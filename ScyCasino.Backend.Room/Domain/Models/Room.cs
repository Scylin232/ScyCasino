﻿using System.ComponentModel.DataAnnotations;
using Shared.Domain.Models.Room;
using Shared.Kernel.Domain;

namespace Domain.Models;

public sealed class Room(Guid id) : Entity(id)
{
    [StringLength(20, MinimumLength = 5)]
    public string Name { get; set; }
    [EnumDataType(typeof(RoomType))]
    public RoomType RoomType { get; set; }
    
    public Dictionary<string, Guid> PlayerConnections { get; init; } = new();
}