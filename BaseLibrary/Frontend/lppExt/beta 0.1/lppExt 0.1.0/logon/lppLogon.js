Ext.Loader.setConfig({ enabled: true });

Ext.onDocumentReady(function() {
Ext.tip.QuickTipManager.init();

// 定义验证码生成域
Ext.define("AuthCodeField", {
extend: "Ext.form.field.Text",
alias: "widget.authcodefield",
codeUrl: window.getAuthCodeUrl,
onRender: function(ct, opt) {
this.callParent(arguments);
this.codeEl = ct.createChild({ tag: 'img', id: "authCodeImg", src: Ext.BLANK_IMAGE_URL, title: "点击重新获取！" });
this.codeEl.addCls('x-form-code');
this.codeEl.on('click', this.loadCodeImg, this);
this.loadCodeImg();
},
loadCodeImg: function() {
this.codeEl.set({ src: this.codeUrl + '?id=' + Math.random() });
}
});

// tabpanel
var tabCtnr = Ext.create("Ext.tab.Panel", {
region: "center",
plain: true,
items: [{
tabConfig: {
title: window.winLoginTxt,
icon: window.winLoginImg
},
layout: "fit",
itemId: "tabLogin",
items: {
xtype: "form",
itemId: "frmLogin",
frame: true,
defaults: {
    labelWidth: 40,
    width: 250,
    margin: "11px 0 0 55px"
},
items: [{
    xtype: "textfield",
    name: "txtAccount",
    fieldLabel: "账　号",
    cls: "txtAccount",
    allowBlank: false,
    minLength: 3,
    msgTarget: "side",
    blankText: "请输入有效账号！",
    minLengthText: "请输入{0}位以上的账号！"
}, {
    xtype: "textfield",
    name: "txtPw",
    fieldLabel: "密　码",
    cls: "txtPw",
    inputType: "password",
    allowBlank: false,
    minLength: 8,
    msgTarget: "side",
    blankText: "请输入密码！",
    minLengthText: "请输入{0}位以上的密码！"
}/*, {
    xtype: "authcodefield",
    fieldLabel: '验证码',
    name: 'txtAuthCode',
    id: 'CheckCode',
    allowBlank: false,
    blankText: '验证码不能为空',
    codeUrl: window.getAuthCodeUrl,
    width: 160
}*/]
}
}, {
tabConfig: {
    title: window.winNoticeTxt,
    icon: window.winNoticeImg
},
html: window.winNoticeContent,
style: {
    margin: "15px"
}
}]
});

//　window
var loginWin = Ext.create("Ext.window.Window", {
width: 400,
height: 280,
resizable: false,
draggable: false,
collapsible: false,
titleCollapsible: false,
closable: false,
title: window.winTitle,
icon: window.winTitleImg,
layout: "border",
items: [{
    xtype: "container",
    region: "north",
    height: 80,
    style: {
        backgroundImage: "url(" + window.systemBigImg + ")"
    }
},
tabCtnr
],
dockedItems: {
    xtype: "toolbar",
    dock: "bottom",
    items: ["->"
, {
xtype: "button",
text: window.winConfirmTxt,
icon: window.winConfirmImg,
tooltip: window.winConfirmTips,
listeners: {
    click: function(e) {
        var basicFrm = tabCtnr.getComponent("tabLogin").getComponent("frmLogin").getForm();
        if (basicFrm.isValid()) {
            basicFrm.submit({
                clientVaildation: true,
                waitTitle: window.loginWaitTitle,
                waitMsg: window.loginWaitMsg,
                url: window.loginUrl,
                params: {
                    sysCode: window.sysCode
                },
                success: function(form, action) {
                    window.open(window.mainPageUrl, "_self");
                },
                failure: function(form, action) {
                    switch (action.failureType) {
                        case Ext.form.action.Action.CONNECT_FAILURE:
                            Ext.Msg.alert(window.connFailTitle, window.connFailTxt);
                            break;
                        case Ext.form.action.Action.SERVER_INVALID:
                            Ext.Msg.alert(window.serverFailTitle, action.result.msg);
                            break;
                    }
                    Ext.getDom(Ext.getCmp("CheckCode").codeEl).click();
                }
            });
        }
    }
}
}, {
xtype: "button",
text: window.winOptTxt,
icon: window.winOptImg,
menu: Ext.create("Ext.menu.Menu", {
    items: [{
        text: window.winModifyKeyTxt,
        icon: window.winModifyKeyImg,
        handler: function() {
            loginWin.hide();

            // 密码修改窗口
            var changeKeyWin = Ext.create("Ext.window.Window", {
                width: 320,
                height: 200,
                resizable: false,
                draggable: false,
                collapsible: false,
                titleCollapsible: false,
                closable: true,
                closeAction: "destroy",
                title: window.winChangeKeyTitle,
                icon: window.winChangeKeyTitleImg,
                layout: "fit",
                listeners: {
                    close: function(panel, opt) {
                        loginWin.show(window.isShowWithAnimate);
                    }
                },
                items: [{
                    xtype: "form",
                    itemId: "frmChangeKey",
                    frame: true,
                    bodyPadding: "15 20 0 20",
                    items: [{
                        xtype: "textfield",
                        name: "txtAccountForChange",
                        fieldLabel: "账　号",
                        cls: "txtAccount",
                        labelWidth: 60,
                        width: 250,
                        allowBlank: false,
                        minLength: 3,
                        msgTarget: "side",
                        blankText: "请输入有效账号！",
                        minLengthText: "请输入{0}位以上的账号！"
                    }, {
                        xtype: "textfield",
                        fieldLabel: "旧密码",
                        name: "txtOldKey",
                        cls: "txtOldKey",
                        labelWidth: 60,
                        width: 250,
                        allowBlank: false,
                        msgTarget: "side",
                        blankText: "请输入旧密码",
                        inputType: "password",
                        minLength: 8,
                        minLengthText: "请输入{0}位以上的密码！"
                    }, {
                        xtype: "textfield",
                        fieldLabel: "新密码",
                        name: "txtNewKey",
                        itemId: "txtNewKey",
                        cls: "txtNewKey",
                        inputType: "password",
                        labelWidth: 60,
                        width: 250,
                        allowBlank: false,
                        msgTarget: "side",
                        blankText: "请输入新密码",
                        minLength: 8,
                        minLengthText: "请输入{0}位以上的密码！"
                    }, {
                        xtype: "textfield",
                        fieldLabel: "重复密码",
                        name: "txtRepeatKey",
                        itemId: "txtRepeatKey",
                        cls: "txtRepeatKey",
                        inputType: "password",
                        labelWidth: 60,
                        width: 250,
                        allowBlank: false,
                        msgTarget: "side",
                        blankText: "请重复输入新密码",
                        minLength: 8,
                        minLengthText: "请输入{0}位以上的密码！",
                        validator: function(val) {
                            var txtNewKeyVal = changeKeyWin.getComponent("frmChangeKey").getComponent("txtNewKey").getRawValue();
                            if (txtNewKeyVal != val) {
                                return "新密码与重复密码不相同！";
                            }

                            return true;
                        }
}]
}],
                        buttons: [{
                            text: "确定",
                            handler: function() {
                                var basicFrm = changeKeyWin.getComponent("frmChangeKey").getForm();
                                if (basicFrm.isValid()) {
                                    basicFrm.submit({
                                        clientVaildation: true,
                                        waitTitle: window.loginWaitTitle,
                                        waitMsg: window.changeKeyWaitMsg,
                                        url: window.resetPwUrl,
                                        success: function(form, action) {
                                            changeKeyWin.close();
                                            loginWin.show(window.isShowWithAnimate);
                                        },
                                        failure: function(form, action) {
                                            switch (action.failureType) {
                                                case Ext.form.action.Action.CONNECT_FAILURE:
                                                    Ext.Msg.alert(window.connFailTitle, window.connFailTxt);
                                                    break;
                                                case Ext.form.action.Action.SERVER_INVALID:
                                                    Ext.Msg.alert(window.serverFailTitle, action.result.msg);
                                                    break;
                                            }
                                        }
                                    })
                                }
                            }
                        }, {
                            text: "取消",
                            handler: function() {
                                changeKeyWin.close();
                                loginWin.show(window.isShowWithAnimate);
                            }
}]
                        }).show(window.isShowWithAnimate);

                    }
}]
                })
}]
}
});

loginWin.show();
});