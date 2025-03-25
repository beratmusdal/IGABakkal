

$(document).ready(function () {
    $('#donationTable').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.11.5/i18n/tr.json'
        },
        order: [[0, 'asc']], // Tarihe göre sıralama
        pageLength: 5, // Sayfa başına gösterilecek kayıt sayısı
        lengthMenu: [[5, 10, 25, 50], [5, 10, 25, 50]]
    });
});

function deleteProduct(id) {
    Swal.fire({
        title: "Silmek istediğinize emin misiniz?",
        text: "Hibe datası silinecektir.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Evet",
        cancelButtonText: "Hayır"
    }).then(function (result) {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: '/Product/DeleteProduct',
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: "Başarılı!",
                            text: "Silme işlemi gerçekleştirildi.",
                            icon: "success",
                            timer: 1500,
                            showConfirmButton: false
                        });

                        // Silinen satırı sayfadan kaldır
                        $("#product-row-" + id).fadeOut(500, function () {
                            $(this).remove();
                        });

                    } else {
                        Swal.fire("Hata!", response.message, "error");
                    }
                },
                error: function () {
                    Swal.fire("Hata!", "Bir hata oluştu, lütfen tekrar deneyin.", "error");
                }
            });
        }
    });
}
