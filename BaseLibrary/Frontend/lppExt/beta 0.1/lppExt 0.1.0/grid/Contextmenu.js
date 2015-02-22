Ext.define("lppExt.grid.Contextmenu", {
     requires: ["lppExt.util.Resource",
        "lppExt.util.Common"],
     getContextmenuItem: function (lppGrid, itemConfig) {
        var type = itemConfig.type || "";
        var item = null;
        if (itemConfig.hidden) return item;
        switch (itemConfig.type) {
            case "add":
                item = {
                    text: itemConfig.text || "新增",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "add.png",
                    scope: lppGrid,
                    disabled: itemConfig.disabled || false,
                    hidden: false,
                    aUrl: itemConfig.aUrl || ''
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onAddClick, item);
                break;
            case "delete":
            case "del":
                item = {
                    text: itemConfig.text || "删除",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "delete.png",
                    scope: lppGrid,
                    disabled: itemConfig.disabled || false,
                    hidden: false,
                    confirmTitle: itemConfig.confirmTitle || "询问",
                    confirmMsg: itemConfig.confirmMsg || "确定删除？",
                    dUrl: itemConfig.dUrl || "",
                    success: itemConfig.success || lppGrid.delSuccess // lppGrid.delSuccess位于lppExt.tree.Util类下
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onDelClick, item);
                break;
            case "update":
                item = {
                    text: itemConfig.text || "修改",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "page_white_edit.png",
                    scope: lppGrid,
                    disabled: itemConfig.disabled || false,
                    hidden: itemConfig.hidden || false,
                    uUrl: itemConfig.uUrl || ''
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onUpdateClick, item);
                break;
            case "refresh":
                item = {
                    text: itemConfig.text || "刷新",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "arrow_refresh.png",
                    scope: lppGrid,
                    disabled: itemConfig.disabled || false,
                    hidden: itemConfig.hidden || false
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onRefreshClick, item);
                break;
            case "check":
                item = {
                    text: itemConfig.text || "查看明细",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "page_green.png",
                    cUrl: itemConfig.cUrl || '',
                    scope: lppGrid,
                    disabled: itemConfig.disabled || false,
                    hidden: false
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onCheckClick, item);
                break;
            case "separator":
                item = {
                    xtype: "menuseparator"
                };
                break;
            default: // 自定义工具，itemCode为配置项对象
                if (!Ext.isObject(itemConfig)) {
                    console.log("工具栏配置有误！");
                    return item;
                }
                else if (Ext.isEmpty(itemConfig.type)) {
                    console.log("工具栏没有type属性！");
                    return item;
                }
                if (itemConfig.hasOwnProperty("xclass")) {
                    item = Ext.create(itemConfig);
                }
                else {
                    item = {
                        text: itemConfig.text || "",
                        icon: itemConfig.icon || "",
                        scope: this,
                        handler: typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                            || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], itemConfig),
                        disabled: itemConfig.disabled || false,
                        hide: itemConfig.hide || false
                    };
                }
                break;
        }

        return item;
    },
    _onContainerContextmenuToggle: function (containerContextmenu, view, e, opt) {
        e.preventDefault();
        e.stopEvent();

        // contextmenu拦截器
        var _containerContextmenu = Ext.clone(containerContextmenu);
        if (this.containerContextmenuInterceptor) {
            _containerContextmenu = this.containerContextmenuInterceptor(_containerContextmenu);
        }

        var items = [];
        for (var i = 0; i < _containerContextmenu.length; ++i) {
            var item = this.getContextmenuItem(this, _containerContextmenu[i]);
            if (null !== item) {
                items.push(item);
            }
        }

        var menu = Ext.create("Ext.menu.Menu", {
            items: items
        });
        menu.showAt(e.getX(), e.getY(), true);
    },
    _onItemContextmenuToggle: function (itemContextmenu, view, model, htmlEl, index, e, opt) {
        e.preventDefault();
        e.stopEvent();

        // contextmenu拦截器
        var _itemContextmenu = Ext.clone(itemContextmenu);
        if (this.itemContextmenuInterceptor) {
            _itemContextmenu = this.itemContextmenuInterceptor(model, _itemContextmenu);
        }

        var items = [];
        for (var i = 0; i < _itemContextmenu.length; ++i) {
            var item = this.getContextmenuItem(this, _itemContextmenu[i]);
            if (null !== item) {
                items.push(item);
            }
        }

        var menu = Ext.create("Ext.menu.Menu", {
            items: items
        });
        menu.showAt(e.getX(), e.getY(), true);

        view.getSelectionModel().select(model);
    },
    _preventItemContextmenuDefault: function (view, model, htmlEl, index, e, eOpt) {
        e.preventDefault();
        e.stopEvent();
    },
    _preventContainerContextmenuDefault: function (view, e, eOpt) {
        e.preventDefault();
        e.stopEvent();
    }
});