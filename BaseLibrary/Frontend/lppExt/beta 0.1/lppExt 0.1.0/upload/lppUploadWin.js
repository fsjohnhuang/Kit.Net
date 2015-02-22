Ext.define('lppExt.upload.lppUploadWin', {
    extend: 'Ext.window.Window',
    alias: 'widget.lppUploadWin',
    layout: 'border',
    requires: ["lppExt.upload.lppUploadPanel"],
    fields: [],
    columns: [],
    remoteProxy: null,
    recordId: '',
    uploadUrl: '',
    height: 400,
    isSingle: false,
    initComponent: function () {
        var grid = Ext.create('lppExt.grid.lppGrid', {
            toolbar: [],
            itemContextmenu: [],
            containerContextmenu: [],
            actions: [],
            fields: this.fields,
            columns: this.columns,
            remoteProxy: this.remoteProxy,
            region: 'north',
            height: 200
        });
        this.items = [grid, {
            xtype: 'lppUploadPanel',
            uploadUrl: this.uploadUrl,
            afterUpload: function(){
                grid.getStore().reload();
            },
            recordId: this.recordId,
            region: 'center',
            isSingle: this.isSingle
        }];

        this.callParent(arguments);
    }
});