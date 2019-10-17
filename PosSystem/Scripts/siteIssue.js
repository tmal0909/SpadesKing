$(function () {
    /****************** 事件初始化 ******************/
    // 事件 - 發行紀錄
    $(document).on('click', '.operationBtn-IssueRecord', function () {
        GetModalContent({
            Title: '發行紀錄',
            Url: Config.SiteRoot + 'Issue/Record',
            Data: null
        });
    });

    // 事件 - 發行紀錄 - 篩選日期異動
    $(document).on('change', '.dateFilter-Issue', function () {
        DataTable.Refresh($('#table-Issue'));
    });
});

// 更新發行概況
function UpdateIssueSummary() {
    $.ajax({
        url: Config.SiteRoot + 'Issue/GetSummary',
        type: 'POST',
        async: true,
        success: function (Data) {
            if (Data.Timeout) location.reload();
            $('#issueSummary-Distribution').text(Data.Distribution);
            $('#issueSummary-Balance').text(Data.Balance);
        },
        error: function (err) {
            ShowMsg("查詢錯誤，請稍後查詢");
        }
    });
}