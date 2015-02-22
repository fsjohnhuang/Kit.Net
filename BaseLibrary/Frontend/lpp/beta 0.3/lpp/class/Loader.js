(function() {
  var Loader, _base, _ref,
    __indexOf = [].indexOf || function(item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

  Loader = (function() {
    var parseClsName2Path, _config;

    _config = {
      auto: true,
      history: [],
      paths: {
        'lpp': './lpp'
      }
    };

    function Loader() {}

    Loader.prototype.getHistory = function() {
      var v, _i, _len, _ref, _results;

      _ref = _config.history;
      _results = [];
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        v = _ref[_i];
        _results.push(v);
      }
      return _results;
    };

    Loader.prototype.setPaths = function(paths) {
      return lpp.merge(_config.paths, paths);
    };

    Loader.prototype.setAuto = function(isAuto) {
      return _config.auto = lpp.isBool(isAuto) ? isAuto : false;
    };

    Loader.prototype.getAuto = function() {
      return _config.auto;
    };

    Loader.prototype.require = function(clsName, callback) {
      var had, item, loadedFn, pNode, path, script, scripts, _i, _len, _ref;

      if (__indexOf.call(_config.history, clsName) >= 0) {
        callback();
        return void 0;
      }
      path = parseClsName2Path(clsName);
      scripts = document.getElementsByTagName('SCRIPT');
      had = false;
      for (_i = 0, _len = scripts.length; _i < _len; _i++) {
        item = scripts[_i];
        had = item.src === path;
        if (had) {
          break;
        }
      }
      if (!had) {
        loadedFn = function() {
          if (__indexOf.call(_config.history, clsName) < 0) {
            _config.history.push(clsName);
          }
          return typeof callback === "function" ? callback() : void 0;
        };
        script = document.createElement('SCRIPT');
        if ((__indexOf.call(script, 'addEventLitener') >= 0)) {
          script.onload = loadedFn;
        } else if ((__indexOf.call(script, 'readyState') >= 0)) {
          script.onreadystatechange = function() {
            if (this.readyState === 'loaded' || this.readyState === 'complete') {
              return loadedFn();
            }
          };
        } else {
          script.onload = loadedFn;
        }
        _ref = ['text/javascript', path], script.type = _ref[0], script.src = _ref[1];
        pNode = document.getElementsByTagName("HEAD").length >= 1 ? document.getElementsByTagName("HEAD")[0] : document.body;
        pNode.appendChild(script);
      }
      return void 0;
    };

    Loader.prototype.syncRequire = function(clsName) {
      var arg, path, xhr, xhrCtor, _ref;

      if (__indexOf.call(_config.history, clsName) >= 0) {
        return void 0;
      }
      path = parseClsName2Path(clsName);
      _ref = window.XMLHttpRequest != null ? [window.XMLHttpRequest, null] : [ActiveXObject, 'Microsoft.XMLHTTP'], xhrCtor = _ref[0], arg = _ref[1];
      xhr = new xhrCtor(arg);
      xhr.onreadystatechange = function() {
        switch (xhr.readyState) {
          case 1:
            return lpp.log('XHR is opened!');
          case 2:
            return lpp.log('XHR is on preload!');
          case 3:
            return lpp.log('XHR is on loading!');
          case 4:
            if (xhr.status === 200 && xhr.statusText.toLocaleLowerCase() === 'ok') {
              return eval(xhr.responseText);
            }
        }
      };
      xhr.open('GET', path, false);
      xhr.send(null);
      if (__indexOf.call(_config.history, clsName) < 0) {
        _config.history.push(clsName);
      }
      return void 0;
    };

    /* 私有方法
    */


    parseClsName2Path = function(clsName) {
      var index, leastPath, p, path, replacedPath, v, _ref;

      path = leastPath = replacedPath = '';
      _ref = _config.paths;
      for (p in _ref) {
        v = _ref[p];
        if ((index = clsName.indexOf(p)) >= 0) {
          leastPath = clsName.substring(index + p.length);
          replacedPath = v;
          break;
        }
      }
      if (index === -1) {
        return path = path.replace(/\./g, '/') + '.js';
      } else {
        return path = replacedPath + leastPath.replace(/\./g, '/') + '.js';
      }
    };

    return Loader;

  })();

  if ((_ref = (_base = this.lpp)["class"]) == null) {
    _base["class"] = {};
  }

  this.lpp["class"].Loader = new Loader();

  this.lpp.Loader = this.lpp["class"].Loader;

  this.lpp.require = this.lpp.Loader.require;

  this.lpp.syncRequire = this.lpp.Loader.syncRequire;

}).call(this);
