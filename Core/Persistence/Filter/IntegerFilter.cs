using System;

namespace Persistence.Filter;

public class IntegerFilter
{
    public IntegerFilter(int equalTo)
    {
        EqualTo = equalTo;
    }

    public IntegerFilter(int greaterThan, int lessThan)
    {
        if (lessThan < greaterThan)
        {
            throw new ArgumentException("Greater than cannot be less than equal to");
        }

        GreaterThan = greaterThan;
        LessThan = lessThan;
    }

    public int? GreaterThan { get; }

    public int? LessThan { get; }

    public int? EqualTo { get; }
}