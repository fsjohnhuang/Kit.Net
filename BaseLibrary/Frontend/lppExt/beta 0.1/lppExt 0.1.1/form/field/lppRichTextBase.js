Ext.define("lppExt.form.field.lppRichTextBase", {
    statics: {
        lppRichTexts: {},
        create: function (id, config) {
            if (!id) return;
            if (this.lppRichTexts[id]) return this.lppRichTexts[id];

            var _richText = KindEditor.create('#' + id, config);
            this.lppRichTexts[id] = _richText;
            return _richText;
        }
    }
});