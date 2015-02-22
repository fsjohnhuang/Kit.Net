Ext.define("lppExt.form.field.lppRichText", {
    extend: "Ext.form.field.TextArea",
    alias: "widget.lppRichText",
    initComponent: function () {
        this.on({
            render: function (field, eOpts) {
                var originalNode = field.bodyEl.dom.childNodes[0];
                originalNode.style.visibility = 'hidden';
                var id = originalNode.getAttribute('id');

                Ext.syncRequire("lppExt.form.field.lppRichTextBase");
                this._richText = lppExt.form.field.lppRichTextBase.create(id, {
                    resizeType: 0,
                    items: [
                        'source', '|', 'undo', 'redo', '|', 'code', 'cut', 'copy', 'paste',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                        'superscript', 'clearhtml', 'quickformat', '|', 'fullscreen', '/',
                        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
                        'table', 'hr',
                        'anchor', 'link', 'unlink'
                    ],
                    urlType : 'domain',
                    uploadJson: './Handlers/article/Upload.ashx',
                    afterBlur: (function (field) {
                        return function () {
                            field._richText.sync();
                        };
                    })(this)
                });
                this.calcHeight(this.rows * 25);
                setTimeout(this.calcWidth, 10);
            },
            scope: this
        })

        // 生成重新计算lppRichText高度的方法
        this.calcHeight = (function (richText) {
            /*
            ** @param contentHeight {number} 输入框的高度，若不输入则以richText的总高度减去工具栏和状态栏的高度作为输入框的高度
            */
            return function (contentHeight) {
                // 修改编辑框高度
                var height = richText.height;
                var richTextDom = richText.bodyEl.dom.childNodes[0];
                var richTextToolDom = richTextDom.childNodes[0]; // 工具栏
                var richTextContentDom = richTextDom.childNodes[1]; // 编辑框
                var richTextStatusDom = richTextDom.childNodes[2]; // 状态栏
                var richTextContentDomHeight = contentHeight || height - richTextToolDom.offsetHeight - richTextStatusDom.offsetHeight;
                richTextContentDom.style.height = richTextContentDomHeight + 'px';
                richTextContentDom.getElementsByTagName('IFRAME')[0].style.height = richTextContentDomHeight + 'px';
            };
        })(this);

        this.calcWidth = (function (richText) {
            return function () {
                // 修改富文本编辑器的宽度
                var width = richText.bodyEl.dom.offsetWidth;
                var richTextDom = richText.bodyEl.dom.childNodes[0];
                var richTextToolDom = richTextDom.childNodes[0]; // 工具栏
                var richTextContentDom = richTextDom.childNodes[1]; // 编辑框
                var richTextStatusDom = richTextDom.childNodes[2]; // 状态栏
                richTextDom.style.width = richTextToolDom.style.width = richTextContentDom.style.width = richTextStatusDom.style.width = (Ext.isIE7 || Ext.isIE7m ? width - 2 : width) + 'px';

            };
        })(this);

        this.callParent(arguments);
    }
});