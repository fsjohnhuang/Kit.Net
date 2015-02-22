(function() {
  var config;

  config = {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.lppCombo',
    queryMode: 'local',
    forceSelection: true,
    editable: true,
    readOnly: false,
    triggerAction: 'all',
    emptyText: '请选择...',
    url: '',
    listeners: {
      beforequery: function(e, eOpts) {
        var combo, value;

        combo = e.combo;
        if (!e.forceAll) {
          value = e.query;
          combo.store.filterBy(function(record, id) {
            var text;

            text = record.get(combo.displayField);
            return text.indexOf(value) >= 0;
          });
          combo.expand();
          return false;
        }
      }
    },
    initComponent: function() {
      var _this = this;

      this.store = Ext.create('Ext.data.Store', {
        autoLoad: true,
        fields: this.fields,
        proxy: {
          type: 'ajax',
          method: 'GET',
          url: this.url,
          reader: {
            root: 'items'
          }
        },
        listeners: {
          load: function(store, records, successful, eOpt) {
            var field, newModel, _i, _len, _ref;

            newModel = {};
            _ref = _this.fields;
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
              field = _ref[_i];
              newModel.field = '';
            }
            return store.insert(0, newModel);
          }
        }
      });
      return this.callParent(arguments);
    }
  };

  Ext.define('lppExt.form.field.lppCombo', config);

}).call(this);
