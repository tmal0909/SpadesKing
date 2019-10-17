DataTable = {
    GetRowIndex: null,
    GetRowIndexByMultiColumn: null,
    ClearTable: null,
    AddRow: null,
    AddRows: null,
    UpdateRow: null,
    RemoveRow: null,
    Refresh: null,
    AddDateFilter: null,
    AddStatusFilter: null
}

// 格式化字串
String.format = function () {

    if (arguments.length == 0) {
        return null;
    }

    var str = arguments[0];

    for (var i = 1; i < arguments.length; i++) {

        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};

// 格式化時間
String.formatDateTime = function (DateTimeString) {
    if (!DateTimeString) return '';
    var miliseconds = DateTimeString.substring(DateTimeString.indexOf('(') + 1, DateTimeString.indexOf(')'));
    var datetime = new Date(parseInt(miliseconds));
    return moment(datetime).format('YYYY/MM/DD HH:mm:ss');
};

// DataTable 找尋 Row Index
DataTable.GetRowIndex = function (Table, ColumnIndex, Key) {
    var rowIndex = Table.dataTable().fnFindCellRowIndexes(Key, ColumnIndex);
    if (rowIndex.length > 0) return rowIndex[0];
    return -1;
};

// DataTable 找尋 Row Index
DataTable.GetRowIndexByMultiColumn = function (Table, Data) {
    var rowIndexArr = [];
    var result = [];

    for (var i = 0; i < Data.length; i++) {
        rowIndexArr.push(Table.dataTable().fnFindCellRowIndexes(Data[i].Key, Data[i].ColumnIndex));
    }

    result = rowIndexArr.shift().reduce(function (res, v) {
        if (res.indexOf(v) === -1 && rowIndexArr.every(function (a) {
            return a.indexOf(v) !== -1;
        })) res.push(v);
        return res;
    }, []);

    if (result.length > 0) return result[0];
    return -1;
}

// DataTable 清除所有資料列
DataTable.ClearTable = function (Table) {
    Table.DataTable().clear().draw();
}

// DataTable 新增資料列
DataTable.AddRow = function (Table, Data) {
    Table.DataTable().row.add(Data).draw();
};

// DataTable 新增資料列
DataTable.AddRows = function (Table, Data) {
    Table.DataTable().rows.add(Data).draw();
}

// DataTable 更新資料列
DataTable.UpdateRow = function (Table, RowIndex, Data) {
    Table.DataTable().row(RowIndex).data(Data).draw();
};

// DataTable 刪除資料列
DataTable.RemoveRow = function (Table, RowIndex) {
    Table.DataTable().row(RowIndex).remove().draw();
};

// DataTable 重新整理
DataTable.Refresh = function (Table) {
    Table.DataTable().draw();
}

// DataTable 加入日期篩選
DataTable.AddDateFilter = function () {
    $.fn.dataTable.ext.search.push(DateFilter);
}

// DataTabke 加入狀態篩選器
DataTable.AddStatusFilter = function () {
    $.fn.dataTable.ext.search.push(StatusFilter);
}

// 日期篩選
function DateFilter(settings, data, dataIndex) {
    var dateStart, dateEnd, date;
    switch (settings.sTableId) {
        case 'table-Operation':
            date = new Date(data[4].substr(0, 10)).getTime();
            dateStart = new Date($('#dateFilter-Operation-DateStart').val() || -8640000000000000).getTime();
            dateEnd = new Date($('#dateFilter-Operation-DateEnd').val() || 8640000000000000).getTime();

            if (date >= dateStart && date <= dateEnd) return true;
            return false;

        case 'table-Issue':
            date = new Date(data[0].substr(0, 10)).getTime();
            dateStart = new Date($('#dateFilter-Issue-DateStart').val() || -8640000000000000).getTime();
            dateEnd = new Date($('#dateFilter-Issue-DateEnd').val() || 8640000000000000).getTime();

            if (date >= dateStart && date <= dateEnd) return true;
            return false;

        case 'table-OrderDetail':
            date = new Date(data[6].substr(0, 10)).getTime();
            dateStart = new Date($('#dateFilter-OrderDetail-DateStart').val() || -8640000000000000).getTime();
            dateEnd = new Date($('#dateFilter-OrderDetail-DateEnd').val() || 8640000000000000).getTime();

            if (date >= dateStart && date <= dateEnd) return true;
            return false;

        case 'table-Accounting':
            date = new Date(data[0].substr(0, 10)).getTime();
            dateStart = new Date($('#dateFilter-Accounting-DateStart').val() || -8640000000000000).getTime();
            dateEnd = new Date($('#dateFilter-Accounting-DateEnd').val() || 8640000000000000).getTime();

            if (date >= dateStart && date <= dateEnd) return true;
            return false;

        default:
            return true;
    }
}

// 狀態篩選器
function StatusFilter(settings, data, dataIndex) {
    switch (settings.sTableId) {
        case 'table-OrderDetail':
            var status = $('#statusFilter-OrderDetail').val();
            if (status == '全部') return true;
            if (data[5] == status) return true;
            return false;

        default:
            return true;
    }
}

// 畫面鎖定
function BlockUI(Message) {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            width: '100%',
            height: '100%',
            top: "0px",
            left: "0px",
            'line-height': '100vh',
            'z-index': 9999,
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: Message
    });
}

