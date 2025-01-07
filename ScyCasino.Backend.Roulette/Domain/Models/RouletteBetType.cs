namespace Domain.Models;

public enum RouletteBetType
{
    // Inside:
    Straight,
    Split,
    Street,
    Corner,
    SixLine,
    
    // Outside:
    Column,
    Dozen,
    Even,
    Odd,
    Range,
    Red,
    Black
}