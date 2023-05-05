// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var chartData = {
        labels: [],
        datasets: [{
            label: 'ETHIBTC',
            data: [],
            backgroundColor: 'rgba(255, 99, 132, 0.2)',
            borderColor: 'rgba(255, 99, 132, 1)',
            borderWidth: 1
        }]
    };

    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: chartData,
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    time: {
                        unit: 'second'
                    }
                }]
            },
            plugins: {
                zoom: {
                  pan: {
                    enabled: true,
                    mode: 'xy'
                  },
                  zoom: {
                    enabled: true,
                    mode: 'xy'
                  }
                }
              }
        }
    });

    function updateChartData(newData) {
        // Add the new data to the chart data
        chartData.labels.push(moment());
        chartData.datasets[0].data.push(newData);

        // Remove the oldest data if we have too many data points
        if (chartData.labels.length > 50) {
            chartData.labels.shift();
            chartData.datasets[0].data.shift();
        }

        // Update the chart
        myChart.update();
    }

    // Fetch data from server every 5 seconds
    setInterval(function () {
        $.getJSON('/Home/TickersData', function (data) {
            updateChartData(data);
            console.log("received ")
        });
    }, 5000);
});