Ext.define("lppExt.grid.Util", {
    _onDelClick: function (item) {
        var selectedModels = this.getSelectionModel().getSelection();

        if (selectedModels.length === 0) {
            Ext.Msg.alert(this.noSelectedTitle, this.noSelectedMsg);
            return;
        }

        Ext.Msg.show({
            title: item.confirmTitle,
            msg: item.confirmMsg,
            buttons: Ext.Msg.OKCANCEL,
            scope: this,
            fn: function (btn, text) {
                if (btn == "ok") {
                    this._delItems(this, item, selectedModels);
                }
            }
        });
    },
    _onDelClickOfAction: function (item, gridView, rowIndex, colIndex) {
        var models = [];
        models.push(gridView.getStore().getAt(rowIndex));

        Ext.Msg.show({
            title: item.confirmTitle,
            msg: item.confirmMsg,
            buttons: Ext.Msg.OKCANCEL,
            scope: this,
            fn: function (btn, text) {
                if (btn == "ok") {
                    this._delItems(this, item, models);
                }
            }
        });
    },
    _delItems: function (lppGrid, item, models) {
        var ids = [];
        Ext.Array.each(models, function (model, index, self) {
            ids.push(model.get(this.idProperty || 'id'));
        }, lppGrid);

        Ext.Ajax.request({
            method: "POST",
            scope: lppGrid,
            url: item.dUrl,
            params: {
                ids: ids,
                r: Math.random()
            },
            success: item.success || function (reponse, opt) {
                Ext.Msg.alert("提示", "删除成功！", function () {
                    lppGrid.getStore().load();
                });
            },
            failure: item.failure || function (response, opt) {
                Ext.Msg.alert("提示", "删除失败！");
            }
        });
    },
    _onRefreshClick: function (item) {
        this.getStore().reload();
        this.getSelectionModel().deselectAll(); // 若刷新前选中了某节点，在重新加载数据后，该选中项将被删除，但tree仍然保存该信息，所以需要刷新该信息
    }
});