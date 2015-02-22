(function() {
  this.lpp.Location = {
    getAP: function(relativePath) {
      return lpp("<div><a href=\"" + relativePath + "\"></a></div>").child('a').dom[0].href;
    },
    getCurDirAP: function() {
      return getAP('./');
    },
    getQueryParam: function(key) {
      var r, reg;

      reg = RegExp("(^|&)" + key + "=([^&]*)(&|$)", "i");
      r = location.search.substr(1).match(reg);
      if (r != null) {
        return unescape(r[2]);
      } else {
        return null;
      }
    }
  };

}).call(this);
