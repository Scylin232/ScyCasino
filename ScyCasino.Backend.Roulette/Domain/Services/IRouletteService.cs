using Domain.Models;

namespace Domain.Services;

public interface IRouletteService
{
    decimal GetBetMultiplier(RouletteBetType betType);
    bool ValidateBet(RouletteBetType betType, int[] values);
}