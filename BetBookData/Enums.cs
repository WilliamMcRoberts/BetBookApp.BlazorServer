
using NetEscapades.EnumGenerators;

namespace BetBookData;

[EnumExtensions]
public enum SeasonType
{
    PRE,
    REG,
    POST
}

[EnumExtensions]
public enum GameStatus
{
    NOT_STARTED,
    IN_PROGRESS,
    FINISHED
}

[EnumExtensions]
public enum BetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum PayoutStatus
{
    UNPAID,
    PAID,
    PARLEY
}

[EnumExtensions]
public enum ParleyBetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum ParleyPayoutStatus
{
    UNPAID,
    PAID
}
