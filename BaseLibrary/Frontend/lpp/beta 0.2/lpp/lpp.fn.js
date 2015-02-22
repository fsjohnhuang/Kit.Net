!function () {
    this.lpp.fn = {
        createDelegate: function (fn, ctx, args) {
            var _args = Array.prototype.slice.apply(arguments).slice(2);
            return function () {
                fn.apply(ctx, _args.concat(Array.prototype.slice.apply(arguments)));
            }
        },
        createCallback: function (fn, args) {
            var _args = Array.prototype.slice.apply(arguments).slice(1);
            return function () {
                fn.apply(this, _args.concat(Array.prototype.slice.apply(arguments)));
            }
        },
        throttle: function (fn, delay, mustRunDelay) {
            var _timer = null, _startTime = null;

            return function () {
                var _cxt = this, _args = arguments, _curTime = +new Date();
                clearTimeout(_timer);

                if (!_startTime) {
                    _startTime = _curTime;
                }
                if (_curTime - _startTime >= mustRunDelay) {
                    fn.apply(_cxt, _args);
                    _startTime = _curTime;
                }
                else {
                    _timer = setTimeout(function () {
                        fn.apply(_cxt, _args);
                    }, delay);
                }
            };
        }
    };
}();
