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