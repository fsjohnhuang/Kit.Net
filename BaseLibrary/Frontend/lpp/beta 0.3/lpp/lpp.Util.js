(function() {
  this.lpp.Util = {
    addFavorites: function() {
      var e, ex;

      try {
        return window.external.AddFavorite(location.href, document.title);
      } catch (_error) {
        e = _error;
        try {
          return window.sidebar.addPanel(document.title, location.href, '');
        } catch (_error) {
          ex = _error;
          return alert('加入收藏失败，请使用Ctrl+D进行添加');
        }
      }
    },
    setHome: function() {
      var e, ex, prefs, target;

      e = event || arguments[0];
      target = e.target || e.srcElement;
      try {
        target.style.behavior = 'url(#default#homepage)';
        return target.setHomePage(location.href);
      } catch (_error) {
        e = _error;
        if (window.netscape != null) {
          try {
            netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');
          } catch (_error) {
            ex = _error;
            alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
            prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
          }
          return prefs.setCharPref('browser.startup.homepage', location.href);
        } else {
          return alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
        }
      }
    }
  };

}).call(this);
