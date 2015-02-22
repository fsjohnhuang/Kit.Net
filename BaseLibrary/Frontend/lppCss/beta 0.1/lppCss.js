!function () {
    var _lppCssSheet,
        _ruleTypes = {
            CSSStyleRule: 1,
            CSSCharsetRule: 2,
            CSSImportRule: 3,
            CSSMediaRule: 4,
            CSSPageRule: 6
        }, // chrome的rule.type.1:CSSStyleRule 2:CSSCharsetRule 3:CSSImportRule 4:CSSMediaRule 6:CSSPageRule
        _isIE = false,
        _sheet;

    var _getAllRulesOfIE8 = function __getAllRulesOfIE8(sheet) {
        var allRules = [],
            i;
        if (sheet.rules) {
            for (i = sheet.rules.length - 1; i >= 0; --i) {
                allRules.push(sheet.rules[i]);
            }
        }
        if (sheet.imports) {
            for (i = sheet.imports.length - 1; i >= 0; --i) {
                allRules = allRules.concat(__getAllRulesOfIE8(sheet.imports[i]));
            }
        }
        if (sheet.pages) {
            for (i = sheet.pages.length - 1; i >= 0; --i) {
                allRules.push(sheet.pages[i]);
            }
        }

        return allRules;
    };

    var _getAllRulesOfChrome = function __getAllRulesOfChrome(sheet) {
        var allRules = [],
                    curRule,
                    i,
                    j;
        for (i = 0; sheet.cssRules && i < sheet.cssRules.length; ++i) {
            curRule = sheet.cssRules[i];
            switch (curRule.type) {
                case _ruleTypes.CSSStyleRule:
                case _ruleTypes.CSSPageRule:
                    allRules.push(curRule);
                    break;
                case _ruleTypes.CSSImportRule:
                    allRules = allRules.concat(__getAllRulesOfChrome(curRule.styleSheet));
                    break;
                case _ruleTypes.CSSMediaRule:
                    allRules = allRules.concat.apply(allRules, curRule.cssRules);
                    break;
                case _ruleTypes.CSSCharsetRule:
                    break;
            }
        }

        return allRules;
    };

    function _getCssTextForChrome(sheet) {
        var cssText = '',
            i;
        for (i = 0; sheet.cssRules && i < sheet.cssRules.length; ++i) {
            cssText += sheet.cssRules[i].cssText;
        }

        return cssText;
    }

    function convertToLppCssRule(lppCssSheet, rules) {
        var _lppCssRules = [],
                    i;
        for (i = 0; i < rules.length; ++i) {
            _lppCssRules.push(new lppCssRule(lppCssSheet, rules[i]));
        }

        return _lppCssRules;
    }

    function lppCssSheet(sheet) {
        this.href = sheet.href || '';
        this.media = (typeof sheet.media === 'string' ? sheet.media : sheet.media.mediaText);
        this.type = sheet.type || '';
        this.title = sheet.title || '';
        this.disabled = sheet.disabled || false;
        this.el = sheet.owningElement || sheet.ownerNode;
        this.parentStyleSheet = (typeof sheet.parentStyleSheet !== 'undefined' && sheet.parentStyleSheet !== null ? new lppCssSheet(sheet.parentStyleSheet) : null);
        var _rules = (_isIE ? _getAllRulesOfIE8(_sheet) : _getAllRulesOfChrome(sheet));
        this.rules = convertToLppCssRule(this, _rules);
        this.cssText = (_isIE ? sheet.cssText : _getCssTextForChrome(sheet));
        this.getRules = function (selectorText) {
            var lppCssRules = [], i;
            for (i = this.rules.length - 1; i >= 0; --i) {
                if (this.rules[i].selectorText === selectorText) {
                    lppCssRules.push(this.rules[i]);
                }
            }

            return lppCssRules;
        };
        this.addRule = (function (lppCssRule) {
            return function (selectorText, ruleLiteral) {
                var rule = null,
                    _lppCssRule = null;
                if (_isIE) {
                    rule = sheet.rules[sheet.addRule(selectorText, ruleLiteral)];
                }
                else {
                    try {
                        rule = sheet.insertRule([selectorText,
                            "{",
                            ruleLiteral,
                            "}"].join(''));
                    }
                    catch (e) {
                        return null;
                    }
                }

                _lppCssRule = new lppCssRule(this, rule);
                if (_lppCssRule) {
                    // 更新rules和cssText属性
                    this.rules.push(_lppCssRule);
                    this.cssText += [_lppCssRule.selectorText, "{", _lppCssRule.cssText, "}"].join('');
                }
                return _lppCssRule;
            };
        })(lppCssRule);
    }

    function lppCssRule(lppCssSheet, rule) {
        if (_isIE) {
            this.type = (rule.selectorText ? _ruleTypes.CSSStyleRule : _ruleTypes.CSSPageRule);
            switch (this.type) {
                case _ruleTypes.CSSStyleRule:
                    this.selectorText = rule.selectorText;
                    this.style = rule.style;
                    this.cssText = this.style.cssText;
                    break;
                case _ruleTypes.CSSPageRule:
                    this.selectorText = ["@page:", rule.pseudoClass].join('');
                    this.style = null;
                    this.cssText = '';
                    break;
            }
        }
        else {
            this.type = rule.type;
            this.selectorText = rule.selectorText;
            this.style = rule.style;
            this.cssText = this.style.cssText;
        }

        this.parentSheet = lppCssSheet;
    };

    this.lppCss = function _lppCss(el) {
        var _el = el;
        if (typeof _el === 'string') {
            _el = document.getElementById(_el);
        }
        if (_el.styleSheet) {
            _isIE = true;
        }

        _sheet = _el.styleSheet || _el.sheet;
        _lppCssSheet = new lppCssSheet(_sheet);

        return _lppCssSheet;
    };

    this.lppCss.addStyle = function (config) {
        var _media = config.media || '',
            _cssText = config.cssText || '',
            _id = config.id || '';

        var _styleEl = document.createElement("STYLE");
        _styleEl.type = "text/css";
        if (_media) {
            _styleEl.media = _media;
        }
        if (_id) {
            _styleEl.id = _id;
        }
        if (_styleEl.styleSheet) {
            _styleEl.styleSheet.cssText = _cssText;
        }
        else {
            _styleEl.appendChild(document.createTextNode(_cssText));
        }

        var _parentEl = document.getElementsByTagName("HEAD")[0] || document.body;
        _parentEl.appendChild(_styleEl);

        return new lppCssSheet(_styleEl.styleSheet || _styleEl.sheet);
    };

    this.lppCss.addLink = function (config) {
        var _media = config.media || '',
            _href = config.href || '',
            _id = config.id || '';
        if (!_href) return null;

        var _linkEl = document.createElement("LINK");
        _linkEl.type = "text/css";
        _linkEl.rel = "stylesheet";
        _linkEl.href = _href;
        if (_media) {
            _linkEl.media = _media;
        }
        if (_id) {
            _linkEl.id = _id;
        }

        var _parentEl = document.getElementsByTagName("HEAD")[0] || document.body;
        _parentEl.appendChild(_linkEl);

        var _lppCssSheet = { el: _linkEl, loading: true };
        _linkEl.onload = function (lppCssSheet) {
            return function () {
                var _tmp = new lppCssSheet(this.styleSheet || this.sheet),
                    prop;
                delete _lppCssSheet.loading;
                delete _lppCssSheet.el;
                for (prop in _tmp) {
                    _lppCssSheet[prop] = _tmp[prop];
                }
            };
        }(lppCssSheet);
        return _lppCssSheet;
    };
}();


/*sheet = {
    href,
    media,
    type,
    title,
    disabled,
    el,
    parentStyleSheet,
    cssText,
    insertRule,
    deleteRule,
    selectRule,
    updateRule,
    allRules,
    pageRules,
    imports
};
rule = {
    selectorText,
    type, // CSSStyleRule, CSSPageRule, CSSImportRule
    style,
    cssText,
    parentSheet,
    href
};*/