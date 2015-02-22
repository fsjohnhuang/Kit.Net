(function() {
  var Browser, _ref;

  Browser = (function() {
    function Browser() {}

    Browser.prototype.isIE = function() {
      return navigator.appName === 'Microsoft Internet Explorer';
    };

    Browser.prototype.isIE7 = function() {
      return navigator.userAgent.indexOf('MSIE 7') >= 0;
    };

    Browser.prototype.isNetscape = function() {
      return navigator.appName === 'Netscape';
    };

    Browser.prototype.clientHeight = function() {
      var de;

      de = document.documentElement;
      if (de != null) {
        return de.clientHeight;
      } else {
        return document.body.clientHeight;
      }
    };

    Browser.prototype.clientWidth = function() {
      var de;

      de = document.documentElement;
      if (de != null) {
        return de.clientWidth;
      } else {
        return document.body.clientWidth;
      }
    };

    return Browser;

  })();

  if ((_ref = this.lpp) == null) {
    this.lpp = {};
  }

  lpp.Browser = new Browser();

  if (typeof lpp.merge === "function") {
    lpp.merge(lpp, lpp.Browser);
  }

}).call(this);
