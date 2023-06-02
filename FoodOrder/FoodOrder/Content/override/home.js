$(document).ready(function () {
    var timeoutId;
    var defaultSearchUrl = '/Products/IndexByProductName';

    $('#search-input').on('input', function () {
        clearTimeout(timeoutId);

        var searchTerm = $('#search-input').val();

        // Cập nhật nội dung và URL cho phần tử kết quả mặc định
        $('#default-search-result').attr('href', defaultSearchUrl + '?searchTerm=' + searchTerm);
        $('#search-term').text(searchTerm);

        // Hiển thị phần tử kết quả ngay lập tức
        $('#search-results').addClass('show');

        timeoutId = setTimeout(function () {
            // Gọi AJAX sau 1 giây
            $.ajax({
                url: '/Products/SearchProducts',
                type: 'GET',
                data: { searchTerm: searchTerm },
                dataType: 'json',
                success: function (data) {
                    // Xử lý dữ liệu trả về từ action
                    if (data.length > 0) {
                        var resultsHtml = '';

                        for (var i = 0; i < data.length; i++) {
                            var product = data[i];
                            resultsHtml += '<a href="' + product.Url + '">' + product.Name + '</a>';
                        }

                        $('#search-results').append(resultsHtml);
                    } else {
                        $('#search-results').html('<span>No results found.</span>');
                    }
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi tìm kiếm.');
                }
            });
        }, 1000); // Khoảng thời gian chờ 1 giây
    });

    $(document).on('click', function (e) {
        var target = $(e.target);
        var searchInput = $('#search-input');
        var searchResults = $('#search-results');

        if (!target.is(searchInput) && !target.is(searchResults) && !searchResults.has(target).length) {
            searchInput.val('');
            searchResults.removeClass('show').empty();
            searchResults.html('<a id="default-search-result" href="' + defaultSearchUrl + '">Tìm kiếm <span id="search-term">All</span> trong shop</a>');
        }
    });
});

function addToWishlist(productId, userId) {
    $.ajax({
        url: '/Products/AddToWishlist',
        type: 'POST',
        data: { productID: productId, userID: userId },
        dataType: 'json',
        success: function (data) {
            alert(data.message);
        },
        error: function () {
            console.log('Đã xảy ra lỗi khi thêm vào danh sách yêu thích.');
        }
    });
}

function removeFromWishlist(productId, userId) {
    $.ajax({
        url: '/Products/RemoveFromWishlist',
        type: 'POST',
        data: { productID: productId, userID: userId },
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            window.location.reload();
        },
        error: function () {
            console.log('Đã xảy ra lỗi khi xóa khỏi danh sách yêu thích.');
        }
    });
}



