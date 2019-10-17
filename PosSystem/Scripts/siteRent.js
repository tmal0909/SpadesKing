$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化 DataTable
    InitDataTable($('#table-Rent'), {
        Order: [[1, 'desc']],
        ColumnDefs: [{ width: '150px', targets: 1 }, {width: '3.5rem', targets: 5}]
    });

    /****************** 事件初始化 ******************/
    // 事件 - 結帳
    $(document).on('click', '.operationBtn-Check', function () {
        GetModalContent({
            Title: '租金結帳',
            Url: Config.SiteRoot + 'Rent/Check',
            Data: null
        });
    });

    // 事件 - 取消
    $(document).on('click', '.operationBtn-Cancel', function () {
        GetModalContent({
            Title: '取消帳務',
            Url: Config.SiteRoot + 'Rent/Cancel',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
});

// 加入資料列
function AppendToView(Data) {
    var operaionBlock = ConstructOperationBlock(Data.RecordID, false, false, false, false, true, false, false, false, false);
    var rowData = [Data.RecordID, String.formatDateTime(Data.UpdateTime), Data.Integration, Data.Status, Data.OperatorName, operaionBlock];

    DataTable.AddRow($('#table-Rent'), rowData);
}

// 更新資料列
function UpdateView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-Rent'), 0, Data.RecordID);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    var operaionBlock = ConstructOperationBlock(Data.RecordID, false, false, false, false, true);
    var rowData = [Data.RecordID, String.formatDateTime(Data.UpdateTime), Data.Integration, Data.Status, Data.OperatorName, operaionBlock];

    DataTable.UpdateRow($('#table-Rent'), rowIndex, rowData);
}