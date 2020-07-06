using System;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.NSubstitute.Tests
{
	public class UnderTest
	{
		private readonly ILogger _logger;

		public UnderTest(ILogger logger)
		{
			_logger = logger;
		}

		public void LogDebug()
		{
			_logger.LogDebug("Message");
		}
		public void LogTrace()
		{
			_logger.LogTrace("Message");
		}
		public void LogInformation()
		{
			_logger.LogInformation("Message");
		}
		public void LogWarning()
		{
			_logger.LogWarning("Message");
		}
		public void LogError()
		{
			_logger.LogError("Message");
		}

		public void LogCritical()
		{
			_logger.LogCritical("Message");
		}

		public void LogErrorWithArgs()
		{
			_logger.LogError("Message with {0}", "args");
		}

		public void LogErrorWithExceptionAndArgs()
		{
			_logger.LogError(new InvalidOperationException(), "Message with exception and {0}", "args");
		}

		public void LogErrorWithExceptionAndEventIdAndArgs()
		{
			_logger.LogError(new EventId(1, "event"), new InvalidOperationException(), "Message with exception and event id and {0}", "args");
		}

		public void LogWithLevel(LogLevel logLevel)
		{
			_logger.Log(logLevel, "Message with level {0}", logLevel);
		}
	}
}