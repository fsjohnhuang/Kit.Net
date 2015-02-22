Ext.define("lppExt.tree.lppTree", {
    extend: "Ext.tree.Panel",
    alias: "widget.lppTree",
    requires: ["Ext.menu.Menu",
        "lppExt.util.Toolbar",
        "lppExt.form.lppEditor",
        "lppExt.form.Util"],
    mixins: ["lppExt.tree.Util", "lppExt.tree.Menu", "lppExt.tree.Toolbar"],
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
    store: null, // {Null}或{TreeStore}
    rootText: "", // {String}
    rootIcon: "", // {String}
    rootExpended: false, // {Boolean}
    localChildren: [], // {Array}
    remoteProxy: null, // {String}为proxyUrl，{Proxy}为Proxy对象
    autoLoadStore: false, // {Boolean}
    // 其他
    noSelectedTitle: "提示",
    noSelectedMsg: "请选择需操作的对象！",
    leafIcon: null,
    // 工具栏
    toolbar: {
        top: [
            [{
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
            }],
            [{
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
        ]
    },
    // 新增、修改输入窗字段设置
    addLayouts: { title: "新增节点", icon: "auto", width: "auto", height: "auto", columns: 1, tableAttrs: "auto", tdAttrs: "auto", trAttrs: "auto" },
    addInputs: [{ xtype: "hiddenfield", name: "id" },
        { xtype: "hiddenfield", name: "pid",
            getValue: function _getValueFn(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                var pId = selectedModels.length && selectedModels[0].getId() || lppTree.getRootNode().getId();
                result.success = true;
                result.value = pId;

                return result;
            }
        },
        { fieldLabel: "上级节点", xtype: "displayfield", labelWidth: 60, name: "pName", fieldBodyCls: "",
            getValue: function _getValueFn(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                var pName = selectedModels.length && selectedModels[0].get("text") || lppTree.getRootNode().get("text");
                result.success = true;
                result.value = pName;

                return result;
            }
        },
        { fieldLabel: "新增节点", xtype: "textfield", labelWidth: 60, name: "curName", fieldBodyCls: "" }
    ],
    addButtons: [{ text: "确定", icon: "auto", addUrl: "", updateUrl: "", type: 0, handler: "auto" },
        { text: "取消", icon: "auto", type: 1, handler: "auto" }
    ],
    updateLayouts: { title: "节点", icon: "auto", width: "auto", height: "auto", columns: 1, tableAttrs: "auto", tdAttrs: "auto", trAttrs: "auto" },
    updateInputs: [{ xtype: "hiddenfield", name: "id" },
        { xtype: "hiddenfield", name: "pid",
            getValue: function _getValueFn(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                var pId = selectedModels.length && selectedModels[0].parentNode.getId() || lppTree.getRootNode().getId();
                result.success = true;
                result.value = pId;

                return result;
            }
        },
        { fieldLabel: "上级节点", xtype: "displayfield", labelWidth: 60, name: "pName", fieldBodyCls: "",
            getValue: function _getValueFn(lppTree) {
                var result = { success: false, value: null },
                    selectedModels = lppTree.getSelectionModel().getSelection();
                if (selectedModels.length > 1) return result;

                var pName = selectedModels.length && selectedModels[0].parentNode.get("text") || lppTree.getRootNode().get("text");
                result.success = true;
                result.value = pName;

                return result;
            }
        },
        { fieldLabel: "新增节点", xtype: "textfield", labelWidth: 60, name: "curName", fieldBodyCls: "" }
    ],
    updateButtons: [{ text: "确定", icon: "auto", addUrl: "", updateUrl: "", type: 0, handler: "auto" },
        { text: "取消", icon: "auto", type: 1, handler: "auto" }
    ],
    operId: "id",
    // 右键菜单
    itemContextmenu: [{ type: "add", aUrl: "getTreeNode.ashx" },
        { type: "del", proxyUrl: "Del.ashx" },
        { type: "update", uUrl: "getTreeNode.ashx" },
        { type: "separator" },
        { type: "refresh"}],
    containerContextmenu: [{ type: "add" }, { type: "refresh"}],
    initComponent: function () {
        // 数据源
        if (Ext.isEmpty(this.store)) {
            if (Ext.typeOf(this.localChildren) === "array"
                && !Ext.isEmpty(this.localChildren)) {
                this.store = Ext.create("Ext.data.TreeStore", {
                    root: {
                        text: this.rootText,
                        icon: this.rootIcon,
                        expended: this.rootExpended,
                        children: this.localChildren
                    }
                });
            }
            else if (!Ext.isEmpty(this.remoteProxy)
                && (Ext.isString(this.remoteProxy)
                    || Ext.getClassName(this.remoteProxy).indexOf("Proxy") >= 0)) {
                this.store = Ext.create("Ext.data.TreeStore", {
                    autoLoad: this.autoLoadStore,
                    root: {
                        text: this.rootText,
                        icon: this.rootIcon,
                        expended: this.rootExpended
                    },
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
        this.dockedItems = lppExt.util.Toolbar.recurseConfig(this, this.toolbar, this.getToolbarItem);
        // 右键菜单
        if (this.itemContextmenu === null || Object.prototype.toString.call(this.itemContextmenu) !== "[object Array]") {
            this.on({ itemcontextmenu: this.preventItemContextmenuDefault, scope: this });
        }
        else {
            this.on({ itemcontextmenu: this.onItemContextmenuToggle, scope: this });
        }
        if (this.containerContextmenu === null || Object.prototype.toString.call(this.containerContextmenu) !== "[object Array]") {
            this.on({ containercontextmenu: this.preventContainerContextmenuDefault, scope: this });
        }
        else {
            this.on({ containercontextmenu: this.onContainerContextmenuToggle, scope: this });
        }


        // 绑定点击带Checkbox的树节点时选择或取消勾选复选框的方法集
        var _checkboxNodeHandlers = this._getCheckboxNodeHandlers();
        this.on({ beforeitemclick: _checkboxNodeHandlers._beforeItemClick,
            beforeitemcollapse: _checkboxNodeHandlers._beforeItemCollapse,
            beforeitemexpand: _checkboxNodeHandlers._beforeItemExpand,
            beforeselect: _checkboxNodeHandlers._beforeSelect,
            scope: this
        });

        this.callParent(arguments);
    },
    // 获取点击带Checkbox的树节点时选择或取消勾选复选框的方法集
    _getCheckboxNodeHandlers: function () {
        var _timeout = null,
            _isCheckedOld = null,
            handlers = {};

        handlers._beforeItemClick = function (self, record, index, e, eOpts) {
            function run() {
                if (!Ext.isEmpty(record.get("checked")) && _isCheckedOld === record.get("checked")) {
                    record.set("checked", !record.get("checked"));
                    this.hasCheckboxNode = true;
                }
            }

            _timeout = setTimeout(run, 10);
            _isCheckedOld = record.get("checked");
        };

        handlers._beforeItemCollapse = function (self, eOpt) {
            if (_timeout) {
                clearTimeout(_timeout);
            }
        };

        handlers._beforeItemExpand = function (self, eOpt) {
            if (_timeout) {
                clearTimeout(_timeout);
            }
        };
        handlers._beforeSelect = function (self, record, index, eOpts) {
            if (!Ext.isEmpty(record.get("checked"))) {
                record.set("checked", !record.get("checked"));
                this.hasCheckboxNode = true;
            }
        };

        return handlers;
    }
});