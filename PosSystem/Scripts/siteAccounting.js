$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化 DataTable
    DataTable.AddDateFilter();
    InitDataTable($('#table-Accounting'), {
        Order: [[0, 'desc']],
        ColumnDefs: [{ width: '6.6rem', targets: 8}]
    });

    // 初始化 DatePicker
    InitDatePicker($('.dateFilter-Accounting'));

    // 初始化 HighChart
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });

    /****************** 事件初始化 ******************/
    // 事件 - 起始日期異動
    $(document).on('change', '.dateFilter-Accounting', function () {
        DataTable.Refresh($('#table-Accounting'));
    });

    // 事件 - 帳務明細
    $(document).on('click', '.operationBtn-Detail-Accounting', function () {
        GetModalContent({
            Title: '帳務明細',
            Url: Config.SiteRoot + 'Accounting/AccountingDetail',
            Data: { Date: $(this).data('date') }
        });
    });

    // 事件 - 餐點明細
    $(document).on('click', '.operationBtn-Detail-Order', function () {
        GetModalContent({
            Title: '點餐明細',
            Url: Config.SiteRoot + 'Accounting/OrderDetail',
            Data: { Date: $(this).data('date') }
        });
    });

    // 事件 - 統計圖表
    $(document).on('click', '.operationBtn-Chart', function () {
        var dateStart = $('#dateFilter-Accounting-DateStart').val();
        var dateEnd = $('#dateFilter-Accounting-DateEnd').val();

        if (!dateStart) {
            ShowMsg("請選擇起始日期");
            return;
        }

        if (!dateEnd) {
            ShowMsg("請選擇結束日期");
            return;
        }

        if (dateStart > dateEnd) {
            ShowMsg("起始日期需在結束日期之前");
            return;
        }

        GetModalContent({
            Title: '統計圖表',
            Url: Config.SiteRoot + 'Accounting/StatisticChart',
            Data: { DateStart: dateStart, DateEnd: dateEnd }
        });
    });
});

// 初始化統計圖表
function InitChart(ChartData) {
    Highcharts.chart('statisticChart', {
        chart: {
            type: 'pie',
            events: {
                load: function (event) {
                    this.renderer.image(window.location.origin + Config.SiteRoot + 'img/logo-bg.png', 0, 0, 1070, 500).attr({ zIndex: 0 }).add();
                }
            }
        },
        title: {
            text: ChartData.Title
        },
        tooltip: {
            /*
            headerFormat: "<span style=''>{point.key}</span><br/><br/><br/>",
            pointFormat: '積分 : {point.y} P<br/>比例 {point.percentage:.1f} %'
            */
            useHTML: true,
            headerFormat: '<div class="highchart-tooltip-title">{point.key}</div><table class="highchart-tooltip-table">',
            pointFormat: '<tr>' +
                            '<td class="highchart-tooltip-td-title">積分 :</td>' +
                            '<td class="highchart-tooltip-td-value">{point.y} P</td>' +
                         '</tr>' +
                         '<tr>' +
                            '<td class="highchart-tooltip-td-title">比例 :</td>' +
                            '<td class="highchart-tooltip-td-value">{point.percentage:.1f} %</td>' +
                         '</tr>',
            footerFormat: '</table>'
        },
        colors: ['#FED558', '#44607C'],
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.name}: {point.y} P  ( {point.percentage:.1f} % )',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            name: '收入',
            colorByPoint: true,
            data: [
                {
                    name: '租金收入',
                    y: ChartData.Rent,
                    sliced: true,
                    selected: true
                },
                {
                    name: '餐點收入',
                    y: ChartData.Order
                }
            ]
        }]
    });
}