using System;
using Extensions.Logging.NSubstitute;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Extensions.Logging.NSubstitute.Tests
{
	public class LoggerSubstituteTests
	{
		[Test]
		public void TestLogLevelMethods()
		{
			var logger = LoggerSubstitute.Create();

			var underTest = new UnderTest(logger);

			underTest.LogDebug();
			logger.Received().LogDebug("Message");

			underTest.LogTrace();
			logger.Received().LogTrace("Message");

			underTest.LogInformation();
			logger.Received().LogInformation("Message");

			underTest.LogWarning();
			logger.Received().LogWarning("Message");

			underTest.LogError();
			logger.Received().LogError("Message");

			underTest.LogCritical();
			logger.Received().LogCritical("Message");
		}

		[Test]
		public void TestLogLevelMethodsExclusive()
		{
			var logger = LoggerSubstitute.Create();

			var underTest = new UnderTest(logger);

			underTest.LogDebug();
			logger.Received().LogDebug("Message");
			logger.DidNotReceive().LogTrace(Arg.Any<string>());
			logger.DidNotReceive().LogInformation(Arg.Any<string>());
			logger.DidNotReceive().LogWarning(Arg.Any<string>());
			logger.DidNotReceive().LogError(Arg.Any<string>());
			logger.DidNotReceive().LogCritical(Arg.Any<string>());
		}

		[Test]
		public void TestDidNotReceive()
		{
			var logger = LoggerSubstitute.Create();

			var underTest = new UnderTest(logger);

			logger.DidNotReceiveWithAnyArgs().Log(default, default);

			underTest.LogDebug();
			logger.DidNotReceive().Log(LogLevel.Critical, Arg.Any<string>());
			logger.DidNotReceive().LogCritical(Arg.Any<string>());

			// Limitation: can not use this form
			// logger.DidNotReceiveWithAnyArgs().LogCritical(default);
		}

		[Test]
		public void TestLogErrorWithArgs()
		{
			var logger = LoggerSubstitute.Create();

			new UnderTest(logger).LogErrorWithArgs();

			logger.Received().LogError("Message with args");
			logger.Received().LogError(Arg.Is<string>(s => s.StartsWith("Message")));

			logger.Received().Log(LogLevel.Error, "Message with args");
			logger.Received().Log(LogLevel.Error, 0, null, "Message with args");
		}

		[Test]
		public void TestLogErrorWithExceptionAndArgs()
		{
			var logger = LoggerSubstitute.Create();

			new UnderTest(logger).LogErrorWithExceptionAndArgs();

			logger.Received().LogError("Message with exception and args");
			logger.Received().LogError(Arg.Is<string>(s => s.StartsWith("Message")));
			logger.Received().LogError(Arg.Any<InvalidOperationException>(), "Message with exception and args");
			logger.Received().LogError(Arg.Any<InvalidOperationException>(), Arg.Is<string>(s => s.StartsWith("Message")));

			logger.Received().Log(LogLevel.Error, "Message with exception and args");
			logger.Received().Log(LogLevel.Error, Arg.Any<InvalidOperationException>(), "Message with exception and args");
			logger.Received().Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Any<InvalidOperationException>(), "Message with exception and args");
		}

		[Test]
		public void TestLogErrorWithExceptionAndEventIdAndArgs()
		{
			var logger = LoggerSubstitute.Create();

			new UnderTest(logger).LogErrorWithExceptionAndEventIdAndArgs();

			logger.Received().LogError(Arg.Is<EventId>(eventId => eventId.Id == 1), "Message with exception and event id and args");
			logger.Received().LogError(Arg.Any<InvalidOperationException>(), "Message with exception and event id and args");
			logger.Received().LogError(Arg.Is<EventId>(eventId => eventId.Id == 1), Arg.Any<InvalidOperationException>(), "Message with exception and event id and args");
			logger.Received().Log(LogLevel.Error, Arg.Is<EventId>(eventId => eventId.Id == 1), Arg.Any<InvalidOperationException>(), "Message with exception and event id and args");
		}

		[Test]
		public void TestLogWithErrorLevel()
		{
			var logger = LoggerSubstitute.Create();

			var underTest = new UnderTest(logger);

			underTest.LogWithLevel(LogLevel.Debug);
			logger.Received().Log(LogLevel.Debug, "Message with level Debug");
			logger.Received().LogDebug("Message with level Debug");

			underTest.LogWithLevel(LogLevel.Error);
			logger.Received().Log(Arg.Is<LogLevel>(level => level > LogLevel.Warning), Arg.Any<string>());
		}
	}
}