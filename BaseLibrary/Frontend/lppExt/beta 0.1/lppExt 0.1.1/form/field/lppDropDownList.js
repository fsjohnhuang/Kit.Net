(function() {
  var config;

  config = {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.lppDropDownList',
    queryMode: 'local',
    allowBlank: false,
    editable: false,
    url: '',
    fields: [],
    initComponent: function() {
      if ((this.url != null) && this.url !== '') {
        lppExt.util.Common.syncRequest({
          url: this.url,
          scope: this,
          success: function(response, eOpt) {
            var jsonResult;

            jsonResult = Ext.decode(response.responseText);
            if (!jsonResult.success) {
              return;
            }
            return this.store = {
              fields: this.fields,
              data: jsonResult.items
            };
          }
        });
      }
      this.callParent(arguments);
      return void 0;
    }
  };

  Ext.define('lppExt.form.field.lppDropDownList', config);

}).call(this);
