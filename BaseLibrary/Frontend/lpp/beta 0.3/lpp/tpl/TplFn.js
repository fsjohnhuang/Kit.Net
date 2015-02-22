/* 内置函数(位于解释器内置作用域)
*/


(function() {
  var TplFn;

  TplFn = (function() {
    function TplFn() {}

    TplFn.prototype.len = function(obj) {
      var p, type, _len;

      type = lpp.getType(obj);
      switch (type) {
        case 'boolean':
        case 'number':
          return obj.toString().length;
        case 'array':
          return obj.length;
        case 'object':
          _len = 0;
          for (p in obj) {
            _len += 1;
          }
          return _len;
        default:
          return 0;
      }
    };

    return TplFn;

  })();

  lpp.define('lpp.tpl.TplFn', TplFn);

}).call(this);
