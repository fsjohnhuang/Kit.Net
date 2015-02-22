Ext.define("lppExt.grid.Actions", {
    requires: ["lppExt.util.Resource",
        "lppExt.util.Common"],
    getItemOfActions: function (lppGrid, itemConfig) {
        var type = itemConfig.type || "";
        var item = null;
        if (itemConfig.hidden) return item;
        switch (type) {
            case 'update':
                item = {
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "page_white_edit.png",
                    tooltip: itemConfig.tooltip || "修改",
                    proxyUrl: itemConfig.proxyUrl || "",
                    scope: lppGrid
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onUpdateActionClick, item);
                break;
            case 'del':
            case 'delete':
                item = {
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "delete.png",
                    tooltip: itemConfig.tooltip || "删除",
                    dUrl: itemConfig.dUrl || "",
                    confirmTitle: itemConfig.confirmTitle || "询问",
                    confirmMsg: itemConfig.confirmMsg || "确定删除？",
                    scope: lppGrid
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onDelClickOfAction, item);
                break;
            default:
                if (!Ext.isObject(itemConfig)) {
                    console.log("工具栏配置有误！");
                    return item;
                }
                else {
                    item = {
                        icon: itemConfig.icon || "",
                        tooltip: itemConfig.tooltip || "",
                        scope: this,
                        handler: typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, itemConfig)
                            || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], itemConfig),
                            disabled: itemConfig.disabled || false
                    };
                    if (itemConfig.isDisabled) {
                        item.isDisabled = itemConfig.isDisabled;
                    }
                }
        }

        return item;
    }
});