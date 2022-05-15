const items = [...document.querySelectorAll('.collection')];
let selected;

function compare(a, b) {
    const aData = new Date($(a).data('event-date'));
    const bData = new Date($(b).data('event-date'));
    switch (selected) {
        case "ascending":
            if(aData < bData)
            {
                return -1;
            }
            if (aData > bData) {
                return 1;
            }
            return 0;
        case "descending":
            if(aData > bData)
            {
                return -1;
            }
            if (aData < bData) {
                return 1;
            }
            return 0;
    }
}

$('#sort').on('change', () => {
    selected = document.getElementById("sort").value
    items.sort(compare);
    $(".collection-description").remove();
    $('#collections').empty();
    items.forEach(item => {
        $('#collections').append(item);
    });
});