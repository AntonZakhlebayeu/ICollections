let filter_select_el = document.getElementById('filter');
let items_el = document.getElementById('collections');

filter_select_el.onchange = function() {
    let items = items_el.getElementsByClassName('collection');
    for (let i=0; i<items.length; i++) {
        if (items[i].classList.contains(this.value)) {
            items[i].style.display = 'block';
        } else {
            items[i].style.display = 'none';
        }
    }
};