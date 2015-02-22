(function() {
  var config;

  config = {
    extend: 'Ext.form.Panel',
    alias: 'widget.lppUploadPanel',
    uploadUrl: '',
    afterUpload: function() {},
    initComponent: function() {
      if (this.isSingle) {
        this.tbar = [
          {
            icon: "" + lppExt.util.Resource.IMG + "arrow_up.png",
            text: '上传',
            handler: this.upload_click,
            scope: this
          }
        ];
      } else {
        this.tbar = [
          {
            icon: "" + lppExt.util.Resource.IMG + "add.png",
            text: '添加',
            handler: this.add_click,
            scope: this
          }, {
            icon: "" + lppExt.util.Resource.IMG + "arrow_up.png",
            text: '上传',
            handler: this.upload_click,
            scope: this
          }
        ];
      }
      if (this.items == null) {
        this.items = [];
      }
      this.items.push({
        xtype: 'lppUploadField',
        name: 'file0',
        deletable: false
      });
      this.callParent(arguments);
      return void 0;
    },
    add_click: function() {
      var now;

      now = new Date();
      now = now.getMilliseconds() + now.getSeconds();
      this.add({
        xtype: 'lppUploadField',
        name: 'file' + now,
        host: this
      });
      return void 0;
    },
    upload_click: function() {
      var basicFrm;

      basicFrm = this.getForm();
      if (basicFrm.isValid()) {
        basicFrm.submit({
          clientVaildation: true,
          waitTitle: lppExt.util.Msg.TIPS_TITLE,
          waitMsg: lppExt.util.Msg.SUBMITING_MSG,
          url: this.uploadUrl,
          scope: this,
          success: function(form, action) {
            var f, field, i, result, uploadFields, _i, _j, _len, _len1, _ref;

            result = action.result;
            uploadFields = [];
            _ref = this.items;
            for (i = _i = 0, _len = _ref.length; _i < _len; i = ++_i) {
              f = _ref[i];
              if (this.items.items[i].xtype === 'lppUploadField') {
                uploadFields.push(this.items.items[i]);
              }
            }
            for (i = _j = 0, _len1 = uploadFields.length; _j < _len1; i = ++_j) {
              field = uploadFields[i];
              if (i > 0) {
                this.remove(field);
              }
            }
            if (typeof this.afterUpload === "function") {
              this.afterUpload(result);
            }
            return void 0;
          },
          failure: function(form, action) {
            switch (action.failureType) {
              case Ext.form.action.Action.CONNECT_FAILURE:
                return Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR);
              case Ext.form.action.Action.SERVER_INVALID:
                return Ext.Msg.alert(lppExt.util.Msg.TIPS_TITLE, action.result.msg);
            }
          }
        });
      }
      return void 0;
    }
  };

  Ext.syncRequire('lppExt.upload.lppUploadField');

  Ext.define('lppExt.upload.lppUploadPanel', config);

}).call(this);
