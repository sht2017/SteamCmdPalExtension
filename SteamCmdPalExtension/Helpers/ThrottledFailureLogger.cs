using System;
using System.Collections.Generic;

namespace SteamCmdPalExtension.Helpers;

/// <summary>
/// Limits log messages for an operation that may fail repeatedly.
/// Within each interval, every distinct failure is logged once (up to a limit) and recovery is
/// logged once; anything else is counted and summarized in the next logged message.
/// </summary>
/// <remarks>Not thread-safe; intended to be used by a single sequential loop.</remarks>
internal sealed class ThrottledFailureLogger
{
    private readonly string _operationName;
    private readonly TimeSpan _interval;
    private readonly int _maxFailuresPerInterval;
    private readonly Action<string> _log;
    private readonly TimeProvider _timeProvider;
    private readonly HashSet<string> _failuresLoggedInInterval = new(StringComparer.Ordinal);

    private long? _intervalStart;
    private bool _recoveryLoggedInInterval;
    private bool _recoveryPending;
    private int _unloggedFailures;

    public ThrottledFailureLogger(
        string operationName,
        TimeSpan interval,
        int maxFailuresPerInterval,
        Action<string> log,
        TimeProvider? timeProvider = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(operationName);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(interval, TimeSpan.Zero);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxFailuresPerInterval, 1);
        ArgumentNullException.ThrowIfNull(log);

        _operationName = operationName;
        _interval = interval;
        _maxFailuresPerInterval = maxFailuresPerInterval;
        _log = log;
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public void ReportFailure(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        _recoveryPending = true;
        StartNewIntervalIfElapsed();

        string failure = $"{exception.GetType().Name}: {exception.Message}";
        if (_failuresLoggedInInterval.Count < _maxFailuresPerInterval &&
            _failuresLoggedInInterval.Add(failure))
        {
            Log($"{_operationName} failed: {failure}");
        }
        else
        {
            _unloggedFailures++;
        }
    }

    public void ReportSuccess()
    {
        if (!_recoveryPending)
        {
            return;
        }

        StartNewIntervalIfElapsed();
        if (_recoveryLoggedInInterval)
        {
            // Rapidly alternating failures and successes; report the recovery in a later interval.
            return;
        }

        _recoveryPending = false;
        _recoveryLoggedInInterval = true;
        Log($"{_operationName} recovered");
    }

    private void Log(string message)
    {
        if (_unloggedFailures > 0)
        {
            string noun = _unloggedFailures == 1 ? "failure" : "failures";
            message += $" ({_unloggedFailures} {noun} not logged since the last report)";
            _unloggedFailures = 0;
        }

        _log(message);
    }

    private void StartNewIntervalIfElapsed()
    {
        if (_intervalStart is { } start && _timeProvider.GetElapsedTime(start) < _interval)
        {
            return;
        }

        _intervalStart = _timeProvider.GetTimestamp();
        _failuresLoggedInInterval.Clear();
        _recoveryLoggedInInterval = false;
    }
}
