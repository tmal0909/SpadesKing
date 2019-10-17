$(function () {
    /****************** 頁面初始化 ******************/
    // 加入日期篩選器
    DataTable.AddDateFilter();

    // 加入狀態篩選器
    DataTable.AddStatusFilter();

    // 初始化 DataTable
    InitDataTableRowSpan($('#table-OrderDetail'), {
        Order: [[1, 'desc'], [3, 'asc'], [6, 'desc']],
        RowsGroup: [1, 0],
        ColumnDefs: [{ width: '150px', targets: 6 }, { width: '3.5rem', targets: 7 }]
    });

    // 初始化 DatePicker
    InitDatePicker($('.dateFilter-OrderDetail'));


    /****************** 事件初始化 ******************/
    // 事件 - 起始日期異動
    $(document).on('change', '.dateFilter-OrderDetail', function () {
        DataTable.Refresh($('#table-OrderDetail'));
    });

    // 事件 - 狀態異動
    $(document).on('change', '#statusFilter-OrderDetail', function () {
        DataTable.Refresh($('#table-OrderDetail'));
    });

    // 事件 - 更新
    $(document).on('click', '.operationBtn-Refresh', function () {
        SendAjaxRequest({
            Url: Config.SiteRoot + 'Order/RefreshOrder',
            Type: 'RefreshOrder',
            Data: null
        });
    });

    // 事件 - 完成
    $(document).on('click', '.operationBtn-Finish', function () {
        SendAjaxRequest({
            Url: Config.SiteRoot + 'Order/Finish',
            Type: 'FinishOrder',
            Data: { RecordID: $(this).data('recordid') }
        });
    });

    // 事件 - 取消
    $(document).on('click', '.operationBtn-Cancel', function () {
        GetModalContent({
            Title: '取消點餐',
            Url: Config.SiteRoot + 'Order/Cancel',
            Data: { OrderNo: $(this).data('orderno') }
        });
    });
});

// 更新資料列
function UpdateViewByMultiRow(Data) {
    for (var i = 0, len = Data.length; i < len; i++) {
        var rowIndex = DataTable.GetRowIndexByMultiColumn($('#table-OrderDetail'), [
            { Key: Data[i].OrderNo, ColumnIndex: 1 },
            { Key: Data[i].MealName, ColumnIndex: 2 },
            { Key: Data[i].MealType, ColumnIndex: 3 },
            { Key: Data[i].Quantity, ColumnIndex: 4 },
        ]);

        if (rowIndex >= 0) {
            var operationBlockRemove = ConstructOperationBlock(Data[i].OrderNo, false, false, false, false, false, false, true);
            var operationBlockFinish = ConstructOperationBlock(Data[i].RecordID, false, false, false, false, false, true, false);
            var rowData = [operationBlockRemove, Data[i].OrderNo, Data[i].MealName, Data[i].MealType, Data[i].Quantity, Data[i].Status, String.formatDateTime(Data[i].UpdateTime), operationBlockFinish];

            DataTable.UpdateRow($('#table-OrderDetail'), rowIndex, rowData);
        }
    }
}

// 重新載入資料
function ReloadDataTable(Data) {
    var rowsData = [];

    for (var i = 0, len = Data.length; i < len; i++) {
        var operationBlockRemove = ConstructOperationBlock(Data[i].OrderNo, false, false, false, false, false, false, true);
        var operationBlockFinish = ConstructOperationBlock(Data[i].RecordID, false, false, false, false, false, true, false);
        var rowData = [operationBlockRemove, Data[i].OrderNo, Data[i].MealName, Data[i].MealType, Data[i].Quantity, Data[i].Status, String.formatDateTime(Data[i].UpdateTime), operationBlockFinish];

        rowsData.push(rowData);
    }

    DataTable.ClearTable($('#table-OrderDetail'));
    DataTable.AddRows($('#table-OrderDetail'), rowsData);
}