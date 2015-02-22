Ext.define("lppExt.tree.lppTreeAccordion", {
    extend: "Ext.panel.Panel",
    alias: "widget.lppTreeAccordion",
    title: "undefined title",
    width: 250,
    minWidth: 250,
    maxWidth: 350,
    split: true,
    collapsible: true,
    autoScroll: false,
    scroll: "both",
    layout: {
        type: 'accordion',
        titleCollapse: true,
        animate: true,
        activeOnTop: false
    },
    defaults: {
        style: {
            border: "none 0px"
        }
    },
    url: '', // 服务端控制子节点显示与否的url
    _items: [], // 用于暂存子节点
    initComponent: function () {
        if (this.url) {
            this._items = this.items;
            this.items = null;

            Ext.Ajax.request({
                sync: true,
                method: "GET",
                url: this.url,
                success: this._onRequestSuccess,
                failure: this._onRequestFailure,
                scope: this
            });
        }


        this.callParent(arguments);
    },
    _onRequestSuccess: function (response) {
        var result = Ext.JSON.decode(response.responseText),
            i, j, item, resultItem, resultId, itemId, xClass, tree,
            items = this._items;
        if (result.success) {
            for (i = 0; i < items.length; ++i) {
                item = items[i];
                item.hidden = false;
                itemId = item.id || item.itemId;
                for (j = result.items.length - 1; j >= 0; --j) {
                    resultItem = result.items[j];
                    resultId = resultItem.id || resultItem.itemId;
                    if (resultId === itemId) {
                        if (resultItem.hidden || resultItem.hide) {
                            item.hidden = true;
                        }
                        break;
                    }
                }

                if (!item.hidden) {
                    if (item.xtype) {
                        xClass = Ext.util.Format.format("widget.{0}", item.xtype);
                        tree = Ext.create(xClass, item);
                    }
                    else {
                        tree = Ext.create(item);
                    }

                    tree.on({
                        expand: this._onExpand,
                        scope: this
                    });
                    if (this.items.length === 0) {
                        tree.getStore().getRootNode().expand();
                    }
                    this.add(tree);
                }
            }
        }
    },
    _onRequestFailure: function (response, opts) {
        Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
    },
    _onExpand: function (tree, a, eOpt) {
        if (tree.getStore().getRootNode().isExpanded()) return;

        tree.getStore().getRootNode().expand();
    }
});