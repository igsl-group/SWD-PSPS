if (typeof String.prototype.trunc !== 'function') {
	String.prototype.trunc = function (n, useWordBoundary) {
		var toLong = this.length > n,
			s_ = toLong ? this.substr(0, n - 1) : this;
		s_ = useWordBoundary && toLong ? s_.substr(0, s_.lastIndexOf(' ')) : s_;
		return toLong ? s_ + '&hellip;' : s_;
	}
}
if (typeof String.prototype.trim !== 'function') {
	String.prototype.trim = function () {
		return this.replace(/^\s+|\s+$/g, '');
	}
}
if (typeof String.prototype.capitalize !== 'function') {
	String.prototype.capitalize = function () {
		return this.charAt(0).toUpperCase() + this.slice(1);
	}
}

//global settings
//validator
$.validator.setDefaults({ ignore: ".data-val-ignore, :hidden, :disabled" });
$.validator.methods.date = function (value, element) {
	return this.optional(element) || moment(value, "D/M/YYYY").isValid();
};
$.validator.methods.range = function (value, element, param) {
	var globalizedValue = value.replace(/,/g, "");
	return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}
//datepicker
$.fn.datepicker.defaults.startDate = "-20y";
$.fn.datepicker.defaults.format = "dd/mm/yyyy";
$.fn.datepicker.defaults.autoclose = true;
$.fn.datepicker.defaults.todayHighlight = true;
$.fn.datepicker.defaults.enableOnReadonly = false;

//timepicker
$.fn.datetimepicker.defaults.format = "dd/mm/yyyy hh:ii";
$.fn.datetimepicker.defaults.autoclose = true;
$.fn.datetimepicker.defaults.todayHighlight = true;

//select2
$.fn.select2.defaults.dropdownAutoWidth = false;
$.fn.select2.defaults.containerCss = 'overflow:hidden';

//jqgrid
$.extend(jQuery.jgrid.defaults, {
	autowidth: true,
	height: "100%",
	ignoreCase: true,
	rownumbers: true,
	rownumWidth: 38,
	viewrecords: true,
	rowNum: 10,
	rowList: [10, 20, 30],
	//multiSort: true,
	altRows: false,
	toppager: false,
	multiselect: false,
	//multikey: "ctrlKey",
	multiboxonly: false,
	loadui: 'disable',
	jsonReader: {
		repeatitems: false,
		root: "data.data",
		page: "data.currentPageIndex",
		total: "data.totalPages",
		records: "data.totalCount"
	},
	loadError: defaultAjaxErrorHandler
});
$.extend(jQuery.jgrid.nav, {
	editicon: 'icon-edit',
	addicon: 'icon-plus',
	delicon: "ui-icon-trash",
	searchicon: 'icon-search',
	refreshicon: 'icon-refresh',
	viewicon: "icon-eye-open",
	edit: false,
	add: false,
	del: false,
	view: false,
	search: false,
	refresh: false,
	refreshtitle: 'Reset Grid',
	refreshstate: 'current'
});
$.extend(jQuery.jgrid.search, {
	sopt: ['eq', 'bw', 'ew', 'cn'],
	multipleSearch: true,
	multipleGroup: true,
	showQuery: false,
	closeOnEscape: true,
	closeAfterSearch: true,
	caption: "Advanced Search",
	// JQGrid Advance Search Width
	width: 1024,
});

//blockUI
$.blockUI.defaults.message = '<h2><img src="/Content/img/loading.gif" />&nbsp;&nbsp;Processing...</h2>';
$.blockUI.defaults.baseZ = 2000;
$.blockUI.defaults.fadeIn = 0;
$.blockUI.defaults.fadeOut = 0;
$.blockUI.defaults.ignoreIfBlocked = true;
$(document).ajaxStart(function () {
	$("#ui_notifIt").remove();
	$.blockUI();
}).ajaxStop(function () {
	$.unblockUI();
});

$.extend(Inputmask.prototype.defaults, {
	'digitsOptional': false
});

Inputmask.extendAliases({
	'pspsDecimal': {
		alias: 'decimal',
		groupSeparator: ',',
		autoGroup: true,
		digitsOptional: false,
		digits: 2
	}
});

//regex support for jquery
jQuery.expr[':'].regex = function (elem, index, match) {
	var matchParams = match[3].split(','),
		validLabels = /^(data|css):/,
		attr = {
			method: matchParams[0].match(validLabels) ? matchParams[0].split(':')[0] : 'attr',
			property: matchParams.shift().replace(validLabels, '')
		},
		regexFlags = 'ig',
		regex = new RegExp(matchParams.join('').replace(/^\s+|\s+$/g, ''), regexFlags);
	return regex.test(jQuery(elem)[attr.method](attr.property));
}

//Helper functions

//convert the datetime to dd/MM/yyyy format
function toDDMMYYYY(s) {
	if (s)
		return moment(s).format('DD/MM/YYYY');
	return '';
}

function clearForm(formSelector) {
	var $form = $(formSelector);
	$form.find('input').val('');
	//$form.find(":input[data-provide='select2']").select2('val', '');
	$form.find(":input[data-provide='select2']").select2('val', []);
	$form.find("label.checkbox-inline input[type=hidden]:first-child").checkboxVal(false);
	$form.find('textarea').text('');
	$form.find('textarea').val('');

	$form.find('.alert.alert-block.alert-warning.validation-summary-errors').remove();
};

