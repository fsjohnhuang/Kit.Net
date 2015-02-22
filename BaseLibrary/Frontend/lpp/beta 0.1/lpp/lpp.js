var lpp = {
    error: function (msg) {
        try {
            console.error(msg);
        }
        catch (e) {
        }
    },
    warn: function (msg) {
        try {
            console.warn(msg);
        }
        catch (e) {
        }
    },
    log: function (msg) {
        try {
            console.log(msg);
        }
        catch (e) {
        }
    },
    getType: function (o) {
        var _t;
        return ((_t = typeof (o)) == "object" ?
            o == null && "null" || Object.prototype.toString.call(o).slice(8, -1) : _t).toLowerCase();
    },
    isNothing: function (o) {
        var typeStr = this.getType(o);
        return typeStr === "null" || typeStr === "undefined";
    },
    get: function (id) {
        return document.getElementById(id);
    },
    // @param {string/HTMLXXXElement} el id 或 dom对象
    // @return {HTMLXXXElement} dom对象
    remove: function (el) {
        var _el = el;
        if (typeof _el === "string") {
            _el = this.get(_el);
            if (_el === null) {
                _t.error("there is no element's id is " + o);
                return false;
            }
        }
        return _el.parentNode.removeChild(_el);
    },
    on: function (el, eventName, handler) {
        var _el = el;
        var _handler = handler;
        if (typeof _el === "string") {
            _el = this.get(_el);
        }
        if (eventName === "leave") {
            var fns = (function (fn, delay) {
                var _timer = null, _fns = {};
                _fns.onmouseout = function () {
                    _timer = setTimeout(fn, delay);
                };

                _fns.onmousemove = function () {
                    clearTimeout(_timer);
                };

                return _fns;
            })(handler, 350);

            this.on(el, "mouseout", fns.onmouseout);
            this.on(el, "mousemove", fns.onmousemove);
        }
        else {
            if (_el.attachEvent) {
                _el.attachEvent("on" + eventName, _handler);
            }
            else if (_el.addEventListener) {
                _el.addEventListener(eventName, _handler, false);
            }
        }
    },
    css: function (el, propertyName, value) {
        var _el = el;
        if (typeof _el === "string") {
            _el = this.get(_el);
        }
        if (arguments.length === 2) {
            return _el.style[propertyName];
        }
        else if (arguments.length === 3) {
            _el.style[propertyName] = value;
            return _el;
        }
        else {
            throw { name: "Arguments Exception", message: "Count of Arguments is wrong!" };
        }
    },
    addStylesheet: function (id, cssRuleText) {
        if (this.get(id)) return;

        var newStyle = document.createElement("STYLE");
        newStyle.id = id;
        newStyle.type = "text/css";
        newStyle.media = "screen";

        if (newStyle.styleSheet) {
            newStyle.styleSheet.cssText = cssRuleText;
        }
        else {
            newStyle.appendChild(document.createTextNode(cssRuleText));
        }
        var parentNode = document.getElementsByTagName("HEAD")[0] || document.body;
        parentNode.appendChild(newStyle);
    },
    getQueryString: function _getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    val: function (el, value) {
        var _el = el;
        if (typeof _el === "string") {
            _el = this.get(el);
        }

        var isGet = arguments.length === 1,
            i = null;

        switch (_el.tagName.toLocaleLowerCase()) {
            case 'select':
                if (isGet) {
                    return _el.options[_el.selectedIndex].value;
                }
                else {
                    var selectedIndex = 0;
                    for (i = 0; i < _el.options.length; ++i) {
                        if (_el.options[i].value === value.toString()) {
                            selectedIndex = i;
                            break;
                        }
                    }
                    _el.selectedIndex = selectedIndex;
                }
                break;
            default:
                if (isGet) {
                    return _el.value;
                }
                else {
                    _el.value = value;
                }
                break;
        }
    },
    attr: function (el, propertyName, value) {
        var _el = el;
        if (typeof _el === "string") {
            _el = this.get(el);
        }
        if (arguments.length === 2) {
            if (!this.isNothing(_el[propertyName])) {
                return _el[propertyName];
            }
            else {
                return _el.getAttribute(propertyName);
            }
        }
        else if (arguments.length === 3) {
            if (propertyName === 'checked' && (this.getType(value) !== 'boolean' ? value == false : !value)) {
                return _el;
            }
            _el.setAttribute(propertyName, value);
            return _el;
        }
        else {
            throw { name: "Arguments Exception", message: "Count of Arguments is wrong!" };
        }
    },
    addFavorites: function _addFavorites() {
        window.external.AddFavorite(location.href, document.title);
    },
    importFavorites: function _importFavorites() {
        window.external.ImportExportFavorites(true);
    },
    exportFavorites: function _exportFavorites() {
        window.external.ImportExportFavorites(false);
    },
    tidyFavorites: function _tidyFavorites() {
        window.external.ShowBrowserUI(OrganizeFavorites, null);
    },
    viewSource: function _viewSource() {
        window.location = "view-source:" + window.location.href;
    },
    setLanguage: function _setLanguage() {
        window.external.ShowBrowserUI(LanguageDialog, null);
    },
    un: function (el, eventName, handler) {
        var _el = el;
        if (typeof _el === "string") {
            _el = this.get(el);
        }

        if (_o.detachEvent) {
            _o.detachEvent("on" + eventName, handler);
        }
        else if (_o.removeEventListener) {
            _o.removeEventListener(eventName, handler, false);
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
    },
    getEventTarget: function (e) {
        return e.target || e.srcElement;
    },
    hasClass: function (el, className) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        return el.className.match(reg);
    },
    addClass: function (el, className) {
        if (!this.hasClass(el, className)) {
            el.className += " " + className;
        }
    },
    removeClass: function (el, className) {
        if (this.hasClass(el, className)) {
            var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
            el.className = el.className.replace(reg, ' ');
        }
    },
    validateType: function (obj, dataTypes) {
        var valid = false;
        if (arguments.length !== 2) throw { message: "arguments is wrong!", name: "Exception!" };

        var _dataTypes = [];
        if ("array" === this.getType(dataTypes)) {
            _dataTypes = dataTypes;
        }
        else if ("string" === this.getType(dataTypes)) {
            _dataTypes.push(dataTypes);
        }
        else {
            throw { message: "Data type of dataTypes is wrong!", name: "Data Type Exception!" };
        }

        for (var i = _dataTypes.length - 1; i >= 0; --i) {
            if (_dataTypes[i].toLocaleLowerCase() === this.getType(obj)) {
                valid = true;
                break;
            }
        }

        return valid;
    },
    getConfigVal: function (config, propertyName, defaultVal, dataTypes, includsion) {
        var _val = null;
        var _valid = false;
        if (config && config[propertyName] && !this.isNothing(dataTypes)) {
            _val = config[propertyName];
            _valid = this.validateType(_val, dataTypes);
            if (!_valid) {
                throw { message: ["Data type of ", propertyName, " is wrong!"].join(), name: "Data Type Exception!" };
            }

            if (dataTypes === "string" && !this.isNothing(includsion)) {
                var _includsion = [];
                if (typeof includsion === "string") {
                    _includsion.push(includsion);
                }
                else {
                    _includsion = includsion;
                }
                _valid = false;
                for (var i = _includsion.length - 1; i >= 0; --i) {
                    var exp = new RegExp(_includsion[i]);
                    if (exp.test(_val)) {
                        _valid = true;
                        break;
                    }
                }

                if (!_valid) {
                    throw { message: ["Value of ", propertyName, " is wrong!"].join(), name: "Value Range Exception!" };
                }
                return _val;
            }
            else {
                return _val;
            }
        }
        else {
            return (config ? config[propertyName] || defaultVal : defaultVal);
        }
    },
    /*
    * @param config{Object}
    *      tagName{String}
    *      attrs{Object}
    *      className{Array}
    *      html{String}
    *      text{String}
    *      id{String}
    *      handlers{Object}
    */
    createEl: function (config) {
        var _tagName = this.getConfigVal(config, "tagName", "DIV", "string"),
        _attrs = this.getConfigVal(config, "attrs", {}, "object"),
        _className = this.getConfigVal(config, "className", [], ["array", "string"]),
        _html = this.getConfigVal(config, "html", "", "string"),
        _text = this.getConfigVal(config, "text", "", "string"),
        _id = this.getConfigVal(config, "id", "", "string"),
        _handlers = this.getConfigVal(config, "handlers", {}, "object");

        var el = document.createElement(_tagName);
        for (p in _attrs) {
            if (_attrs.hasOwnProperty(p) && "string" === this.getType(_attrs[p])) {
                el.setAttribute(p, _attrs[p]);
            }
        }
        if (this.getType(_className) === "array") {
            for (var i = 0; i < _className.length; ++i) {
                this.addClass(el, _className[i]);
            }
        }
        else {
            this.addClass(el, _className);
        }

        if (_html || _text) {
            el.innerText = _text;
            if (_html) {
                el.innerHTML = _html;
            }
        }

        if (_id) {
            el.setAttribute("id", _id);
        }

        for (var p in _handlers) {
            if (_attrs.hasOwnProperty(p) && "function" === this.getType(_handlers[p])) {
                this.on(el, p, _handlers[p]);
            }
        }

        return el;
    },
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
    /*
    *  @param config{Object}
    *      el{HtmlElement} 执行动画的Dom对象
    *      pos{string} 定位：relative,absolute；默认为absolute
    *      animation{String} value: easeIn
    *      x{number/string} unit: px number时为相对于容器左上角的x坐标，string且格式为"+=数字"或"-=数字"时为相对现在位置的x位移
    *      y{number/string} unit: px number时为相对于容器左上角的x坐标，string且格式为"+=数字"或"-=数字"时为相对现在位置的y位移
    *      time{number} unit: ms 动画时间
    *      beforeSlide{Function}
    *          @param el{HTMLElement}
    *      afterSlide{Function}
    *          @param el{HTMLElement}
    */
    slide: function (config) {
        var _el = this.getConfigVal(config, "el", null);
        if (!_el) return false;

        var _animation = this.getConfigVal(config, "animation", "easeIn", "string", ["easeIn"]),
        _pos = this.getConfigVal(config, "pos", "absolute", "string", ["absolute", "relative"]),
        _x = this.getConfigVal(config, "x", "+=0", ["number", "string"], ["^+=\\d+$", "^-=\\d+$"]),
        _y = this.getConfigVal(config, "y", "+=0", ["number", "string"], ["^+=\\d+$", "^-=\\d+$"]),
        _time = this.getConfigVal(config, "time", 0, "number"),
        _beforeSlide = this.getConfigVal(config, "beforeSlide", function () { }, "function"),
        _afterSlide = this.getConfigVal(config, "afterSlide", function () { }, "function");

        var parentNodePos = this.css(_el.parentNode, "position");
        if (parentNodePos !== "absolute" && parentNodePos !== "relative") {
            this.css(_el.parentNode, "position", "relative");
        }

        if (_pos === "absolute") {
            this.css(_el, "position", "absolute");
        }
        else {
            this.css(_el, "position", "relative");
        }

        var _dX = 0, _dY = 0; // X、Y轴的位移量
        if (typeof _x === "number") {
            _dX = _x - this.attr(el, "offsetLeft");
        }
        else {
            _dX = +(_x.substring(0, 1) + _x.substring(2));
        }
        if (typeof _y === "number") {
            _dY = _y - this.attr(el, "offsetTop");
        }
        else {
            _dY = +(_y.substring(0, 1) + _y.substring(2));
        }

        var animateFn = null;
        switch (_animation) {
            case "easeIn":
                animateFn = function (lpp) {
                    var _tX = null, _tY = null;
                    if (_dX !== 0) {
                        _tX = _time / (Math.pow(_dX, 2));
                    }
                    if (_dY !== 0) {
                        _tY = _time / (Math.pow(_dY, 2));
                    }

                    for (var i = 1; i <= _time; ++i) {
                        if (i === 1) {
                            _beforeSlide(_el);
                        }
                        setTimeout(lpp.createDelegate(function (el, tX, tY, time, xDir, yDir, isLast, afterSlide) {
                            if (tX) {
                                var originalLeft = null;
                                if (time === 1) {
                                    originalLeft = lpp.attr(el, "offsetLeft");
                                    lpp.attr(el, "originalLeft", originalLeft.toString());
                                }
                                else {
                                    originalLeft = parseInt(lpp.attr(el, "originalLeft"));
                                }
                                var tmpLeft = xDir * Math.sqrt(time / tX);
                                var newLeft = (originalLeft + tmpLeft) + "px";
                                lpp.css(el, "left", newLeft);
                            }
                            if (tY) {
                                var originalTop = null;
                                if (time === 1) {
                                    originalTop = lpp.attr(el, "offsetTop");
                                    lpp.attr(el, "originalTop", originalTop.toString());
                                }
                                else {
                                    originalTop = parseInt(lpp.attr(el, "originalTop"));
                                }
                                var tmpTop = yDir * Math.sqrt(time / tY);
                                var newTop = (originalTop + tmpTop) + "px";
                                lpp.css(el, "top", newTop);
                            }
                            if (isLast) {
                                afterSlide(el);
                            }
                        }, this, _el, _tX, _tY, i, (_dX >= 0 ? 1 : -1), (_dY >= 0 ? 1 : -1), i === _time, _afterSlide), i);
                    }
                };
                break;
        }

        if (animateFn) {
            animateFn(this);
            return true;
        }

        return false;
    },
    /*
    * @param config{Object}
    *      el{HTMLElement}
    *      time{number} unit:ms
    *      display{String} 值：block(使用display: block), inline(使用display: inline), visible(使用visibility: visible), none(仅使用opacity:1); 默认为block
    *      beforeShow{Function}
    *          @param  el{HTMLElement}
    *      afterShow{Function}
    *          @param  el{HTMLElement}
    */
    show: function (config) {
        var _el = this.getConfigVal(config, "el", null),
        _time = this.getConfigVal(config, "time", 0, "number"),
        _afterShow = this.getConfigVal(config, "afterShow", function () { }, "function"),
        _beforeShow = this.getConfigVal(config, "beforeShow", function () { }, "function"),
        _display = this.getConfigVal(config, "display", "block", "string", ["block", "inline", "visible", "none"]);

        if (!_el) return false;

        if (_time) {
            this.css(_el, "opacity", "0");
            this.css(_el, "filter", "alpha(opacity=0)");
            switch (_display) {
                case "block":
                    this.css(_el, "display", "block");
                    break;
                case "inline":
                    this.css(_el, "display", "inline");
                    break;
                case "visible":
                    this.css(_el, "visibility", "visible");
                    break;
                default:
                    break;
            }

            var _t = _time;
            for (var i = 1; i <= _time; ++i) {
                setTimeout(this.createCallback(function (lpp, el, t, time, isFirst, beforeShow, isLast, afterShow) {
                    lpp.css(el, "opacity", time / t);
                    lpp.css(el, "filter", "alpha(opacity=" + (time / t * 100) + ")");
                    if (isFirst) {
                        beforeShow(el);
                    }
                    if (isLast) {
                        afterShow(el);
                    }
                }, this, _el, _t, i, i === 1, _beforeShow, i === _time, _afterShow), i);
            }
        }
        else {
            this.css(_el, "display", "block");
        }
    },
    /*
    * @param config{Object}
    *      el{HTMLElement}
    *      time{Number} unit:ms
    *      display{String} 值：hidden(使用display: none), invisible(使用visibility: hidden), none(仅使用opacity:0); 默认为hidden
    *      afterHide{Function}
    *          @param  el{HTMLElement}
    */
    hide: function (config) {
        var _el = this.getConfigVal(config, "el", null),
        _time = this.getConfigVal(config, "time", 0, "number"),
        _afterHide = this.getConfigVal(config, "afterHide", function () { }, "function"),
        _display = this.getConfigVal(config, "display", "hidden", "string", ["hidden", "invisible", "none"]);

        if (!_el) return false;

        if (_time) {
            this.css(_el, "opacity", "1");
            this.css(_el, "filter", "alpha(opacity=100)");
            var _t = _time;
            for (var i = _time; i >= 1; --i) {
                setTimeout(this.createCallback(function (lpp, el, t, time, isHide, afterHide, display) {
                    lpp.css(el, "opacity", time / t);
                    lpp.css(el, "filter", "alpha(opacity=" + (time / t * 100) + ")");
                    if (isHide) {
                        if (display === "hidden") {
                            lpp.css(el, "display", "none");
                        }
                        if (display === "invisible") {
                            lpp.css(el, "visibility", "hidden");
                        }
                        afterHide(el);
                    }
                }, this, _el, _t, i, i === 1, _afterHide, _display), 1 + _time - i);
            }
        }
        else {
            this.css(_el, "display", "none");
        }
    },
    getDir: function (config) {
        var _config = {
            domEl: null,
            event: null
        }, _p;

        for (_p in _config) {
            if (config[_p]) {
                _config[_p] = config[_p];
            }
        }
        if (!_config.domEl || !_config.event) return;


        var w = _config.domEl.offsetWidth,
            h = _config.domEl.offsetHeight;
        var x = (_config.event.clientX - _config.domEl.offsetLeft - (w / 2)) * (w > h ? (h / w) : 1),
            y = (_config.event.clientY - _config.domEl.offsetTop - (h / 2)) * (h > w ? (w / h) : 1),
            dirs = ['t', 'r', 'b', 'l'];
        var dir = Math.round((((Math.atan2(y, x) * (180 / Math.PI)) + 180) / 90) + 3) % 4;
        return dirs[dir];
    }
};