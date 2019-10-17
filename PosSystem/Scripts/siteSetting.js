$(function () {
    /****************** 事件初始化 ******************/
    // 事件 - 編輯
    $(document).on('click', '.operationBtn-Update', function () {
        GetModalContent({
            Title: '編輯帳號',
            Url: Config.SiteRoot + 'Setting/Update',
            Data: { RecordID: $(this).data('recordid') }
        });
    });
});


// 更新畫面
function UpdateView(Data) {
    $('#setting-Name').text(Data.Name);
    $('#setting-IdentityNumber').text(Data.IdentityNumber);
    $('#setting-BirthDay').text(String.formatDateTime(Data.BirthDay).substr(0, 10));
    $('#setting-Phone').text(Data.Phone);
    $('#setting-Address').text(Data.Address);
    $('#setting-Email').text(Data.Email);
    $('#setting-LineID').text(Data.LineID);
    $('#setting-WeChatID').text(Data.WeChatID);
    $('#setting-UserPwd').text(Data.UserPwd);
    $('#setting-AuthLevel').text(Data.AuthLevel);
    $('#setting-ConstructTime').text(String.formatDateTime(Data.ConstructTime));
    $('#setting-UpdateTime').text(String.formatDateTime(Data.UpdateTime));
}