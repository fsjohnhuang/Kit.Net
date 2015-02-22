(function() {
  var config;

  config = {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.lppTreeList2',
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
          return document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = _this.defaultValue;
        }
      });
      return this.callParent(arguments);
    },
    expand: function() {
      var on_click, set_on_click, _ref,
        _this = this;

      this.callParent(arguments);
      if (!((_ref = this.tree) != null ? _ref.rendered : void 0)) {
        this.tree = Ext.create('lppExt.tree.lppTreeView', {
          collapsible: false,
          width: this.inputEl.dom.offsetWidth + this.triggerEl.elements[0].dom.offsetWidth,
          minWidth: 'auto',
          maxWidth: 'auto',
          floating: true,
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
                  _this.tree.getSelectionModel().deselectAll();
                  document.getElementsByName(_this.hiddenName, _this.bodyEl.dom)[0].value = '';
                  _this.setValue('');
                  return _this.tree.hide();
                }
              }, {
                type: 'submit',
                icon: "" + lppExt.util.Resource.IMG + "check.png",
                tooltip: '确定',
                handler: function(item) {
                  var ids, selection, selections, texts, _i, _len;

                  _this.tree.hide();
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
          remoteProxy: this.popupUrl
        });
      }
      this.tree.showAt(this.getX() + this.labelWidth + 5, this.getY() + 22);
      on_click = function() {
        _this.tree.hide();
        return _this.bodyEl.un('click', on_click);
      };
      set_on_click = function() {
        return _this.bodyEl.on('click', on_click);
      };
      setTimeout(set_on_click, 20);
      return this.on({
        /*collapse: =>
        				selections = @tree.getSelectionModel().getSelection()
        				@tree.hide()
        
        				if @allowMultiSelect
        					texts = ''
        					ids = ''
        					for selection in selections
        						texts += selection.get('text') + '; '
        						ids += selection.getId() + ','
        					@setValue texts
        					ids = ids.substring(0, ids.length - 1) if ids.length >= 1
        					document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = ids
        				else
        					return if selections.length == 0
        					@setValue selections[0].get('text')
        					document.getElementsByName(@hiddenName, @bodyEl.dom)[0].value = selections[0].getId()
        */

        destory: function() {
          _this.callParent(arguments);
          _this.tree.destory();
          return delete _this.tree[_this.treeId];
        }
      });
    }
  };

  Ext.define('lppExt.form.field.lppTreeList2', config);

}).call(this);
