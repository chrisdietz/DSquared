var initCompetitiveEventCreate = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

        $(".createMerchandiseSelect").select2({
            dropdownParent: $('#createPartial'),
            width: '100%'
        });
    };
    return {
        init: function () {
            e();
        }
    };
}();

function CreateCompetitiveEventModal() {
    $.when($.ajax({
        url: '/Redbook/CreateCompetitiveEvent',
        type: "GET",
        success: function (data) {
            $('#createPartial').html(data);
            $('#createModal').modal('show');
        }
    })).then(function () {
        initCompetitiveEventCreate.init();
    });
}