//function deleteFire(id) {
//    Swal.fire({
//        title: "Silmek istediğinize emin misiniz?",
//        text: "Hibe datası silinecektir.",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Evet",
//        cancelButtonText: "Hayır"
//    }).then(function (result) {
//        if (result.isConfirmed) {
//            $.ajax({
//                dataType: 'json',
//                type: 'POST',
//                url: '/Fire/DeleteFire',
//                data: { id: id },
//                success: function (value) {
//                    location.reload();

//                },
//                failure: function (response) {

//                }
//            });
//        }
//    });
//}
function deleteFire(id) {
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
                url: '/Fire/DeleteFire',
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
                        $("#fire-row-" + id).fadeOut(500, function () {
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
