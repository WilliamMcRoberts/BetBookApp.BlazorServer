
namespace BetBookData;


public enum SeasonType
{
    PRE,
    REG,
    POST
}

public enum GameStatus
{
    NOT_STARTED,
    IN_PROGRESS,
    FINISHED
}

public enum BetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

public enum PayoutStatus
{
    UNPAID,
    PAID,
    PARLEY
}

public enum ParleyBetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

public enum ParleyPayoutStatus
{
    UNPAID,
    PAID
}
