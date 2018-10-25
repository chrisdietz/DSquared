var initIndex = function () {
    var e = function () {

        //hides button after submit in an attempt to prevent a double post via client side disable
        $(document).on('submit', 'form', function () {
            var button = $(this).find('input[type="submit"]');
            setTimeout(function () {
                button.attr('disabled', 'disabled');
            }, 0);
        });

        //onchange event handler to update page based on changed location or date
        $('.refreshOnChange').on('change', function () {
            $("#hiddenSearch").attr('name', 'action:SearchDate');
            document.getElementById("indexForm").submit();
        });
    };

    return {
        init: function () {
            e();
        }
    };
}();
jQuery(document).ready(function () {
    initIndex.init();
});

function GetLastYearRedbookEntryDetail(d) {
    $.when($.ajax({
        url: '/Redbook/Details',
        type: "GET",
        data: { redbookId: 0, isLastYear: true, date: d },
        success: function (data) {
            $('#detailPartial').html(data);
            $('#detailModal').modal('show');
        }
    })).then(function () {
        initDetail.init();
    });
}

function GetSalesForecastDetail(d) {
    $.when($.ajax({
        url: '/SalesForecast/Details',
        type: "GET",
        data: { date: d },
        success: function (data) {
            $('#salesForecastDetailPartial').html(data);
            $('#salesForecastDetailModal').modal('show');
        }
    })).then(function () {
        initDetail.init();
    });
}

var initDetail = function () {
    var e = function () {

        //function to prevent page sliding when scrolling inside event check list
        $.fn.scrollGuard2 = function () {
            return this
                .on('wheel', function (e) {
                    var $this = $(this);
                    if (e.originalEvent.deltaY < 0) {
                        /* scrolling up */
                        return ($this.scrollTop() > 0);
                    } else {
                        /* scrolling down */
                        return ($this.scrollTop() + $this.innerHeight() < $this[0].scrollHeight);
                    }
                });
        };
    };

    return {
        init: function () {
            e();
        }
    };
}();