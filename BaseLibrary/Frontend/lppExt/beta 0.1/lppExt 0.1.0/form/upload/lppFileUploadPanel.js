/// <reference path="../fileupload/FileUploadPanel/FileUploadPanel.js" />
Ext.define('lppExt.form.upload.lppFileUploadPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.lppFileUploadPanel',
    requires: ['lppExt.form.upload.lppFileUploadField'],
    fileUpload : true,
    autoScroll : true,
    labelAlign : 'right',
    bodyStyle: 'padding: 10px 10px 0 10px;',
    defaults: { anchor: '98%' },
    afterUpload: null,
    url: '',
    recordId: '',
    items : [
        {
            _itemId : 'uf_1',
            xtype: 'fieldcontainer',
            fieldLabel: '文件',
            msgTarget : 'side',
            anchor    : '-20',
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'fileuploadfield',
                    emptyText: '请选择文件...',			            
                    name: 'file',
                    buttonText: '',
                    allowBlank: false,
                    buttonCfg: {
                        iconCls: 'upload-icon'
                    }
                },
                {
                    xtype: 'button',
                    width : 25,
                    iconCls : 'delete',
                    scope : this,
                    _ownerCtId : 'uf_1',
                    handler : this.removeField
                }
            ]
        }
    ],
    tbar : [
        {text:'添加',iconCls:'add',handler:this.addField,scope:this},'-',
        {text:'上传',iconCls:'up',handler:this.toSubmit,scope:this},'-'
    ],
    toSubmit: function () {//文件上传
        var basicFrm = this.getForm();
        if (basicFrm.isValid()) {
            basicFrm.submit({
                clientVaildation: true,
                waitTitle: lppExt.util.Msg.TIPS_TITLE,
                waitMsg: lppExt.util.Msg.SUBMITING_MSG,
                url: this.url,
                scope: this,
                params: {id: this.recordId},
                success: function (form, action) {
                    if (this.afterUpload) {
                        this.afterUpload(action);
                    }
                },
                failure: function (form, action) {
                    switch (action.failureType) {
                        case Ext.form.action.Action.CONNECT_FAILURE:
                            Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
                            break;
                        case Ext.form.action.Action.SERVER_INVALID:
                            Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, action.result.msg);
                            break;
                    }
                }
            });
        }
        
    },
    addField: function () {//添加文件上传选择框
        var n = this.maxItems || 8;
        var k = this.getNextItemNum();
        if (this.items.length >= n) {
            Ext.Msg.show({
                title: '提示',
                msg: '最大上传文件数量为' + n,
                icon: Ext.Msg.INFO,
                width: 230,
                buttons: Ext.Msg.OK
            });
            return;
        } else {
            this.add({
                _itemId: 'uf_' + k,
                xtype: 'compositefield',
                fieldLabel: '文件',
                msgTarget: 'side',
                anchor: '-20',
                defaults: {
                    flex: 1
                },
                items: [
                    {
                        xtype: 'fileuploadfield',
                        emptyText: '请选择文件...',
                        name: 'file',
                        buttonText: '',
                        allowBlank: false,
                        buttonCfg: {
                            iconCls: 'upload-icon'
                        }
                    },
                    {
                        xtype: 'button',
                        width: 25,
                        iconCls: 'delete',
                        scope: this,
                        _ownerCtId: 'uf_' + k,
                        handler: this.removeField
                    }
                ]
            });
            this.doLayout();
        }
    },
    getNextItemNum: function () {//获取准备添加的文件选择框索引
        var n = this.items.length || 0;
        return Number(n) + 1
    },
    removeField: function (btn) {//移除文件选择框
        var itemId = btn._ownerCtId;
        var items = this.items;
        for (var i = 0; i < items.length; i++) {
            if (items.itemAt(i)._itemId == itemId) {
                this.remove(items.itemAt(i), true);
                this.doLayout();
                return;
            }
        }
    }
});