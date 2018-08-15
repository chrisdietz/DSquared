function UpdateSearchPartial(lId, mAM, mPM) {
    $('.dtoSelect').prop('disabled', true);

    $.ajax({
        url: '/Redbook/UpdateSearchPartialDropdowns',
        type: 'GET',
        data: { lId: lId, mAM: mAM, mPM: mPM },
        success: function (data) {
            $('#searchPartial').html(data);
        },
        complete: function () {
            initIndex.init();
            AddPartialPrefixes();
        }
    });
}

//used to manually add the partial viewmodel prefix for binding
//"SearchViewModel." has to be appended to the front for binding to occur
//unsure why this fails to occur on the html update for the partial 
//(binds correctly initially, but not on update)
function AddPartialPrefixes() {
    $('.locationSelect').attr("name", 'SearchViewModel.' + $('.locationSelect').attr('name'));
    $('.managerAM').attr("name", 'SearchViewModel.' + $('.managerAM').attr('name'));
    $('.managerPM').attr("name", 'SearchViewModel.' + $('.managerPM').attr('name'));
}