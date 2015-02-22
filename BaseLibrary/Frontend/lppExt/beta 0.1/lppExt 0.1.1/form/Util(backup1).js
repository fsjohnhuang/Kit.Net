Ext.define("lppExt.form.Util", {
    requires: ["lppExt.util.Common"],
    statics: {
        ButtonType: {
            SUBMIT_OF_TREE: 0,
            CANCEL_OF_TREE: 1,
            SUBMIT_OF_GRID: 2,
            CANCEL_OF_GRID: 3
        },
        configButtons: function _configButtons(btnConfigs, editor) {
            var i;
            for (i = 0; i < btnConfigs.length; ++i) {
                switch (btnConfigs[i].type) {
                    case this.ButtonType.SUBMIT_OF_TREE:
                        if (btnConfigs[i].icon === 'auto') {
                            btnConfigs[i].icon = lppExt.util.Resource.IMG + "accept.png";
                        }
                        if (btnConfigs[i].handler === 'auto') {
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onSubmitOfTreeClick, btnConfigs[i], editor);
                        }
                        break;
                    case this.ButtonType.CANCEL_OF_TREE:
                        if (btnConfigs[i].icon === 'auto') {
                            btnConfigs[i].icon = lppExt.util.Resource.IMG + "cross.png";
                        }
                        if (btnConfigs[i].handler === 'auto') {
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onCancelOfTreeClick, btnConfigs[i], editor);
                        }
                        break;
                    case this.ButtonType.SUBMIT_OF_GRID:
                        break;
                    case this.ButtonType.CANCEL_OF_GRID:
                        break;
                }
            }

            return btnConfigs;
        },
        _onSubmitOfTreeClick: function (item, editor) {
            var basicFrm = editor.down("form").getForm(),
                url = '';
            if (basicFrm.isValid()) {
                if (editor.down("form").getComponent(editor.getId()).getRawValue() == "") {
                    url = item.url || item.addUrl;
                }
                else {
                    url = item.url || item.updateUrl;
                }
                basicFrm.submit({
                    clientVaildation: true,
                    waitTitle: lppExt.util.Msg.TIPS_TITLE,
                    waitMsg: lppExt.util.Msg.SUBMITING_MSG,
                    url: url,
                    scope: editor,
                    success: function (form, action) {
                        var result = action.result.msg,
                            treeCmp = editor.getHost(); // 树组件
                        var models = treeCmp.getSelectionModel().getSelection();
                        if (editor.down("form").getComponent(editor.getId()).getRawValue() == "") {
                            // 新增
                            var parentNode = null;
                            if (models.length >= 1) {
                                // 添加3级及以上节点
                                parentNode = models[0];
                                if (parentNode.isLeaf()) {
                                    parentNode.set("leaf", false);
                                    parentNode.set("icon", "");
                                }
                                if (parentNode.hasChildNodes()) {
                                    parentNode.appendChild({ id: result.id, text: result.text, leaf: true, icon: this.leafIcon });
                                }
                                parentNode.expand(false);
                                new Ext.util.DelayedTask(function () {
                                    treeCmp.getSelectionModel().select(this.treeCmp.getStore().getNodeById(result.id));
                                }, this).delay(400);

                            }
                            else {
                                // 添加2级节点
                                parentNode = treeCmp.getRootNode();
                                var curNode = parentNode.appendChild({ id: result.id, text: result.text, leaf: true, icon: this.leafIcon });
                                treeCmp.getSelectionModel().select(curNode);
                            }
                        }
                        else {
                            // 修改
                            models[0].set("text", result.text);
                            models[0].updateInfo(models[0].raw);
                        }

                        editor.close();
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
        _onCancelOfTreeClick: function (item, editor) {
            editor.close();
        }
    }
});