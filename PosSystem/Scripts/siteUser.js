$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化 DataTable
    InitDataTable($('#table-User'), {
        Order: [[4, 'desc']],
        ColumnDefs: [
            { width: '150px', targets: 4 },
            { width: '9.9rem', targets: 5 }
        ]
    });

    /****************** 事件初始化 ******************/
    // 事件 - 新增
    $(document).on('click', '.operationBtn-Create', function () {
        GetModalContent({
            Title: '新增員工',
            Url: Config.SiteRoot + 'User/Create',
            Data: null
        });
    });

    // 事件 - 編輯
    $(document).on('click', '.operationBtn-Update', function () {
        GetModalContent({
            Title: '編輯員工',
            Url: Config.SiteRoot + 'User/Update',
            Data: { RecordID: $(this).data('recordid') }
        });
    });

    // 事件 - 刪除
    $(document).on('click', '.operationBtn-Delete', function () {
        GetModalContent({
            Title: '刪除員工',
            Url: Config.SiteRoot + 'User/Delete',
            Data: { RecordID: $(this).data('recordid') }
        });
    });


    // 事件 - 詳細資料
    $(document).on('click', '.operationBtn-Detail', function () {
        GetModalContent({
            Title: '詳細資訊',
            Url: Config.SiteRoot + 'User/Detail',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
});


// 加入資料列
function AppendToView(Data) {
    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, false, false, false, false, false, false);
    var rowData = [Data.Name, Data.AuthLevel, Data.Phone, Data.Address, String.formatDateTime(Data.UpdateTime), operaionBlock];

    DataTable.AddRow($('#table-User'), rowData);
}

// 更新資料列
function UpdateView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-User'), 2, Data.Phone);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    var operaionBlock = ConstructOperationBlock(Data.RecordID, true, true, true, false, false);
    var rowData = [Data.Name, Data.AuthLevel, Data.Phone, Data.Address, String.formatDateTime(Data.UpdateTime), operaionBlock];

    DataTable.UpdateRow($('#table-User'), rowIndex, rowData);
}

// 刪除資料列
function RemoveFromView(Data) {
    var rowIndex = DataTable.GetRowIndex($('#table-User'), 2, Data.Phone);

    if (rowIndex < 0) {
        ShowMsg("無法取得資料列，請重新整理頁面");
        return;
    }

    DataTable.RemoveRow($('#table-User'), rowIndex);
}