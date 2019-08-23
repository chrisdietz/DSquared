var initSearch = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");

        $(".locationSelect").select2({
            width: '67%'
        }); 
        $(".employeeSelect").select2({
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

    $('#dtReports').DataTable({
        "order": []
    });

    $('.dataTables_length').addClass('bs-select');

});

function DisableOtherDateOption(rdBtnId) {
    if (rdBtnId == "DateId") {
        $("#SearchDTO_SelectedDate").datepicker("option", "disabled", false);
        $("#SearchDTO_SelectedDateRangeBegin").datepicker("option", "disabled", true);
        $("#SearchDTO_SelectedDateRangeEnd").datepicker("option", "disabled", true);
    } else {
        $("#SearchDTO_SelectedDate").datepicker("option", "disabled", true);
        $("#SearchDTO_SelectedDateRangeBegin").datepicker("option", "disabled", false);
        $("#SearchDTO_SelectedDateRangeEnd").datepicker("option", "disabled", false);
    }
}