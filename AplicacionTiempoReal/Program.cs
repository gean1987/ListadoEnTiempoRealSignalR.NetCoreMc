using AplicacionTiempoReal.DAO;
using AplicacionTiempoReal.HUB;
using AplicacionTiempoReal.Datos;
using AplicacionTiempoReal;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

// Registrar ConexionDB como un servicio singleton
builder.Services.AddSingleton<ConexionDB>();

// Registrar DAO_Matricula con inyección de dependencia para ConexionDB
builder.Services.AddScoped<DAO_Matricula>();

// Configurar SignalR
builder.Services.AddSignalR();

// Agregar el servicio en segundo plano PopulationHostedServices
builder.Services.AddHostedService<PopulationHostedServices>();

// Configurar CORS para permitir todas las conexiones necesarias
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)); // Permitir todas las solicitudes
});

var app = builder.Build();

app.UseCors("CorsPolicy"); // Activar CORS antes de los enrutamientos

app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapear el Hub de SignalR
app.MapHub<PopulationHub>("/PopulationHub");

app.Run();
