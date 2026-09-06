using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PTrampert.QueryObjects.Integration.Test.Stores;

/// <summary>
/// Records the last command Entity Framework Core sent, so tests can assert on the SQL and the parameters that
/// actually reached the database rather than on a rendering of the query.
/// </summary>
public class CommandCapturingInterceptor : DbCommandInterceptor
{
    public CapturedCommand? LastCommand { get; private set; }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Capture(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        Capture(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private void Capture(DbCommand command)
    {
        LastCommand = new CapturedCommand(
            command.CommandText,
            command.Parameters
                .Cast<DbParameter>()
                .ToDictionary(p => p.ParameterName, p => p.Value == DBNull.Value ? null : p.Value));
    }
}
