!function () {
    var m_lppPrinter = {},
        m_HKEY_Root = 'HKEY_CURRENT_USER', // 注册表
        m_HKEY_Path = '\\Software\\Microsoft\\Internet Explorer\\PageSetup\\', // 注册表
        m_HKEY_Key = {
            header: "header",
            footer: "footer",
            left: "margin_left",
            right: "margin_right",
            top: "margin_top",
            bottom: "margin_bottom",
            font: "font",
            printBackground: "Print_Background",
            shrinkToFit: "Shrink_To_Fit"
        }, // 注册表
        m_WSH = null, // 操作注册表的ActiveX对象
        m_Original = {
            header: null,
            footer: null,
            left: null,
            right: null,
            top: null,
            bottom: null,
            font: null,
            printBackground: null,
            shrinkToFit: null
        }; // 暂存原PageSetup属性

    var _addStylesheet = function _addStylesheet(cssRuleText, media) {
        var newStyle = document.createElement("STYLE");
        newStyle.type = "text/css";
        newStyle.media = media;

        if (newStyle.styleSheet) {
            newStyle.styleSheet.cssText = cssRuleText;
        }
        else {
            newStyle.appendChild(document.createTextNode(cssRuleText));
        }
        var parentNode = document.getElementsByTagName("HEAD")[0] || document.body;
        parentNode.appendChild(newStyle);
    };

    try {
        m_WSH = new ActiveXObject("WScript.Shell");
    }
    catch (e) {
        _addStylesheet('body{-webkit-print-color-adjust: exact;}', "print");
    }

    // 当document.body渲染完后，插入OBJECT标签
    (function () {
        if (m_WSH) {
            var webBrowserEl = document.createElement("DIV");
            webBrowserEl.style.display = 'none';
            webBrowserEl.innerHTML = '<OBJECT  id="WebBrowser"  classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" style="display:none"></OBJECT>';
            setTimeout((function (webBrowserEl) {
                var m_self = arguments.callee;
                return function _initLppPrinter() {
                    if (!document.body) {
                        setTimeout(m_self(webBrowserEl), 250);
                        return false;
                    }

                    document.body.appendChild(webBrowserEl);
                };
            })(webBrowserEl), 250);
        }
    })();

    /* 设置注册表的页面配置（PageSetup）
    ** @param config{Object} {header,footer,left,right,top,bottom,font}
    ** @return {Boolean}
    */
    m_lppPrinter.setPageSetup = function _setPageSetup(config) {
        if (typeof config === "undefined" || null === config || !m_WSH) return false;

        var m_New = {
            header: (config.head === '' ? config.head : config.head || null),
            footer: (config.head === '' ? config.foot : config.foot || null),
            left: config.left || null,
            right: config.right || null,
            top: config.top || null,
            bottom: config.bottom || null,
            font: config.font || null,
            printBackground: config.printBackground || null,
            shrinkToFit: config.shrinkToFit || null
        };

        var key = null,
            fullPath = null;
        for (key in m_New) {
            if (m_New[key] !== null) {
                fullPath = [m_HKEY_Root, m_HKEY_Path, m_HKEY_Key[key]].join('');
                // Save original PageSetup
                if (m_Original[key] === null) {
                    m_Original[key] = m_WSH.RegRead(fullPath);
                }

                // Set new PageSetup
                m_WSH.RegWrite(fullPath, m_New[key]);
            }
        }

        return true;
    };

    /* 打印(非直接打印)
    ** @param resetAfterPrint{Boolean} true:打印后恢复PageSetup;
    **                                 false:打印后不恢复PageSetup
    ** @return {undefined}
    */
    m_lppPrinter.print = function _print(resetAfterPrint) {
        if (!m_WSH) {
            // 非IE浏览器
            window.print();
            return;
        }

        document.body.all.WebBrowser.ExecWB(6, 1);

        var m_resetAfterPrint = resetAfterPrint || false,
            fullPath = null,
            key = null;
        if (m_resetAfterPrint) {
            for (key in m_Original) {
                if (m_Original[key] !== null) {
                    fullPath = [m_HKEY_Root, m_HKEY_Path, m_HKEY_Key[key]].join('');
                    m_WSH.RegWrite(fullPath, m_Original[key]);
                }
            }
        }
    };

    /* 打印页面部分内容
    ** @param el{string/HTMLElement} 打印的页面内容ID或DOM对象
    ** @return {void}
    */
    m_lppPrinter.printPartially = function _printPartially(el) {
        var _el = el;
        if (typeof _el === 'string') {
            _el = document.getElementById(_el);
        }

        var _tmpFrame = document.createElement("IFRAME");
        _tmpFrame.style.width = '0';
        _tmpFrame.style.height = '0';
        _tmpFrame.style.left = '-100px';
        _tmpFrame.style.top = '-100px';
        _tmpFrame.style.position = "absolute";
        document.body.appendChild(_tmpFrame);
        var _frameDoc = _tmpFrame.contentWindow.document,
            _partialContent = ['<div',
                _el.className && ' class="' + _el.className + '" ' || '',
                _el.style.cssText && ' style="' + _el.style.cssText + '" ' || '',
                '>',
                _el.innerHTML,
                '</div>'].join(''),
            _linkEls = document.getElementsByTagName('LINK'),
            _styleEls = document.getElementsByTagName('STYLE'),
            _writingContent = [],
            i = null,
            lppCssSheet = null;

        for (i = 0; i < _linkEls.length; ++i) {
            if (_linkEls[i].rel === 'stylesheet') {
                lppCssSheet = lppCss(_linkEls[i]);
                _writingContent.push([
                    '<link type="text/css" rel="stylesheet" href="',
                    lppCssSheet.href,
                    '" media="',
                    lppCssSheet.media,
                    '"></link>'
                ].join(''));
            }
        }
        for (i = 0; i < _styleEls.length; ++i) {
            lppCssSheet = lppCss(_styleEls[i]);
            if (lppCssSheet.media === ''
                || lppCssSheet.media.toLocaleLowerCase().indexOf('print') >= 0) {
                _writingContent.push([
                    '<style type="text/css" media="print">',
                    lppCssSheet.cssText,
                    '</style>'
                ].join(''));
            }
        }
        _writingContent.push(_partialContent);

        _frameDoc.open();
        _frameDoc.write(_writingContent.join(''));
        _frameDoc.close();

        _tmpFrame.contentWindow.focus();
        _tmpFrame.contentWindow.print();
        document.body.removeChild(_tmpFrame);
    };

    /* 恢复PageSetup
    ** @return {undefined}
    */
    m_lppPrinter.resetPageSetup = function _resetPageSetup() {
        if (!m_WSH) return;

        var m_resetAfterPrint = resetAfterPrint || false,
            fullPath = null,
            key = null;
        if (m_resetAfterPrint) {
            for (key in m_Original) {
                if (m_Original[key] !== null) {
                    fullPath = [m_HKEY_Root, m_HKEY_Path, m_HKEY_Key[key]].join('');
                    m_WSH.RegWrite(fullPath, m_Original[key]);
                }
            }
        }
    };

    /* 打印预览
    ** @return {undefined}
    */
    m_lppPrinter.preview = function _preview() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(7, 1);
    };

    /* 查看页面属性
    ** @return {undefined}
    */
    m_lppPrinter.checkPageProp = function _setPageProp() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(10, 1);
    };

    /* 页面设置
    ** @return {undefined}
    */
    m_lppPrinter.setPageProp = function _setPageProp() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(8, 1);
    };

    /* 另存为
    ** @return {undefined}
    */
    m_lppPrinter.saveAs = function _saveAs() {
        if (!m_WSH) return;
        document.body.all.all.WebBrowser.ExecWB(4, 1);
    }

    /* 打开
    ** @return {undefined}
    */
    m_lppPrinter.open = function _open() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(1, 1);
    }

    /* 直接打印
    ** @return {undefined}
    */
    m_lppPrinter.printDirectly = function _printDirectly() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(6, 6);
    }

    /* 全选
    ** @return {undefined}
    */
    m_lppPrinter.selectAll = function _selectAll() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(17, 1);
    }

    /* 刷新
    ** @return {undefined}
    */
    m_lppPrinter.refresh = function _refresh() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(22, 1);
    }

    /* 关闭
    ** @return {undefined}
    */
    m_lppPrinter.close = function _close() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(45, 1);
    }

    /* 关闭所有
    ** @return {undefined}
    */
    m_lppPrinter.closeAll = function _closeAll() {
        if (!m_WSH) return;
        document.body.all.WebBrowser.ExecWB(2, 1);
    }


    this.lppPrinter = m_lppPrinter;
} ();