// 畫面解鎖
function UnBlockUI() {
    $.unblockUI();
}

// 揭示訊息
function ShowMsg(Message) {
    BlockUI(Message);
    setTimeout(function () {
        UnBlockUI();
    }, 1200);
}

// 客製化搜尋
function CustomMacher(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.indexOf(params.term) > -1) {
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // check custom attributes
    if ($(data.element).data('phone')) {
        if ($(data.element).data('phone').toString().indexOf(params.term) > -1) {
            return data;
        }
    }

    // Return `null` if the term should not be displayed
    return null;
}

// 建立操作區
function ConstructOperationBlock(ID, UpdateBtn, DeleteBtn, DetailBtn, RecordBtn, CancelBtn, FinisOrderhBtn, CancelOrderBtn) {
    var operationBlock = '<div>';
    if (UpdateBtn) operationBlock += String.format('<div class="btn operationBtn-Update" title="編輯" data-recordid="{0}"><span class="icon-ui-edit"></span></div> ', ID);
    if (DeleteBtn) operationBlock += String.format('<div class="btn operationBtn-Delete" title="刪除" data-recordid="{0}"><span class="icon-ui-trash"></span></div> ', ID);
    if (DetailBtn) operationBlock += String.format('<div class="btn operationBtn-Detail" title="詳細資料" data-recordid="{0}"><span class="icon-ui-description"></span></div> ', ID);
    if (RecordBtn) operationBlock += String.format('<div class="btn operationBtn-Record" title="消費紀錄" data-recordid="{0}"><span class="icon-ui-review"></span></div> ', ID);
    if (CancelBtn) operationBlock += String.format('<div class="btn operationBtn-Cancel" title ="取消" data-recordid="{0}"><span class="icon-ui-trash"></span></div> ', ID);
    if (FinisOrderhBtn) operationBlock += String.format('<div class="btn operationBtn-Finish icon-ui-check-mark" title="完成" data-recordid="{0}"></div> ', ID);
    if (CancelOrderBtn) operationBlock += String.format('<div class="operationBtn-Cancel icon-ui-close" title="取消" data-orderno="{0}"></div> ', ID);
    operationBlock += '</div>';
    return operationBlock;
}

// 初始化 SelectInput
function InitSelectInput(SelectObj) {
    SelectObj.select2({
        matcher: CustomMacher,
        language: {
            noResults: function () {
                return "請輸入其他關鍵字，無相關資料";
            }
        },
    });
}

// 初始化 DatePicker
function InitDatePicker(DateObj) {
    DateObj.datepicker({
        dateFormat: 'yy/mm/dd',
        changeYear: true,
        changeMonth: true,
        yearRange: '1960:' + (new Date).getFullYear()
    });
}

