(function() {
  var config;

  config = {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.lppTreeList3',
    popupHeight: 200,
    popupUrl: './d.js',
    defaultValue: '',
    defaultText: '',
    allowMultiSelect: false,
    tree: null,
    initComponent: function() {
      var _this = this;

      this.treeId = Ext.id() + '-tree';
      this.store = null;
      this.editable = false;
      this.autoScroll = false;
      this.minWidth = 280;
      if ((this.width == null) || this.width < 280) {
        this.width = 280;
      }
      this.value = this.defaultText;
      this.hiddenName = this.name;
      this.name = this.name + '-tree';
      this.on({
        afterrender: function() {
          document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = _this.defaultValue;
          return _this.bodyEl.on('click', function() {
            var bind_Hide_Global, hide_Global, _ref;

            if (!((_ref = _this.tree) != null ? _ref.rendered : void 0)) {
              _this.tree = Ext.create('lppExt.tree.lppTreeView', {
                collapsible: false,
                width: _this.inputEl.dom.offsetWidth + _this.triggerEl.elements[0].dom.offsetWidth,
                minWidth: 'auto',
                maxWidth: 'auto',
                floating: true,
                selModel: _this.allowMultiSelect ? Ext.create('Ext.selection.CheckboxModel') : Ext.create('Ext.selection.RowModel'),
                height: _this.popupHeight,
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
                        _this.tree.getSelectionModel().deselectAll();
                        document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = '';
                        _this.setValue('');
                        _this.tree.hide();
                        return Ext.getBody().un('click', hide_Global);
                      }
                    }, {
                      type: 'submit',
                      icon: "" + lppExt.util.Resource.IMG + "check.png",
                      tooltip: '确定',
                      handler: function(item) {
                        var ids, selection, selections, texts, _i, _len;

                        _this.tree.hide();
                        Ext.getBody().un('click', hide_Global);
                        selections = _this.tree.getSelectionModel().getSelection();
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
                      }
                    }
                  ]
                },
                autoLoadStore: true,
                remoteProxy: _this.popupUrl
              });
            }
            hide_Global = function(e, eOpt) {
              if (!!window.find && (HTMLElement.prototype.contains != null)) {
                HTMLElement.prototype.contains = function(B) {
                  return this.compareDocumentPosition(B) - 19 > 0;
                };
              }
              if (!_this.tree.getEl().dom.contains(e.target) || e.target === window) {
                _this.tree.hide();
                return Ext.getBody().un('click', hide_Global);
              }
            };
            bind_Hide_Global = function() {
              if (!_this.tree.isHidden()) {
                return Ext.getBody().on('click', hide_Global);
              }
            };
            setTimeout(bind_Hide_Global, 10);
            if (!_this.allowMultiSelect) {
              _this.tree.on('itemdblclick', function(view, record, item, index, e, eOpts) {
                if (record.isLeaf()) {
                  _this.setValue(record.get('text'));
                  document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = record.getId();
                  _this.tree.hide();
                  return Ext.getBody().un('click', hide_Global);
                }
              });
            }
            _this.isFirstOpen || (_this.isFirstOpen = 1);
            if (_this.tree.isHidden() || _this.isFirstOpen++ === 1) {
              return _this.tree.showAt(_this.getX() + _this.labelWidth + 5, _this.getY() + 21);
            } else {
              return _this.tree.hide();
            }
          });
        },
        beforedestroy: function() {
          if (this.tree != null) {
            return this.tree.hide();
          }
        }
      });
      return this.callParent(arguments);
    }
  };

  Ext.define('lppExt.form.field.lppTreeList3', config);

}).call(this);
