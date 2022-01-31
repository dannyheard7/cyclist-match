using System;

namespace Persistence.SQL.Filters;

internal class InvalidFilterException : InvalidOperationException
{
    public InvalidFilterException() : base("Cannot apply filter")
    {
    }
}