function addRequired(searchClass) {
	$(searchClass).each(function () {
		var $this = $(this);
		var $label = $this.parents().prev("label[for='" + $this.attr('name').replace('.', '_') + "']");

		if ($label.length === 0) {
			$label = $this.prevAll('label:first');
		}

		if ($label.length === 0) {
			// label wasn't found
		} else if ($label.find('abbr').length == 0) {
			$label.html($label.html() + ' <abbr title="Required" class="red">*</abbr>');
		}
	});
};

function removeRequired(searchClass) {
	$(searchClass).each(function () {
		var $this = $(this);
		var $label = $this.parents().prev("label[for='" + $this.attr('name') + "']");

		if ($label.length === 0) {
			$label = $this.prevAll('label:first');
		}

		if ($label.length === 0) {
			// label wasn't found
		} else {
			$label.find('abbr').remove();
		}
	});
};

function addCalendarIcon(searchClass) {
	$(searchClass).each(function () {
		var $this = $(this);
		var $parent = $this.parent();
		var calendar = '<span class="input-group-addon"><i class="icon-calendar"></i></span>';
		if ($parent.hasClass('input-group') && $parent.children().length === 0) {
			$parent.addClass('date');
			$parent.on('click', function () {
				$this.focus();
			});
			$this.after(calendar);
		} else {
			var inputGroup = $("<div>", {
				"class": "input-group date",
				"style": "width: 130px;"
			}).on('click', function () {
				$this.focus();
			});
			$this.wrap($("<div>", {
				"class": "form-inline inline"
			})).wrap(inputGroup)
				.after(calendar);
		}
	});
};

function initCustomDataApi() {
	$("button[data-href]").click(function (e) {
		e.preventDefault();
		redirectTo($(this).data("href"));
	});

	$(":input[data-provide='select2']").each(function (e) {
		var $this = $(this),
			placeholder = $this.prop('placeholder') || " Please select ";

		$this.select2({
			placeholder: placeholder,
			allowClear: true
		}).focus(function () {
			$(this).select2('focus');
		});
	});
};

function redirectTo(href) {
	$.blockUI();
	location.href = href;
}

function formatSelect2CodeDescription(result, container, query, escapeMarkup) {
	if (!result.id) return result.text;
	return result.id + " - " + result.text;
};

function styleEditForm(form) {
	//update buttons classes
	var buttons = form.next().find('.EditButton .fm-button');
	buttons.addClass('btn btn-sm').find('[class*="-icon"]').remove(); //ui-icon, s-icon
	buttons.eq(0).addClass('btn-primary').prepend('<i class="icon-ok"></i>');
	buttons.eq(1).prepend('<i class="icon-remove"></i>')

	buttons = form.next().find('.navButton a');
	buttons.find('.ui-icon').remove();
	buttons.eq(0).append('<i class="icon-chevron-left"></i>');
	buttons.eq(1).append('<i class="icon-chevron-right"></i>');
};

function styleDeleteForm(form) {
	var buttons = form.next().find('.EditButton .fm-button');
	buttons.addClass('btn btn-sm').find('[class*="-icon"]').remove(); //ui-icon, s-icon
	buttons.eq(0).addClass('btn-primary').prepend('<i class="icon-trash"><\/i>');
	buttons.eq(1).prepend('<i class="icon-remove"><\/i>')
};

function styleSearchFilters(form) {
	form.find('.add-rule').addClass('btn btn-xs btn-primary').val('+');
	form.find('.delete-rule').addClass('btn btn-xs btn-primary').val('-');
	form.find('.add-group').addClass('btn btn-xs btn-primary').val('+{}');
	form.find('.delete-group').addClass('btn btn-xs btn-primary').val('-');
};

function styleSearchForm(form) {
	var dialog = form.closest('.ui-jqdialog');
	var buttons = dialog.find('.EditTable')
	buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-primary').find('.ui-icon').attr('class', 'icon-retweet');
	buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-primary').find('.ui-icon').attr('class', 'icon-comment-alt');
	buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-primary').find('.ui-icon').attr('class', 'icon-search');
};

function beforeDeleteCallback(e) {
	var form = $(e[0]);
	if (form.data('styled')) return false;

	form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
	styleDeleteForm(form);

	form.data('styled', true);
};

//it causes some flicker when reloading or navigating grid
//it may be possible to have some custom formatter to do this as the grid is being created to prevent this
//or go back to default browser checkbox styles for the grid
function styleCheckbox(table) {
	/**
  $(table).find('input:checkbox').addClass('ace')
  .wrap('<label />')
  .after('<span class="lbl align-top" />')

  $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
  .find('input.cbox[type=checkbox]').addClass('ace')
  .wrap('<label />').after('<span class="lbl align-top" />');
*/
};

//unlike navButtons icons, action icons in rows seem to be hard-coded
//you can change them like this in here if you want
function updateActionIcons(table) {
	var replacement = {
		'ui-icon-plus': 'icon-edit blue',
		'ui-icon-trash': 'icon-trash red',
		'ui-icon-disk': 'icon-ok green',
		'ui-icon-cancel': 'icon-remove red'
	};
	$(table).find('.ui-pg-div span.ui-icon').each(function () {
		var icon = $(this);
		var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
		if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
	})
};

