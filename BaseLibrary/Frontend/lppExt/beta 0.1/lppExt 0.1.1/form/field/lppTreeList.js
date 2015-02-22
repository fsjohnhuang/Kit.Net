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
    tree: null,
    selectedUrl: '',
    selectedItems: null,
    recText: '',
    recValue: '',
    initComponent: function() {
      var defaultText, defaultValue, selectedItem, _ids, _txts,
        _this = this;

      defaultText = this.defaultText;
      defaultValue = null;
      if (!Ext.isEmpty(this.selectedUrl)) {
        lppExt.util.Common.syncRequest({
          url: this.selectedUrl,
          success: function(response, eOpt) {
            var ids, item, items, txts;

            items = Ext.decode(response.responseText).items;
            txts = (function() {
              var _i, _len, _results;

              _results = [];
              for (_i = 0, _len = items.length; _i < _len; _i++) {
                item = items[_i];
                _results.push(item[this.recText]);
              }
              return _results;
            }).call(_this);
            ids = (function() {
              var _i, _len, _results;

              _results = [];
              for (_i = 0, _len = items.length; _i < _len; _i++) {
                item = items[_i];
                _results.push(item[this.recValue]);
              }
              return _results;
            }).call(_this);
            defaultText = txts.join(';');
            return defaultValue = ids.join(',');
          }
        });
      } else if (!Ext.isEmpty(this.selectedItems)) {
        _txts = (function() {
          var _i, _len, _ref, _results;

          _ref = this.selectedItems;
          _results = [];
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            selectedItem = _ref[_i];
            _results.push(selectedItem[this.recText]);
          }
          return _results;
        }).call(this);
        _ids = (function() {
          var _i, _len, _ref, _results;

          _ref = this.selectedItems;
          _results = [];
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            selectedItem = _ref[_i];
            _results.push(selectedItem[this.recValue]);
          }
          return _results;
        }).call(this);
        defaultText = _txts.join(';');
        defaultValue = _ids.join(',');
      }
      this.treeId = Ext.id() + '-tree';
      this.store = null;
      this.editable = false;
      this.autoScroll = false;
      this.minWidth = 280;
      if ((this.width == null) || this.width < 280) {
        this.width = 280;
      }
      this.value = defaultText;
      this.hiddenName = this.name;
      this.name = this.name + '-tree';
      this.on({
        afterrender: function() {
          if (defaultValue != null) {
            document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = defaultValue;
          }
          return _this.bodyEl.on('click', function() {
            var bind_Hide_Global, hide_Global, selectedSource, _ref;

            if (!((_ref = _this.tree) != null ? _ref.rendered : void 0)) {
              selectedSource = {};
              if (!Ext.isEmpty(_this.selectedUrl)) {
                selectedSource.url = _this.selectedUrl;
              }
              if (!Ext.isEmpty(_this.selectedItems)) {
                selectedSource.items = _this.selectedItems;
              }
              _this.tree = Ext.create('lppExt.tree.lppTreeView', {
                collapsible: false,
                width: _this.inputEl.dom.offsetWidth + _this.triggerEl.elements[0].dom.offsetWidth,
                minWidth: 'auto',
                maxWidth: 'auto',
                floating: true,
                recKey: _this.recValue,
                selModel: _this.allowMultiSelect ? Ext.create('Ext.selection.CheckboxModel') : Ext.create('Ext.selection.RowModel'),
                height: _this.popupHeight,
                selectedSource: selectedSource,
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
            _this.tree.on('show', function(lppTree, eOpts) {
              var id, ids, _i, _len, _results;

              lppTree.getSelectionModel().deselectAll();
              ids = document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value.split(',');
              _results = [];
              for (_i = 0, _len = ids.length; _i < _len; _i++) {
                id = ids[_i];
                _results.push(lppTree.getSelectionModel().select(lppTree.getStore().getNodeById(id), true));
              }
              return _results;
            });
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

  Ext.define('lppExt.form.field.lppTreeList', config);

}).call(this);
