$(function () {
    /****************** 頁面初始化 ******************/
    // 初始化餐點選單
    InitSelectInput($('#order-ChoiceFood'));
    InitSelectInput($('#order-ChoiceDrink'));
    InitSelectInput($('#order-ChoiceOther'));

    /****************** 事件初始化 ******************/
    // 食物點餐
    $(document).on('click', '#operationBtn-OrderFood', function () {
        AddDetail({
            RecordID: $('#order-ChoiceFood').val(),
            Meal: $('#order-ChoiceFood option:selected').text(),
            Type: '餐點',
            Quantity: $('#order-QuantityFood').val(),
            Integration: $('#order-ChoiceFood option:selected').data('integration')
        });
        CalculateSum();
        ResetOrderBlock(true, false, false);
    });

    // 飲料點餐
    $(document).on('click', '#operationBtn-OrderDrink', function () {
        AddDetail({
            RecordID: $('#order-ChoiceDrink').val(),
            Meal: $('#order-ChoiceDrink option:selected').text(),
            Type: '飲料',
            Quantity: $('#order-QuantityDrink').val(),
            Integration: $('#order-ChoiceDrink option:selected').data('integration')
        });
        CalculateSum();
        ResetOrderBlock(false, true, false);
    });

    // 其他點餐
    $(document).on('click', '#operationBtn-OrderOther', function () {
        AddDetail({
            RecordID: $('#order-ChoiceOther').val(),
            Meal: $('#order-ChoiceOther option:selected').text(),
            Type: '其他',
            Quantity: $('#order-QuantityOther').val(),
            Integration: $('#order-ChoiceOther option:selected').data('integration')
        });
        CalculateSum();
        ResetOrderBlock(false, false, true);
    });

    // 明細數量異動
    $(document).on('change', '.orderDetail-quantity', function () {
        var integration = parseInt($(this).data('integration'));
        var amount = parseInt($(this).val()) * integration || 0;

        $(this).parent().parent().find('.orderDetail-amount').first().text(amount);
        CalculateSum();
    });

    // 刪除
    $(document).on('click', '.operationBtn-Delete', function () {
        $(this).parent().parent().remove();
        CalculateSum();
    });

    // 清除
    $(document).on('click', '#operationBtn-Clear', function () {
        Reset();
    });

    // 結帳
    $(document).on('click', '.operationBtn-Check', function () {
        if ($('#table-OrderList-Body .orderDetail-quantity').length <= 0) {
            ShowMsg('請新增餐點後再行結帳');
            return;
        }

        GetOrderModal($(this).data('type'));
    });
});

// 重置頁面
function Reset() {
    ResetOrderBlock(true, true, true);
    ResetDetailBlock();
    CalculateSum();
}

// 重置點餐區
function ResetOrderBlock(Food, Drink, Other) {
    if (Food) {
        $('#order-ChoiceFood').val($("#order-ChoiceFood option:first").val()).trigger('change.select2');;
        $('#order-QuantityFood').val('1');
    }

    if (Drink) {
        $('#order-ChoiceDrink').val($("#order-ChoiceDrink option:first").val()).trigger('change.select2');;
        $('#order-QuantityDrink').val('1');
    }

    if (Other) {
        $('#order-ChoiceOther').val($("#order-ChoiceOther option:first").val()).trigger('change.select2');;
        $('#order-QuantityDrink').val('1');
    }
}

// 重置餐點明細
function ResetDetailBlock() {
    $('#table-OrderList-Body').empty();
}

// 新增餐點明細
function AddDetail(Data) {
    var amount = parseInt(Data.Integration) * parseInt(Data.Quantity);
    var content = '';

    content += '<tr>';
    content += String.format('<td>{0}</td>', Data.Meal);
    content += String.format('<td>{0}</td>', Data.Type);
    content += String.format('<td class="orderDetail-amount">{0}</td>', amount);
    content += String.format('<td><input type="number" class="orderDetail-quantity" value="{0}" min="1" step="1" data-mealid="{1}" data-integration="{2}"/></td>', Data.Quantity, Data.RecordID, Data.Integration);
    content += String.format('<td><div class="btn operationBtn-Delete" title="刪除"><span class="icon-ui-trash"></span></div></td>');
    content += '</tr>';

    $('#table-OrderList-Body').append(content);
}

// 計算總金額
function CalculateSum() {
    var sum = 0;

    $('#table-OrderList-Body .orderDetail-quantity').each(function () {
        var integration = parseInt($(this).data('integration'));
        sum += parseInt($(this).val()) * integration;
    });

    $('#order-Sum').text(sum || 0);
}

// 取得點餐資料
function GetOrderModal(CheckType) {
    var data = { PaymentType: CheckType, Order: [] };
    var sumQuantity = 0;

    $('#table-OrderList-Body .orderDetail-quantity').each(function () {
        data.Order.push({
            MealID: parseInt($(this).data('mealid')),
            Quantity: parseInt($(this).val())
        });

        sumQuantity += parseInt($(this).val());
    });

    // 驗證餐點數量
    sumQuantity = sumQuantity || 0;
    if (sumQuantity <= 0) {
        ShowMsg('總餐點數量為 0 客，請確認餐點資訊');
        return;
    }

    $.ajax({
        url: Config.SiteRoot + 'Order/Check',
        type: "POST",
        data: data,
        async: true,
        beforeSend: function () {
            BlockUI("查詢中");
            $('#modal-title').text('');
            $('#modal-content').html('');
        },
        success: function (PartialView) {
            if (PartialView.Timeout) location.reload();

            $('#modal-title').text('餐點結帳');
            $('#modal-content').html(PartialView);
            $('#modal-btn-Show').click();

            UnBlockUI();
        },
        error: function (err) {
            ShowMsg("查詢錯誤，請稍後查詢");
        }
    });
}