var initCompetitiveEventCreate = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

        $(".createDistanceSelect").select2({
            dropdownParent: $('#createPartial'),
            width: '100%'
        });

        $(".date-picker").datepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: "-1:+0",
            dateFormat: 'mm/dd/yy'
        });
    };
    return {
        init: function () {
            e();
        }
    };
}();

function CreateCompetitiveEventModal(r) {
    $.when($.ajax({
        url: '/Redbook/CreateCompetitiveEvent',
        type: "GET",
        data: {redbookId: r},
        success: function (data) {
            $('#createPartial').html(data);
            $('#createModal').modal('show');
        }
    })).then(function () {
        initCompetitiveEventCreate.init();
    });
}