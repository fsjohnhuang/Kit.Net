Ext.define("lppExt.util.Common", {
    statics: {
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
        syncRequest: function (config) {
            if (Ext.isEmpty(config.url)) return;

            var _reqConfig = {
                url: null,
                method: 'GET',
                params: null,
                success: null,
                failure: null,
                scope: null
            }, p;
            for (p in _reqConfig) {
                switch (p) {
                    case 'params':
                    case 'method':
                        if (!Ext.isEmpty(config[p])) {
                            _reqConfig[p] = config[p];
                        }
                        break;
                    case 'success':
                    case 'failure':
                    case 'scope':
                        if (Ext.isEmpty(config[p])) {
                            delete _reqConfig[p];
                            continue;
                        }

                        _reqConfig[p] = config[p];
                        break;
                    case 'url':
                        _reqConfig[p] = config[p];
                        break;
                }
            }
            _reqConfig.async = false;

            Ext.Ajax.request(_reqConfig);
        },
        /* 获取字符串的适用宽度
        ** @param text{String} 字符串
        ** @param lrSpace{Numeric} 左右两边空位宽度
        ** @return {Numeric} 长度（单位px）
        */
        calcWidth: function (text, lrSpace) {
            var width = 0, i;
            for (i = text.length - 1; i >= 0; --i) {
                if (/^[\u4e00-\u9fa5]*$/.test(text[i])) {
                    width += 14;//14;
                }
                else {
                    width += 10;
                }
            }

            return width + lrSpace*2;
        },
        /* 异步加载文件
        ** @required param path{String} 文件路径
        ** @optional param callback{Function} 回调函数
        */
        require: function (path, callback) {
            var _pNode = document.getElementsByTagName("HEAD");
            if (_pNode && _pNode.length >= 1) {
                _pNode = _pNode[0];
            }
            else {
                _pNode = document.body;
            }
            //排除重复加载
            var _childScripts = _pNode.getElementsByTagName("SCRIPT"),
                i = null;
            for (i = 0; i < _childScripts.length; ++i) {
                if (_childScripts[i].src === path) {
                    if (callback) {
                        callback();
                    }
                    return;
                }
            }

            var _script = document.createElement('SCRIPT');
            _script.type = 'text/javascript';
            _script.src = path;
            _script.onload = _script.onreadystatechange = function () {
                if (!this.readyState
                    || this.readyState === 'loaded'
                    || this.readyState === 'complete') {
                    if (callback) {
                        callback();
                    }
                    _script.onload = _script.onreadystatechange = null;
                }
            };
          
            _pNode.appendChild(_script)
        }
    }
});