//replace icons with FontAwesome icons like above
function updatePagerIcons(table) {
	var replacement = {
		'ui-icon-seek-first': 'icon-double-angle-left bigger-140',
		'ui-icon-seek-prev': 'icon-angle-left bigger-140',
		'ui-icon-seek-next': 'icon-angle-right bigger-140',
		'ui-icon-seek-end': 'icon-double-angle-right bigger-140'
	};
	$('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
		var icon = $(this);
		var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

		if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
	})
};

function enableTooltips(table) {
	$('.navtable .ui-pg-button').tooltip({
		container: 'body'
	});
	$(table).find('.ui-pg-div').tooltip({
		container: 'body'
	});
};

function defaultGridLoadComplete(grid) {
	setTimeout(function () {
		styleCheckbox(grid);
		updateActionIcons(grid);
		updatePagerIcons(grid);
		enableTooltips(grid);
	}, 0);
};

function defaultSearchFormAfterShowSearch(e) {
	var form = $(e[0]);
	form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
	styleSearchForm(form);
};

function defaultSearchFormAfterRedraw() {
	styleSearchFilters($(this));
};

function styleBootboxFooterButton($bootbox) {
	var $btnOk = $bootbox.find('.modal-footer button[data-bb-handler=confirm]');
	var $btnCancel = $bootbox.find('.modal-footer button[data-bb-handler=cancel]');

	$btnOk.removeClass().addClass('btn btn-primary btn-sm').html('<span class="icon-ok"></span>&nbsp;OK');
	$btnCancel.removeClass().addClass('btn btn-default btn-sm').html('<span class="icon-remove"></span>&nbsp;Cancel');

	$btnOk.insertBefore($btnCancel);
};

function rowDeleteButton(cellValue, options, rowObject) {
	return '<div title="Delete selected row" class="ui-pg-div ui-inline-del" id="jDeleteButton_1" onmouseover="jQuery(this).addClass(\'ui-state-hover\');" onmouseout="jQuery(this).removeClass(\'ui-state-hover\');">\
				<span class="ui-icon ui-icon-trash"></span>\
			</div>';
};

function confirmDelete(callback) {
	return confirm("Are you sure you want to delete the record?", callback);
};

function confirm(message, callback) {
	var $confirm = bootbox.confirm("<h4>" + message + "</h4>", callback);
	styleBootboxFooterButton($confirm);
	return $confirm;
};

function prompt(title, callback) {
	var $prompt = bootbox.prompt(title, callback);
	styleBootboxFooterButton($prompt);
	return $prompt;
};

function alert(message, callback) {
	var $alert = bootbox.alert("<h4>" + message + "</h4>", callback);
	return $alert;
};

function scrollTo(id) {
	var navbarHeight = $('#navbar').height();
	var $id = typeof id === "string" ? $("#" + id) : id;
	if ($id.offset().top > 0) {
		$('html,body').animate({
			scrollTop: $id.offset().top - navbarHeight
		}, 'fast');
	} else {
		$id.parents('div[id]').first().animate({
			scrollTop: 0
		}, 'fast');
	}
};

function getColumnIndexByName(grid, columnName) {
	var cm = grid.jqGrid('getGridParam', 'colModel'),
		i, l = cm.length;
	for (i = 0; i < l; i++) {
		if (cm[i].name === columnName) {
			return i; // return the index
		}
	}
	return -1;
};

function getValidationSummary($form) {
	var $el;
	if (typeof $form != 'undefined')
		$el = $form.find('.alert.alert-block.alert-warning.validation-summary-errors');

	if (typeof $el == 'undefined' || $el.length == 0) {
		var $fieldset = $form.find('fieldset:first');
		$el = $('<div id="validationSummary" class="alert alert-block alert-warning validation-summary-errors" data-valmsg-summary="true"><div><strong>Please fix the following errors.</strong></div><ol></ol></div>')
				 .hide()
				 .insertBefore($fieldset)
				 .find('ol');
	} else {
		$el = $el.hide().find('ol');
	}
	return $el;
};

function processServerSideValidationError(response, form, summaryElement) {
	var $list,
		data = (response && response.errors) ? response : null;
	if (!data) return false;
	$list = summaryElement || getValidationSummary($(form));
	$list.html('');
	$.each(data.errors, function (fieldName, item) {
		fieldName = fieldName.capitalize().replace('.', '\\.'); //capitalize the first letter
		var $form = $(form), field = $form.find(':input[name="' + fieldName + '"]');
		var $val, errorList = "", fieldId = field.length ? field[0].id.replace('.', '\\.') : "";
		$form.find(".has-error").removeClass("has-error");

		if (fieldName) {
			$val = $form.find(".field-validation-valid,.field-validation-error")
						.first('[data-valmsg-for="' + fieldName + '"]')
						.removeClass("field-validation-valid")
						.addClass("field-validation-error");
			$form.find("#" + fieldId).addClass("input-validation-error");
			//$form.find("#" + fieldId).parentsUntil("div.form-group").parent().addClass("has-error");
		}

		if (!item.errors.length) return;
		if ($val && $val.length) $val.text(item.errors.shift());

		$.each(item.errors, function (c, val) {
			if (fieldId) {
				errorList += '<li><a title="click to view" href="javascript:setFocus(\'#' + fieldId + '\');" class="alert-warning">' + val + '</a></li>';
			} else {
				errorList += '<li>' + val + '</li>';
			}
		});
		$list.append(errorList);
	});
	if ($list.find("li:first").length) {
		$list.closest("div").show();
		scrollTo($list.parent());
	}
	return true;
};

