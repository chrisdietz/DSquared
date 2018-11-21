var initIndex = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");

        $(".weatherSelect").select2({
            width: '67%'
        });

        $(".managerSelect").select2({
            width: '67%'
        });

        $(".locationSelect").select2({
            width: '67%'
        });

        $(function () {
            $(".date-picker").datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "-1:+0",
                dateFormat: 'mm-dd-yy'
            });
        });

        $("#searchSubmit").prop("disabled", false);
    };

    return {
        init: function () {
            e()
        }
    };
}();
jQuery(document).ready(function () {
    initIndex.init();
});

function GetRedbookEntryDetail(r) {
    $.when($.ajax({
        url: '/Redbook/Details',
        type: "GET",
        data: { redbookId: r, isLastYear: false },
        success: function (data) {
            $('#detailPartial').html(data);
            $('#detailModal').modal('show');
        }
    })).then(function () {
        initDetail.init();
    });
}

function GetSalesForecastDetail(d, sn) {
    $.when($.ajax({
        url: '/SalesForecast/Details',
        type: "GET",
        data: { date: d, storeNumber: sn },
        success: function (data) {
            $.when($('#salesForecastDetailPartial').empty())
                .then(function () {
                    $('#salesForecastDetailPartial').html(data);
                    $('#salesForecastDetailModal').modal('show');
            });
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