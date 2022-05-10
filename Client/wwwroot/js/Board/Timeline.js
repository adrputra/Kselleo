$.ajax({
    url: "https://localhost:44308/api/boards/detail/09fa86f6-01e1-4fb9-b04c-66bc3bd005f2",
    success: function (result) {
        var board = result.data.name;
        var awal = moment(result.data.user.lists[0].cards[0].createdAt).format('YYYY-MM-DD');
        var akhir = moment(result.data.user.lists[0].cards[0].due).format('YYYY-MM-DD');
        var list1 = result.data.user.lists[0].name;
        var list2 = result.data.user.lists[0].name;
        var test = result.data.user;

        var data = [];
        for (var i = 0; i < result.data.user.lists.length; i++) {
            for (var j = 0; j < result.data.user.lists[i].cards.length; j++) {
                var name = result.data.user.lists[i].name;
                var awal = moment(result.data.user.lists[i].cards[j].createdAt).format('YYYY-MM-DD');
                var akhir = moment(result.data.user.lists[i].cards[j].due).format('YYYY-MM-DD');
                let data1 = {
                    x: name,
                    y: [{
                        awal, akhir
                    }]
                }
                data.push(data1);
            }
        }
        console.log(data);


        var options = {
            series: [
                {
                    name: board,
                    data: data
                },
            ],
            chart: {
                height: 450,
                type: 'rangeBar'
            },
            plotOptions: {
                bar: {
                    horizontal: true,
                    barHeight: '80%'
                }
            },
            xaxis: {
                type: 'datetime'
            },
            stroke: {
                width: 1
            },
            fill: {
                type: 'solid',
                opacity: 0.6
            },
            legend: {
                position: 'top',
                horizontalAlign: 'left'
            }
        };

        var chart = new ApexCharts(document.querySelector("#chart"), options);
        chart.render();
            
    }
})

function test(name) {
    name = "aditya";
}