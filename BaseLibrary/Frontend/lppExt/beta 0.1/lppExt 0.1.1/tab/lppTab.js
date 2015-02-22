Ext.define("lppExt.tab.lppTab", {
    extend: "Ext.tab.Panel",
    alias: "widget.lppTab",
    statics: {
        tmpSelectionCmp: null
    },
    defaultTab: null, // 默认tab
    showLoadMask: null, // 显示遮罩层，lppTab初始化时自动赋值
    hideLoadMask: null, // 隐藏遮罩层，lppTab初始化时自动赋值
    initComponent: function () {
        if (this.defaultTab) {
            this.items = [this.defaultTab];
        }

        this.on({
            add: this._onAddTab,
            beforeadd: this._onBeforeAdd,
            scope: this
        });

        // 生成遮罩层
        var loadMask = Ext.create("Ext.LoadMask", {
            target: this
        });
        this.showLoadMask = function (callback) {
            if (!Ext.isFunction(callback)) return;

            loadMask.show();
            setTimeout(callback, 5);
        };
        this.hideLoadMask = function () {
            loadMask.hide();
        }


        this.callParent(arguments);
    },
    _onAddTab: function (cmp, container, index, opt) {
        setTimeout(function () {
            cmp.setActiveTab(cmp.items.length - 1);
        }, 1); // 要对象生成后才能选中
    },
    _onBeforeAdd: function (lppTab, addingCmp, index, eOpts) {
        addingCmp.on({
            render: function () {
                this.hideLoadMask();
            },
            scope: lppTab
        });
    },
    addUniqueTab: function (config) {
        var id = config.id || config.itemId,
            exist = false;
        if (!id) {
            console.log("The new tab item has no id configuration!");
            return;
        }
        if (this.items.length >= 1) {
            exist = this.child(Ext.util.Format.format("#{0}", id));
            if (exist) {
                this.setActiveTab(id);
                this.hideLoadMask();
                return;
            }
        }

        var newTab = null;
        if (config.componentCls) {
            newTab = config;
        }
        else {
            if (!Ext.isEmpty(config.selectionModel) && !Ext.isEmpty(config.nodeIndex)) {
                if (typeof (config.closable) === "undefined") {
                    config.closable = true;
                }
                if (!Ext.isObject(config.listeners)) {
                    config.listeners = {};
                }
                config.listeners["scope"] = this;
                config.listeners["close"] = function (cmp, eOpts) {
                    if (cmp == this.getActiveTab()) {
                        config.selectionModel.deselectAll(true);
                    }
                };
                config.listeners["beforedeactivate"] = function (cmp, eOpts) {
                    if (config.selectionCmp
                            && lppExt.tab.lppTab.tmpSelectionCmp != config.selectionCmp) {
                        config.selectionModel.deselectAll(true);
                        lppExt.tab.lppTab.tmpSelectionCmp = null;
                    }
                };
                config.listeners["beforeactivate"] = function (cmp, eOpts) {
                    config.selectionModel.select(config.nodeIndex, false, true);
                    if (config.selectionCmp) {
                        config.selectionCmp.expand();
                        lppExt.tab.lppTab.tmpSelectionCmp = config.selectionCmp;
                    }
                };
            }
            newTab = Ext.create(config);
        }
        this.add(newTab);
    }
});