function processClientSideValidationError(errorMap, errorList) {
	var $form = $(this.currentForm),
		$list = getValidationSummary($form),
		errorList;

	$list.html('');
	$form.find(".has-error").removeClass("has-error");

	$.each(errorList, function (index, error) {
		var $element = $(error.element);
		var fieldId = $element.length ? $element[0].id.replace('.', '\\.') : "";
		var $val = $form.find(".field-validation-valid,.field-validation-error")
						.first('[data-valmsg-for="' + $element[0].name + '"]')
						.removeClass("field-validation-valid")
						.addClass("field-validation-error");
		$form.find("#" + fieldId).addClass("input-validation-error")
		//$form.find("#" + fieldId).parentsUntil("div.form-group").parent().addClass("has-error");

		//if (!error.errors.length) return;
		//if ($val.length) $val.text(error.errors.shift());

		$list.append('<li><a title="click to view field" href="javascript:setFocus(\'#' + fieldId + '\');" class="alert-warning">' + error.message + '</a></li>');
	});
	if ($list.find("li:first").length) {
		$list.closest("div").show();
		scrollTo($list.parent());
	}
	return true;
};

function setFocus(ele) {
	$(ele).focus();
};

function notifError(message, callback) {	
	notif({
		type: "error",
		msg: message,
		position: "center",
		autohide: false,
		multiline: true,
		callback: callback
	});
};

function notifSuccess(message, callback) {
	notif({
		type: "success",
		msg: message,
		multiline: true,
		timeout: 5000,
		callback: callback
	});
};

function ajaxGet(url, data, success, fail, type) {
	// shift arguments if data argument was omitted
	if ($.isFunction(data)) {
		type = type || fail || success;
		fail = success;
		success = data;
		data = undefined;
	}

	return $.ajax({
		url: url,
		type: "GET",
		dataType: "json",
		data: data,
		success: function (data, status, xhr) {
			if (data.success) {
				success(data);
			} else if (data.tag === "ValidationError") {
				processServerSideValidationError(data);
			} else if ($.isFunction(fail)) {
				fail(data);
			} else {
				notifError(data.message);
			}
		},
		error: defaultAjaxErrorHandler
	});
};

function ajaxPost(url, data, success, fail, type) {
	// shift arguments if data argument was omitted
	if ($.isFunction(data)) {
		type = type || fail || success;
		fail = success;
		success = data;
		data = undefined;
	}

	return $.ajax({
		url: url,
		type: "POST",
		dataType: "json",
		data: data,
		success: function (data, status, xhr) {
			if (data.success) {
				success(data);
			} else if ($.isFunction(fail)) {
				fail(data);
			} else if (data.tag === "ValidationError") {
				processServerSideValidationError(data);
			} else {
				notifError(data.message);
			}
		},
		error: defaultAjaxErrorHandler
	});
};

function resetPostData($form, $grid) {
	var postData = $grid.jqGrid('getGridParam', 'postData');
	$form.find("[name]").each(function () {
		var name = $(this).prop("name");

		if (postData[name] != undefined)
			delete postData[name];
	});
}

$.fn.ajaxPostForm = function (url, success, fail, callback) {
	if (!this.length) {
		log('ajaxPostForm: skipping submit process - no element selected');
		return this;
	}

	var $form = this;

	$form.ajaxSubmit({
		url: url,
		type: 'POST',
		dataType: 'json',
		success: function (data, status, xhr) {
			if (data.success) {
				success(data);
			} else if ($.isFunction(fail)) {
				fail(data);
				if (typeof callback == "function") { callback(); }
			} else if (data.tag === "ValidationError") {
				processServerSideValidationError(data, $form);
				if (typeof callback == "function") { callback(); }
			} else {
				notifError(data.message);
				if (typeof callback == "function") { callback(); }
			}
		},
		error: defaultAjaxErrorHandler
	});
}

function defaultAjaxErrorHandler(response, status, error) {
	if (response.status == 401) {
		alert(response.responseText, function () {
			redirectTo("/Login?ReturnUrl=" + window.location.pathname);
		});
	} else {
		var message = response.responseJSON ? response.responseJSON.message : error.message;
		message = message || response.responseText;
		message = message.replace(/(?:\r\n|\r|\n)/g, '<br />');
		notifError(message);
	}
};

var addAntiForgeryToken = function (data) {
	var $requestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]');
	if ($requestVerificationToken.length == 1)
		data.__RequestVerificationToken = $requestVerificationToken.val();
	return data;
};

$.fn.readonly = function (enable) {
	if (this.is('form') || this.is('div')) {
		this.find('input').prop('readonly', enable);
		//$form.find("input[type='file']").replaceWith($form.find("input[type='file']").clone(true));
		this.find(":input[data-provide='select2'], :input.form-control.select2-offscreen").select2('readonly', enable);
		//$form.find("label.checkbox-inline input[type=hidden]:first-child").checkboxVal(false);
		this.find('textarea').prop('readonly', enable);
	} else {
		this.prop('readonly', enable);

		if (this.next().prop('type') == 'checkbox')
			this.next().prop('readonly', enable);
	}
}

