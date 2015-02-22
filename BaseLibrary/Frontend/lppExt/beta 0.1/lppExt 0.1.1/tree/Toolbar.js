Ext.define("lppExt.tree.Toolbar", {
    requires: ["lppExt.tree.LocalFilter",
        "lppExt.util.Resource",
        "lppExt.util.Common"],
    // 获取Toolbar工具组件
    getToolbarItem: function (lppTree, itemConfig) {
        var type = itemConfig.type || "";
        var item = null;
        if (itemConfig.hidden) return item;

        switch (type) {
            case "add":
                item = {
                    text: itemConfig.text || "新增",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "add.png",
                    scope: lppTree,
                    disabled: itemConfig.disabled || false,
                    hidden: false,
                    aUrl: itemConfig.aUrl || ''
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppTree._onAddClick, item);
                break;
            case "delete":
            case "del":
                item = {
                    text: itemConfig.text || "删除",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "delete.png",
                    scope: lppTree,
                    disabled: itemConfig.disabled || false,
                    hidden: false,
                    confirmTitle: itemConfig.confirmTitle || "询问",
                    confirmMsg: itemConfig.confirmMsg || "确定删除？",
                    dUrl: itemConfig.dUrl || "",
                    success: itemConfig.success || lppTree.delSuccess
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppTree._onDelClick, item);
                break;
            case "update":
                item = {
                    text: itemConfig.text || "修改",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "page_white_edit.png",
                    scope: lppTree,
                    disabled: itemConfig.disabled || false,
                    hidden: itemConfig.hidden || false,
                    uUrl: itemConfig.uUrl || ''
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], item)
                    || lppExt.util.Common.createCallback(lppTree._onUpdateClick, item);
                break;
            case "refresh":
                item = {
                    text: itemConfig.text || "刷新",
                    icon: itemConfig.icon || lppExt.util.Resource.IMG + "arrow_refresh.png",
                    scope: lppTree,
                    disabled: itemConfig.disabled || false,
                    hidden: itemConfig.hidden || false
                };
                item.handler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], itemConfig)
                    || lppExt.util.Common.createCallback(lppTree._onRefreshClick, item);
                break;
            case "localFilter":
                itemConfig.disabled = itemConfig.disabled || false;
                itemConfig.fieldLabel = itemConfig.fieldLabel || false;
                itemConfig.emptyText = itemConfig.emptyText || "请输入关键字";
                itemConfig.text = (typeof itemConfig.text != 'undefined' ? itemConfig.text : "搜索");
                itemConfig.icon = itemConfig.icon || lppExt.util.Resource.IMG + "magnifier.png";
                itemConfig.filterFn = itemConfig.filterFn || function _filterFn(model, valObj) {
                    return model.get("text").toLocaleLowerCase().indexOf(valObj.text.toLocaleLowerCase()) >= 0;
                };
                var _localFilterHandler = typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                    || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], itemConfig)
                    || lppExt.util.Common.createCallback(lppTree._onLocalFilterClick, itemConfig);
                
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
                        listeners: {
                            specialkey: function (field, e, eOpt) {
                                if (e.getKey() === 13) {
                                    lppExt.util.Common.createDelegate(_localFilterHandler, lppTree)();
                                }
                            }
                        },
                        emptyText: itemConfig.emptyText
                    }, {
                        xtype: "button",
                        text: itemConfig.text,
                        icon: itemConfig.icon,
                        scope: lppTree,
                        handler: _localFilterHandler
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
                        text: '',
                        icon: '',
                        scope: this,
                        handler: typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                            || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], itemConfig),
                        disabled: false,
                        hide: false
                    };
                    for (var p in itemConfig) {
                        if (p != 'handler')
                            item[p] = itemConfig[p];
                    }

                    /*item = {
                        text: itemConfig.text || "",
                        icon: itemConfig.icon || "",
                        scope: this,
                        handler: typeof itemConfig.handler === 'function' && lppExt.util.Common.createCallback(itemConfig.handler, item)
                            || lppTree[itemConfig.handler] && lppExt.util.Common.createCallback(lppTree[itemConfig.handler], itemConfig),
                        disabled: itemConfig.disabled || false,
                        hide: itemConfig.hide || false
                    };*/
                }

        }

        return item;
    }
});