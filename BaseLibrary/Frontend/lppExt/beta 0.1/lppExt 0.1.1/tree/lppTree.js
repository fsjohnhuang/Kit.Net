Ext.define("lppExt.tree.lppTree", {
    extend: "lppExt.tree.lppTreeView",
    alias: "widget.lppTree",
    requires: ["lppExt.form.lppEditor", "lppExt.form.Button"],
    mixins: ["lppExt.form.Factory"],
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
                aUrl: "" // 用于服务端重设新建窗口的输入框属性，如对于某些角色的用户只能操作某部分输入框，而部分输入框仅显示信息不能编辑
            }, {
                type: "del",
                //icon: lppExt.util.Resource.IMG + "delete.png",
                text: "删除",
                handler: "_del_click",
                confirmTitle: "询问",
                confirmMsg: "确定删除？",
                dUrl: "",
                disable: false,
                hidden: false
            }, {
                type: "update",
                //icon: lppExt.util.Resource.IMG + "page_white_edit.png",
                text: "修改",
                handler: "_update_click",
                disable: false,
                hidden: false,
                uUrl: "" // 用于服务端重设修改窗口的输入框属性和值，如对于某些角色的用户只能操作某部分输入框，而部分输入框仅显示信息不能编辑
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
    addLayouts: { title: "新增节点", icon: "auto", width: "auto", height: "auto", columns: 1, cellAutoWidth: 250, tableAttrs: "auto", tdAttrs: "auto", trAttrs: "auto" },
    addInputs: [{ xtype: "hiddenfield", name: "id", isPK: true },
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
        { fieldLabel: "上级节点", xtype: "displayfield", labelWidth: 60, name: "pName", fieldStyle: "",
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
        { fieldLabel: "新增节点", xtype: "textfield", labelWidth: 60, name: "curName", fieldStyle: "" }
    ],
    addButtons: [{ text: "确定", icon: "auto", addUrl: "", updateUrl: "", type: lppExt.form.Button.ButtonType.SUBMIT_OF_TREE, handler: "auto" },
        { text: "取消", icon: "auto", type: lppExt.form.Button.ButtonType.CANCEL_OF_TREE, handler: "auto" }
    ],
    updateLayouts: { title: "节点", icon: "auto", width: "auto", height: "auto", cellAutoWidth: 250, columns: 1, tableAttrs: "auto", tdAttrs: "auto", trAttrs: "auto" },
    updateInputs: [{ xtype: "hiddenfield", name: "id", isPK: true },
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
    updateButtons: [{ text: "确定", icon: "auto", addUrl: "", updateUrl: "", type: lppExt.form.Button.ButtonType.SUBMIT_OF_TREE, handler: "auto" },
        { text: "取消", icon: "auto", type: lppExt.form.Button.ButtonType.CANCEL_OF_TREE, handler: "auto" }
    ],
    // 右键菜单
    itemContextmenu: [{ type: "add", aUrl: "" },
        { type: "del", dUrl: "" },
        { type: "update", uUrl: "" },
        { type: "check", cUrl: "" },
        { type: "separator" },
        { type: "refresh"}],
    containerContextmenu: [{ type: "add", aUrl: "" }, { type: "refresh"}],
    initComponent: function () {
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