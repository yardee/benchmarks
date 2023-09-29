using System;

namespace SplitBrainPrimaryKey.Db
{
    [Flags]
    public enum CompanyWorkIndicatorFlag : uint
    {
        Dirty = 0b1,
        ActionQueued = 0b10, //this is for tracing purpose, to be able to design actions more efficient
    }
}
