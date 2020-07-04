# Extensions.Logging.NSubstitute
Provides NSubstitute for `Microsoft.Extenstions.Logging.ILogger` that can be use with `.Received()`

## Usage
Instead of `Substitute.For<ILogger>()`, use `LoggerSubstitute.Instance`. This returns a Substitute which can be used in the normal NSubstitute way, for example:

```c#
var logger = LoggerSubstitute.Instance;
SomeMethodThatLogsAnError(logger);
logger.Received().LogError("some message");
```

or more complex cases 
```c#
logger.Received().Log(Arg.Is<LogLevel>(level => level > LogLevel.Warning), Arg.Is<string>(s => s.Contains("expected message content"));

logger.DidNotReceive().LogError(Arg.Any<string>());
```

## Limitations
`DidNotReceiveWithAnyArgs()`can't be used with the `Log`_Level_ (`LogError`, `LogWarning`, etc.) methods. Instead, either use `DidNotReceive()` with `Arg.Any<>()` or use `DidNotReceiveWithAnyArgs().Log(`

The message is presented already formatted, so checks are made against the formatted output message, not against the parameters. So if your code under test is:
```c#
_logger.LogError("Error code: {0}", 5);
```
this would be checked with
```c#
loggerSubstitute.Received().LogError("Error code: 5");
```
