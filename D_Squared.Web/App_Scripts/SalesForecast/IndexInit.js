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