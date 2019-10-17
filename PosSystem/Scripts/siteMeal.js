$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化 DataTable
    InitDataTable($('#table-Meal'), {
        Order: [[3, 'desc'], [1, 'asc']],
        ColumnDefs: [{ width: '9.9rem', targets: 4 }]
    });

    /****************** 事件初始化 ******************/
    // 事件 - 新增
    $(document).on('click', '.operationBtn-Create', function () {
        GetModalContent({
            Title: '新增餐點',
            Url: Config.SiteRoot + 'Meal/Create',
            Data: null
        });
    });

    // 事件 - 編輯
    $(document).on('click', '.operationBtn-Update', function () {
        GetModalContent({
            Title: '編輯餐點',
            Url: Config.SiteRoot + 'Meal/Update',
            Data: { RecordID: $(this).data('recordid') }
        });
    });

    // 事件 - 刪除
    $(document).on('click', '.operationBtn-Delete', function () {
        GetModalContent({
            Title: '刪除餐點',
            Url: Config.SiteRoot + 'Meal/Delete',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
    
    // 事件 - 詳細資料
    $(document).on('click', '.operationBtn-Detail', function () {
        GetModalContent({
            Title: '詳細資訊',
            Url: Config.SiteRoot + 'Meal/Detail',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
});


// 加入資料列
function AppendToView(Data) {
    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, false, false, false, false);
    var rowData = [Data.Name, Data.Type, Data.Integration, String.formatDateTime(Data.UpdateTime), operaionBlock];

    DataTable.AddRow($('#table-Meal'), rowData);
}

// 更新資料列
function UpdateView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-Meal'), 0, Data.Name);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, false, false, false, false);
    var rowData = [Data.Name, Data.Type, Data.Integration, String.formatDateTime(Data.UpdateTime), operaionBlock];

    DataTable.UpdateRow($('#table-Meal'), rowIndex, rowData);
}

// 刪除資料列
function RemoveFromView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-Meal'), 0, Data.Name);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    DataTable.RemoveRow($('#table-Meal'), rowIndex);
}