!function () {
    this.lpp.loader = {
        syncRequire: function (path) {
            var scripts = document.getElementsByTagName('SCRIPT'),
                i, l;
            for (i = 0, l = scripts.length; i < l; ++i) {
                if (scripts[i].src === path) {
                    if (callback) {
                        callback();
                        return;
                    }
                }
            }

            var _pNode = document.getElementsByTagName("HEAD") && document.getElementsByTagName("HEAD")[0] || document.body;
            var _script = document.createElement('script');
            _script.type = 'text/javascript';
            _script.src = path;

            _pNode.appendChild(_script);
        },
        require: function (path, callback) {
            var scripts = document.getElementsByTagName('SCRIPT'),
                i, l;
            for (i = 0, l = scripts.length; i < l; ++i) {
                if (scripts[i].src === path){
                    if (callback){
                        callback();
                        return;
                    }
                }
            }

            var _pNode = document.getElementsByTagName("HEAD") && document.getElementsByTagName("HEAD")[0] || document.body;
            var _script = document.createElement('script');
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
          
            _pNode.appendChild(_script);
        }
    };
}();