(function() {
  var config;

  config = {
    extend: 'Ext.form.field.File',
    alias: 'widget.lppUploadField',
    fieldLabel: '文件',
    labelWidth: 30,
    allowBlank: false,
    msgTarget: 'side',
    anchor: '100%',
    style: {
      margin: '5px'
    },
    buttonText: '',
    buttonConfig: {
      icon: "" + lppExt.util.Resource.IMG + "image_add.png",
      style: {
        position: 'relative',
        marginLeft: '5px'
      }
    },
    host: null,
    onRender: function(ct, eOpt) {
      var delBtn, delImg, tbl, td,
        _this = this;

      this.callParent(arguments);
      if ((this.deletable != null) && !this.deletable) {
        return;
      }
      tbl = this.getEl().dom.getElementsByTagName('TABLE');
      td = tbl[0].getElementsByTagName('td');
      delImg = document.createElement('DIV');
      delImg.className = 'x-btn-wrap';
      delImg.style.height = '17px';
      delImg.style.width = '17px';
      delImg.style.backgroundImage = "url(" + lppExt.util.Resource.IMG + "delete.png)";
      delImg.style.position = 'relative';
      if (Ext.isIE8m) {
        delImg.style.top = '3px';
      }
      delBtn = document.createElement('DIV');
      delBtn.className = 'x-btn x-form-file-btn x-unselectable x-btn-default-small x-icon x-btn-icon x-btn-default-small-icon';
      delBtn.style.width = '22px';
      delBtn.style.height = '22px';
      console.log(this.name);
      delBtn.onclick = function() {
        _this.host.remove(_this);
        return void 0;
      };
      delBtn.appendChild(delImg);
      td[1].appendChild(delBtn);
      td[1].style.width = '53px';
      return void 0;
    }
  };

  Ext.define('lppExt.upload.lppUploadField', config);

}).call(this);
