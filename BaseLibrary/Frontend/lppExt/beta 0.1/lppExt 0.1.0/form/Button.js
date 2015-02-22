Ext.define("lppExt.form.Button", {
    requires: ["lppExt.util.Common", "lppExt.util.Resource"],
    statics: {
        ButtonType: {
            SUBMIT_OF_TREE: 0,
            CANCEL_OF_TREE: 1,
            SUBMIT_OF_GRID: 2,
            CANCEL_OF_GRID: 3,
            SEARCH: 4
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
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onCancelClick, btnConfigs[i], editor);
                        }
                        break;
                    case this.ButtonType.SUBMIT_OF_GRID:
                        if (btnConfigs[i].icon === 'auto') {
                            btnConfigs[i].icon = lppExt.util.Resource.IMG + "accept.png";
                        }
                        if (btnConfigs[i].handler === 'auto') {
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onSubmitOfGridClick, btnConfigs[i], editor);
                        }
                        break;
                    case this.ButtonType.CANCEL_OF_GRID:
                        if (btnConfigs[i].icon === 'auto') {
                            btnConfigs[i].icon = lppExt.util.Resource.IMG + "cross.png";
                        }
                        if (btnConfigs[i].handler === 'auto') {
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onCancelClick, btnConfigs[i], editor);
                        }
                        break;
                    case this.ButtonType.SEARCH:
                        if (!btnConfigs[i].icon || btnConfigs[i].icon === 'auto') {
                            btnConfigs[i].icon = lppExt.util.Resource.IMG + "magnifier.png";
                        }
                        if (!btnConfigs[i].handler || btnConfigs[i].handler === 'auto') {
                            btnConfigs[i].handler = lppExt.util.Common.createCallback(this._onSearchClick, btnConfigs[i], editor);
                        }
                        break;
                }
            }

            return btnConfigs;
        },
        _onSubmitOfTreeClick: function (item, editor) {
            var basicFrm = editor.down("form").getForm(),
                url = '',
                pkField = editor.down("#" + editor.pkItemId);
            if (basicFrm.isValid()) {
                if (!pkField) {
                    console.warn("There is no primary key input field!");
                    editor.close();
                }
                if (pkField.getRawValue() == "") {
                    url = item.url || item.addUrl;
                }
                else {
                    url = item.url || item.updateUrl;
                }
                debugger;
                basicFrm.submit({
                    clientVaildation: true,
                    waitTitle: lppExt.util.Msg.TIPS_TITLE,
                    waitMsg: lppExt.util.Msg.SUBMITING_MSG,
                    url: url,
                    scope: editor,
                    success: function (form, action) {
                        if (this.getHost().localChildren === null || this.getHost().localChildren.length === 0) {
                            this.getHost().getStore().reload();
                            editor.close();
                            return;
                        }
                        var result = action.result,
                            treeCmp = editor.getHost(), // 树组件
                            models = treeCmp.getSelectionModel().getSelection(),
                            newNode = null;
                        newNode = result.items[0];
                        newNode.leaf = true;
                        newNode.icon = this.leafIcon;

                        if (pkField.getRawValue() == "") {
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
                                    parentNode.appendChild(newNode);
                                }
                                parentNode.expand(false);
                                setTimeout(function () {
                                    treeCmp.getSelectionModel().select(treeCmp.getStore().getNodeById(result.items[0].id));
                                }, 400);
                            }
                            else {
                                // 添加2级节点
                                parentNode = treeCmp.getRootNode();
                                var curNode = parentNode.appendChild(newNode);
                                treeCmp.getSelectionModel().select(curNode);
                            }
                        }
                        else {
                            // 修改
                            models[0].set(newNode);
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
        _onSubmitOfGridClick: function (item, editor) {
            var basicFrm = editor.down("form").getForm(),
                url = '',
                pkField = editor.down("#" + editor.pkItemId);
            if (basicFrm.isValid()) {

                if (!pkField) {
                    console.warn("There is no primary key input field!");
                    editor.close();
                }
                if (pkField.getRawValue() == "") {
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
                        var result = action.result,
                            gridCmp = editor.getHost(),
                            p = null; // Grid组件

                        if (pkField.getRawValue() == "") {
                            // 新增
                            gridCmp.getStore().reload();
                        }
                        else {
                            // 修改
                            if (gridCmp && gridCmp.getSelectionModel){
                                var models = gridCmp.getSelectionModel().getSelection(),
                                    fields = models[0].fields,
                                    fieldNames = [];
                                for (i = 0; i < fields.length; ++i) {
                                    if (typeof fields[i] === "string") {
                                        fieldNames.push(fields[i]);
                                    }
                                    else if (Ext.isObject(fields.items[i]) && !Ext.isEmpty(fields.items[i].name)) {
                                        fieldNames.push(fields.items[i].name);
                                    }
                                }
                                var editorField = null;
                                for (i = 0; i < fieldNames.length; ++i) {
                                    editorField = editor.down("#" + fieldNames[i]);
                                    if (editorField) {
                                        models[0].set(fieldNames[i], editor.down("#" + fieldNames[i]).getValue());
                                    }
                                }
                            }
                            if (item.afterUpdate) {
                                item.afterUpdate(gridCmp);
                            }
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
        _onCancelClick: function (item, editor) {
            editor.close();
        },
        _onSearchClick: function (item, editor) {
            var basicFrm = editor.getForm();
            var queryParam = basicFrm.getValues();
            if (Ext.isString(item.host)) {
                item.host = Ext.getCmp(item.host);
            }
            var fields = basicFrm.getFields().items;
            for (var i = 0, len = fields.length; i < len; ++i) {
                if (fields[i].xtype === 'lppTreeList') {
                    queryParam[fields[i].hiddenName] = document.getElementsByName(fields[i].hiddenName)[0].value;
                }
            }

            item.host._queryParam = queryParam;
            for (var p in queryParam) {
                item.host.getStore().getProxy().setExtraParam(p, queryParam[p]);
            }
            item.host.getStore().loadPage(1);
        }
    }
});