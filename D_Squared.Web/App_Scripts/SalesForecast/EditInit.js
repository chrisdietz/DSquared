var initDetailEdit = function () {
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

function GetSalesForecastDetailEditModal(d, s) {
    $.when($.ajax({
        url: '/SalesForecast/EditDetail',
        type: "GET",
        data: { date: d, storeNumber: s },
        success: function (data) {
            $('#editDetailPartial').html(data);
            $('#editDetailModal').modal('show');
        }
    })).then(function () {
        initDetailEdit.init();
    });
}



var initEdit = function () {
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

function GetSalesForecastEditModal(id) {
    $.when($.ajax({
        url: '/SalesForecast/Edit',
        type: "GET",
        data: { id: id },
        success: function (data) {
            $('#editPartial').html(data);
            $('#editModal').modal('show');
        }
    })).then(function () {
        initEdit.init();
    });
}