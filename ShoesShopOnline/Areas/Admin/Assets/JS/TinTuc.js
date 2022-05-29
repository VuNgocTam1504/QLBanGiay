$(document).ready(function () {
    $('.btnDelete').off('click').on('click', function () {
        let id = $(this).attr("dataname");
        swal({
            title: "Xác nhận xóa?",
            text: "Bạn đã chắc chắn xóa bài viết này ?",
            icon: "info",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { "id": id },
                        url: '/TinTucs/Delete',
                        success: function (response) {
                            if (response.status == true) {
                                $("#row-" + id).remove();
                            }
                        },
                        error: function (response, xhr) {
                            console.log(xhr.responseText);
                            alert("Error has occurred..");
                        }
                    });
                }

            })
    })
});