$.fn.initOrganisationCodeSearchBox = function (searchOrgUrl, getOrgUrl, pageSize) {
	this.select2({
		placeholder: "Search for a organisation",
		minimumInputLength: 3,
		allowClear: true,
		ajax: {
			params: { global: false },
			quietMillis: 300,
			url: searchOrgUrl,
			dataType: 'json',
			data: function (term, page) {
				return {
					pageSize: pageSize,
					pageNum: page,
					searchTerm: term
				};
			},
			results: function (data, page, query) {
				var more = (page * pageSize) < data.data.total;
				return { results: data.data.results, more: more };
			}
		}, initSelection: function (element, callback) {
			var orgRef = $(element).val();
			if (orgRef) {
				$.ajax(getOrgUrl.replace('orgRef', orgRef), {
					dataType: "json"
				}).done(function (response) {
					var result = {
						id: response.data.organisationReference,
						text: response.data.organisationName,
						cText: response.data.organisationChiName,
						disableIndicator: response.data.organisationDisabled,
						section88: response.data.section88,
						withholdingIndicator: response.data.withholdingIndicator
					};
					callback(result);
				});
			}
		}, formatSelection: function (data) {
			return data.id + " - " + data.text + " " + data.cText
		}, formatResult: function (data) {
			return data.id + " - " + data.text + " " + data.cText
		}
	});
}

$.fn.initPspPermitSearchBox = function (searchPspPermitUrl, getPspPermitUrl, pageSize, $orgRef) {
	this.select2({
		placeholder: "Search for a (PSP)Permit No.",
		minimumInputLength: 3,
		allowClear: true,
		ajax: {
			params: { global: false },
			quietMillis: 300,
			url: searchPspPermitUrl,
			dataType: 'json',
			data: function (term, page) {
				return {
					pageSize: pageSize,
					pageNum: page,
					searchTerm: term,
					orgRef: $orgRef == undefined ? "" : $orgRef.select2("val")
				};
			},
			results: function (data, page, query) {
				var more = (page * pageSize) < data.data.total;
				return { results: data.data.results, more: more };
			}
		}, initSelection: function (element, callback) {
			var pspApprovalHistoryId = $(element).val();
			if (pspApprovalHistoryId) {
				$.ajax(getPspPermitUrl.replace('-1', pspApprovalHistoryId), {
					dataType: "json"
				}).done(function (response) {
					var result = {
						id: response.data.id,
						pspRef: response.data.pspRef,
						permitNum: response.data.permitNum
					};
					callback(result);
				});
			}
		}, formatSelection: function (data) {
			return "File Reference: " + data.pspRef + "&nbsp;&nbsp;&nbsp; PSP number: " + data.permitNum
		}, formatResult: function (data) {
			return "File Reference: " + data.pspRef + "&nbsp;&nbsp;&nbsp; PSP number: " + data.permitNum
		}
	});
}

$.fn.initFdPermitSearchBox = function (searchFdPermitUrl, getFdPermitUrl, pageSize, $orgRef) {
	this.select2({
		placeholder: "Search for a (Fd)Permit No.",
		minimumInputLength: 3,
		allowClear: true,
		ajax: {
			params: { global: false },
			quietMillis: 300,
			url: searchFdPermitUrl,
			dataType: 'json',
			data: function (term, page) {
				return {
					pageSize: pageSize,
					pageNum: page,
					searchTerm: term,
					orgRef: $orgRef == undefined ? "" : $orgRef.select2("val")
				};
			},
			results: function (data, page, query) {
				var more = (page * pageSize) < data.data.total;
				return { results: data.data.results, more: more };
			}
		}, initSelection: function (element, callback) {
			var fdEventId = $(element).val();
			if (fdEventId) {
				$.ajax(getFdPermitUrl.replace('-1', fdEventId), {
					dataType: "json"
				}).done(function (response) {
					var result = {
						id: response.data.fdEventId,
						fdRef: response.data.referenceNumber,
						permitNum: response.data.permitNo
					};
					callback(result);
				});
			}
		}, formatSelection: function (data) {
			return "File Reference: " + data.fdRef + "&nbsp;&nbsp;&nbsp; FD number: " + data.permitNum
		}, formatResult: function (data) {
			return "File Reference: " + data.fdRef + "&nbsp;&nbsp;&nbsp; FD number:  " + data.permitNum
		}
	});
}

