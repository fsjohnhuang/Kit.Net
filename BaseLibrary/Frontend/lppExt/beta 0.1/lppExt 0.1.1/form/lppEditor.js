Ext.define("lppExt.form.lppEditor", {
    extend: "Ext.window.Window",
    requires: ["lppExt.util.Resource", "lppExt.util.Msg", "lppExt.form.Button"],
    mixins: ['lppExt.form.lppInputBase'],
    config: {
        layouts: null,
        inputs: null,
        btns: null,
        host: null
    },
    modal: true,
    resizable: false,
    layout: "fit",
    constructor: function (config) {
        this.initConfig(config);
        this.callParent(arguments);
    },
    initComponent: function () {
        this.init.call(this);

        this.callParent(arguments);
    }
});