$(function () {
    /****************** 頁面初始化 ******************/
    // 加入日期篩選器
    DataTable.AddDateFilter();

    // 初始化 DataTable
    InitDataTable($('#table-Operation'), {
        Order: [[4, 'desc']],
        ColumnDefs: [{ width: '150px', targets: 4 }, { width: '100px', targets: 5 }]
    });

    // 初始化 DatePicker
    InitDatePicker($('.dateFilter-Operation'));

    /****************** 事件初始化 ******************/
    // 事件 - 購買
    $(document).on('click', '.operationBtn-Purchase', function () {
        GetModalContent({
            Title: '積分購買',
            Url: Config.SiteRoot + 'Operation/Purchase',
            Data: null
        });
    });

    // 事件 - 提領
    $(document).on('click', '.operationBtn-Withdraw', function () {
        GetModalContent({
            Title: '積分提領',
            Url: Config.SiteRoot + 'Operation/Withdraw',
            Data: null
        });
    });

    // 事件 - 回存
    $(document).on('click', '.operationBtn-Deposit', function () {
        GetModalContent({
            Title: '積分回存',
            Url: Config.SiteRoot + 'Operation/Deposit',
            Data: null
        });
    });

    // 事件 - 轉移
    $(document).on('click', '.operationBtn-Transfer', function () {
        GetModalContent({
            Title: '積分轉移',
            Url: Config.SiteRoot + 'Operation/Transfer',
            Data: null
        });
    });

    // 事件 - 修正
    $(document).on('click', '.operationBtn-Modify', function () {
        GetModalContent({
            Title: '積分修改',
            Url: Config.SiteRoot + 'Operation/Modify',
            Data: null
        });
    });

    // 事件 - 篩選日期異動
    $(document).on('change', '.dateFilter-Operation', function () {
        DataTable.Refresh($('#table-Operation'));
    });

    // 事件 - 積分操作 - 會員異動
    $(document).on('change', '.operation-Member', function () {
        var option = $(this).find('option:selected');
        var balance = option.data('integration');
        var isWithdraw = $('.operation-Integration-Withdraw').length > 0 ? true : false;

        // 設定積分餘額
        $('.operation-Balance').val(balance);

        // 判斷是否需提供使用者密碼
        if (isWithdraw) {
            var freeChangeAmount = parseInt(option.data('freechangeamount'));
            var integrationWithdraw = parseInt($('.operation-Integration-Withdraw').val()) || 0;

            if (integrationWithdraw > freeChangeAmount) {
                $('.operation-MemberPwd').prop('disabled', false);
            }
            else {
                $('.operation-MemberPwd').prop('disabled', true);
            }
        }
    });

    // 事件 - 積分操作 - 提領積分異動
    $(document).on('change', '.operation-Integration-Withdraw', function () {
        var freeChangeAmount = parseInt($('.operation-Member').find('option:selected').data('freechangeamount'));
        var integrationWithdraw = parseInt($(this).val()) || 0;

        if (integrationWithdraw > freeChangeAmount) {
            $('.operation-MemberPwd').prop('disabled', false);
        }
        else {
            $('.operation-MemberPwd').prop('disabled', true);
        }
    });
});

// 加入資料列
function AppendToView(Data) {
    var rowData = [Data.MemberName, Data.OperationType, Data.Integration, Data.Note, String.formatDateTime(Data.UpdateTime), Data.OperatorName];
    DataTable.AddRow($('#table-Operation'), rowData);
}