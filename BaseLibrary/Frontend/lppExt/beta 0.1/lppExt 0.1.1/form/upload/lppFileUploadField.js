Ext.define('lppExt.form.upload.lppFileUploadField', {
    extend: 'Ext.form.field.Text',
    alias: 'widget.lppFileUploadField',
    buttonText: 'Browse...',
    buttonOnly: false,
    buttonOffset: 3,
    readOnly: true,
    autoSize: Ext.emptyFn,
    initComponent: function () {
        this.callParent(arguments);

        this.addEvents(
            'fileselected'
        );
    },
    onRender: function (ct, position) {
        this.callParent(arguments);

        this.wrap = this.el.wrap({ cls: 'x-form-field-wrap x-form-file-wrap' });
        this.el.addClass('x-form-file-text');
        this.el.dom.removeAttribute('name');
        this.createFileInput();

        var btnCfg = Ext.applyIf(this.buttonCfg || {}, {
            text: this.buttonText
        });
        this.button = new Ext.Button(Ext.apply(btnCfg, {
            renderTo: this.wrap,
            cls: 'x-form-file-btn' + (btnCfg.iconCls ? ' x-btn-icon' : '')
        }));

        if (this.buttonOnly) {
            this.el.hide();
            this.wrap.setWidth(this.button.getEl().getWidth());
        }

        this.bindListeners();
        this.resizeEl = this.positionEl = this.wrap;
    },
    bindListeners: function () {
        this.fileInput.on({
            scope: this,
            mouseenter: function () {
                this.button.addClass(['x-btn-over', 'x-btn-focus'])
            },
            mouseleave: function () {
                this.button.removeClass(['x-btn-over', 'x-btn-focus', 'x-btn-click'])
            },
            mousedown: function () {
                this.button.addClass('x-btn-click')
            },
            mouseup: function () {
                this.button.removeClass(['x-btn-over', 'x-btn-focus', 'x-btn-click'])
            },
            change: function () {
                var v = this.fileInput.dom.value;
                this.setValue(v);
                this.fireEvent('fileselected', this, v);
            }
        });
    },
    createFileInput: function () {
        this.fileInput = this.wrap.createChild({
            id: this.getFileInputId(),
            name: this.name || this.getId(),
            cls: 'x-form-file',
            tag: 'input',
            type: 'file',
            size: 1
        });
    },
    reset: function () {
        if (this.rendered) {
            this.fileInput.remove();
            this.createFileInput();
            this.bindListeners();
        }
        this.callParent(arguments);
    },
    getFileInputId: function () {
        return this.id + '-file';
    },
    onResize: function (w, h) {
        this.callParent(arguments);

        this.wrap.setWidth(w);

        if (!this.buttonOnly) {
            var w = this.wrap.getWidth() - this.button.getEl().getWidth() - this.buttonOffset;
            this.el.setWidth(w);
        }
    },
    onDestroy: function () {
        this.callParent(arguments);
        Ext.destroy(this.fileInput, this.button, this.wrap);
    },

    onDisable: function () {
        this.callParent(arguments);
        this.doDisable(true);
    },

    onEnable: function () {
        this.callParent(arguments);
        this.doDisable(false);

    },
    doDisable: function (disabled) {
        this.fileInput.dom.disabled = disabled;
        this.button.setDisabled(disabled);
    },
    preFocus: Ext.emptyFn,
    alignErrorIcon: function () {
        this.errorIcon.alignTo(this.wrap, 'tl-tr', [2, 0]);
    }
});