$('.stock-update-input').on('change', function () {
    var productId = $(this).attr('data-product-id'); // .data yerine .attr kullan
    var newStock = $(this).val();

    console.log("Product ID:", productId); // Hangi değer alındığını görmek için
    console.log("New Stock:", newStock);

    $.ajax({
        url: '/Daily/UpdateProductStock',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: productId, newStock: parseInt(newStock) }),
        success: function (response) {
            if (response.success) {
                alert('Stok başarıyla güncellendi!');
                location.reload();
            } else {
                alert('Stok güncelleme başarısız: ' + response.message);
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

