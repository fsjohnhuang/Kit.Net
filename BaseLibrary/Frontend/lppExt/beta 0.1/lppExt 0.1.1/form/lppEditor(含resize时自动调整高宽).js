Ext.define("lppExt.form.lppEditor", {
    extend: "Ext.window.Window",
    requires: ["lppExt.util.Resource", "lppExt.util.Msg", "lppExt.form.Button"],
    config: {
        layouts: null,
        inputs: null,
        btns: null,
        host: null
    },
    layout: "fit",
    constructor: function (config) {
        this.initConfig(config);
        this.callParent(arguments);
    },
    _fieldStyleFields: [],
    _autoHeightFields: [],
    _fixHeight: 0,
    initComponent: function () {
        var layouts = this.layouts,
            inputs = this.inputs,
            btns = [],
            host = this.host,
            cellAutoWidth = this.layouts && this.layouts.cellAutoWidth || 50,
            p = null, // 循环块中对象的属性
            i = null, // 循环块中的索引
            j = null; // 隐藏域个数

        this._fieldStyleFields = [];
        this._autoHeightFields = [];
        this._fixHeight = 0;
        if (!layouts || !inputs || !host || !Ext.isArray(inputs)) {
            console.warn("lppEditor congif is wrong!");
            return;
        }

        layouts.title = layouts.title || "undefined title";
        layouts.icon = layouts.icon && layouts.icon !== "auto" && (layouts.icon === "none" ? null : layouts.icon) || lppExt.util.Resource.IMG + "add.png";
        layouts.height = layouts.height && layouts.height !== "auto" && layouts.height || null;
        layouts.columns = layouts.columns || 1;

        // 设置窗口属性
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

        var setFieldStyle = null;
        for (i = 0; i < inputs.length; ++i) {
            inputs[i].itemId = inputs[i].name; // 设置field组件的itemId与name相同

            // 获取主键输入域itemId
            if (inputs[i].isPK) {
                this.pkItemId = inputs[i].itemId;
            }

            // 修复Extjs field组件fieldStyle失效问题Start
            var setFieldStyle = null;
            if (!Ext.isEmpty(inputs[i].fieldStyle)) {
                if (typeof inputs[i].fieldStyle === "string" || Ext.isObject(inputs[i].fieldStyle)) {
                    setFieldStyle = lppExt.util.Common.createCallback(function (i, field, eOpt) {
                        setTimeout(function () {
                            if (typeof inputs[i].fieldStyle === "string") {
                                field.bodyEl.dom.style.cssText = inputs[i].fieldStyle;
                            }
                            else {
                                field.bodyEl.setStyle(inputs[i].fieldStyle);
                            }
                        }, 10);
                    }, i);
                }

                // 存储调整field组件
                this._fieldStyleFields.push({ id: inputs[i].itemId, fieldStyle: inputs[i].fieldStyle });
            }

            // 存储设置isAutoHeight的field组件
            var setAutoHeight = null;
            if (inputs[i].isAutoHeight) {
                this._autoHeightFields.push({ id: inputs[i].itemId });
            }
            else {
                setAutoHeight = lppExt.util.Common.createCallback(function (i, field, eOpt) {
                    setTimeout(function () {
                        // 对非自动调整高度的Field为其配置固定高度
                        if (!inputs[i].isAutoHeight) {
                            field.getEl().dom.parentNode.style.height = field.getEl().dom.offsetHeight + "px";
                            this._fixHeight += field.getEl().dom.offsetHeight;
                        }
                    }, 10);
                }, i);
            }
            if (Ext.isEmpty(inputs[i].listeners)) {
                inputs[i].listeners = {};
            }
            inputs[i].listeners["render"] = lppExt.util.Common.createCallback(function (setFieldStyle, setAutoHeight, field, eOpts) {
                if (setFieldStyle) {
                    setFieldStyle.apply(this, [field, eOpts]);
                }
                if (setAutoHeight) {
                    setAutoHeight.apply(this, [field, eOpts]);
                }
            }, setFieldStyle, setAutoHeight);
            // 修复Extjs field组件fieldStyle失效问题End

            // 默认组件宽度为100%
            if (Ext.isEmpty(inputs[i].width)) {
                inputs[i].width = "100%";
            }

            if (inputs[i].xtype !== "hidden" && inputs[i].xtype !== "hiddenfield") continue;
            ++j;
        }

        layouts.tableAttrs = layouts.tableAttrs && layouts.tableAttrs !== "auto" && layouts.tableAttrs || { style: { width: "100%", padding: (i >= 1 ? "0" : "5px 0")} };
        layouts.tableAttrs.style = layouts.tableAttrs.style || {};
        layouts.tableAttrs.style.height = layouts.tableAttrs.style.height || "100%";
        layouts.trAttrs = layouts.trAttrs && layouts.trAttrs !== "auto" && layouts.trAttrs || { style: { margin: (i >= 1 ? "0" : "5px")} };
        layouts.tdAttrs = layouts.tdAttrs && layouts.tdAttrs !== "auto" && layouts.tdAttrs || { style: { width: "33%", padding: "0 10px"} };

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

        // 用于调整当editor重调高宽时小于最小高宽时，field走样的异常
        var styleSetter = (function () {
            var _interval = null;
            var fns = {};

            fns.configStyle = function (editor) {
                if (_interval) return;
                _interval = setInterval(function () {
                    // 调整fieldStyle
                    var i = null,
                        fieldStyle = null;
                    for (i = 0; i < editor._fieldStyleFields.length; ++i) {
                        fieldStyle = editor._fieldStyleFields[i];
                        if (typeof fieldStyle.fieldStyle === "string") {
                            editor.down("#" + fieldStyle.id).bodyEl.dom.style.cssText = fieldStyle.fieldStyle;
                        }
                        else {
                            editor.down("#" + fieldStyle.id).bodyEl.setStyle(fieldStyle.fieldStyle);
                        }
                    }
                }, 80);
            };

            fns.close = function () {
                if (_interval) {
                    clearInterval(_interval);
                    _interval = null;
                }
            };

            return fns;
        })();

        // 设置editor resize时调整fieldStyle 和 isAutoHeight
        this.on({
            resize: function _resize(editor, width, height, oldWidth, oldHeight, eOpts) {
                // 调整fieldStyle和isAutoHeight
                var i = null,
                    j = null,
                    fieldStyle = null,
                    autoHeightField = null;
                if (oldWidth && oldHeight) {
                    var variableHeight = (height - oldHeight) / (editor._autoHeightFields.length >= 1 ? editor._autoHeightFields.length : 1),
                        newElH, newInputEl;
                    for (i = 0; i < editor._autoHeightFields.length; ++i) {
                        autoHeightField = editor.down("#" + editor._autoHeightFields[i].id);
                        newElH = editor._autoHeightFields[i].elHeight + variableHeight;
                        newInputH = editor._autoHeightFields[i].inputHeight + variableHeight;
                        if (height >= editor.minHeight) {
                            autoHeightField.setHeight(newElH);
                            if (autoHeightField.calcHeight) {
                                setTimeout(autoHeightField.calcHeight, 5);
                            }
                            autoHeightField.getEl().dom.style.height = newElH + 'px';
                            editor._autoHeightFields[i].elHeight = newElH;
                            if (newInputH >= 0) {
                                autoHeightField.inputEl.dom.style.height = newInputH + 'px';
                                editor._autoHeightFields[i].inputHeight = newInputH;
                            }
                            
                        }
                    }
                }
                else {
                    for (i = 0; i < editor._autoHeightFields.length; ++i) {
                        autoHeightField = editor.down("#" + editor._autoHeightFields[i].id);
                        editor._autoHeightFields[i].height = autoHeightField.getEl().dom.offsetHeight;
                        editor._autoHeightFields[i].elHeight = autoHeightField.getEl().dom.offsetHeight;
                        editor._autoHeightFields[i].inputHeight = autoHeightField.inputEl.dom.offsetHeight;
                    }
                }
                /*if (oldWidth && oldHeight) {
                    var variableHeight = (height - oldHeight) / (editor._autoHeightFields.length >= 1 ? editor._autoHeightFields.length : 1),
                        newElH, newInputEl, displayInputEl;
                    for (i = 0; i < editor._autoHeightFields.length; ++i) {
                        autoHeightField = editor.down("#" + editor._autoHeightFields[i].id);
                        newElH = editor._autoHeightFields[i].elHeight + variableHeight;
                        newInputH = editor._autoHeightFields[i].inputHeight + variableHeight;
                        if (height >= editor.minHeight) {
                            autoHeightField.setHeight(newElH);
                            autoHeightField.getEl().dom.style.height = newElH + 'px';
                            displayInputEl = null;
                            for (j = 0; j < autoHeightField.bodyEl.dom.childNodes.length; ++j) {
                                if (autoHeightField.bodyEl.dom.childNodes[j].style.display !== 'none') {
                                    displayInputEl = autoHeightField.bodyEl.dom.childNodes[j];
                                    break;
                                }
                            }
                            if (displayInputEl) {
                                displayInputEl.style.height = newInputH + 'px';
                            }
                            editor._autoHeightFields[i].elHeight = newElH;
                            editor._autoHeightFields[i].inputHeight = newInputH;
                        }
                    }
                }
                else {
                    for (i = 0; i < editor._autoHeightFields.length; ++i) {
                        autoHeightField = editor.down("#" + editor._autoHeightFields[i].id);
                        editor._autoHeightFields[i].height = autoHeightField.getEl().dom.offsetHeight;
                        editor._autoHeightFields[i].elHeight = autoHeightField.getEl().dom.offsetHeight;
                        for (j = 0; j < autoHeightField.bodyEl.dom.childNodes.length; ++j) {
                            if (autoHeightField.bodyEl.dom.childNodes[j].style.display !== 'none') {
                                editor._autoHeightFields[i].inputHeight = autoHeightField.bodyEl.dom.childNodes[j].offsetHeight;
                                break;
                            }
                        }
                       
                    }
                }*/

                // 因仅在width或height等于最小宽高时，再进行resize操作才有机会出现field走样的异常
                if (width === this.minWidth || height === this.minHeight) {
                    styleSetter.configStyle(this); // 开启定时器调整fieldStyle 和 isAutoHeight
                }
                else {
                    styleSetter.close(); // 关闭定时器后调整fieldStyle
                    for (i = 0; i < editor._fieldStyleFields.length; ++i) {
                        fieldStyle = editor._fieldStyleFields[i];
                        var bodyEl = editor.down("#" + fieldStyle.id).bodyEl;
                        if (typeof fieldStyle.fieldStyle === "string") {
                            bodyEl.dom.style.cssText = fieldStyle.fieldStyle;
                        }
                        else {
                            bodyEl.setStyle(fieldStyle.fieldStyle);
                        }
                    }
                }
            },
            close: styleSetter.close,
            scope: this
        })


        this.callParent(arguments);
    }
});