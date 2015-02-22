Ext.define("lppExt.grid.Toolbar", {
    requires: ["lppExt.util.Resource",
        "lppExt.util.Common"],
    // 获取Toolbar工具组件
    getToolbarItem: function (lppGrid, itemConfig) {
        var type = itemConfig.type || "";
        var item = null;
        if (itemConfig.hidden) return item;

        switch (type) {
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
                    success: itemConfig.success || lppGrid.delSuccess
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
                    hidden: false,
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
                    hidden: false
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppGrid._onRefreshClick, item);
                break;
            case "simpleSearch":
                itemConfig.label = itemConfig.label || '关键字'
                itemConfig.disabled = itemConfig.disabled || false;
                itemConfig.fieldLabel = itemConfig.fieldLabel || false;
                itemConfig.emptyText = itemConfig.emptyText || "请输入关键字";
                itemConfig.text = itemConfig.text || "搜索";
                itemConfig.icon = itemConfig.icon || lppExt.util.Resource.IMG + "magnifier.png";
                itemConfig.filterFn = itemConfig.filterFn || function _filterFn(model, valObj) {
                    return model.get("text").toLocaleLowerCase().indexOf(valObj.text.toLocaleLowerCase()) >= 0;
                };

                item = Ext.create({
                    xclass: "Ext.form.FieldContainer",
                    disabled: itemConfig.disabled,
                    fieldLabel: itemConfig.label,
                    labelWidth: "auto",
                    layout: "hbox",
                    style: {
                        marginLeft: "2px"
                    },
                    items: [{
                        xtype: "textfield",
                        itemId: "filterField",
                        style: {
                            marginRight: "5px"
                        },
                        emptyText: itemConfig.emptyText
                    }, {
                        xtype: "button",
                        text: itemConfig.text,
                        icon: itemConfig.icon,
                        scope: lppGrid,
                        handler:function(){
                            lppGrid.getStore().clearFilter(false);
                            if (itemConfig.filterFn) {
                                lppGrid.getStore().filterBy(function (record, id) {
                                    return itemConfig.filterFn(record, lppGrid.down("#filterField").getValue());
                                });
                            }
                        }
                    }]
                });
                break;
            case '->':
                item = '->';
                break;
            case ' ':
                item = ' ';
                break;
            case '-':
                item = '-';
                break;
            case 'paging':
                item = {
                    xtype: 'pagingtoolbar',
                    displayInfo: itemConfig.displayInfo || true,
                    displayMsg: itemConfig.displayMsg || "显示{0}条到{1}条，共{2}条",
                    disabled: itemConfig.disabled || false,
                    prevText: itemConfig.prevText || '上一页',
                    nextText: itemConfig.nextText || '下一页',
                    refreshText: itemConfig.refreshText || '刷新',
                    firstText: itemConfig.firstText || '首页',
                    lastText: itemConfig.lastText || '末页',
                    beforePageText: itemConfig.beforePageText || 'Page',
                    afterPageText: itemConfig.afterPageText || 'of {0}',
                    hidden: false,
                    store: lppGrid.store
                };
                break;
            default:
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
                        handler: typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, itemConfig)
                            || lppGrid[itemConfig.handler] && lppExt.util.Common.createCallback(lppGrid[itemConfig.handler], itemConfig),
                        disabled: itemConfig.disabled || false,
                        hide: itemConfig.hide || false
                    };
                }
        }

        return item;
    }
});