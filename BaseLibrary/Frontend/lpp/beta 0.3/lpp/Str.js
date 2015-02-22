/*
字符串处理模块
*/


(function() {
  var __slice = [].slice;

  this.lpp.Str = {
    calcWidth: function(str, lrSpace) {
      var character, strArray, width, _i, _len;

      if (lrSpace == null) {
        lrSpace = 0;
      }
      width = 0;
      strArray = lpp.toArray(str);
      for (_i = 0, _len = strArray.length; _i < _len; _i++) {
        character = strArray[_i];
        if (/^[\u4e00-\u9fa5]*$/.test(character)) {
          width += 14;
        } else {
          width += 8;
        }
      }
      return width + lrSpace * 2;
    },
    trim: function(str) {
      return str.replace(/(^\s*)|(\s*$)/g, '');
    },
    ltrim: function(str) {
      return str.replace(/(^\s*)/g, '');
    },
    rtrim: function(str) {
      return str.replace(/(\s*$)/g, '');
    },
    splice: function() {
      var els, len, result, startIndex, str, strResult;

      str = arguments[0], startIndex = arguments[1], len = arguments[2], els = 4 <= arguments.length ? __slice.call(arguments, 3) : [];
      els.unshift(len);
      els.unshift(startIndex);
      els.unshift(str);
      result = lpp.splice.apply(this, els);
      strResult = {};
      strResult.str = result.items.join('');
      strResult.deleted = result.deleted.join('');
      return strResult;
    },
    format: function() {
      var i, str, val, value, _i, _len;

      str = arguments[0], value = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
      for (i = _i = 0, _len = value.length; _i < _len; i = ++_i) {
        val = value[i];
        str = str.replace(RegExp("\\{" + i + "\\}", "g"), val);
      }
      return str;
    },
    tpl: function(str, json, prefix) {
      var prop, val;

      if (prefix == null) {
        prefix = '@';
      }
      for (prop in json) {
        val = json[prop];
        str = str.replace(RegExp("" + prefix + prop, "g"), val);
      }
      return str;
    }
  };

}).call(this);
