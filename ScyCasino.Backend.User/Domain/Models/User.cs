using SharedKernel.Domain;

namespace Domain.Models;

public sealed class User(Guid id) : Entity(id)
{
    public int Coins { get; set; }
    public string Nickname { get; set; }
}