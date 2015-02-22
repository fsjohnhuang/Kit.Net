(function() {
  var ClsMgr, _base, _ref,
    __slice = [].slice;

  ClsMgr = (function() {
    var implementing;

    function ClsMgr() {}

    implementing = function(cls, mixins) {
      var key, member, mixin, _i, _len, _ref;

      for (_i = 0, _len = mixins.length; _i < _len; _i++) {
        mixin = mixins[_i];
        for (key in mixin) {
          member = mixin[key];
          if (!lpp.hasOwnProperty(cls.prototype, key)) {
            cls.prototype[key] = member;
          }
        }
        _ref = mixin.prototype;
        for (key in _ref) {
          member = _ref[key];
          if (!lpp.hasOwnProperty(cls.prototype, key)) {
            cls.prototype[key] = member;
          }
        }
      }
      return cls;
    };

    ClsMgr.prototype.define = function() {
      var cls, clsName, extend, fullname, i, item, key, mixins, ns, target, value, _base, _i, _len;

      fullname = arguments[0], cls = arguments[1], extend = arguments[2], mixins = 4 <= arguments.length ? __slice.call(arguments, 3) : [];
      target = window;
      ns = fullname.split('.');
      for (i = _i = 0, _len = ns.length; _i < _len; i = ++_i) {
        item = ns[i];
        if (i < ns.length - 1) {
          target = target[item] || (target[item] = {});
        }
        if (i === ns.length - 1) {
          clsName = item;
        }
      }
      if (extend != null) {
        if (lpp.isStr(extend) && lpp.Loader.getAuto()) {
          lpp.syncRequire(extend);
        }
        if (lpp.isStr(extend)) {
          eval("var extend = " + extend);
        }
        for (key in extend) {
          value = extend[key];
          if (lpp.hasOwnProperty(extend, key) && !lpp.hasOwnProperty(cls, key)) {
            cls[key] = value;
          }
        }
        _base = function() {
          var _ref;

          this.constructor = cls;
          _ref = cls.prototype;
          for (key in _ref) {
            value = _ref[key];
            this[key] = value;
          }
          return this;
        };
        _base.prototype = extend.prototype;
        cls.prototype = new _base();
        cls.__super__ = extend.prototype;
      }
      implementing(cls, mixins);
      target[clsName] = cls;
      return void 0;
    };

    ClsMgr.prototype.create = function() {
      var args, fullname;

      fullname = arguments[0], args = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
      if (lpp.Loader.getAuto()) {
        lpp.syncRequire(fullname);
      }
      eval("var targetCls = " + fullname);
      return (function(func, args, ctor) {
        ctor.prototype = func.prototype;
        var child = new ctor, result = func.apply(child, args);
        return Object(result) === result ? result : child;
      })(targetCls, args, function(){});
    };

    return ClsMgr;

  })();

  if ((_ref = (_base = this.lpp)["class"]) == null) {
    _base["class"] = {};
  }

  this.lpp["class"].ClsMgr = new ClsMgr();

  this.lpp.define = this.lpp["class"].ClsMgr.define;

  this.lpp.create = this.lpp["class"].ClsMgr.create;

}).call(this);