$.fn.initComplaintRefSearchBox = function (searchComplaintRefUrl, getComplaintRefUrl, pageSize, mode) {
	this.select2({
		placeholder: "Search Related Enquiry / Complaint Case",
		minimumInputLength: 3,
		allowClear: true,
		ajax: {
			params: { global: false },
			quietMillis: 300,
			url: searchComplaintRefUrl,
			dataType: 'json',
			data: function (term, page) {
				return {
					pageSize: pageSize,
					pageNum: page,
					searchTerm: term
				};
			},
			results: function (data, page, query) {
				var more = (page * pageSize) < data.data.total;
				return { results: data.data.results, more: more };
			}
		}, initSelection: function (element, callback) {
			var complaintMasterId = $(element).val();
			if (complaintMasterId) {
				$.ajax(getComplaintRefUrl.replace('complaintMasterId', complaintMasterId), {
					dataType: "json"
				}).done(function (response) {
					var result = {
						id: response.data.complaintMasterId,
						complaintRef: response.data.complaintRef,
						complaintRecordType: response.data.complaintRef,
						complaintSource: response.data.complaintSource,
						complaintDate: response.data.complaintDate,
						orgRef: response.data.orgRef,
						engOrgName: response.data.engOrgName,
						chiOrgName: response.data.chiOrgName
					};
					callback(result);
				});
			}
		}, formatSelection: function (data) {
			return data.complaintRef
		}, formatResult: function (data) {
			if (mode.toUpperCase() == "ADD") {
				return data.complaintRef + " &nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp; " +
					"Type: " + data.complaintRecordType + "&nbsp;&nbsp;&nbsp; " +
					"Source: " + data.complaintSource + "<br/> &nbsp;&nbsp;&nbsp; " +
					"Date of Enquiry / Complaint: " + data.complaintDate + "&nbsp;&nbsp;&nbsp; " +
					"Organisation Ref: " + data.orgRef + "<br/> &nbsp;&nbsp;&nbsp; " +
					"Organisation Name: " + data.engOrgName + " " + data.chiOrgName
			}
			else {
				return data.complaintRef + "<br>" +
					"&nbsp;&nbsp;&nbsp; Type: " + data.complaintRecordType +
					"&nbsp;&nbsp;&nbsp; Source: " + data.complaintSource +
					"&nbsp;&nbsp;&nbsp; Date of Enquiry / Complaint: " + data.complaintDate +
					"&nbsp;&nbsp;&nbsp; Organisation Ref: " + data.orgRef + "<br/>" +
					"&nbsp;&nbsp;&nbsp; Organisation Name: " + data.engOrgName + " " + data.chiOrgName
			}
		}
	});
}

$.fn.radioVal = function (value) {
	if (this.is(':checked') === true) {
		this.prop('checked', false);
	}
	this.filter('[value=' + value + ']').prop('checked', true);
	return this;
};

$.fn.checkboxVal = function (value) {
	var checked = value ? true : false;
	this.val(checked);
	this.next('input[type=checkbox]').prop('checked', checked);
	return this;
};

var handleSideMenu = function (a) {
	a("#menu-toggler").on("click", function () {
		a("#sidebar").toggleClass("display");
		a(this).toggleClass("display");
		return false
	});
	var c = a("#sidebar").hasClass("menu-min");
	a("#sidebar-collapse").on("click", function () {
		c = a("#sidebar").hasClass("menu-min");
		ace.settings.sidebar_collapsed(!c)
	});
	var b = navigator.userAgent.match(/OS (5|6|7)(_\d)+ like Mac OS X/i);
	a(".nav-list").on("click", function (h) {
		var g = a(h.target).closest("a");
		if (!g || g.length == 0) {
			return
		}
		c = a("#sidebar").hasClass("menu-min");
		if (!g.hasClass("dropdown-toggle")) {
			if (c && "click" == "tap" && g.get(0).parentNode.parentNode == this) {
				var i = g.find(".menu-text").get(0);
				if (h.target != i && !a.contains(i, h.target)) {
					return false
				}
			}
			if (b) {
				document.location = g.attr("href");
				return false
			}
			return
		}
		var f = g.next().get(0);
		if (!a(f).is(":visible")) {
			var d = a(f.parentNode).closest("ul");
			if (c && d.hasClass("nav-list")) {
				return
			}
			d.find("> .open > .submenu").each(function () {
				if (this != f && !a(this.parentNode).hasClass("active")) {
					a(this).slideUp(200).parent().removeClass("open")
				}
			})
		} else { } if (c && a(f.parentNode.parentNode).hasClass("nav-list")) {
			return false
		}
		a(f).slideToggle(200).parent().toggleClass("open");
		return false
	})
};

function resizeApp() {
	// Resize all visible jqGrids that are children of ".grid-stretch-container" elements.
	// Note: You should call resizeApp when any grid visibility changes.
	$(".grid-stretch-container:visible .ui-jqgrid-btable").each(function (index) {
		if (this.id.indexOf("_frozen", this.id.length - 7) == -1) {
			// Resize the grid to it's parent.
			var container = $(this).closest(".grid-stretch-container");
			$(this).jqGrid().setGridWidth(container.width());
			// The grid height does not include the caption, pagers or column headers
			// so we need to compute an offset.
			// There's probably a better method than accessing the jqGrid "gbox".
			var gbox = $(this).closest("#gbox_" + this.id);
			var height = $(this).getGridParam("height") + (container.height() - gbox.height());
			$(this).jqGrid().setGridHeight(height);

			// Destroy and recreate the group headers to work around the bug.
			var groupHeaders = $(this).jqGrid("getGridParam", "groupHeader");
			if (groupHeaders != null) {
				$(this).jqGrid("destroyGroupHeader").jqGrid("setGroupHeaders", groupHeaders);
			}
		}
	});
};

function intiJqGridAutoResize() {
	// Bind the window resize event.
	// The timer prevents multiple resize events while resizing.
	// You might want to play with the 200ms delay if you're getting
	// multiple events or noticeable lag.
	var resizeTimer;
	$(window).bind("resize", function () {
		clearTimeout(resizeTimer);
		resizeTimer = setTimeout(resizeApp, 200);
	});

	// Trigger an initial resize so everything looks good on load.
	$(window).trigger("resize");
};

function convertStringToHtml(str) {
	str = str.replace(/\&lt;/g, "<");
	str = str.replace(/\&gt;/g, ">");
	str = str.replace(/\&lt;br&gt;/g, "\n");
	//str = str.Replace("&", "&amp;");
	return str;
}

