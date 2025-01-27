// Configuración global de ApexCharts
window.Apex = {
    chart: {
        foreColor: '#fff',
        toolbar: { show: false },
    },
    colors: ['#FCCF31', '#17ead9', '#f02fc2'],
    stroke: { width: 3 },
    dataLabels: { enabled: false },
    grid: { borderColor: "#40475D" },
    xaxis: {
        axisTicks: { color: '#333' },
        axisBorder: { color: "#333" }
    },
    fill: {
        type: 'gradient',
        gradient: {
            gradientToColors: ['#F55555', '#6078ea', '#6094ea']
        },
    },
    tooltip: {
        theme: 'dark',
        x: {
            formatter: function (val) {
                return moment(new Date(val)).format("HH:mm:ss");
            }
        }
    },
    yaxis: {
        decimalsInFloat: 2,
        opposite: true,
        labels: { offsetX: -10 }
    }
};

// Funciones auxiliares para datos
function getRandom() {
    return Math.random() * 100;
}

function getRangeRandom(yrange) {
    return Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;
}

function generateMinuteWiseTimeSeries(baseval, count, yrange) {
    let series = [];
    for (let i = 0; i < count; i++) {
        series.push([baseval, getRangeRandom(yrange)]);
        baseval += 300000;
    }
    return series;
}

// Opciones y creación de gráficos
var optionsColumn = {
    chart: {
        height: 350,
        type: 'bar',
        animations: { enabled: false },
        toolbar: { show: false },
        zoom: { enabled: false }
    },
    series: [{
        name: 'Load Average',
        data: generateMinuteWiseTimeSeries(new Date().getTime(), 12, { min: 10, max: 110 })
    }],
    xaxis: { type: 'datetime', range: 2700000 },
    title: { text: 'Load Average', align: 'left', style: { fontSize: '12px' } }
};

var chartColumn = new ApexCharts(document.querySelector("#columnchart"), optionsColumn);
chartColumn.render();

var optionsLine = {
    chart: {
        height: 350,
        type: 'line',
        animations: {
            enabled: true,
            dynamicAnimation: { speed: 1000 }
        },
        zoom: { enabled: false }
    },
    series: [{
        name: 'Running',
        data: generateMinuteWiseTimeSeries(new Date().getTime(), 12, { min: 30, max: 110 })
    }, {
        name: 'Waiting',
        data: generateMinuteWiseTimeSeries(new Date().getTime(), 12, { min: 30, max: 110 })
    }],
    xaxis: { type: 'datetime', range: 2700000 },
    title: { text: 'Processes', align: 'left', style: { fontSize: '12px' } }
};

var chartLine = new ApexCharts(document.querySelector("#linechart"), optionsLine);
chartLine.render();

var optionsCircle = {
    chart: {
        type: 'radialBar',
        height: 320
    },
    series: [71, 63],
    labels: ['Device 1', 'Device 2']
};

var chartCircle = new ApexCharts(document.querySelector('#circlechart'), optionsCircle);
chartCircle.render();

var optionsProgress1 = {
    chart: { height: 70, type: 'bar', sparkline: { enabled: true } },
    series: [{ name: 'Process 1', data: [44] }],
    subtitle: { text: '44%', align: 'right', style: { fontSize: '20px' } },
    xaxis: { categories: ['Process 1'] },
    yaxis: { max: 100 }
};

var chartProgress1 = new ApexCharts(document.querySelector('#progress1'), optionsProgress1);
chartProgress1.render();

var optionsProgress2 = {
    chart: { height: 70, type: 'bar', sparkline: { enabled: true } },
    series: [{ name: 'Process 2', data: [80] }],
    subtitle: { text: '80%', align: 'right', style: { fontSize: '20px' } },
    xaxis: { categories: ['Process 2'] },
    yaxis: { max: 100 }
};

var chartProgress2 = new ApexCharts(document.querySelector('#progress2'), optionsProgress2);
chartProgress2.render();

var optionsProgress3 = {
    chart: { height: 70, type: 'bar', sparkline: { enabled: true } },
    series: [{ name: 'Process 3', data: [74] }],
    subtitle: { text: '74%', align: 'right', style: { fontSize: '20px' } },
    xaxis: { categories: ['Process 3'] },
    yaxis: { max: 100 }
};

var chartProgress3 = new ApexCharts(document.querySelector('#progress3'), optionsProgress3);
chartProgress3.render();

// Integración con SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/stocks") // Cambia por la URL de tu servidor SignalR
    .build();

connection.start().then(() => {
    console.log("Conectado a SignalR");
}).catch(err => console.error("Error al conectar con SignalR:", err));

connection.on("Receive", (chartData) => {
    console.log("Datos recibidos desde SignalR:", chartData);

    if (chartData.columnChart) {
        chartColumn.updateSeries([{ data: chartData.columnChart }]);
    }

    if (chartData.lineChart) {
        chartLine.updateSeries([
            { data: chartData.lineChart.running },
            { data: chartData.lineChart.waiting }
        ]);
    }

    if (chartData.circleChart) {
        chartCircle.updateSeries(chartData.circleChart);
    }

    if (chartData.progress1) {
        chartProgress1.updateOptions({
            series: [{ data: [chartData.progress1] }],
            subtitle: { text: chartData.progress1 + "%" }
        });
    }

    if (chartData.progress2) {
        chartProgress2.updateOptions({
            series: [{ data: [chartData.progress2] }],
            subtitle: { text: chartData.progress2 + "%" }
        });
    }

    if (chartData.progress3) {
        chartProgress3.updateOptions({
            series: [{ data: [chartData.progress3] }],
            subtitle: { text: chartData.progress3 + "%" }
        });
    }

    //CARDS BTC ETH SOL
    if (chartData.cardBtc?.price != null) {
        document.getElementById("btc_price").innerHTML = "$" + chartData.cardBtc.price;
        document.getElementById("btc_higth").innerHTML = "$" + chartData.cardBtc.hight + " (24 Hight)";
        document.getElementById("btc_low").innerHTML = "$" + chartData.cardBtc.low + " (24 Low)";
    }
    if (chartData.cardEth?.price != null) {
        document.getElementById("eth_price").innerHTML = "$" + chartData.cardEth.price;
        document.getElementById("eth_higth").innerHTML = "$" + chartData.cardEth.hight + " (24 Hight)";
        document.getElementById("eth_low").innerHTML = "$" + chartData.cardEth.low + " (24 Low)";
    }
    if (chartData.cardSol?.price != null) {
        document.getElementById("sol_price").innerHTML = "$" + chartData.cardSol.price;
        document.getElementById("sol_higth").innerHTML = "$" + chartData.cardSol.hight + " (24 Hight)";
        document.getElementById("sol_low").innerHTML = "$" + chartData.cardSol.low + " (24 Low)";
    }
});