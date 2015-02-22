(function() {
  var Array, _ref,
    __slice = [].slice;

  Array = (function() {
    function Array() {}

    Array.prototype.toArray = function(arrayLike) {
      var e, i, item, resultArray, _i, _j, _len, _len1, _results;

      try {
        if (lpp.isIE() && lpp.isStr(arrayLike)) {
          resultArray = [];
          for (i = _i = 0, _len = arrayLike.length; _i < _len; i = ++_i) {
            item = arrayLike[i];
            resultArray.push(arrayLike.substr(i, 1));
          }
          return resultArray;
        } else {
          return Array.prototype.slice.apply(arrayLike);
        }
      } catch (_error) {
        e = _error;
        _results = [];
        for (_j = 0, _len1 = arrayLike.length; _j < _len1; _j++) {
          item = arrayLike[_j];
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

    Array.prototype.grep = function(arraryLike, fn) {
      var i, item, _array, _i, _len, _results;

      _array = lpp.isArray(arrayLike) ? arrayLike : this.toArray(arrayLike);
      if (_array.filter != null) {
        return _array.filter(fn);
      } else {
        _results = [];
        for (i = _i = 0, _len = _array.length; _i < _len; i = ++_i) {
          item = _array[i];
          if (fn(item, i)) {
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
