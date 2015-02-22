(function() {
  var Fn, _ref;

  Fn = (function() {
    function Fn() {}

    Fn.prototype.createInterceptor = function(origFn, newFn, owner) {
      var fn, i, interceptor, interceptors, ofn, origFns, prop, _fn, _i, _innerOwner, _j, _k, _len, _len1, _len2, _newFns, _origFns, _owner;

      if (owner == null) {
        owner = null;
      }
      _origFns = [];
      _owner = owner != null ? owner : window;
      if ((origFn.test != null) && (owner != null)) {
        _origFns = (function() {
          var _results;

          _results = [];
          for (prop in owner) {
            fn = owner[prop];
            if (lpp.isFn(fn) && origFn.test(prop)) {
              _results.push(prop);
            }
          }
          return _results;
        })();
      } else if (lpp.isStr(origFn)) {
        origFns = origFn.split(',');
        _origFns = (function() {
          var _i, _len, _results;

          _results = [];
          for (i = _i = 0, _len = origFns.length; _i < _len; i = ++_i) {
            fn = origFns[i];
            _results.push(fn);
          }
          return _results;
        })();
      } else if (lpp.isFn(origFn)) {
        for (prop in _owner) {
          fn = _owner[prop];
          if (lpp.isFn(fn)) {
            if (fn.toString() === origFn.toString()) {
              _origFns.push(prop);
              break;
            }
          }
        }
      } else if (lpp.isArray(origFn)) {
        for (i = _i = 0, _len = origFn.length; _i < _len; i = ++_i) {
          _fn = origFn[i];
          if (lpp.isFn(_fn)) {
            for (prop in _owner) {
              fn = _owner[prop];
              if (lpp.isFn(fn)) {
                if (fn.toString() === _fn.toString()) {
                  _origFns.push(prop);
                  break;
                }
              }
            }
          } else if (lpp.isStr(_fn)) {
            _origFns.push(_fn);
          }
        }
      } else {
        return null;
      }
      if (lpp.isFn(newFn)) {
        _newFns = [newFn];
      } else if (lpp.isArray(newFn)) {
        _newFns = newFn;
      } else {
        return _origFns;
      }
      if (owner != null) {
        _innerOwner = {};
        for (i = _j = 0, _len1 = _origFns.length; _j < _len1; i = ++_j) {
          ofn = _origFns[i];
          _innerOwner[ofn] = owner[ofn];
          owner[ofn] = (function(_ofn) {
            return function() {
              var _k, _len2;

              for (i = _k = 0, _len2 = _newFns.length; _k < _len2; i = ++_k) {
                fn = _newFns[i];
                if (!fn.apply(owner, arguments)) {
                  return;
                }
              }
              return _innerOwner[_ofn].apply(owner, arguments);
            };
          })(ofn);
          owner[ofn].orig = function() {
            return _innerOwner[ofn].apply(owner, arguments);
          };
        }
        return void 0;
      } else {
        interceptors = [];
        for (i = _k = 0, _len2 = _origFns.length; _k < _len2; i = ++_k) {
          ofn = _origFns[i];
          interceptor = (function(_ofn) {
            return function() {
              var _l, _len3;

              for (i = _l = 0, _len3 = _newFns.length; _l < _len3; i = ++_l) {
                fn = _newFns[i];
                if (!fn.apply(window, arguments)) {
                  return;
                }
              }
              return window[_ofn].apply(window, arguments);
            };
          })(ofn);
          interceptors.push(interceptor);
        }
        if (interceptors.length === 1) {
          return interceptors[0];
        } else {
          return interceptors;
        }
      }
    };

    Fn.prototype.createReceptor = function(origFn, newFn, owner) {
      var fn, i, ofn, origFns, prop, receptor, receptors, _fn, _i, _innerOwner, _j, _k, _len, _len1, _len2, _newFns, _origFns, _owner;

      if (owner == null) {
        owner = null;
      }
      _origFns = [];
      _owner = owner != null ? owner : window;
      if ((origFn.test != null) && (owner != null)) {
        _origFns = (function() {
          var _results;

          _results = [];
          for (prop in owner) {
            fn = owner[prop];
            if (lpp.isFn(fn) && origFn.test(prop)) {
              _results.push(prop);
            }
          }
          return _results;
        })();
      } else if (lpp.isStr(origFn)) {
        origFns = origFn.split(',');
        _origFns = (function() {
          var _i, _len, _results;

          _results = [];
          for (i = _i = 0, _len = origFns.length; _i < _len; i = ++_i) {
            fn = origFns[i];
            _results.push(fn);
          }
          return _results;
        })();
      } else if (lpp.isFn(origFn)) {
        for (prop in _owner) {
          fn = _owner[prop];
          if (lpp.isFn(fn)) {
            if (fn.toString() === origFn.toString()) {
              _origFns.push(prop);
              break;
            }
          }
        }
      } else if (lpp.isArray(origFn)) {
        for (i = _i = 0, _len = origFn.length; _i < _len; i = ++_i) {
          _fn = origFn[i];
          if (lpp.isFn(_fn)) {
            for (prop in _owner) {
              fn = _owner[prop];
              if (lpp.isFn(fn)) {
                if (fn.toString() === _fn.toString()) {
                  _origFns.push(prop);
                  break;
                }
              }
            }
          } else if (lpp.isStr(_fn)) {
            _origFns.push(_fn);
          }
        }
      } else {
        return null;
      }
      if (lpp.isFn(newFn)) {
        _newFns = [newFn];
      } else if (lpp.isArray(newFn)) {
        _newFns = newFn;
      } else {
        return _origFns;
      }
      if (owner != null) {
        _innerOwner = {};
        for (i = _j = 0, _len1 = _origFns.length; _j < _len1; i = ++_j) {
          ofn = _origFns[i];
          _innerOwner[ofn] = owner[ofn];
          owner[ofn] = (function(_ofn) {
            return function() {
              var result, _k, _len2;

              result = _innerOwner[_ofn].apply(owner, arguments);
              for (i = _k = 0, _len2 = _newFns.length; _k < _len2; i = ++_k) {
                fn = _newFns[i];
                result = fn.call(owner, result);
              }
              return result;
            };
          })(ofn);
          owner[ofn].orig = function() {
            return _innerOwner[ofn].apply(owner, arguments);
          };
        }
        return void 0;
      } else {
        receptors = [];
        for (i = _k = 0, _len2 = _origFns.length; _k < _len2; i = ++_k) {
          ofn = _origFns[i];
          receptor = (function(_ofn) {
            return function() {
              var result, _l, _len3;

              result = window[_ofn].apply(window, arguments);
              for (i = _l = 0, _len3 = _newFns.length; _l < _len3; i = ++_l) {
                fn = _newFns[i];
                result = fn.call(window, result);
              }
              return result;
            };
          })(ofn);
          receptors.push(receptor);
        }
        if (receptors.length === 1) {
          return receptors[0];
        } else {
          return receptors;
        }
      }
    };

    return Fn;

  })();

  if ((_ref = this.lpp) == null) {
    this.lpp = {};
  }

  lpp.Fn = new Fn();

}).call(this);
