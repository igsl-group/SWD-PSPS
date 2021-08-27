function jqGridYesNoFormatter(cellvalue, options, rowObject) {
    return true === cellvalue ? "Yes" : "No";
};

function jqGridUnformatYesNo(cellvalue, options) {
    return "Yes" === cellvalue ? true : false;
};

function datePick(elem) {
    $(elem).datepicker({
        showButtonPanel: true
    });
};

var dateTemplate = {
    sorttype: 'date',
    formatter: 'date',
    formatoptions: {
        srcformat: 'Y/m/d H:i:s',
        newformat: 'd/m/Y'
    },
    editoptions: { dataInit: datePick, size: 11 },
    searchoptions: {
        dataInit: datePick,
        sopt: ['eq', 'le', 'ge']
    }
};

var dateTimeTemplate = {
    sorttype: 'date',
    formatter: 'date',
    formatoptions: {
        srcformat: 'Y/m/d H:i:s',
        newformat: 'd/m/Y H:i:s'
    },
    searchoptions: {
        dataInit: datePick,
        sopt: ['eq', 'le', 'ge']
    }
};

var numTemplate = {
    sorttype: 'integer',
    searchoptions: {
        sopt: ['eq', 'le', 'ge']
    }
};

var yesNoTemplate = {
    formatter: jqGridYesNoFormatter,
    unformat: jqGridUnformatYesNo,
    stype: 'select',
    searchoptions: {
        sopt: ['eq'],
        value: "true:Yes;false:No"
    }
};

var rowActionTemplate = {
    width: 70,
    fixed: true,
    sortable: false,
    search: false,
    resize: false,
    formatter: 'actions',
    formatoptions: {
        keys: true,
        delOptions: {
            recreateForm: true,
            beforeShowForm: beforeDeleteCallback,
        },
        editbutton: false,
        delbutton: false
    }
};