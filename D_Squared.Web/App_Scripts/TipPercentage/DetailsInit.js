var initDetail = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

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

function GetTipPercentagesModal(storeNbr, emplNbr, startDt, endDt) {
    $.when($.ajax({
        url: '/TipPercentage/Detail',
        type: "GET",
        data: { storeNumber: storeNbr, employeeNumber: emplNbr, startDate: startDt, endDate: endDt },
        success: function (data) {
            $('#employeeDetailPartial').html(data);
            $('#employeeSalesAndTipsModal').modal('show');
        }
    })).then(function () {
        initDetail.init();
    });
}

