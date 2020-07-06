using System;
using NSubstitute;

// Specifically not using Microsoft.Extensions.Logging as we don't want the LoggerExtensions
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using EventId = Microsoft.Extensions.Logging.EventId;
using NSubstitute.Core;
using System.Linq;

namespace Extensions.Logging.NSubstitute
{
	public abstract class LoggerSubstitute : ILogger
	{
		public static LoggerSubstitute Create() => Substitute.ForPartsOf<LoggerSubstitute>();

		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			Log(logLevel, eventId, exception, formatter(state, exception));
		}

		public void LogDebug(EventId eventId, Exception exception, string message) => Log(LogLevel.Debug, eventId, exception, message);
		public void LogDebug(EventId eventId, string message) => Log(LogLevel.Debug, eventId, message);
		public void LogDebug(Exception exception, string message) => Log(LogLevel.Debug, exception, message);
		public void LogDebug(string message) => Log(LogLevel.Debug, message);

		public void LogTrace(EventId eventId, Exception exception, string message) => Log(LogLevel.Trace, eventId, exception, message);
		public void LogTrace(EventId eventId, string message) => Log(LogLevel.Trace, eventId, message);
		public void LogTrace(Exception exception, string message) => Log(LogLevel.Trace, exception, message);
		public void LogTrace(string message) => Log(LogLevel.Trace, message);

		public void LogInformation(EventId eventId, Exception exception, string message) => Log(LogLevel.Information, eventId, exception, message);
		public void LogInformation(EventId eventId, string message) => Log(LogLevel.Information, eventId, message);
		public void LogInformation(Exception exception, string message) => Log(LogLevel.Information, exception, message);
		public void LogInformation(string message) => Log(LogLevel.Information, message);

		public void LogWarning(EventId eventId, Exception exception, string message) => Log(LogLevel.Warning, eventId, exception, message);
		public void LogWarning(EventId eventId, string message) => Log(LogLevel.Warning, eventId, message);
		public void LogWarning(Exception exception, string message) => Log(LogLevel.Warning, exception, message);
		public void LogWarning(string message) => Log(LogLevel.Warning, message);

		public void LogError(EventId eventId, Exception exception, string message) => Log(LogLevel.Error, eventId, exception, message);
		public void LogError(EventId eventId, string message) => Log(LogLevel.Error, eventId, message);
		public void LogError(Exception exception, string message) => Log(LogLevel.Error, exception, message);
		public void LogError(string message) => Log(LogLevel.Error, message);

		public void LogCritical(EventId eventId, Exception exception, string message) => Log(LogLevel.Critical, eventId, exception, message);
		public void LogCritical(EventId eventId, string message) => Log(LogLevel.Critical, eventId, message);
		public void LogCritical(Exception exception, string message) => Log(LogLevel.Critical, exception, message);
		public void LogCritical(string message) => Log(LogLevel.Critical, message);

		public void Log(LogLevel logLevel, EventId eventId, string message) => LogArrangeArgs(logLevel, eventId, Arg.Any<Exception>(), message);
		public void Log(LogLevel logLevel, Exception exception, string message) => LogArrangeArgs(logLevel, Arg.Any<EventId>(), exception, message);
		public void Log(LogLevel logLevel, string message) => LogArrangeArgs(logLevel, Arg.Any<EventId>(), Arg.Any<Exception>(), message);

		private void LogArrangeArgs(LogLevel logLevel, EventId eventId, Exception exception, string message)
		{
			var argSpecifications = SubstitutionContext.Current.ThreadContext.DequeueAllArgumentSpecifications();
			void enqueueArgSpecification<T>()
			{
				var argSpecification = argSpecifications.SingleOrDefault(_ => typeof(T).IsAssignableFrom(_.ForType));
				if (argSpecification != null)
				{
					SubstitutionContext.Current.ThreadContext.EnqueueArgumentSpecification(argSpecification);
				}
			}

			// Enqueue in correct order
			enqueueArgSpecification<LogLevel>();
			enqueueArgSpecification<EventId>();
			enqueueArgSpecification<Exception>();
			enqueueArgSpecification<string>();

			Log(logLevel, eventId, exception, message);
		}

		public abstract void Log(LogLevel logLevel, EventId eventId, Exception exception, string message);
		/*
		{
			Log(logLevel, eventId, message);
			Log(logLevel, exception, message);
			Log(logLevel, message);
		}
        */

		public virtual bool IsEnabled(LogLevel logLevel) => true;
		public abstract IDisposable BeginScope<TState>(TState state);
	}
}
