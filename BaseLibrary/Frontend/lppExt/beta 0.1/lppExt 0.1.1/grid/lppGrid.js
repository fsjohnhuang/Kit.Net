Ext.define("lppExt.grid.lppGrid", {
    extend: "Ext.grid.Panel",
    requires: ["lppExt.util.Toolbar",
        "lppExt.util.Common",
        "lppExt.util.Operation",
        "lppExt.util.Contextmenu"],
    mixins: ["lppExt.grid.Util", "lppExt.grid.Contextmenu", "lppExt.grid.Toolbar", "lppExt.grid.Actions", "lppExt.form.Factory"],
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
    commonResetUrl: "", // 非空时，toolbar、itemContextmenu、containerContextmenu和actions均使用该url重设子项目
    // 表头
    columns: [],
    columnsUrl: '', // 服务端筛选表头，响应JSON：{success: true, items:[{dataIndex:""},{dataIndex:""}]}
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
            aUrl: ""
        }, {
            type: "del",
            //icon: lppExt.util.Resource.IMG + "delete.png",
            text: "删除",
            handler: "_del_click",
            confirmTitle: "询问",
            confirmMsg: "确定删除？",
            proxyUrl: "",
            disable: false,
            hidden: false
        }, {
            type: "update",
            //icon: lppExt.util.Resource.IMG + "edit1.png",
            text: "修改",
            handler: "_update_click",
            disable: false,
            hidden: false,
            uUrl: ""
        }, {
            type: "refresh",
            //icon: lppExt.util.Resource.IMG + "arrow_refresh.png",
            text: "刷新",
            handler: "_refresh_click",
            disable: false,
            hidden: false
        }, {
            type: "->"
        }/*, {
            type: "simpleSearch",
            //icon: lppExt.util.Resource.IMG + "magnifier.png",
            text: "搜索",
            label: "关键字",
            emptyText: "请输入关键字",
            filterFn: null,
            handler: "getLocalFilterClickHandler",
            disable: false,
            hidden: false
        }*/],
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
    itemContextmenuInterceptor: null, // function (model, itemContextmenu)
    itemContextmenuUrl: '',
    itemContextmenu: [{ type: "add", aUrl: "" },
        { type: "del", dUrl: "" },
        { type: "update", uUrl: "" },
        { type: "check", cUrl: "" },
        { type: "separator" },
        { type: "refresh" }],
    containerContextmenuInterceptor: null, // function (containerContextmenu),
    containerContextmenuUrl: '',
    containerContextmenu: [{ type: "add", aUrl: "" }, { type: "refresh"}],
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
                    autoLoad: true,
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
        // 统一重设toolbar、itemContextmenu、containerContextmenu和actions子项目
        var commonResetItems = [];
        if (this.commonResetUrl) {
            lppExt.util.Common.syncRequest({
                url: this.commonResetUrl,
                scope: this,
                success: function (response, opt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    commonResetItems = obj.items;
                }
            });
        }

        // 表头
        var _copyRowNumberer = {},
            _copyActions,
            actionCol = {
                xtype: 'actioncolumn',
                text: this.actionText,
                menuText: this.actionText,
                width: lppExt.util.Common.calcWidth(this.actionText, 5)
            },
            _numActionIndex,
            _columns = Ext.clone(this.columns),
            i = null;
        if (this._columns) {
            _columns = Ext.clone(this._columns);
        }
        else {
            _columns = Ext.clone(this.columns);
            this._columns = Ext.clone(this.columns);
        }
        if (this.columnsUrl) {
            lppExt.util.Common.syncRequest({
                method: "GET",
                url: this.columnsUrl,
                success: function (response, eOpts) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].dataIndex) {
                            if (obj.items[i].remove) {
                                lppExt.util.Operation.remove(_columns, obj.items[i].dataIndex, "dataIndex");
                            }
                            else {
                                lppExt.util.Operation.update(_columns, obj.items[i].dataIndex, obj.items[i], null, "dataIndex");
                            }
                        }
                    }
                }
            });
        }

        if (this.rowNumberer) {
            _copyRowNumberer = Ext.clone(this.rowNumberer);
            _copyRowNumberer.xtype = 'rownumberer';
            _copyRowNumberer.width = lppExt.util.Common.calcWidth(_copyRowNumberer.text, 5);
            _columns.unshift(_copyRowNumberer);
        }
        if (!Ext.isEmpty(this.actions)) {
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
            else if (!Ext.isEmpty(commonResetItems)) {
                for (i = 0; i < commonResetItems.length; ++i) {
                    if (commonResetItems[i].type) {
                        lppExt.util.Operation.update(_copyActions, commonResetItems[i].type, commonResetItems[i]);
                    }
                }
            }
            _copyActions = lppExt.util.Operation.recurseConfig(this, _copyActions, this.getItemOfActions);
            actionCol.items = _copyActions;
            actionCol.width = (actionCol.width > 20 * _copyActions.length ? actionCol.width : 20 * _copyActions.length);
            actionCol.menuDisabled = true;
            _numActionIndex = Number(this.actionIndex);
            if (Ext.isNumeric(this.actionIndex) && _numActionIndex < this.columns.length && _numActionIndex >= 0) {
                Ext.insert(this.columns, _numActionIndex, [actionCol]);
            }
            else {
                _columns.push(actionCol);
            }
        }
        for (i = 0; i < _columns.length; ++i) {
            if (!_columns[i].align) {
                _columns[i].align = "center";
            }
        }
        this.columns = _columns;


        // 工具栏
        var _copyToolbar = Ext.clone(this.toolbar);
        if (!Ext.isEmpty(this.toolbarUrl)) {
            _copyToolbar = lppExt.util.Toolbar.resetToolbarRemotely(this.toolbarUrl, _copyToolbar);
        }
        else if (!Ext.isEmpty(commonResetItems)) {
            for (i = 0; i < commonResetItems.length; ++i) {
                if (commonResetItems[i].type) {
                    lppExt.util.Toolbar.update(_copyToolbar, commonResetItems[i].type, commonResetItems[i]);
                }
            }
        }
        this.dockedItems = lppExt.util.Toolbar.recurseConfig(this, _copyToolbar, this.getToolbarItem);

        // 右键菜单
        var _containerContextmenu = Ext.clone(this.containerContextmenu),
            _itemContextmenu = Ext.clone(this.itemContextmenu);
        if (!Ext.isEmpty(this.containerContextmenuUrl)) {
            lppExt.util.Contextmenu.resetContextmenuRemotely(this.containerContextmenuUrl, _containerContextmenu);
        }
        else if (!Ext.isEmpty(commonResetItems)) {
            for (i = 0; i < commonResetItems.length; ++i) {
                if (commonResetItems[i].type) {
                    lppExt.util.Operation.update(_containerContextmenu, commonResetItems[i].type, commonResetItems[i]);
                }
            }
        }
        if (!Ext.isEmpty(this.itemContextmenuUrl)) {
            lppExt.util.Contextmenu.resetContextmenuRemotely(this.itemContextmenuUrl, _itemContextmenu);
        }
        else if (!Ext.isEmpty(commonResetItems)) {
            for (i = 0; i < commonResetItems.length; ++i) {
                if (commonResetItems[i].type) {
                    lppExt.util.Operation.update(_itemContextmenu, commonResetItems[i].type, commonResetItems[i]);
                }
            }
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
    },
    reload: function (url, params) {
        var _options = {};
        if (url) {
            _options.url = url;
        }
        if (params) {
            _options.params = params;
        }
        this.getStore().load(_options);
    }
});