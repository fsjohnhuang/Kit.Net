Ext.define("lppExt.tree.lppTreeView", {
    extend: "Ext.tree.Panel",
    alias: "widget.lppTreeView",
    requires: ["lppExt.util.Toolbar", "Ext.menu.Menu"],
    mixins: ["lppExt.tree.Util", "lppExt.tree.Contextmenu", "lppExt.tree.Toolbar"],
    width: 250,
    minWidth: 250,
    maxWidth: 350,
    split: true,
    collapsible: true,
    autoScroll: false,
    scroll: "both",
    lines: false,
    useArrows: true,
    rootVisible: false,
    listeners: {},
    /* 扩展功能 */
    // 数据源
    fields: null, // 用于TreeGrid
    store: null, // {Null}或{TreeStore}
    rootText: "", // {String}
    rootIcon: "", // {String}
    rootExpended: false, // {Boolean}
    rootConfig: null, // {Object} 设置root的qtip等属性，若设置该属性则rootText和rootIcon、rootExpended均无效
    localChildren: [], // {Array}
    remoteProxy: null, // {String}为proxyUrl，{Proxy}为Proxy对象
    autoLoadStore: false, // {Boolean}
    // 工具栏
    toolbarUrl: '',
    toolbar: {
        top: [{
            type: "localFilter",
            //icon: lppExt.util.Resource.IMG + "magnifier.png",
            text: "搜索",
            label: "关键字",
            emptyText: "请输入关键字",
            filterFn: null,
            handler: "_onLocalFilterClick",
            disable: false,
            hidden: false
        }]
    },
    // 右键菜单
    itemContextmenuInterceptor: null, // function (model, itemContextmenu)
    itemContextmenuUrl: '',
    itemContextmenu: [],
    containerContextmenuInterceptor: null, // function (containerContextmenu),
    containerContextmenuUrl: '',
    containerContextmenu: [],
    // 已选值
    selectedSource: null/*{
        url: '',
        items: '',
        fn: null
    }*/,
    recKey: '', // record的主键字段名
    initComponent: function () {
        // 数据源
        if (Ext.isEmpty(this.store)) {
            var _rootConfig = {};
            if (this.rootConfig) {
                _rootConfig = this.rootConfig;
            }
            else {
                _rootConfig.text = this.rootText;
                _rootConfig.icon = this.rootIcon;
                _rootConfig.expended = this.rootExpended;
            }
            if (Ext.typeOf(this.localChildren) === "array"
                && !Ext.isEmpty(this.localChildren)) {
                _rootConfig.children = this.localChildren;
                this.store = Ext.create("Ext.data.TreeStore", {
                    fields: this.fields,
                    root: _rootConfig/*{
                        text: this.rootText,
                        icon: this.rootIcon,
                        expended: this.rootExpended,
                        children: this.localChildren
                    }*/
                });

                // 筛选本地节点
                if (Ext.isString(this.remoteProxy) && !Ext.isEmpty(this.remoteProxy)) {
                    lppExt.util.Common.syncRequest({
                        url: this.remoteProxy,
                        scope: this,
                        success: this._filterInitStore
                    });
                }
            }
            else if (!Ext.isEmpty(this.remoteProxy)
                && (Ext.isString(this.remoteProxy)
                    || Ext.getClassName(this.remoteProxy).indexOf("Proxy") >= 0)) {
                var _self = this;
                this.store = Ext.create("Ext.data.TreeStore", {
                    autoLoad: this.autoLoadStore,
                    fields: this.fields,
                    root: _rootConfig/*{
                        text: this.rootText,
                        icon: this.rootIcon,
                        expended: this.rootExpended
                    }*/,
                    listeners: (Ext.isEmpty(_self.selectedSource) ? null : {
                        load: function (store, node, records, successful, eOpts) {
                            if (!Ext.isEmpty(_self.selectedSource.url)) {
                                lppExt.util.Common.syncRequest({
                                    url: _self.selectedSource.url,
                                    scope: _self,
                                    success: function (response, eOpt) {
                                        var _self = this;
                                        var delay = function () {
                                            var items = Ext.decode(response.responseText).items;
                                            var selModel = _self.getSelectionModel();
                                            var selRecs = [];
                                            for (var i = 0, len = items.length; i < len; ++i) {
                                                for (var j = 0, jlen = records.length; j < jlen; ++j) {
                                                    var itemId = items[i][_self.recKey] || items[i].id || items[i].ID;
                                                    var recId = items[i][_self.recKey] || records[j].raw.id || records[j].raw.ID;
                                                    if (itemId == recId) {
                                                        selModel.select(records[j]);
                                                        selRecs.push(records[j]);
                                                        break;
                                                    }
                                                }
                                            }

                                            if (_self.selectedSource.fn) {
                                                _self.selectedSource.fn(selRecs);
                                            }
                                        };

                                        setTimeout(delay, 10);
                                    }
                                });
                            }
                            else {
                                var _delay = function () {
                                    var items = _self.selectedSource.items;
                                    var selModel = _self.getSelectionModel();
                                    var selRecs = [];
                                    debugger
                                    for (var i = 0, len = items.length; i < len; ++i) {
                                        for (var j = 0, jlen = records.length; j < jlen; ++j) {
                                            var itemId = items[i][_self.recKey] || items[i].id || items[i].ID;
                                            var recId = records[j].raw[_self.recKey] || records[j].raw.id || records[j].raw.ID;
                                            if (itemId == recId) {
                                                selModel.select(records[j], true);
                                                selRecs.push(records[j]);
                                                break;
                                            }
                                        }
                                    }

                                    if (_self.selectedSource.fn) {
                                        _self.selectedSource.fn(selRecs);
                                    }
                                };

                                setTimeout(_delay, 10);
                            }
                        }
                    }),
                    proxy: (Ext.isString(this.remoteProxy) ? {
                        type: "ajax",
                        method: "GET",
                        url: this.remoteProxy,
                        reader: {
                            type: "json"
                        }
                    } : this.remoteProxy)
                });
            }
            else {
                console.log('Data source configuration is wrong!');
            }
        }
        // 工具栏
        var _copyToolbar = Ext.clone(this.toolbar);
        if (!Ext.isEmpty(this.toolbarUrl)) {
            lppExt.util.Toolbar.resetToolbarRemotely(this.toolbarUrl, _copyToolbar);
        }
        this.dockedItems = lppExt.util.Toolbar.recurseConfig(this, _copyToolbar, this.getToolbarItem);
        // 右键菜单
        var _containerContextmenu = Ext.clone(this.containerContextmenu),
            _itemContextmenu = Ext.clone(this.itemContextmenu);
        if (!Ext.isEmpty(this.containerContextmenuUrl)) {
            lppExt.util.Contextmenu.resetContextmenuRemotely(this.containerContextmenuUrl, _containerContextmenu);
        }
        if (!Ext.isEmpty(this.itemContextmenuUrl)) {
            lppExt.util.Contextmenu.resetContextmenuRemotely(this.itemContextmenuUrl, _itemContextmenu);
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

        this.callParent(arguments);
    },
    /* 当localChildren有值且remoteProxy为url时，就以localChildren基础数据，而remoteProxy为筛选Url。
    ** remoteProxy的筛选Url返回值格式为:{success:true, items: ["nodeText1", "nodeText2", "nodeText3"]}，
    ** 其中nodeText1等值为显示的节点text值。
    */
    _filterInitStore: function (response, eOpt) {
        var obj = Ext.JSON.decode(response.responseText, true);
        if (obj === null || !obj.success) return;

        lppExt.tree.LocalFilter.filter(this, function (model, nodeTexts) {
            var result = false,
                i;
            for (i = 0; i < nodeTexts.length; ++i) {
                if (model.get("text").toLocaleLowerCase() === nodeTexts[i]) {
                    result = true;
                    break;
                }
            }

            return result;
        }, obj.items);
    }
});