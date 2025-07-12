using System;

[Serializable]
public class Conflict
{
    public Queen OtherQueen { get; }
    public ConflictType Type;

    public Conflict(Queen otherQueen, ConflictType type)
    {
        OtherQueen = otherQueen;
        Type = type;
    }
}


[Serializable]
public enum ConflictType
{
    Color,
    Row,
    Column,
    Diagonal,
}