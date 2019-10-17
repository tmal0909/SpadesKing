$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化 DataTable
    InitDataTable($('#table-Member'), {
        Order: [[3, 'desc'], [1, 'desc']],
        ColumnDefs: [{ width: '13rem', targets: 4 }]
    });

    /****************** 事件初始化 ******************/
    // 事件 - 新增
    $(document).on('click', '.operationBtn-Create', function () {
        GetModalContent({
            Title: '新增會員',
            Url: Config.SiteRoot + 'Member/Create',
            Data: null
        });
    });

    // 事件 - 編輯
    $(document).on('click', '.operationBtn-Update', function () {
        GetModalContent({
            Title: '編輯會員',
            Url: Config.SiteRoot + 'Member/Update',
            Data: { RecordID: $(this).data('recordid') }
        });
    });

    // 事件 - 刪除
    $(document).on('click', '.operationBtn-Delete', function () {
        GetModalContent({
            Title: '刪除會員',
            Url: Config.SiteRoot + 'Member/Delete',
            Data: { RecordID: $(this).data('recordid') }
        });
    });

    // 事件 - 詳細資料
    $(document).on('click', '.operationBtn-Detail', function () {
        GetModalContent({
            Title: '詳細資訊',
            Url: Config.SiteRoot + 'Member/Detail',
            Data: { RecordID: $(this).data('recordid') }
        });
    });


    // 事件 - 消費紀錄
    $(document).on('click', '.operationBtn-Record', function () {
        GetModalContent({
            Title: '消費紀錄',
            Url: Config.SiteRoot + 'Member/OperationRecord',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
});

// 加入資料列
function AppendToView(Data) {
    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, true, false, false, false, false, false);
    var rowData = [Data.Name, Data.Integration, Data.Phone, String.formatDateTime(Data.LastConsumptionDate), operaionBlock];

    DataTable.AddRow($('#table-Member'), rowData);
}

// 更新資料列
function UpdateView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-Member'), 2, Data.Phone);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, true, false);
    var rowData = [Data.Name, Data.Integration, Data.Phone, String.formatDateTime(Data.LastConsumptionDate), operaionBlock];

    DataTable.UpdateRow($('#table-Member'), rowIndex, rowData);
}

// 刪除資料列
function RemoveFromView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-Member'), 2, Data.Phone);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    DataTable.RemoveRow($('#table-Member'), rowIndex);
}