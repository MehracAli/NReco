using Microsoft.Extensions.Configuration;
using NReco.Logging.File;

namespace LogTest
{
	public class Startup
	{
		IWebHostEnvironment HostingEnv;
		public IConfigurationRoot Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			HostingEnv = env;
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging(loggingBuilder =>
			{
				var loggingSection = Configuration.GetSection("Logging");
				loggingBuilder.AddConfiguration(loggingSection);
				loggingBuilder.AddConsole();

				Action<FileLoggerOptions> resolveRelativeLoggingFilePath = (fileOpts) =>
				{
					fileOpts.FormatLogFileName = fName =>
					{
						return Path.IsPathRooted(fName) ? fName : Path.Combine(HostingEnv.ContentRootPath, fName);
					};
				};

				loggingBuilder.AddFile(loggingSection.GetSection("FileOne"), resolveRelativeLoggingFilePath);
				loggingBuilder.AddFile(loggingSection.GetSection("FileTwo"), resolveRelativeLoggingFilePath);
			});

		}
	}
}
