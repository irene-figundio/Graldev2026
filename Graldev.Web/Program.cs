var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Antiforgery token
builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "__RequestVerificationToken";
    options.HeaderName = "X-XSRF-TOKEN";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error500");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error404");

app.UseHttpsRedirection();

// 301 Legacy Redirects Middleware
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    var query = context.Request.QueryString.Value ?? "";

    if (path.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/", permanent: true);
        return;
    }
    if (path.Equals("/Home/CicDetails", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/consulenza-informatica", permanent: true);
        return;
    }
    if (path.Equals("/Project/Geordie", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/case-study/geordie", permanent: true);
        return;
    }
    if (path.Equals("/Project/Ludirex", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/labs", permanent: true);
        return;
    }
    if (path.Equals("/Project/AR", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/case-study/gralcall", permanent: true);
        return;
    }
    if (path.Equals("/Project/Parcor", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/labs", permanent: true);
        return;
    }
    if (path.Equals("/Home/ChangeLanguage", StringComparison.OrdinalIgnoreCase))
    {
        if (query.Contains("lang=EN", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Redirect("/en", permanent: true);
        }
        else
        {
            context.Response.Redirect("/", permanent: true);
        }
        return;
    }

    await next();
});

// Serve static assets
app.UseStaticFiles();

app.UseRouting();

// Language detection middleware
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    var culture = "it-IT"; // Default is Italian
    if (path.StartsWith("/en", StringComparison.OrdinalIgnoreCase))
    {
        culture = "en";
    }

    var cultureInfo = new System.Globalization.CultureInfo(culture);
    System.Globalization.CultureInfo.CurrentCulture = cultureInfo;
    System.Globalization.CultureInfo.CurrentUICulture = cultureInfo;

    await next();
});

app.UseAuthorization();

app.MapStaticAssets();

// Map controllers using attribute routing
app.MapControllers();

// Fallback to MapControllerRoute in case there are other actions
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

public partial class Program { }
