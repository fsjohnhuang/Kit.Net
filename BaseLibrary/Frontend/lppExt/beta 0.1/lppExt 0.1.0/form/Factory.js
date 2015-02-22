Ext.define("lppExt.form.Factory", {
    requires: ["lppExt.util.Msg"],
    _onAddClick: function (item) {
        if (item.aUrl) {
            Ext.Ajax.request({
                url: item.aUrl,
                method: "GET",
                scope: this,
                success: function (response) {
                    var result = Ext.JSON.decode(response.responseText),
                        i, j, p, uiVal;
                    for (i = 0; i < this.addInputs.length; ++i) {
                        if (Ext.isFunction(this.addInputs[i].getValue)) {
                            uiVal = this.addInputs[i].getValue(this);
                            if (uiVal.success) {
                                this.addInputs[i].value = uiVal.value;
                            }
                            else {
                                console.log("getValue function has error!");
                            }
                        }
                        if (result.success) {
                            for (j = 0; j < result.items.length; ++j) {
                                if (this.addInputs[i].name === result.items[j].name) {
                                    for (p in result.items[j]) {
                                        this.addInputs[i][p] = result.items[j][p];
                                    }
                                }
                            }
                        }
                    }

                    Ext.create("lppExt.form.lppEditor", {
                        layouts: Ext.clone(this.addLayouts),
                        inputs: Ext.clone(this.addInputs),
                        btns: Ext.clone(this.addButtons),
                        host: this
                    }).show();
                },
                failure: function (response, opts) {
                    Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
                }
            });
        }
        else {
            var i = null,
                uiVal = null;
            for (i = 0; i < this.addInputs.length; ++i) {
                if (Ext.isFunction(this.addInputs[i].getValue)) {
                    uiVal = this.addInputs[i].getValue(this);
                    if (uiVal.success) {
                        this.addInputs[i].value = uiVal.value;
                    }
                    else {
                        console.log("getValue function has error!");
                    }
                }
            }

            Ext.create("lppExt.form.lppEditor", {
                layouts: this.addLayouts,
                inputs: this.addInputs,
                btns: this.addButtons,
                host: this
            }).show();
        }
    },
    _onUpdateClick: function (item) {
        var selectedModels = this.getSelectionModel().getSelection(),
            checkedModels = Ext.isFunction(this.getChecked) && this.getChecked() || [],
            hasChecked = false,
            i = null;
        for (i = 0; i < selectedModels.length; ++i) {
            if (!Ext.isEmpty(selectedModels[i].get("checked"))) {
                hasChecked = true;
                break;
            }
        }

        if ((selectedModels.length === 0 && checkedModels.length === 0)
            || (this.hasCheckboxNode && checkedModels.length === 0 && hasChecked)) {
            Ext.Msg.alert(this.noSelectedTitle, this.noSelectedMsg);
            return;
        }

        if (!item.uUrl) {
            console.log("uUrl is null!");
            return;
        }

        var i = null,
            result = null;
        Ext.Ajax.request({
            url: item.uUrl,
            method: "GET",
            params: { id: checkedModels.length && checkedModels[0].get("id") || selectedModels[0].get("id") },
            scope: this,
            success: function (response) {
                var result = Ext.JSON.decode(response.responseText),
                    i, j, p,
                    layouts = this.updateLayouts,
                    inputs = this.updateInputs && Ext.clone(this.updateInputs) || this.addInputs && Ext.clone(this.addInputs),
                    btns = this.updateButtons && Ext.clone(this.updateButtons) || this.addButtons && Ext.clone(this.addButtons);
                if (!layouts) {
                    layouts = Ext.clone(this.addLayouts);
                    layouts.title = "更新";
                    layouts.icon = lppExt.util.Resource.IMG + "page_white_edit.png";
                }
                for (i = 0; i < inputs.length; ++i) {
                    if (Ext.isFunction(inputs[i].getValue)) {
                        uiVal = inputs[i].getValue(this);
                        if (uiVal.success) {
                            inputs[i].value = uiVal.value;
                        }
                        else {
                            console.log("getValue function has error!");
                        }
                    }
                    else {
                        if (result.items.length === 1) {
                            for (var itemP in result.items[0]) {
                                if (inputs[i].name === itemP) {
                                    inputs[i].value = result.items[0][itemP];
                                }
                            }
                        }
                        else {
                            for (j = 0; j < result.items.length; ++j) {
                                if (inputs[i].name === result.items[j].name) {
                                    for (p in result.items[j]) {
                                        inputs[i][p] = result.items[j][p];
                                    }
                                }
                            }
                        }
                    }

                    if (inputs[i].handler) {
                        inputs[i].handler = lppExt.util.Common.createCallback(inputs[i].handler, this);
                    }
                }

                Ext.create("lppExt.form.lppEditor", {
                    layouts: layouts,
                    inputs: inputs,
                    btns: btns,
                    host: this
                }).show();
            },
            failure: function (response, opts) {
                Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
            }
        });
    },
    _onUpdateActionClick: function (item, gridView, rowIndex, colIndex) {
        if (!item.proxyUrl) {
            console.log("proxyUrl is null!");
            return;
        }

        var i = null,
            result = null;
        Ext.Ajax.request({
            url: item.proxyUrl,
            method: "GET",
            params: { id: gridView.getStore().getAt(rowIndex).getId() },
            scope: this,
            success: function (response) {
                var result = Ext.JSON.decode(response.responseText),
                    i, j, p,
                    layouts = this.updateLayouts,
                    inputs = this.updateInputs && Ext.clone(this.updateInputs) || this.addInputs && Ext.clone(this.addInputs),
                    btns = this.updateButtons && Ext.clone(this.updateButtons) || this.addButtons && Ext.clone(this.addButtons);
                if (!layouts) {
                    layouts = Ext.clone(this.addLayouts);
                    layouts.title = "更新";
                    layouts.icon = lppExt.util.Resource.IMG + "page_white_edit.png";
                }
                for (i = 0; i < inputs.length; ++i) {
                    if (Ext.isFunction(inputs[i].getValue)) {
                        uiVal = inputs[i].getValue(this);
                        if (uiVal.success) {
                            inputs[i].value = uiVal.value;
                        }
                        else {
                            console.log("getValue function has error!");
                        }
                    }
                    else {
                        if (result.items.length === 1) {
                            for (var itemP in result.items[0]) {
                                if (inputs[i].name === itemP) {
                                    inputs[i].value = result.items[0][itemP];
                                }
                            }
                        }
                        else {
                            for (j = 0; j < result.items.length; ++j) {
                                if (inputs[i].name === result.items[j].name) {
                                    for (p in result.items[j]) {
                                        inputs[i][p] = result.items[j][p];
                                    }
                                }
                            }
                        }
                    }
                }

                Ext.create("lppExt.form.lppEditor", {
                    layouts: layouts,
                    inputs: inputs,
                    btns: btns,
                    host: this
                }).show();
            },
            failure: function (response, opts) {
                Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
            }
        });
    },
    _onCheckClick: function (item) {
        var selectedModels = this.getSelectionModel().getSelection(),
            checkedModels = Ext.isFunction(this.getChecked) && this.getChecked() || [],
            hasChecked = false,
            i = null;
        for (i = 0; i < selectedModels.length; ++i) {
            if (!Ext.isEmpty(selectedModels[i].get("checked"))) {
                hasChecked = true;
                break;
            }
        }

        if ((selectedModels.length === 0 && checkedModels.length === 0)
            || (this.hasCheckboxNode && checkedModels.length === 0 && hasChecked)) {
            Ext.Msg.alert(this.noSelectedTitle, this.noSelectedMsg);
            return;
        }

        if (!item.cUrl) {
            console.log("cUrl is null!");
            return;
        }

        var i = null,
            result = null;
        Ext.Ajax.request({
            url: item.cUrl,
            method: "GET",
            params: { id: checkedModels.length && checkedModels[0].get("id") || selectedModels[0].get("id") },
            scope: this,
            success: function (response) {
                var result = Ext.JSON.decode(response.responseText),
                    i, j, p,
                    layouts = this.checkLayouts,
                    inputs = this.checkInputs && Ext.clone(this.checkInputs) || this.updateInputs && Ext.clone(this.updateInputs) || this.addInputs && Ext.clone(this.addInputs),
                    btns = [];
                if (!layouts) {
                    layouts = this.updateLayouts && Ext.clone(this.updateLayouts) || this.addLayouts && Ext.clone(this.addLayouts);
                    layouts.title = "查看";
                    layouts.icon = lppExt.util.Resource.IMG + "page_green.png";
                }
                for (i = 0; i < inputs.length; ++i) {
                    inputs[i].readOnly = true;
                    if (Ext.isFunction(inputs[i].getValue)) {
                        uiVal = inputs[i].getValue(this);
                        if (uiVal.success) {
                            inputs[i].value = uiVal.value;
                        }
                        else {
                            console.log("getValue function has error!");
                        }
                    }
                    else {
                        if (result.items.length === 1) {
                            for (var itemP in result.items[0]) {
                                if (inputs[i].name === itemP) {
                                    inputs[i].value = result.items[0][itemP];
                                }
                            }
                        }
                        else {
                            for (j = 0; j < result.items.length; ++j) {
                                if (inputs[i].name === result.items[j].name) {
                                    for (p in result.items[j]) {
                                        inputs[i][p] = result.items[j][p];
                                    }
                                }
                            }
                        }
                    }
                }

                Ext.create("lppExt.form.lppEditor", {
                    layouts: layouts,
                    inputs: inputs,
                    btns: btns,
                    host: this
                }).show();
            },
            failure: function (response, opts) {
                Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
            }
        });
    }
});