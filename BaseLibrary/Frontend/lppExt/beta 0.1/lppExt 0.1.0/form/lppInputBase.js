Ext.define('lppExt.form.lppInputBase', {
    init: function (saveOriginalValue) {
        // 内置基本配置信息
        var _config = {
            rowHeight: 15,
            fieldHorizontalPadding: 10, // 水平单边padding值
            attachUnit: function (num) {
                return num + 'px';
            }
        };

        var layouts = this.layouts,
            inputs = this.inputs,
            btns = [],
            host = this.host,
            cellAutoWidth = (this.layouts && this.layouts.cellAutoWidth || 50) + 2 * _config.fieldHorizontalPadding,
            p = null, // 循环块中对象的属性
            i = null, // 循环块中的索引
            j = null; // 隐藏域个数
        if (!layouts || !inputs || !host || !Ext.isArray(inputs)) {
            console.warn("lppEditor congif is wrong!");
            return;
        }

        // 配置基本布局
        layouts.title = layouts.title || "undefined title";
        layouts.icon = layouts.icon && layouts.icon !== "auto" && (layouts.icon === "none" ? null : layouts.icon) || lppExt.util.Resource.IMG + "add.png";
        layouts.height = layouts.height && layouts.height !== "auto" && layouts.height || null;
        layouts.columns = layouts.columns || 1;
        for (p in layouts) {
            if (p === "tableAttrs" || p === "trAttrs" || p === "tdAttrs") continue;

            if (p === "columns") {
                this.width = layouts.columns * cellAutoWidth;
                this.minWidth = this.width - 1;
            }
            else {
                this[p] = layouts[p];
            }

            if (p === "height") {
                setTimeout((function (lppEditor) {
                    return function _setlppEditorHeight() {
                        lppEditor.minHeight = lppEditor.getEl().getHeight() - 1;
                    };
                })(this), 100);
            }
        }
        layouts.tableAttrs = layouts.tableAttrs && layouts.tableAttrs !== "auto" && layouts.tableAttrs || { style: { width: "100%", tableLayout: 'fixed' } };
        layouts.tableAttrs.style = layouts.tableAttrs.style || {};
        layouts.tableAttrs.style.height = layouts.tableAttrs.style.height || "100%";
        layouts.trAttrs = layouts.trAttrs && layouts.trAttrs !== "auto" && layouts.trAttrs || { style: { margin: "5px" } };
        layouts.tdAttrs = layouts.tdAttrs && layouts.tdAttrs !== "auto" && layouts.tdAttrs || { style: { paddingLeft: _config.attachUnit(_config.fieldHorizontalPadding), paddingRight: _config.attachUnit(_config.fieldHorizontalPadding) } };

        this.recoverFieldStyles = [];
        this.fixHeights = [];
        var recoverFieldStyle = null;
        for (i = 0; i < inputs.length; ++i) {
            inputs[i].itemId = inputs[i].name; // 设置field组件的itemId与name相同

            // 保存字段原值
            if (saveOriginalValue) {
                inputs[i].defaultVal = (inputs[i].value !== null && typeof inputs[i].value !== 'undefined' ? inputs[i].value : '');
            }

            // 获取主键输入域itemId
            if (inputs[i].isPK) {
                this.pkItemId = inputs[i].itemId;
            }

            if (inputs[i].xtype === 'hidden' || inputs[i].xtype === 'hiddenfield') {
                ++j;
                continue;
            }

            // 修复IE6、7下textarea的rows失效问题
            if ((inputs[i].xtype === 'textarea' || inputs[i].xtype === 'lppRichText') && inputs[i].rows) {
                inputs[i].height = inputs[i].rows * _config.rowHeight;
            }

            // 修复非隐藏输入框不占满所在td宽度问题
            recoverFieldStyle = { handler: null, field: null };
            recoverFieldStyle.handler = lppExt.util.Common.createCallback(function (i, j, editor, field, eOpt) {
                setTimeout(function () {
                    if (field.xtype === 'button') return;
                    editor.recoverFieldStyles[i - j].field = field;
                    var fieldWidth = field.getEl().dom.parentNode.offsetWidth - 2 * _config.fieldHorizontalPadding;
                    field.getEl().dom.style.width = _config.attachUnit(fieldWidth);
                    var bodyElWidth = field.bodyEl.dom.parentNode.offsetWidth - field.bodyEl.dom.previousSibling.width.replace(/[^0-9]*/ig, '') - _config.fieldHorizontalPadding;
                    field.bodyEl.dom.style.width = _config.attachUnit(bodyElWidth);
                    if (field.xtype === 'checkbox') return;
                    field.inputEl.dom.style.width = '100%';

                    // 对非自动调整高度的Field为其配置固定高度(ie6,7无效)
                    if (!inputs[i].isAutoHeight) {
                        var fixHeight = {
                            handler: lppExt.util.Common.createCallback(
                                function (field, height) {
                                    field.getEl().dom.parentNode.parentNode.style.height = height;
                                    field.getEl().dom.parentNode.style.height = height;
                                    field.getEl().dom.style.height = height;
                                    field.getEl().dom.childNodes[0].style.height = height;
                                    field.getEl().dom.childNodes[0].childNodes[0].style.height = height;
                                    field.getEl().dom.childNodes[0].childNodes[0].childNodes[0].style.height = height;
                                    field.getEl().dom.childNodes[0].childNodes[0].childNodes[1].style.height = height;
                                },
                                field, _config.attachUnit(field.getEl().dom.offsetHeight)
                            )
                        };
                        editor.fixHeights.push(fixHeight);
                    }
                }, 1);
            }, i, j, this);
            this.recoverFieldStyles.push(recoverFieldStyle);
            if (!inputs[i].listeners) {
                inputs[i].listeners = {};
            }
            inputs[i].listeners['render'] = this.recoverFieldStyles[i - j].handler;
        }

        this.on({
            resize: function (editor, width, height, oldWidth, oldHeight, eOpts) {
                var i;
                // 初始化时会触发resize事件，排除该次触发
                if (oldWidth && oldHeight) {
                    for (i = 0; i < editor.recoverFieldStyles.length; ++i) {
                        editor.recoverFieldStyles[i].handler(editor.recoverFieldStyles[i].field);
                    }
                    for (i = 0; i < editor.fixHeights.length; ++i) {
                        editor.fixHeights[i].handler();
                    }
                }
            }
        });

        p = null;
        for (i = 0; i < this.btns.length; ++i) {
            btns[i] = {};
            for (p in this.btns[i]) {
                btns[i][p] = this.btns[i][p];
            }
        }
        lppExt.form.Button.configButtons(btns, this);

        if (j >= 1) {
            // 有隐藏域
            var hiddenFields = inputs.slice(0, j);
            var displayFields = inputs.slice(j);
            this.items = {
                xtype: "form",
                frame: true,
                bodyStyle: {
                    border: 0
                },
                style: {
                    border: 0
                },
                layout: "card",
                items: [{
                    xtype: "container",
                    frame: true,
                    style: {
                        border: 0
                    },
                    bodyStyle: {
                        border: 0
                    },
                    layout: {
                        type: "table",
                        columns: layouts.columns,
                        tableAttrs: layouts.tableAttrs,
                        trAttrs: layouts.trAttrs,
                        tdAttrs: layouts.tdAttrs
                    },
                    items: displayFields
                }, {
                    xtype: "container",
                    height: 0,
                    items: hiddenFields
                }],
                buttons: btns
            };
        }
        else {
            // 无隐藏域
            this.items = {
                xtype: "form",
                frame: true,
                bodyStyle: {
                    border: 0
                },
                style: {
                    border: 0
                },
                layout: {
                    type: "table",
                    columns: layouts.columns,
                    tableAttrs: layouts.tableAttrs,
                    trAttrs: layouts.trAttrs,
                    tdAttrs: layouts.tdAttrs
                },
                items: inputs,
                buttons: btns
            };
        }
    }
});