function reloadSelect2(dropdown, options, textOnly) {
	//alert(typeof (dropdown))
	if (typeof (dropdown) == 'string') {
		dropdown = $("#" + dropdown);
	}
	textOnly = textOnly || false;

	//clear dropdown
	dropdown.empty();
	//for options , add to dropdown
	dropdown.append("<option value=''></option>");
	for (var value in options) {
		var text = options[value];
		if (textOnly)
			dropdown.append("<option value='" + text + "'>" + text + "</option>");
		else
			dropdown.append("<option value='" + value + "'>" + text + "</option>");
	}
}

// pares datetime into farmat 'dd/mm/yyyy'
function parseDatetimeToDate(inDate) {
	var processDate = new Date(inDate);

	if (processDate.getDate() < 10) {
		var day = '0' + processDate.getDate();
	}
	else
		var day = processDate.getDate();

	if (processDate.getMonth() + 1 < 10) {
		var month = '0' + (processDate.getMonth() + 1);
	}
	else
		var month = (processDate.getMonth() + 1);

	var outDate = day + '/' + month + '/' + processDate.getFullYear();

	return outDate;
};

$.fn.buildFilters = function () {
	var filters = {
		groupOp: "AND",
		rules: [],
		groups: []
	};

	this.find('input.jqgrid-colmodel, select.jqgrid-colmodel').each(function (e) {
		var $self = $(this);
		var data = $self.val();
		var fields = $self.data('jqgrid-colmodel-name').split(',');
		var op = $self.data('jqgrid-op');
		var prefix = $self.data('jqgrid-prefix');
		var suffix = $self.data('jqgrid-suffix');

		if (fields[0] == "disableIndicator") {
			if (data == "0")
				data = false;
			else if (data == "1")
				data = true;
		}

		if (!data)
			return;

		if (prefix)
			data = prefix + '' + data;

		if (suffix)
			data = data + '' + suffix;

		//If fields is array
		if (fields.length > 1) {
			//If data is array
			if (data.constructor === Array) {
				var innerFilters = {
					groupOp: "OR",
					rules: []
				};

				$.each(fields, function (index, value) {
					for (var i = 0; i < data.length; i++) {
						innerFilters.rules.push({
							'field': value,
							'op': op,
							'data': data[i]
						});
					}
				});

				filters.groups.push(innerFilters);
			}
				//If data is not array
			else {
				var innerFilters = {
					groupOp: "OR",
					rules: []
				};

				$.each(fields, function (index, value) {
					innerFilters.rules.push({
						'field': value,
						'op': op,
						'data': data
					});
				});

				filters.groups.push(innerFilters);
			}
		} else {
			//If data is array
			if (data.constructor === Array) {
				var innerFilters = {
					groupOp: "OR",
					rules: []
				};

				for (var i = 0; i < data.length; i++) {
					innerFilters.rules.push({
						'field': fields[0],
						'op': op,
						'data': data[i]
					});
				}

				filters.groups.push(innerFilters);
			}
				//If data is not array
			else {
				filters.rules.push({
					'field': fields[0],
					'op': op,
					'data': data
				});
			}
		}
	});

	return filters;
};

$.fn.jqGridFilter = function (filters) {
	this.jqGrid('clearGridData');
	this.jqGrid('setGridParam', {
		postData: {
			filters: filters
		},
		search: true
	});
	this.trigger("reloadGrid", [{ current: true }]);
};

$.fn.exportGrid = function (url, success, fail) {
	var $grid = this;

	var gridSettings = $grid.jqGrid('getGridParam', 'postData');
	var columnModel = {
		names: $grid.jqGrid('getGridParam', 'colNames'),
		models: $grid.jqGrid('getGridParam', 'colModel')
	};

	for (m in columnModel.names) {
		if (columnModel.names[m].indexOf("<input") == 0) {
			columnModel.names[m] = "";
		} else if (columnModel.names[m].indexOf("<br />") > 0) {
			columnModel.names[m] = columnModel.names[m].replace(/<br \/>/g, " ");
		}
	}

	var $gridSettings = $('<input></input>').attr({ 'type': 'hidden', 'id': 'gridSettings', 'name': 'gridSettings' }).val(JSON.stringify(gridSettings));
	var $columnModel = $('<input></input>').attr({ 'type': 'hidden', 'id': 'columnModel', 'name': 'columnModel' }).val(JSON.stringify(columnModel));
	var $form = $('<form></form>').append($gridSettings).append($columnModel);

	$form.ajaxPostForm(url, success, fail);
}

$.fn.isJqGrid = function () {
	var $grid = this;

	return $grid.hasClass('ui-jqgrid-btable');
}

$.fn.resetJqGrid = function (url) {
	var $grid = this;

	if (!$grid.isJqGrid()) {
		alert($grid.id + ' is not a jqGrid instance!');
		return;
	}

	$grid.jqGrid('clearGridData');
	$grid.jqGrid('setGridParam', { search: false });
	var postData = $grid.jqGrid('getGridParam', 'postData');
	$.extend(postData, { filters: "" });

	var newGridParam = { datatype: 'json' };
	if (url) {
		newGridParam = {
			url: url,
			datatype: 'json'
		};
	}

	$grid
		.jqGrid('setGridParam', newGridParam)
		.trigger("reloadGrid", [{ page: 1 }]);
}

