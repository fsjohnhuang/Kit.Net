(function() {
  var Array, _ref,
    __slice = [].slice;

  Array = (function() {
    function Array() {}

    Array.prototype.toArray = function(arrayLike) {
      var e, item, _i, _len, _results;

      try {
        if (lpp.isIE() && lpp.isStr(arrayLike)) {
          return arrayLike.split('');
        } else {
          return Array.prototype.slice.apply(arrayLike);
        }
      } catch (_error) {
        e = _error;
        _results = [];
        for (_i = 0, _len = arrayLike.length; _i < _len; _i++) {
          item = arrayLike[_i];
          _results.push(item);
        }
        return _results;
      }
    };

    Array.prototype.splice = function() {
      var arrayLike, els, len, result, startIndex, _array;

      arrayLike = arguments[0], startIndex = arguments[1], len = arguments[2], els = 4 <= arguments.length ? __slice.call(arguments, 3) : [];
      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      els.unshift(len);
      els.unshift(startIndex);
      result = {};
      result.deleted = [].splice.apply(_array, els);
      result.items = _array;
      return result;
    };

    Array.prototype.grep = function(arraryLike, fn, fetch) {
      var i, item, _array, _i, _len, _results;

      if (fetch == null) {
        fetch = true;
      }
      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if ((_array.filter != null) && fetch === true) {
        return _array.filter(fn);
      } else {
        _results = [];
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          if (fn(item, i) === fetch) {
            _results.push(item);
          }
        }
        return _results;
      }
    };

    Array.prototype.each = function(arrayLike, fn) {
      var i, item, _array, _i, _len;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if (_array.forEach != null) {
        _array.forEach(fn);
      } else {
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          fn(item, i);
        }
      }
      return void 0;
    };

    Array.prototype.map = function(arrayLike, fn) {
      var i, item, _array, _i, _len, _results;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if (_array.map != null) {
        return _array.map(fn);
      } else {
        _results = [];
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          _results.push(fn(item, i));
        }
        return _results;
      }
    };

    Array.prototype.every = function(arrayLike, fn) {
      var i, item, _array, _i, _len;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if (_array.every != null) {
        return _array.every(fn);
      } else {
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          if (!fn(item, i)) {
            return false;
          }
        }
        return true;
      }
    };

    Array.prototype.some = function(arrayLike, fn) {
      var i, item, _array, _i, _len;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if (_array.some != null) {
        return _array.some(fn);
      } else {
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          if (fn(item, i)) {
            return true;
          }
        }
        return false;
      }
    };

    Array.prototype.empty = function(arrayLike) {
      var _array;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      _array.length = 0;
      return _array;
    };

    Array.prototype.max = function(arrayLike) {
      var _array;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      return Math.max.apply(Math, _array);
    };

    Array.prototype.min = function(arrayLike) {
      var _array;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      return Math.min.apply(Math, _array);
    };

    Array.prototype.random = function(arrayLike) {
      var _array;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      return _array[Math.floor(Math.random() * _array.length)];
    };

    return Array;

  })();

  if ((_ref = this.lpp) == null) {
    this.lpp = {};
  }

  lpp.Array = new Array();

  if (typeof lpp.merge === "function") {
    lpp.merge(lpp, lpp.Array);
  }

}).call(this);
