(function() {
  var config;

  config = {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.lppTreeList',
    popupHeight: 200,
    popupUrl: './d.js',
    defaultValue: '',
    defaultText: '',
    allowMultiSelect: false,
    tree: {},
    tmpPDiv: {},
    initComponent: function() {
      var _this = this;

      this.treeId = Ext.id() + '-tree';
      this.store = null;
      this.tpl = new Ext.Template('<div id="' + this.treeId + '" style="height:' + this.popupHeight + 'px;" ></div>');
      this.editable = false;
      this.autoScroll = false;
      this.minWidth = 275;
      if ((this.width == null) || this.width < 275) {
        this.width = 275;
      }
      this.value = this.defaultText;
      this.hiddenName = this.name;
      this.name = this.name + '-tree';
      this.on({
        afterrender: function() {
          return document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = _this.defaultValue;
        }
      });
      return this.callParent(arguments);
    },
    expand: function() {
      var renderTree, _ref,
        _this = this;

      this.callParent(arguments);
      if (this.tmpDivId == null) {
        this.tmpDivId = this.treeId + (+new Date());
      }
      if (!((_ref = this.tree[this.treeId]) != null ? _ref.rendered : void 0)) {
        this.tree[this.treeId] = Ext.create('lppExt.tree.lppTreeView', {
          collapsible: false,
          width: 'auto',
          minWidth: 'auto',
          maxWidth: 'auto',
          selModel: this.allowMultiSelect ? Ext.create('Ext.selection.CheckboxModel') : Ext.create('Ext.selection.RowModel'),
          height: this.popupHeight,
          toolbar: {
            top: [
              {
                type: 'localFilter',
                text: ''
              }, {
                type: 'clearAll',
                icon: "" + lppExt.util.Resource.IMG + "cross.png",
                tooltip: '清空',
                handler: function(item) {
                  _this.tree[_this.treeId].getSelectionModel().deselectAll();
                  document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = '';
                  return _this.setValue('');
                }
              }
            ]
          },
          autoLoadStore: true,
          remoteProxy: this.popupUrl,
          border: false,
          listeners: {
            afterrender: function(treePanel, eOpt) {
              _this.tmpPDiv[_this.tmpDivId] = treePanel.getEl().dom;
              return document.getElementById(_this.treeId).appendChild(_this.tmpPDiv[_this.tmpDivId]);
            }
          }
        });
      }
      renderTree = function() {
        var div;

        if (_this.tree[_this.treeId].rendered) {
          return document.getElementById(_this.treeId).appendChild(_this.tmpPDiv[_this.tmpDivId]);
        } else {
          div = document.createElement('DIV');
          div.style.display = 'none';
          div.id = _this.tmpDivId;
          document.body.appendChild(div);
          return _this.tree[_this.treeId].render(div);
        }
      };
      setTimeout(renderTree, 1);
      return this.on({
        collapse: function() {
          var ids, selection, selections, texts, _i, _len;

          selections = _this.tree[_this.treeId].getSelectionModel().getSelection();
          if (_this.allowMultiSelect) {
            texts = '';
            ids = '';
            for (_i = 0, _len = selections.length; _i < _len; _i++) {
              selection = selections[_i];
              texts += selection.get('text') + '; ';
              ids += selection.getId() + ',';
            }
            _this.setValue(texts);
            if (ids.length >= 1) {
              ids = ids.substring(0, ids.length - 1);
            }
            return document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = ids;
          } else {
            if (selections.length === 0) {
              return;
            }
            _this.setValue(selections[0].get('text'));
            return document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = selections[0].getId();
          }
        },
        destory: function() {
          _this.callParent(arguments);
          _this.tree[_this.treeId].destory();
          return delete _this.tree[_this.treeId];
        }
      });
    }
  };

  Ext.define('lppExt.form.field.lppTreeList', config);

}).call(this);
