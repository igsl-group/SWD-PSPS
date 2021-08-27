//  - plugin is external module for customizing.
$.extend($.summernote.plugins, {
	/**
		* @param {Object} context - context object has status of editor.
		*/
	'editorExt': function (context) {
		var self = this;

		var $editor = context.layoutInfo.editor;

		this.plainText = function () {
			if (context && context.code()) {
				var code = context.code().replace(/(<ol>)|(<\/p>)|(<ul>)|(<\/li>)/gi, "\r\n").replace(/\r\n$/, "");
				return $("<div />").html(code).text();
			} else
				return "";
		};
	}
});