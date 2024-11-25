// Datos ficticios para los gráficos
document.addEventListener("DOMContentLoaded", function() {
    // Gráfico de Habitaciones más/menos ocupadas
    var ctx1 = document.getElementById('habitacionesChart').getContext('2d');
    var habitacionesChart = new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: ['Habitación 101', 'Habitación 202', 'Habitación 303', 'Habitación 404', 'Habitación 505'],
            datasets: [{
                label: 'Ocupación (%)',
                data: [90, 70, 55, 40, 95],
                backgroundColor: [
                    'rgba(75, 192, 192, 0.5)',
                    'rgba(54, 162, 235, 0.5)',
                    'rgba(255, 206, 86, 0.5)',
                    'rgba(153, 102, 255, 0.5)',
                    'rgba(255, 99, 132, 0.5)'
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 99, 132, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return value + "%"; // Añadir porcentaje a las etiquetas del eje Y
                        }
                    }
                }
            }
        }
    });

    // Gráfico de Servicios más utilizados
    var ctx2 = document.getElementById('serviciosChart').getContext('2d');
    var serviciosChart = new Chart(ctx2, {
        type: 'doughnut',
        data: {
            labels: ['Spa', 'Restaurante', 'Gimnasio', 'Traslado Aeropuerto', 'Tours'],
            datasets: [{
                label: 'Reservas de Servicios',
                data: [120, 150, 100, 50, 80],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.7)',
                    'rgba(54, 162, 235, 0.7)',
                    'rgba(255, 206, 86, 0.7)',
                    'rgba(75, 192, 192, 0.7)',
                    'rgba(153, 102, 255, 0.7)'
                ],
                borderWidth: 1
            }]
        }
    });
});

document.getElementById("filterButton").addEventListener("click", function () {
    const startDate = document.getElementById("startDate").value;
    const endDate = document.getElementById("endDate").value;

    fetch(`/Dashboard/ObtenerMetricas?startDate=${startDate}&endDate=${endDate}`)
        .then(response => response.json())
        .then(data => actualizarDashboard(data))
        .catch(error => console.error('Error:', error));
});

function actualizarDashboard(metricas) {
    // Procesar y actualizar KPIs
    const ingresosTotales = metricas.reduce((sum, m) => sum + m.ingresosTotales, 0);
    const promedioEstadia = (metricas.reduce((sum, m) => sum + m.duracionPromedioEstadia, 0) / metricas.length).toFixed(2);

    document.getElementById("avgReservas").innerText = metricas.length;
    document.getElementById("totalOcupacion").innerText =
        (metricas.reduce((sum, m) => sum + m.tasaOcupacion, 0) / metricas.length).toFixed(2) + "%";
    document.getElementById("duracionEstadia").innerText = `${promedioEstadia} días`;
    document.getElementById("ingresosMensuales").innerText = `$${ingresosTotales.toLocaleString()}`;

    // Actualizar gráficos
    actualizarGraficoHabitaciones(metricas);
    actualizarGraficoServicios(metricas);
}

// Ejemplo de actualización de un gráfico
function actualizarGraficoHabitaciones(data) {
    habitacionesChart.data.datasets[0].data = data.map(m => m.ocupacionDiaria);
    habitacionesChart.update();
}