$.fn.reloadJqGrid = function () {
	var $grid = this;
	$grid.trigger("reloadGrid", [{ current: true }]);
}

function queryStringToJson(qs) {
	var pairs = qs.split('&');
	var result = {};
	$.each(pairs, function (i, pair) {
		pair = pair.replace(/\+/g, ' ').split('=');
		var key = pair[0];
		var val = pair[1];

		key = decodeURIComponent(key);
		// missing `=` should be `null`:
		// http://w3.org/TR/2012/WD-url-20120524/#collect-url-parameters
		val = val === undefined ? null : decodeURIComponent(val);

		if (!result.hasOwnProperty(key)) {
			result[key] = val;
		} else if ($.isArray(result[key])) {
			result[key].push(val);
		} else {
			result[key] = [result[key], val];
		}

		if (!result[key])
			result[key] = decodeURIComponent(pair[1] || '');
	});

	return JSON.parse(JSON.stringify(result));
}

$(function () {
	intiJqGridAutoResize();
	initCustomDataApi();
	addCalendarIcon(':input[data-provide="datepicker"]');
	addCalendarIcon(':input[data-provide="datetimepicker"]');
	addRequired(':input[data-rule-required]');
	$('.read-mode-only').addClass('hide');
	$("input").placeholder();

	$("input").filter('.inputmask-decimal').inputmask();
	//$(":input.inputmask-decimal").inputmask("decimal", { 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false });

	$.goup({
		trigger: 100,
		hideUnderWidth: 100
	});
	handleSideMenu(jQuery);

	$(window).on('resize', function () {
		if (grid = $('.ui-jqgrid-btable:visible')) {
			grid.each(function (index) {
				gridId = $(this).attr('id');
				gridParentWidth = $('#gbox_' + gridId).parent().width();
				$('#' + gridId).setGridWidth(gridParentWidth);
			});
		};
	});

	//disable cache in all ajax
	$.ajaxSetup({
		cache: false,
		error: defaultAjaxErrorHandler
	});

	$("form").each(function () {
		$(this).validate({
			onfocusout: false,
			onkeyup: false,
			onclick: false,
			showErrors: processClientSideValidationError
		})
	});

	//This will automatically add the token to any ajax POST call
	var token = $('input[name="__RequestVerificationToken"]').val();
	if (token) {
		$.ajaxPrefilter(function (options, originalOptions) {
			if (options.data && options.type.toUpperCase() == "POST" && options.files == undefined) {
				if (typeof originalOptions.data === "string") {
					options.data = $.param($.extend(queryStringToJson(originalOptions.data), { __RequestVerificationToken: token }));
				} else {
					options.data = $.param($.extend(originalOptions.data, { __RequestVerificationToken: token }));
				}
			}
		});
	}

	// textarea event handler to add support for maxlength attribute
	$(document).on('input keyup', 'textarea[maxlength]', function (e) {
		// maxlength attribute does not in IE prior to IE10
		// http://stackoverflow.com/q/4717168/740639
		var $this = $(this);
		var maxlength = $this.attr('maxlength');
		if (!!maxlength) {
			var text = $this.val();

			if (text.length > maxlength) {
				// truncate excess text (in the case of a paste)
				$this.val(text.substring(0, maxlength));
				e.preventDefault();
			}
		}
	});

	$("#btnSpellCheck, [id^='btnSpellCheck']").click(function (e) {
		//Re-initalize incorrectwords container
		$('.spellchecker-incorrectwords').empty().remove();
		standalonespellchecker = new $.SpellChecker("textarea", {
			lang: 'en',
			parser: 'text',
			webservice: {
				path: '/SpellCheckerHandler.ashx',
				driver: 'hunspell'
			},
			suggestBox: {
				position: 'above',
			},
			incorrectWords: {
				position: function (container) {
					this.after(container);
				}
			}
		});
		standalonespellchecker.on('check.success', function () {
			notifSuccess('There are no incorrectly spelled words.');
		});
		standalonespellchecker.check();
	});

	$.SpellChecker.prototype.onSelectWord = function (e, word, element) {
	    e.preventDefault();
	    this.replaceWord(this.incorrectWord, word);

	    var $element = $(this.spellCheckerElement);
	    if ($element.hasClass("wysiwyg-editor"))
	        $element.summernote('code', $element.val());
	};

	//summernote init for .wysiwyg-editor
	$('.wysiwyg-editor').summernote({
		callbacks: {
			onInit: function () {
				var $self = $(this);
				var hiddenFieldId = $self.attr('owner');
				$('#' + hiddenFieldId).val($self.summernote('editorExt.plainText'));
			},
			onBlur: function () {
				var $self = $(this);
				var hiddenFieldId = $self.attr('owner');
				$('#' + hiddenFieldId).val($self.summernote('editorExt.plainText'));
			},
		},
		toolbar: [
		  // [groupName, [list of button]]
		  ['style', ['bold', 'italic', 'underline', 'clear']],
		  ['fontsize', ['fontsize']],
		  ['color', ['color']],
		],
	});

	$(window).unload(function () {
	    $('table.searchGrid').each(function () {
	        var $el = $(this);

	        if ($el.isJqGrid()) {
                var id = $el.prop('id');
                if ($el.jqGrid('getGridParam', 'postData') != null)
	                localStorage.setItem(id, $el.jqGrid('getGridParam', 'postData').filters);
	        }
	    });
	});
});