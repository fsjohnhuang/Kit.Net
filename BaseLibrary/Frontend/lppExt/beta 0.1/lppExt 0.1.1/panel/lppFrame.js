Ext.define("lppExt.panel.lppFrame", {
    extend: "Ext.container.Container",
    requires: ["lppExt.util.Common"],
    alias: "widget.lppFrame",
    url: '',
    initComponent: function () {
        // 组装iframe
        var _iframe = '<iframe id="{0}" frameborder="0" src="{1}" width="{2}" height="{3}" style="overflow: hidden"/>';
        var _id = "lppFrameId_" + (+new Date());

        // 绑定事件
        this.on({
            afterrender: lppExt.util.Common.createCallback(this._onAfterRender, _iframe, _id, this.url),
            resize: lppExt.util.Common.createCallback(this._onResize, _id),
            scope: this
        });

        this.callParent(arguments);
    },
    _onAfterRender: function (iframeHtml, id, url, cmp, eOpt) {
        setTimeout(function () {
            cmp.update(Ext.util.Format.format(iframeHtml,
                id,
                url,
                cmp.getWidth() + "px",
                cmp.getHeight() + "px"));
        }, 1);
    },
    _onResize: function (id, cmp, w, h, ow, oh, eOpt) {
        setTimeout(function () {
            if (!cmp.iframeEl) {
                cmp.iframeEl = Ext.query("#" + id, cmp.getEl().dom)[0];
            }
            cmp.iframeEl.width = cmp.getWidth() + "px";
            cmp.iframeEl.height = cmp.getHeight() + "px";
        }, 1);
    }
});