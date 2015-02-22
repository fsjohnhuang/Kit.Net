Ext.define("lppExt.form.lppSearch", {
    extend: "Ext.form.Panel",
    alias: "widget.lppSearch",
    requires: ["lppExt.util.Resource", "lppExt.util.Msg", "lppExt.form.Button"],
    defaultValues: [/*{
        itemId: '',
        value: ''
    }*/],
    frame: true,
    constructor: function (config) {
        this.initConfig(config);
        this.callParent(arguments);
    },
    initComponent: function () {
        this.buttons = [{
            text: this.btnText || '搜索',
            type: lppExt.form.Button.ButtonType.SEARCH,
            host: this.host
        }];
        lppExt.form.Button.configButtons(this.buttons, this);
        this.callParent(arguments);
    },
    init: function () {
        var input;
        for (var i = 0, l = this.defaultValues.length; i < l; ++i) {
            input = this.down('#' + this.defaultValues[i].itemId);
            if (input){
                input.setValue(this.defaultValues[i].value);
            }
        }
    }
});