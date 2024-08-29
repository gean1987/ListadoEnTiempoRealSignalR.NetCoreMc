using AplicacionTiempoReal.Models;
using AplicacionTiempoReal.DAO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacionTiempoReal
{
    public class PopulationHostedServices : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PopulationHub> _populationHub;
        private Timer _timer;

        // Inyectar IServiceScopeFactory a través del constructor
        public PopulationHostedServices(IHubContext<PopulationHub> populationHub, IServiceScopeFactory scopeFactory)
        {
            _populationHub = populationHub;
            _scopeFactory = scopeFactory; // Guardar la fábrica de scopes
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendInfo, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void SendInfo(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                // Resolver DAO_Matricula dentro del scope
                var gestionMatricula = scope.ServiceProvider.GetRequiredService<DAO_Matricula>();

                // Obtener la lista de alumnos
                List<Alumnos> alumnos = await gestionMatricula.GetAllAsync();

                // Verifica si la lista está vacía o tiene datos
                if (alumnos != null && alumnos.Any())
                {
                    Console.WriteLine($"Enviando {alumnos.Count} alumnos a los clientes...");
                }
                else
                {
                    Console.WriteLine("No se encontraron alumnos para enviar.");
                }

                // Enviar la lista a través de SignalR
                await _populationHub.Clients.All.SendAsync("Receptorlistas", alumnos);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
