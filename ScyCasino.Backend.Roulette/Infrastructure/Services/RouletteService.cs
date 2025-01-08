using Domain.Models;
using Domain.Services;

namespace Infrastructure.Services;

// Although this service is quite functional, personally, I
// would take a more declarative approach because:
//
// A) It takes up less space in the database  
// B) It’s more reliable  
// C) It’s simpler to maintain  
//
// Although this might not be the most elegant solution, where we would have to declare
// about a hundred ENUM values, it is definitely better than writing validators
// this way.

public class RouletteService : IRouletteService
{
    private static readonly HashSet<int> RedNumbers =
        [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
    
    private static readonly HashSet<int> BlackNumbers =
        [2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35];
    
    private static readonly Dictionary<RouletteBetType, decimal> Odds = new()
    {
        { RouletteBetType.Straight, 35.0m },
        { RouletteBetType.Split, 17.0m },
        { RouletteBetType.Street, 11.0m },
        { RouletteBetType.Corner, 8.0m },
        { RouletteBetType.SixLine, 5.0m },
        { RouletteBetType.Column, 2.0m },
        { RouletteBetType.Dozen, 2.0m },
        { RouletteBetType.Even, 1.0m },
        { RouletteBetType.Odd, 1.0m },
        { RouletteBetType.Range, 1.0m },
        { RouletteBetType.Red, 1.0m },
        { RouletteBetType.Black, 1.0m }
    };
    
    private static readonly Dictionary<RouletteBetType, Func<int[], bool>> BetValidators = new()
    {
        { RouletteBetType.Straight, IsStraight },
        { RouletteBetType.Split, IsSplit },
        { RouletteBetType.Street, IsStreet },
        { RouletteBetType.Corner, IsCorner },
        { RouletteBetType.SixLine, IsSixLine },
        { RouletteBetType.Column, IsColumn },
        { RouletteBetType.Dozen, IsDozen },
        { RouletteBetType.Even, IsEven },
        { RouletteBetType.Odd, IsOdd },
        { RouletteBetType.Range, IsRange },
        { RouletteBetType.Red, IsRed },
        { RouletteBetType.Black, IsBlack }
    };
    
    public decimal GetBetMultiplier(RouletteBetType betType)
    {
        return Odds.GetValueOrDefault(betType, 0.0m);
    }
    
    public bool ValidateBet(RouletteBetType betType, int[] values)
    {
        return BetValidators.TryGetValue(betType, out var validator) && validator(values);
    }
    
    private static bool IsStraight(int[] values)
    {
        return values.Length == 1 && (IsValidValues(values) || values[0] == 0);
    }
    
    private static bool IsSplit(int[] values)
    {
        return values.Length == 2 && IsValidValues(values) && IsConsecutiveSequence(values);
    }
    
    private static bool IsStreet(int[] values)
    {
        return values.Length == 3 && IsValidValues(values) && IsConsecutiveSequence(values);
    }
    
    private static bool IsSixLine(int[] values)
    {
        return values.Length == 6 && IsValidValues(values) && IsConsecutiveSequence(values);
    }
    
    private static bool IsCorner(int[] values)
    {
        if (values.Length != 4 || !IsValidValues(values)) return false;
        
        int first = values.Min();
        int second = first + 1;
        int third = first + 3;
        int fourth = first + 4;
        
        return values[0] == first && values[1] == second && values[2] == third && values[3] == fourth;
    }
    
    private static bool IsColumn(int[] values)
    {
        if (values.Length != 12 || !IsValidValues(values)) return false;
        
        return values.All(value => value % 3 == values[0] % 3);
    }
    
    private static bool IsDozen(int[] values)
    {
        if (values.Length != 12 || !IsValidValues(values)) return false;
        
        int first = values.Min();
        
        return first is >= 1 and <= 12 || first is >= 13 and <= 24 || first is >= 25 and <= 36;
    }
    
    private static bool IsRed(int[] values)
    {
        if (values.Length != RedNumbers.Count || !IsValidValues(values)) return false;
        
        return values.All(value => RedNumbers.Contains(value));
    }
    
    private static bool IsBlack(int[] values)
    {
        if (values.Length != BlackNumbers.Count || !IsValidValues(values)) return false;
        
        return values.All(value => BlackNumbers.Contains(value));
    }
    
    private static bool IsOdd(int[] values)
    {
        if (values.Length != 18 || !IsValidValues(values)) return false;
        
        return values.All(value => value % 2 != 0);
    }
    
    private static bool IsEven(int[] values)
    {
        if (values.Length != 18 || !IsValidValues(values)) return false;
        
        return values.All(value => value % 2 == 0);
    }
    
    private static bool IsRange(int[] values)
    {
        if (values.Length != 18 || !IsValidValues(values)) return false;
        
        return values[0] switch
        {
            1 => IsConsecutiveSequence(values),
            19 => IsConsecutiveSequence(values),
            _ => false
        };
    }
    
    private static bool IsValidValues(int[] values)
    {
        return values.All(value => RedNumbers.Contains(value) || BlackNumbers.Contains(value));
    }
    
    private static bool IsConsecutiveSequence(int[] values)
    {
        for (int i = 1; i < values.Length; i++)
            if (values[i] != values[i - 1] + 1)
                return false;
        
        return true;
    }
}