// 初始化 DataTable
function InitDataTable(TableObj, Options) {
    TableObj.DataTable({
        scrollY: "80%",
        columnDefs: Options.ColumnDefs,
        order: Options.Order,
        pageLength: 10,
        aLengthMenu: [5, 10, 15, 20, 30],
        oLanguage: {
            sLengthMenu: '每頁揭示_MENU_ 筆',
            sZeroRecords: '無相關資訊，請更換關鍵字',
            sSearch: '',
            sSearchPlaceholder: '請輸入關鍵字',
            sInfo: '顯示第 _START_ -  _END_ 筆資料',
            sInfoEmpty: '顯示第 0 -  0 筆資料',
            sInfoFiltered: '(由 _MAX_ 項結果篩選)',
            oPaginate: {
                sFirst: '第一頁',
                sPrevious: '上一頁',
                sNext: '下一頁',
                sLast: '最後一頁'
            }
        }
    });
}

// 初始化 DataTable
function InitDataTableRowSpan(TableObj, Options) {
    TableObj.DataTable({
        scrollY: "80%",
        columnDefs: Options.ColumnDefs,
        rowsGroup: Options.RowsGroup,
        order: Options.Order,
        pageLength: 10,
        aLengthMenu: [5, 10, 15, 20, 30],
        oLanguage: {
            sLengthMenu: '每頁揭示_MENU_ 筆',
            sZeroRecords: '無相關資訊，請更換關鍵字',
            sSearch: '',
            sSearchPlaceholder: '請輸入關鍵字',
            sInfo: '顯示第 _START_ -  _END_ 筆資料',
            sInfoEmpty: '顯示第 0 -  0 筆資料',
            sInfoFiltered: '(由 _MAX_ 項結果篩選)',
            oPaginate: {
                sFirst: '第一頁',
                sPrevious: '上一頁',
                sNext: '下一頁',
                sLast: '最後一頁'
            }
        }
    });
}

// 取得介面
function GetModalContent(RequestInfo) {
    if (!RequestInfo) {
        ShowMsg('參數錯誤，請聯絡系統管理員');
        return;
    }

    $.ajax({
        url: RequestInfo.Url,
        data: RequestInfo.Data,
        type: "GET",
        async: true,
        beforeSend: function () {
            BlockUI("查詢中");
            $('#modal-title').text('');
            $('#modal-content').html('');
        },
        success: function (PartialView) {
            if (PartialView.Timeout) location.reload();

            $('#modal-title').text(RequestInfo.Title);
            $('#modal-content').html(PartialView);
            $('#modal-btn-Show').click();

            UnBlockUI();
        },
        error: function (err) {
            ShowMsg("查詢錯誤，請稍後查詢");
        }
    });
}

// 發出 Ajax Request
function SendAjaxRequest(RequestInfo) {
    $.ajax({
        url: RequestInfo.Url,
        type: "POST",
        data: RequestInfo.Data,
        async: true,
        success: function (Result) {
            if (Result.Timeout) location.reload();

            switch (RequestInfo.Type) {
                case 'FinishOrder':
                    if (Result.Status && Result.Data[0]) {
                        UpdateViewByMultiRow(Result.Data);
                    }
                    else {
                        ShowMsg(Result.Message);
                    }
                    break;

                case 'RefreshOrder':
                    if (Result && Result.length > 0) ReloadDataTable(Result);
            }
        },
        error: function (err) {
            ShowMsg("要求失敗，請稍後操作");
        }
    });
}

// 要求開始
function OnRequestBegin() {
    BlockUI("處理中");
}

// 要求失敗
function OnRequestFail() {
    ShowMsg("系統錯誤，請聯絡系統管理員。");
}

// 要求成功
function OnRequestSuccess(Result) {
    if (Result.Timeout) location.reload();

    ShowMsg(Result.Message);
    if (Result.Status && Result.Data.length > 0 && Result.Data[0]) {
        switch (Result.Type) {
            case 'Create':
            case 'Transfer':
            case 'CheckRent':
                AppendToView(Result.Data[0]);
                break;

            case 'Purchase':
            case 'Withdraw':
            case 'Deposit':
            case 'Modify':
                UpdateIssueSummary();
                AppendToView(Result.Data[0]);
                break;

            case 'Update':
            case 'CancelRent':
                UpdateView(Result.Data[0]);
                break;

            case 'Delete':
                RemoveFromView(Result.Data[0]);
                break;

            case 'CancelOrder':
                UpdateViewByMultiRow(Result.Data);
                break;

            case 'CheckOrder':
                Reset();
                break;
        }

        $('#modal-btn-Close').click();
    }
}