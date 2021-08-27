function applyCssTableStriped() {
    $('.table-striped > tbody > tr:odd > td').addClass('odd');
    $('.table-striped > tbody > tr:even > td').addClass('even');
}

$(document).ready(function() {
    applyCssTableStriped();
});