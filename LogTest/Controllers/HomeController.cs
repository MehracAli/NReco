using Microsoft.AspNetCore.Mvc;

namespace LogTest.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult LogMessage(string logLevel, string msg)
		{
			string user = "Mehraj Mammad0v";

			_logger.Log((LogLevel)Enum.Parse(typeof(LogLevel), logLevel, true), msg+" by: "+user);
			return Ok();
		}
	}
}
