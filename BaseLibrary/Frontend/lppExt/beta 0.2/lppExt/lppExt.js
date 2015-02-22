(function() {
  var __slice = [].slice;

  Ext.define('lppExt', {
    statics: {
      createDelegate: function() {
        var addition, ctx, fn;

        fn = arguments[0], ctx = arguments[1], addition = 3 <= arguments.length ? __slice.call(arguments, 2) : [];
        if (ctx == null) {
          ctx = window;
        }
        return function() {
          var args;

          args = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
          return fn.apply(ctx, addition.concat(args));
        };
      },
      createCallback: function() {
        var addition, fn;

        fn = arguments[0], addition = 2 <= arguments.length ? __slice.call(arguments, 1) : [];
        return function() {
          var args;

          args = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
          return fn.apply(window, addition.concat(args));
        };
      }
    }
  });

}).call(this);
