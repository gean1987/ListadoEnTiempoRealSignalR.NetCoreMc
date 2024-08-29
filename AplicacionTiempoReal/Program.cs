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

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
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

// Mapear el Hub de SignalR
app.MapHub<PopulationHub>("/PopulationHub");

app.Run();
