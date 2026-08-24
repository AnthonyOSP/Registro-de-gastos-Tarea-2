using Microsoft.AspNetCore.DataProtection;
using Registro_de_gastos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ubicación explícita para las claves de Data Protection (las que validan los
// tokens antiforgery de los formularios). Sin esto, ASP.NET Core no sabe bien
// dónde guardarlas y avisa que podrían no persistir; con esto al menos quedan
// estables mientras la misma instancia del contenedor siga viva. En Render
// (sin disco persistente en el plan gratis) igual se generan de nuevo si el
// contenedor se recrea — para que sobrevivan a eso haría falta un volumen
// persistente, que es una función paga.
var keysPath = Path.Combine(AppContext.BaseDirectory, "keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("MisGastos");

// Servicio en memoria para administrar los gastos (singleton para que los datos
// persistan entre peticiones mientras la aplicación esté en ejecución).
builder.Services.AddSingleton<GastoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Sin UseHttpsRedirection(): en Render (y la mayoría de PaaS) el HTTPS lo
// termina el proxy/edge, no esta app — dentro del contenedor solo llega HTTP,
// así que este middleware no tenía a qué puerto redirigir y solo generaba
// una advertencia en cada request.
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
