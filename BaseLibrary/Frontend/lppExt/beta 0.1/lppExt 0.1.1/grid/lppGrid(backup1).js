Ext.define("lppExt.grid.lppGrid", {
    extend: "Ext.grid.Panel",
    requires: ["lppExt.util.Toolbar",
        "lppExt.util.Common",
        "lppExt.util.Operation"],
    mixins: ["lppExt.grid.Util"],
    alias: "widget.lppGrid",
    /* 扩展功能 */
    // 数据源
    store: null, // {Null}或{Store}
    remoteProxy: null, // {String}为proxyUrl，{Proxy}为Proxy对象
    fields: [],
    pageSize: 30,
    // 其他
    noSelectedTitle: "提示",
    noSelectedMsg: "请选择需操作的对象！",
    // 表头
    columns: [],
    columnsUrl: '',
    rowNumberer: {
        text: "序号",
        width: 35
    },
    actionText: '操作',
    actionIndex: 'last',
    actionUrl: '',
    actions: [{
        type: 'update',
        icon: '',
        handler: '_addOfActions'
    }, {
        type: 'del',
        icon: '',
        handler: '_onDelClickOfAction',
        proxyUrl: 'getData.ashx',
        confirmTitle: '询问',
        confirmMsg: '确定删除？'
    }],
    // 工具栏
    toolbarUrl: '',
    toolbar: {
        top: [{
            type: "add",
            //icon: lppExt.util.Resource.IMG + "add.png",
            text: "新增",
            handler: "_add_click",
            disable: false,
            hidden: false,
            aUrl: "getTreeNode.ashx"
        }, {
            type: "del",
            //icon: lppExt.util.Resource.IMG + "delete.png",
            text: "删除",
            handler: "_del_click",
            confirmTitle: "询问",
            confirmMsg: "确定删除？",
            proxyUrl: "Del.ashx",
            disable: false,
            hidden: false
        }, {
            type: "update",
            //icon: lppExt.util.Resource.IMG + "edit1.png",
            text: "修改",
            handler: "_update_click",
            disable: false,
            hidden: false,
            uUrl: "getTreeNode.ashx"
        }, {
            type: "refresh",
            //icon: lppExt.util.Resource.IMG + "arrow_refresh.png",
            text: "刷新",
            handler: "_refresh_click",
            disable: false,
            hidden: false
        }, {
            type: "->"
        }, {
            type: "simpleSearch",
            //icon: lppExt.util.Resource.IMG + "magnifier.png",
            text: "搜索",
            label: "关键字",
            emptyText: "请输入关键字",
            filterFn: null,
            handler: "getLocalFilterClickHandler",
            disable: false,
            hidden: false
        }],
        bottom: {
            type: "paging",
            displayInfo: true,
            displayMsg: "显示{0}条到{1}条，共{2}条",
            disabled: false,
            hidden: false,
            prevText: '上一页',
            nextText: '下一页',
            refreshText: '刷新',
            firstText: '首页',
            lastText: '末页',
            beforePageText: 'Page',
            afterPageText: 'of {0}'
        }
    },
    // 右键菜单
    itemContextmenuUrl: '',
    itemContextmenu: [{ type: "add", aUrl: "getTreeNode.ashx" },
        { type: "del", proxyUrl: "Del.ashx" },
        { type: "update", uUrl: "getTreeNode.ashx" },
        { type: "separator" },
        { type: "refresh"}],
    containerContextmenuUrl: '',
    containerContextmenu: [{ type: "add" }, { type: "refresh"}],
    initComponent: function () {
        // 数据源
        if (Ext.isEmpty(this.store)) {
            if (!Ext.isEmpty(this.fields)
                && !Ext.isEmpty(this.remoteProxy)
                && (Ext.isString(this.remoteProxy)
                    || Ext.getClassName(this.remoteProxy).indexOf("Proxy") >= 0)) {
                this.store = Ext.create("Ext.data.Store", {
                    pageSize: this.pageSize || 30,
                    fields: this.fields,
                    proxy: (Ext.isString(this.remoteProxy) ? {
                        type: "ajax",
                        method: "GET",
                        url: this.remoteProxy,
                        reader: {
                            type: "json",
                            root: 'items',
                            totalProperty: "total"
                        }
                    } : this.remoteProxy)
                });
            }
            else {
                console.log('Data source configuration is wrong!');
            }
        }

        // 表头
        var _copyRowNumberer = {},
            _copyActions,
            actionCol = {
                xtype: 'actioncolumn',
                text: this.actionText,
                width: lppExt.util.Common.calcWidth(this.actionText, 5)
            },
            _numActionIndex;
        if (this.rowNumberer) {
            _copyRowNumberer = Ext.clone(this.rowNumberer);
            _copyRowNumberer.xtype = 'rownumberer';
            this.columns.unshift(_copyRowNumberer);
        }
        if (this.actions) {
            _copyActions = Ext.clone(this.actions);
            if (!Ext.isEmpty(this.actionUrl)) {
                lppExt.util.Common.syncRequest({
                    url: this.actionUrl,
                    scope: this,
                    success: lppExt.util.Common.createCallback(function (_copyActions, response, opt) {
                        var obj = Ext.JSON.decode(response.responseText, true);
                        if (obj === null || !obj.success) return;

                        var i;
                        for (i = 0; i < obj.items.length; ++i) {
                            if (obj.items[i].type) {
                                lppExt.util.Operation.update(_copyActions, obj.items[i].type, obj.items[i]);
                            }
                        }
                    }, _copyActions)
                });
            }
            _copyActions = lppExt.util.Operation.recurseConfig(this, _copyActions, this.getItemOfActions);
            actionCol.items = _copyActions;
            actionCol.width = (actionCol.width > 20 * _copyActions.length ? actionCol.width : 20 * _copyActions.length);
            _numActionIndex = Number(this.actionIndex);
            if (Ext.isNumeric(this.actionIndex) && _numActionIndex < this.columns.length && _numActionIndex >= 0) {
                Ext.insert(this.columns, _numActionIndex, [actionCol]);
            }
            else {
                this.columns.push(actionCol);
            }
        }

        // 工具栏
        var _copyToolbar = Ext.clone(this.toolbar);
        if (this.toolbarUrl) {
            lppExt.util.Common.syncRequest({
                scope: this,
                url: this.toolbarUrl,
                success: lppExt.util.Common.createCallback(function (_copyToolbar, response, eOpt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].type) {
                            lppExt.util.Toolbar.update(_copyToolbar, obj.items[i].type, obj.items[i]);
                        }
                    }
                }, _copyToolbar)
            });
        }
        this.dockedItems = lppExt.util.Toolbar.recurseConfig(this, _copyToolbar, this.getToolbarItem);

        // 右键菜单
        var _containerContextmenu = Ext.clone(this.containerContextmenu),
            _itemContextmenu = Ext.clone(this.itemContextmenu);
        if (this.containerContextmenuUrl) {
            lppExt.util.Common.syncRequest({
                url: this.containerContextmenuUrl,
                scope: this,
                success: function (response, eOpt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].type) {
                            lppExt.util.Operation.update(_containerContextmenu, obj.items[i].type, obj.items[i]);
                        }
                    }
                }
            });
        }
        if (this.itemContextmenuUrl) {
            lppExt.util.Common.syncRequest({
                url: this.itemContextmenuUrl,
                scope: this,
                success: function (response, eOpt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].type) {
                            lppExt.util.Operation.update(_itemContextmenu, obj.items[i].type, obj.items[i]);
                        }
                    }
                }
            });
        }
        if (this.itemContextmenu === null || Object.prototype.toString.call(this.itemContextmenu) !== "[object Array]" || this.itemContextmenu.length === 0) {
            this.on({ itemcontextmenu: this._preventItemContextmenuDefault, scope: this });
        }
        else {
            this.on({ itemcontextmenu: lppExt.util.Common.createCallback(this._onItemContextmenuToggle, _itemContextmenu), scope: this });
        }
        if (this.containerContextmenu === null || Object.prototype.toString.call(this.containerContextmenu) !== "[object Array]" || this.containerContextmenu.length === 0) {
            this.on({ containercontextmenu: this._preventContainerContextmenuDefault, scope: this });
        }
        else {
            this.on({ containercontextmenu: lppExt.util.Common.createCallback(this._onContainerContextmenuToggle, _containerContextmenu), scope: this });
        }

        // 当没有设置multiSelect属性时，就根据selType来设置multiSelect
        if (!this.multiSelect) {
            switch (this.selType.toLocaleLowerCase()) {
                case 'checkboxmodel':
                    this.multiSelect = true;
                    break;
                default:
                    this.multiSelect = false;
                    break;
            }
        }


        this.callParent(arguments);
    }
});