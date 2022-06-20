namespace BetBookDataLogic;

/// <summary>
/// Enum for the type of season for a GameModel
/// </summary>
public enum SeasonType
{
    PRE,
    REG,
    POST
}

/// <summary>
/// Enum for the status of a GameModel
/// </summary>
public enum GameStatus
{
    NOT_STARTED,
    IN_PROGRESS,
    FINISHED
}

/// <summary>
/// Enum for the status of a BetModel
/// </summary>
public enum BetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER
}

/// <summary>
/// Enum for the status of the payout of a BetModel
/// </summary>
public enum PayoutStatus
{
    UNPAID,
    PAID
}
