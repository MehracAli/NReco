using NReco.Logging.File;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddLogging(loggingBuilder => {

	loggingBuilder.AddFile("app.log", append: true);
    loggingBuilder.AddConsole();

	loggingBuilder.AddFile("app.log", fileLoggerOpts => {
		fileLoggerOpts.HandleFileError = (err) => {
			err.UseNewLogFileName(Path.GetFileNameWithoutExtension(err.LogFileName) + "_alt" + Path.GetExtension(err.LogFileName));
		};
	});

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
