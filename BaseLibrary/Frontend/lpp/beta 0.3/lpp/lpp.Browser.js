(function() {
  this.lpp.Browser = {
    isIE: function() {
      return navigator.appName === 'Microsoft Internet Explorer';
    },
    isIE7: function() {
      return navigator.userAgent.indexOf('MSIE 7') >= 0;
    },
    isNetscape: function() {
      return navigator.appName === 'Netscape';
    },
    clientHeight: function() {
      var de;

      de = document.documentElement;
      if (de != null) {
        return de.clientHeight;
      } else {
        return document.body.clientHeight;
      }
    },
    clientWidth: function() {
      var de;

      de = document.documentElement;
      if (de != null) {
        return de.clientWidth;
      } else {
        return document.body.clientWidth;
      }
    }
  };

}).call(this);
