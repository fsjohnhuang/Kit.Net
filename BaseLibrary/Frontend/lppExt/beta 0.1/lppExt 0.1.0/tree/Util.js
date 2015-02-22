Ext.define("lppExt.tree.Util", {
    requires: ["lppExt.util.Msg"],
    _onDelClick: function (item) {
        var selectedModels = this.getSelectionModel().getSelection(),
            checkedModels = this.getChecked(),
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

        Ext.Msg.show({
            title: item.confirmTitle,
            msg: item.confirmMsg,
            buttons: Ext.Msg.OKCANCEL,
            scope: this,
            fn: function (btn, text) {
                if (btn === "ok") {
                    var _ids = [],
                        i = null;
                    if (checkedModels.length) {
                        for (i = 0; i < checkedModels.length; ++i) {
                            _ids.push(checkedModels[i].get("id"));
                        }
                    }
                    else {
                        for (i = 0; i < selectedModels.length; ++i) {
                            _ids.push(selectedModels[i].get("id"));
                        }
                    }
                    console.log(_ids);
                    Ext.Ajax.request({
                        scope: this,
                        url: item.dUrl,
                        params: { id: _ids },
                        method: "POST",
                        success: item.success
                    });
                }
            }
        });
    },
    delSuccess: function (response) {
        var result = Ext.JSON.decode(response.responseText);
        if (!result.success) {
            Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, result.msg);
        }
        else {
            var selectedModels = this.getSelectionModel().getSelection(),
                checkedModels = this.getChecked(),
                pathArray = null,
                parentNode = null,
                i = null;
            if (checkedModels.length) {
                for (i = 0; i < checkedModels.length; ++i) {
                    pathArray = checkedModels[i].getPath().split("/");
                    parentNode = this.getStore().getNodeById(pathArray[pathArray.length - 2]);
                    parentNode.removeChild(checkedModels[i], true);

                    if (!parentNode.hasChildNodes()) {
                        parentNode.set("leaf", true);
                        parentNode.set("icon", this.leafIcon);
                    }
                    parentNode.expand(false);
                }
                return;
            }

            for (i = 0; i < selectedModels.length; ++i) {
                pathArray = selectedModels[i].getPath().split("/");
                parentNode = this.getStore().getNodeById(pathArray[pathArray.length - 2]);
                parentNode.removeChild(selectedModels[i], true);

                if (!parentNode.hasChildNodes()) {
                    parentNode.set("leaf", true);
                    parentNode.set("icon", this.leafIcon);
                }
                parentNode.expand(false);
            }
        }
    },
    _onRefreshClick: function (item) {
        if (Ext.isEmpty(this.localChildren)) {
            this.getStore().load();
        }
        this.getSelectionModel().deselectAll(); // 若刷新前选中了某节点，在重新加载数据后，该选中项将被删除，但tree仍然保存该信息，所以需要刷新该信息
    },
    _onLocalFilterClick: function (item) {
        var mask = Ext.create('Ext.LoadMask', {
            target: this,
            msg: 'Searching...'
        });
        mask.show();
        var filterVal = this.down("#filterField").getValue();
        lppExt.tree.LocalFilter.filter(this, item.filterFn, { text: filterVal }, mask);
    }
});