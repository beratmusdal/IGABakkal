$('.stock-update-input').on('change', function () {
    var productId = $(this).attr('data-product-id');
    var newStock = $(this).val();

    console.log("Product ID:", productId);
    console.log("New Stock:", newStock);

    $.ajax({
        url: '/Daily/UpdateProductStock',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: productId, newStock: parseInt(newStock) }),
        success: function (response) {
            console.log("Gelen response:", response);

            if (response && response.success) {
                // Alert yerine kullanıcıya görsel geri bildirim
                var row = $("input[data-product-id='" + productId + "']").closest("tr");
                row.css("background-color", "#d4edda"); // Yeşil arka plan
                setTimeout(function () {
                    row.css("background-color", ""); // Geri eski haline dön
                }, 1500);
            } else {
                alert('Stok güncelleme başarısız: ' + (response.message || "Sunucu hatası"));
            }
        },
        error: function (xhr, status, error) {
            alert('Bir hata oluştu: ' + error);
        }
    });
});

$(document).ready(function () {
    $(".stock-confirm-checkbox").on("change", function () {
        var productId = $(this).data("input-id");
        var inputField = $(".stock-update-input[data-product-id='" + productId + "']");

        if ($(this).is(":checked")) {
            inputField.prop("disabled", true); // Kilitle
        } else {
            inputField.prop("disabled", false); // Aç
        }
    });
});
