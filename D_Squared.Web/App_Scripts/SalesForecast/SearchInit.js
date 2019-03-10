var initSearch = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");

        $(".weekdaySelect").select2({
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

        $('.fyicon').on('click', (e) => {
            const theYear = e.currentTarget.dataset.year;
            const theDirection = e.currentTarget.dataset.direction;

            if ((theYear === '2018' && theDirection === 'left') || (theYear === '2016' && theDirection === 'right')) {
                $('.fy18col').hide();
                $('.fy17col').show();
                $('.fy16col').hide();
            }
            else if (theYear === '2017' && theDirection === 'left') {
                $('.fy18col').hide();
                $('.fy17col').hide();
                $('.fy16col').show();
            }
            else {
                $('.fy18col').show();
                $('.fy17col').hide();
                $('.fy16col').hide();
            }
        });
    };

    return {
        init: function () {
            e()
        }
    };
}();
jQuery(document).ready(function () {
    initSearch.init();
});

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