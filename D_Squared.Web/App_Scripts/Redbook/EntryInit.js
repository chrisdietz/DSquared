var initEntry = function () {
    var e = function () {

        $.fn.select2.defaults.set("theme", "bootstrap");

        //$(".dateSelect").select2({
        //    width: '40%'
        //});

        $(".weatherSelect").select2({
            width: '100%'
        });

        $(".managerSelect").select2({
            width: '100%'
        });

        //$(".locationSelect").select2({
        //    width: '40%'
        //});

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
                })
                ;
        };

        //hides button after submit in an attempt to prevent a double post via client side disable
        $(document).on('submit', 'form', function () {
            var button = $(this).find('input[type="submit"]');
            setTimeout(function () {
                button.attr('disabled', 'disabled');
            }, 0);
        });

        //onchange event handler to update page based on changed location or date
        $('.refreshOnChange').on('change', function () {
            $("#hiddenRefresh").attr('name', 'action:Entry');
            document.getElementById("entryForm").submit();
        });
    };

    //evens out panel heights for passed row based on largest height in row
    var heightAdjust = function (x) {
        var heights = $(x).map(function () {
            return $(this).height();
        }).get(),

            maxHeight = Math.max.apply(null, heights);

        $(x).height(maxHeight);
    };
    return {
        init: function () {
            e(),
            heightAdjust(".firstRowPanel"),
            heightAdjust(".thirdRowPanel");
        }
    };
}();
jQuery(document).ready(function () {
    initEntry.init();
});