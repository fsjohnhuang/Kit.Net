/*
私有类：lppEl
*/


(function() {
  var lppEl, _is;

  lppEl = (function() {
    function lppEl(dom, _pDom, _isDown) {
      var i, len, orginalLen, symbol, tblEl, tmpDom, _i, _j, _k, _len, _len1, _len2, _node, _nodeList, _pDomItem, _ref, _ref1,
        _this = this;

      if (_pDom == null) {
        _pDom = document;
      }
      if (_isDown == null) {
        _isDown = false;
      }
      if (lpp.isStr(dom)) {
        if (lpp.Str.trim(dom).indexOf('<') === 0) {
          if (!/^<(\w+)(\s+?.*?)*(\/>|>.*<\/\s*\1>)$/i.test(dom)) {
            return;
          }
          tblEl = /^<(tr|td)\b/i.exec(dom);
          tmpDom = tblEl != null ? tmpDom = document.createElement(tblEl[1]) : tmpDom = document.createElement('DIV');
          tmpDom.innerHTML = dom;
          this.dom = tblEl != null ? tmpDom : lpp.toArray(tmpDom.childNodes);
        } else {
          if ((_pDom.querySelectorAll != null) && !_isDown) {
            this.dom = lpp.toArray(_pDom.querySelectorAll(dom));
          } else {
            _ref = dom.split('>');
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
              symbol = _ref[_i];
              if (!lpp.isEmpty(symbol)) {
                symbol = lpp.Str.trim(symbol);
                if (lpp.isArray(_pDom)) {
                  len = _pDom.length;
                  i = 0;
                  while (i < len) {
                    i += 1;
                    if (symbol.indexOf('.') === 0) {
                      _pDom.push(_pDom.shift().getElementsByClassName(symbol.replace('.', '')));
                    } else if (symbol.indexOf('#') === 0) {
                      _nodeList = _pDom.shift().getElementById(symbol.replace('#', ''));
                      for (_j = 0, _len1 = _nodeList.length; _j < _len1; _j++) {
                        _node = _nodeList[_j];
                        _pDom.push(_node);
                      }
                    } else if (symbol.indexOf('&') === 0) {
                      _pDom.push(_pDom.shift().getElementsByClassName(symbol.replace('&', '')));
                    } else {
                      _nodeList = _pDom.shift().getElementsByTagName(symbol);
                      for (_k = 0, _len2 = _nodeList.length; _k < _len2; _k++) {
                        _node = _nodeList[_k];
                        _pDom.push(_node);
                      }
                    }
                  }
                } else {
                  if (symbol.indexOf('.') === 0) {
                    _pDom = lpp.toArray(_pDom.getElementsByClassName(symbol.replace('.', '')));
                  } else if (symbol.indexOf('#') === 0) {
                    _pDom = [_pDom.getElementById(symbol.replace('#', ''))];
                  } else if (symbol.indexOf('&') === 0) {
                    _pDom = lpp.toArray(_pDom.getElementsByClassName(symbol.replace('&', '')));
                  } else {
                    _pDom = lpp.toArray(_pDom.getElementsByTagName(symbol));
                  }
                  _ref1 = [_pDom.length, 0], orginalLen = _ref1[0], i = _ref1[1];
                  while (i < orginalLen) {
                    _pDomItem = _pDom.pop();
                    if (_pDomItem != null) {
                      _pDom.unshift(_pDomItem);
                    }
                    i += 1;
                  }
                }
              }
              if (lpp.isNodeList(_pDom)) {
                _pDom = lpp.toArray(_pDom);
              }
            }
            this.dom = _pDom;
          }
        }
      } else if (lpp.isArray(dom)) {
        this.dom = dom;
      } else {
        this.dom = [dom];
      }
      if (this.dom == null) {
        return;
      }
      /* 元素操作函数
      */

      this.firstChild = function(dom) {
        var descendantLppEl;

        if (_this.dom.length !== 0) {
          descendantLppEl = new lppEl(dom, _this.dom[0]);
          if (!lpp.isEmpty(descendantLppEl.dom)) {
            descendantLppEl = lpp(descendantLppEl.dom[0]);
          }
          descendantLppEl.back = _this;
          return descendantLppEl;
        }
      };
      this.child = function(dom) {
        var children, _dom, _doms, _l, _len3, _len4, _m, _ref2, _ref3, _ref4, _ref5;

        children = [];
        _doms = [];
        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          _dom = _ref2[_l];
          children = Array.prototype.concat.apply(children, lpp.toArray(_dom.childNodes));
        }
        for (_m = 0, _len4 = children.length; _m < _len4; _m++) {
          _dom = children[_m];
          if (dom.indexOf('.') >= 0) {
            dom = lpp.Str.trim(dom).substring(0, 1);
            if ((_ref3 = _dom.className) != null ? _ref3.indexOf(dom >= 0) : void 0) {
              _doms.push(_dom);
            }
          } else if (dom.indexOf('#') >= 0) {
            dom = lpp.Str.trim(dom).substring(0, 1);
            if ((_ref4 = _dom.id) != null ? _ref4.indexOf(dom >= 0) : void 0) {
              _doms.push(_dom);
            }
          } else {
            if (((_ref5 = _dom.tagName) != null ? _ref5.toLocaleLowerCase() : void 0) === dom.toLocaleLowerCase()) {
              _doms.push(_dom);
            }
          }
        }
        return lpp(_doms);
      };
      this.down = function(dom) {
        var descendantLppEl, resultDom, _dom, _l, _len3, _ref2;

        resultDom = [];
        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          _dom = _ref2[_l];
          resultDom = resultDom.concat(new lppEl(dom, _dom, true).dom);
        }
        descendantLppEl = new lpp(resultDom);
        descendantLppEl.back = _this;
        return descendantLppEl;
      };
      this.each = function(callback) {
        var _l, _len3, _ref2;

        if (callback == null) {
          return _this;
        }
        _ref2 = _this.dom;
        for (i = _l = 0, _len3 = _ref2.length; _l < _len3; i = ++_l) {
          dom = _ref2[i];
          callback(new lppEl(dom), i);
        }
        return _this;
      };
      this.find = function(prop, val) {
        var goalDom, goalLppEls, key, value, _l, _len3, _prop, _ref2, _ref3;

        goalLppEls = [];
        _prop = {};
        if (lpp.isObj(prop)) {
          _prop = prop;
        } else {
          _prop[prop] = val;
        }
        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          dom = _ref2[_l];
          goalDom = null;
          for (key in _prop) {
            value = _prop[key];
            goalDom = ((_ref3 = dom.getAttribute(key)) != null ? _ref3.match(RegExp("" + value)) : void 0) ? dom : null;
          }
          if (goalDom != null) {
            goalLppEls.push(new lppEl(goalDom));
          }
        }
        return goalLppEls;
      };
      this.single = function(prop, val) {
        var goalDom, key, value, _l, _len3, _prop, _ref2, _ref3;

        _prop = {};
        if (lpp.isObj(prop)) {
          _prop = prop;
        } else {
          _prop[prop] = val;
        }
        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          dom = _ref2[_l];
          goalDom = null;
          for (key in _prop) {
            value = _prop[key];
            goalDom = ((_ref3 = dom.getAttribute(key)) != null ? _ref3.match(RegExp("" + value)) : void 0) ? dom : null;
          }
          if (goalDom != null) {
            return new lppEl(goalDom);
          }
        }
      };
      this.contains = function(dynEl) {
        var child, dynEls, isDescendant, _l, _len3, _len4, _m, _ref2;

        if (lpp.isEmpty(_this.dom || lpp.isEmpty(dynEl))) {
          return false;
        }
        dynEls = dynEl;
        if (!lpp.isArray(dynEls)) {
          (dynEls = []).push(dynEl);
        }
        isDescendant = false;
        for (i = _l = 0, _len3 = dynEls.length; _l < _len3; i = ++_l) {
          dynEl = dynEls[i];
          child = dynEl.down != null ? dynEl : new lppEl(dynEl);
          if ((child != null ? child.dom : void 0) == null) {
            isDescendant = false;
            break;
          }
          _ref2 = child.dom;
          for (_m = 0, _len4 = _ref2.length; _m < _len4; _m++) {
            dom = _ref2[_m];
            if (!(isDescendant = _this.dom[0].contains(dom))) {
              break;
            }
          }
          if (!isDescendant) {
            break;
          }
        }
        return isDescendant;
      };
      this.append = function(dynEl) {
        var child, childNode, children, dynEls, plppEl, _l, _len3, _len4, _m, _ref2, _ref3, _ref4;

        dynEls = dynEl;
        if (!lpp.isArray(dynEls)) {
          (dynEls = []).push(dynEl);
        }
        children = [];
        for (i = _l = 0, _len3 = dynEls.length; _l < _len3; i = ++_l) {
          dynEl = dynEls[i];
          child = dynEl.down != null ? dynEl : new lppEl(dynEl);
          children.push(child);
          if (_this.dom.length !== 0 && (child != null ? (_ref2 = child.dom) != null ? _ref2.length : void 0 : void 0) >= 1) {
            plppEl = new lppEl(_this.dom[0]);
            _ref3 = child.dom;
            for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
              childNode = _ref3[_m];
              plppEl.dom[0].appendChild(childNode);
            }
            child.parent = plppEl;
            if ((_ref4 = plppEl.children) == null) {
              plppEl.children = [];
            }
            plppEl.children.push(child);
          }
        }
        if (children.length === 1) {
          child.back = function() {
            return _this;
          };
          return child;
        } else {
          children.back = function() {
            return _this;
          };
          return children;
        }
      };
      this.appendAt = function(dynEl) {
        var parent, _ref2;

        parent = dynEl.down != null ? dynEl : new lppEl(dynEl);
        if ((parent != null ? (_ref2 = parent.dom) != null ? _ref2.length : void 0 : void 0) !== 0) {
          parent.append(_this);
        }
        return parent;
      };
      this.insert = function(dynEl, index) {
        var child, curChildNodes, insertingDom, plppEl, refDom, _l, _len3, _ref2, _ref3, _ref4;

        child = dynEl.down != null ? dynEl : new lppEl(dynEl);
        if (_this.dom.length !== 0 && (child != null ? (_ref2 = child.dom) != null ? _ref2.length : void 0 : void 0) >= 1) {
          plppEl = new lppEl(_this.dom[0]);
          curChildNodes = plppEl.dom[0].childNodes;
          refDom = (function() {
            switch (index) {
              case index < 0:
                return curChildNodes[0];
              case index > curChildNodes.lenght:
                return null;
              default:
                return curChildNodes[index];
            }
          })();
          _ref3 = child.dom;
          for (_l = 0, _len3 = _ref3.length; _l < _len3; _l++) {
            insertingDom = _ref3[_l];
            plppEl.dom[0].insertBefore(insertingDom, reDom);
          }
          child.parent = plppEl;
          if ((_ref4 = plppEl.children) == null) {
            plppEl.children = [];
          }
          plppEl.children.push(child);
        }
        return child;
      };
      this.insertAt = function(dynEl, index) {
        var parent;

        parent = dynEl.down != null ? dynEl : new lppEl(dynEl);
        parent.insert(_this, index);
        return parent;
      };
      this.remove = function(dynEl) {
        var child, removingDom, _l, _len3, _ref2;

        child = dynEl.down != null ? dynEl : new lppEl(dynEl);
        if (!((child != null ? child.dom.length : void 0) > 0)) {
          return child;
        }
        _ref2 = child.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          removingDom = _ref2[_l];
          _this.dom[0].removeChild(removingDom);
        }
        return child;
      };
      this.css = function(prop, val) {
        var key, value, _l, _len3, _len4, _m, _ref2, _ref3, _ref4;

        if (prop == null) {
          return _this;
        }
        if (lpp.isObj(prop)) {
          _ref2 = _this.dom;
          for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
            dom = _ref2[_l];
            for (key in prop) {
              value = prop[key];
              dom.style[key] = value;
            }
          }
          return _this;
        } else if (val != null) {
          _ref3 = _this.dom;
          for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
            dom = _ref3[_m];
            if ((prop === 'width' || prop === 'height' || prop === 'left' || prop === 'top') && lpp.isNum(val)) {
              dom.style[prop] = "" + val + "px";
            } else {
              dom.style[prop] = val;
              if (prop === 'opacity') {
                dom.style['filter'] = "alpha(opacity=" + (val * 100) + ")";
              }
            }
          }
          return _this;
        } else if (((_ref4 = _this.dom) != null ? _ref4.length : void 0) > 0) {
          return _this.dom[0].style[prop];
        }
      };
      this.hasCls = function(clsName) {
        var clsRegEx, _ref2;

        if (!(((_ref2 = _this.dom) != null ? _ref2.length : void 0) > 0)) {
          false;
        }
        clsRegEx = RegExp("(\\s|^)" + clsName + "(\\s|$)");
        return clsReEx.match(_this.dom[0].className);
      };
      this.addCls = function(clsName) {
        var clsRegEx, curDom, _l, _len3, _ref2;

        clsRegEx = RegExp("(\\s|^)" + clsName + "(\\s|$)");
        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          curDom = _ref2[_l];
          if (!clsRegEx.test(curDom.className)) {
            curDom.className = "" + clsName + " " + curDom.className;
          }
        }
        return _this;
      };
      this.removeCls = function(clsName) {
        var curDom, _l, _len3, _ref2;

        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          curDom = _ref2[_l];
          curDom.className = curDom.className.replace(RegExp("(\\s|^)" + clsName + "(\\s|$)"), '');
        }
        return _this;
      };
      this.emptyCls = function() {
        var curDom, _l, _len3, _ref2;

        _ref2 = _this.dom;
        for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
          curDom = _ref2[_l];
          curDom.className = '';
        }
        return _this;
      };
      this.html = function(val) {
        var _l, _len3, _ref2;

        if (val != null) {
          _ref2 = _this.dom;
          for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
            dom = _ref2[_l];
            dom.innerHTML = val;
          }
          return _this;
        } else {
          return ((function() {
            var _len4, _m, _ref3, _results;

            _ref3 = this.dom;
            _results = [];
            for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
              dom = _ref3[_m];
              _results.push(dom.innerHTML);
            }
            return _results;
          }).call(_this)).join('');
        }
      };
      this.value = function(val) {
        var optVal, value, _l, _len3, _len4, _m, _ref2, _ref3, _results;

        if (val != null) {
          _ref2 = _this.dom;
          _results = [];
          for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
            dom = _ref2[_l];
            switch (dom.tagName.toLocaleLowerCase()) {
              case 'select':
                _results.push((function() {
                  var _len4, _m, _ref3, _results1;

                  _ref3 = dom.options;
                  _results1 = [];
                  for (i = _m = 0, _len4 = _ref3.length; _m < _len4; i = ++_m) {
                    optVal = _ref3[i];
                    if (optVal === val) {
                      dom.selectedIndex = i;
                      break;
                    } else {
                      _results1.push(void 0);
                    }
                  }
                  return _results1;
                })());
                break;
              default:
                _results.push(dom.value = val);
            }
          }
          return _results;
        } else {
          value = [];
          _ref3 = _this.dom;
          for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
            dom = _ref3[_m];
            switch (dom.tagName.toLocaleLowerCase()) {
              case 'select':
                value.push(dom.options[dom.selectedIndex]);
                break;
              default:
                value.push(dom.value);
            }
          }
          return value.join('');
        }
      };
      this.attr = function(prop, val) {
        var key, value, _l, _len3, _len4, _m, _ref2, _ref3, _ref4;

        if (prop == null) {
          return;
        }
        if (lpp.isObj(prop)) {
          _ref2 = _this.dom;
          for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
            dom = _ref2[_l];
            for (key in prop) {
              value = prop[key];
              dom.setAttribute(key, value);
            }
          }
          return _this;
        } else if (val != null) {
          _ref3 = _this.dom;
          for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
            dom = _ref3[_m];
            dom.setAttribute(prop, val);
          }
          return _this;
        } else if (((_ref4 = _this.dom) != null ? _ref4.length : void 0) > 0) {
          return _this.dom[0].getAttribute(prop);
        }
      };
      this.on = function(eventName, handler, onePiece) {
        var addingHandler, _base, _dealedHandler, _eventName, _eventSubscriber, _handler, _l, _len3, _len4, _m, _multiHanlders, _ref2, _ref3, _ref4, _ref5;

        if (onePiece == null) {
          onePiece = false;
        }
        if (eventName == null) {
          return _this;
        }
        _eventSubscriber = {};
        if (lpp.isObj(eventName)) {
          _eventSubscriber = eventName;
        } else if (lpp.isStr(eventName) && (handler != null)) {
          _eventSubscriber[eventName] = handler;
        }
        for (_eventName in _eventSubscriber) {
          _handler = _eventSubscriber[_eventName];
          switch (_eventName) {
            case 'leave':
            case 'mouseleave':
              if (onePiece) {
                _multiHanlders = (function(handler, delay) {
                  var _handlers, _timer;

                  _timer = null;
                  _handlers = {};
                  _handlers.onmouseout = function(e) {
                    _handler = function() {
                      return handler(e);
                    };
                    return _timer = setTimeout(_handler, delay);
                  };
                  _handlers.onmousemove = function(e) {
                    return clearTimeout(_timer);
                  };
                  return _handlers;
                })(_handler, 150);
                _this.on({
                  mouseout: _multiHanlders.onmouseout,
                  mousemove: _multiHanlders.onmousemove
                });
              } else {
                _ref2 = _this.dom;
                for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
                  dom = _ref2[_l];
                  _multiHanlders = (function(handler, delay) {
                    var _handlers, _timer;

                    _timer = null;
                    _handlers = {};
                    _handlers.onmouseout = function(e) {
                      _handler = function() {
                        return handler(e);
                      };
                      return _timer = setTimeout(_handler, delay);
                    };
                    _handlers.onmousemove = function(e) {
                      return clearTimeout(_timer);
                    };
                    return _handlers;
                  })(_handler, 150);
                  lpp(dom).on({
                    mouseout: _multiHanlders.onmouseout,
                    mousemove: _multiHanlders.onmousemove
                  });
                }
              }
              break;
            default:
              _ref3 = _this.dom;
              for (_m = 0, _len4 = _ref3.length; _m < _len4; _m++) {
                dom = _ref3[_m];
                _dealedHandler = (function(handler, dom) {
                  var innerhandler;

                  return innerhandler = function() {
                    var dealedE, e, propofE, valofE;

                    e = event || arguments[0];
                    dealedE = {};
                    for (propofE in e) {
                      valofE = e[propofE];
                      dealedE[propofE] = valofE;
                    }
                    if (lpp.isIE()) {
                      dealedE.target = e.srcElement;
                      dealedE.relatedTarget = e.srcElement === e.fromElement ? e.toElement : e.fromElement;
                      dealedE.currentTarget = dom;
                      dealedE.preventDefault = function() {
                        if (e.preventDefault != null) {
                          return e.preventDefault();
                        } else {
                          return e.returnValue = false;
                        }
                      };
                      dealedE.stopPropagation = function() {
                        if (e.stopPropagation != null) {
                          return e.stopPropagation();
                        } else {
                          return e.cancelBubble = true;
                        }
                      };
                    }
                    dealedE.detail = (function() {
                      if (e.detail) {
                        return e.detail / 3;
                      } else {
                        return e.wheelDelta / 120;
                      }
                    })();
                    return handler(dealedE);
                  };
                })(_handler, dom);
                if ((_ref4 = _this.subscribed) == null) {
                  _this.subscribed = {};
                }
                if ((_ref5 = (_base = _this.subscribed)[_eventName]) == null) {
                  _base[_eventName] = [];
                }
                addingHandler = {};
                addingHandler[_handler] = _dealedHandler;
                _this.subscribed[_eventName].push(addingHandler);
                if (dom.attachEvent != null) {
                  dom.attachEvent("on" + _eventName, _dealedHandler);
                } else {
                  dom.addEventListener(_eventName, _dealedHandler, false);
                }
              }
          }
        }
        return _this;
      };
      this.un = function(eventName, handler) {
        var handlerMap, _dealedHandler, _eventName, _eventSubscriber, _handler, _l, _len3, _len4, _len5, _len6, _m, _multiHanlders, _n, _o, _ref2, _ref3, _ref4, _ref5, _ref6, _ref7;

        if (eventName == null) {
          return _this;
        }
        _eventSubscriber = {};
        if (lpp.isObj(eventName)) {
          _eventSubscriber = eventName;
        } else if (lpp.isStr(eventName) && (handler != null)) {
          _eventSubscriber[eventName] = handler;
        } else if (lpp.isStr(eventName)) {
          switch (eventName) {
            case 'leave':
            case 'mouseleave':
              _multiHanlders = (function(handler, delay) {
                var _handlers, _timer;

                _timer = null;
                _handlers = {};
                _handlers.onmouseout = function(e) {
                  return _timer = setTimeout(handler, delay);
                };
                _handlers.onmousemove = function(e) {
                  return clearTimeout(_timer);
                };
                return _handlers;
              })(_handler, 150);
              _this.un('mouseout');
              _this.un('mousemove');
              break;
            default:
              _ref2 = _this.dom;
              for (_l = 0, _len3 = _ref2.length; _l < _len3; _l++) {
                dom = _ref2[_l];
                _dealedHandler = null;
                if (((_ref3 = _this.subscribed) != null ? _ref3[eventName] : void 0) == null) {
                  break;
                }
                _ref4 = _this.subscribed[eventName];
                for (_m = 0, _len4 = _ref4.length; _m < _len4; _m++) {
                  handlerMap = _ref4[_m];
                  for (_handler in handlerMap) {
                    _dealedHandler = handlerMap[_handler];
                    if (dom.detachEvent != null) {
                      dom.detachEvent("on" + eventName, _dealedHandler);
                    } else {
                      dom.removeEventListener(eventName, _dealedHandler, false);
                    }
                  }
                }
              }
          }
        }
        for (_eventName in _eventSubscriber) {
          _handler = _eventSubscriber[_eventName];
          switch (_eventName) {
            case 'leave':
            case 'mouseleave':
              _multiHanlders = (function(handler, delay) {
                var _handlers, _timer;

                _timer = null;
                _handlers = {};
                _handlers.onmouseout = function(e) {
                  return _timer = setTimeout(handler, delay);
                };
                _handlers.onmousemove = function(e) {
                  return clearTimeout(_timer);
                };
                return _handlers;
              })(_handler, 150);
              _this.un({
                mouseout: _multiHanlders.onmouseout,
                mousemove: _multiHanlders.onmousemove
              });
              break;
            default:
              _ref5 = _this.dom;
              for (_n = 0, _len5 = _ref5.length; _n < _len5; _n++) {
                dom = _ref5[_n];
                _dealedHandler = null;
                if (((_ref6 = _this.subscribed) != null ? _ref6[_eventName] : void 0) == null) {
                  break;
                }
                _ref7 = _this.subscribed[_eventName];
                for (_o = 0, _len6 = _ref7.length; _o < _len6; _o++) {
                  handlerMap = _ref7[_o];
                  if ((handlerMap != null ? handlerMap[_handler] : void 0) != null) {
                    _dealedHandler = handlerMap[_handler];
                    if (dom.detachEvent != null) {
                      dom.detachEvent("on" + _eventName, _dealedHandler);
                    } else {
                      dom.removeEventListener(_eventName, _dealedHandler, false);
                    }
                  }
                }
              }
          }
        }
        return _this;
      };
      this.one = function(eventName, handler) {
        var _handler;

        _handler = function(event) {
          _this.un(eventName, _handler);
          return handler(event);
        };
        return _this.on(eventName, _handler);
      };
      /* fx模块
      */

      this.isHidden = false;
      this.hide = function(config) {
        var callback, display, during, _l, _ref2;

        if (lpp.isObj(config)) {
          _ref2 = lpp.getConfigVal(config, {
            during: {
              defaultVal: 1,
              dataTypes: ['number']
            },
            callback: {
              defaultVal: function() {},
              dataTypes: ['function']
            },
            display: {
              defaultVal: 'hidden',
              dataTypes: ['string'],
              valRange: ['hidden', 'invisible', 'transparent']
            }
          }), during = _ref2[0], callback = _ref2[1], display = _ref2[2];
        } else if (lpp.isNum(config)) {
          during = config;
          callback = function() {};
          display = 'hidden';
        } else if (lpp.isStr(config)) {
          if (config === 'hidden' || config === 'invisible' || config === 'transparent') {
            display = config;
          } else {
            display = 'hidden';
          }
        } else if (lpp.isFn(config)) {
          callback = config;
          during = 1;
          display = 'hidden';
        }
        _this.css('opacity', '1');
        for (i = _l = during; _l >= 0; i = _l += -1) {
          setTimeout((function(i, during, display, callback) {
            return function() {
              _this.css('opacity', i / during);
              if (i === 0) {
                switch (display) {
                  case 'hidden':
                    _this.css('display', 'none');
                    break;
                  case 'invisible':
                    _this.css('visiblity', 'hidden');
                    break;
                  case 'transparent':
                    _this.css('opacity', 0);
                }
                return callback(_this);
              }
            };
          })(i, during, display, callback), 1 + during - i);
        }
        return _this;
      };
      this.show = function(config) {
        var callback, during, _l, _ref2;

        if (lpp.isObj(config)) {
          _ref2 = lpp.getConfigVal(config, {
            during: {
              defaultVal: 1,
              dataTypes: ['number']
            },
            callback: {
              defaultVal: function() {},
              dataTypes: ['function']
            }
          }), during = _ref2[0], callback = _ref2[1];
        } else if (lpp.isNum(config)) {
          during = config;
          callback = function() {};
        } else if (lpp.isFn(config)) {
          callback = config;
          during = 1;
        }
        _this.css('opacity', '0');
        for (i = _l = 0; 0 <= during ? _l <= during : _l >= during; i = 0 <= during ? ++_l : --_l) {
          setTimeout((function(i, during, callback) {
            return function() {
              if (i === 0) {
                _this.css('display', 'block');
                _this.css('visiblity', 'visible');
              }
              _this.css('opacity', i / during);
              if (i === during) {
                return callback(_this);
              }
            };
          })(i, during, callback), i);
        }
        return _this;
      };
    }

    return lppEl;

  })();

  /* lpp函数
  */


  this.lpp = function(dom) {
    return new lppEl(dom);
  };

  _is = function(obj, type) {
    return lpp.getType(obj) === type.toLocaleLowerCase();
  };

  this.lpp.getType = function(obj) {
    var type;

    type = typeof obj;
    if (type === 'object') {
      type = obj == null ? 'null' : Object.prototype.toString.call(obj).slice(8, -1);
    }
    return type.toLowerCase();
  };

  this.lpp.isEmpty = function(obj) {
    var type;

    if (obj == null) {
      return true;
    }
    switch (type = lpp.getType(obj)) {
      case 'string':
        return lpp.Str.trim(obj) === '';
      case 'array':
        return obj.length === 0;
      default:
        return false;
    }
  };

  /*
  @lpp.toArray = (arrayLike)->
  	try
  		if lpp.isIE() && lpp.isStr arrayLike
  			resultArray = []
  			for item, i in arrayLike
  				resultArray.push arrayLike.substr(i, 1)
  			resultArray
  		else
  			Array.prototype.slice.apply arrayLike
  	catch e
  		(item for item in arrayLike)
  
  
  # 切割类数组
  # 返回 reduce：删除的值数组，items: 删除后的数组
  @lpp.splice = (obj, startIndex, len, els...) ->
  	_obj = if lpp.isArray obj then obj else lpp.toArray obj
  	els.unshift len
  	els.unshift startIndex
  	result = {}
  	result.deleted = [].splice.apply _obj,els
  	result.items = _obj
  	result
  */


  this.lpp.isArray = function(obj) {
    return _is(obj, 'array');
  };

  this.lpp.isObj = function(obj) {
    return _is(obj, 'object');
  };

  this.lpp.isStr = function(obj) {
    return _is(obj, 'string');
  };

  this.lpp.isBool = function(obj) {
    return _is(obj, 'boolean');
  };

  this.lpp.isDate = function(obj) {
    return _is(obj, 'date');
  };

  this.lpp.isNum = function(obj) {
    return _is(obj, 'number');
  };

  this.lpp.isInt = function(obj) {
    return lpp.isNum(obj) && (obj + '').indexOf('.') === -1;
  };

  this.lpp.isDecimal = function(obj) {
    return lpp.isNum(obj) && (obj + '').indexOf('.') >= 0;
  };

  this.lpp.isFn = function(obj) {
    return _is(obj, 'function');
  };

  this.lpp.isNodeList = function(obj) {
    return _is(obj, 'nodelist');
  };

  this.lpp.is = function(obj, type) {
    var lowerCaseType;

    if (!_is(type, 'string')) {
      false;
    }
    lowerCaseType = type.toLocaleLowerCase();
    switch (lowerCaseType) {
      case 'string':
        return lpp.isStr(obj);
      case 'date':
        return lpp.isDate(obj);
      case 'number':
      case 'num':
        return lpp.isNum(obj);
      case 'int':
      case 'integer':
        return lpp.isInt(obj);
      case 'decimal':
        return lpp.isDecimal(obj);
      case 'bool':
      case 'boolean':
        return lpp.isBool(obj);
      case 'obj':
      case 'object':
        return lpp.isObj(obj);
      case 'array':
      case '[]':
        return lpp.isArray(obj);
      case 'nodelist':
        return lpp.isNodeList(obj);
      case 'fn':
      case 'func':
      case 'function':
        return lpp.isFn(obj);
    }
  };

  /* 工具类
  */


  this.lpp.merge = function(dest, src) {
    var p, v, _results;

    if (!lpp.isObj(dest) && !lpp.isObj(src)) {
      return dest;
    }
    _results = [];
    for (p in src) {
      v = src[p];
      _results.push(dest[p] = v);
    }
    return _results;
  };

  this.lpp.hasOwnProperty = function(obj, propName) {
    var isOwnProp, isPrototypeProp, p, v, _ref;

    if ({}.hasOwnProperty != null) {
      return {}.hasOwnProperty.call(obj, propName);
    }
    isPrototypeProp = false;
    if (obj.constructor != null) {
      _ref = obj.constructor.prototype;
      for (p in _ref) {
        v = _ref[p];
        isPrototypeProp = propName === p;
        if (isPrototypeProp) {
          break;
        }
      }
    }
    if (!isPrototypeProp) {
      isOwnProp = false;
      for (p in obj) {
        v = obj[p];
        isOwnProp = propName === p;
      }
    }
    return isOwnProp;
  };

  this.lpp.log = function(msg) {
    if (window.location.hash.match(/debug/)) {
      return typeof console !== "undefined" && console !== null ? console.log(msg) : void 0;
    }
  };

  this.lpp.assert = function(condition, msg) {
    if (window.location.hash.match(/debug/)) {
      return typeof console !== "undefined" && console !== null ? console.assert(condition, msg) : void 0;
    }
  };

  this.lpp["debugger"] = function() {
    if (window.location.hash.match(/debug/)) {
      debugger;
    }
  };

  this.lpp.getConfigVal = function(config, propName, defaultVal, dataTypes, valRange, success) {
    var constraints, dataType, rangeItem, val, valid, _i, _j, _k, _l, _len, _len1, _len2, _len3, _propName, _ref, _success, _val;

    if (defaultVal == null) {
      defaultVal = '';
    }
    if (success == null) {
      success = function(val) {
        return val;
      };
    }
    if (lpp.isStr(propName)) {
      if ((config != null ? config[propName] : void 0) == null) {
        return defaultVal;
      }
      _success = success;
      if (lpp.isFn(dataTypes)) {
        _success = dataTypes;
      }
      if (lpp.isFn(valRange)) {
        _success = valRange;
      }
      val = config[propName];
      valid = true;
      if (!(lpp.isEmpty(dataTypes) || lpp.isFn(dataTypes))) {
        valid = false;
        if (lpp.isStr(dataTypes)) {
          valid = lpp.is(val, dataTypes);
        } else if (lpp.isArray(dataTypes)) {
          for (_i = 0, _len = dataTypes.length; _i < _len; _i++) {
            dataType = dataTypes[_i];
            if (valid = lpp.is(val, dataType)) {
              break;
            }
          }
        }
      }
      if (!(lpp.isEmpty(valRange) || lpp.isFn(valRange))) {
        valid = false;
        if (lpp.isArray(valRange)) {
          for (_j = 0, _len1 = valRange.length; _j < _len1; _j++) {
            rangeItem = valRange[_j];
            if (valid = val === rangeItem) {
              break;
            }
          }
        } else {
          valid = val === valRange;
        }
      }
      if (valid) {
        return _success(val);
      } else {
        return defaultVal;
      }
    } else if (lpp.isObj(propName)) {
      val = [];
      for (_propName in propName) {
        constraints = propName[_propName];
        if ((config != null ? config[_propName] : void 0) == null) {
          val.push(constraints.defaultVal != null ? constraints.defaultVal : '');
          continue;
        }
        _success = (_ref = constraints.success) != null ? _ref : function(val) {
          return val;
        };
        _val = config[_propName];
        valid = true;
        if (!lpp.isEmpty(constraints.dataTypes)) {
          valid = false;
          dataTypes = constraints.dataTypes;
          if (lpp.isStr(dataTypes)) {
            valid = lpp.is(_val, dataTypes);
          } else if (lpp.isArray(dataTypes)) {
            for (_k = 0, _len2 = dataTypes.length; _k < _len2; _k++) {
              dataType = dataTypes[_k];
              if (valid = lpp.is(_val, dataType)) {
                break;
              }
            }
          }
        }
        if (!lpp.isEmpty(constraints.valRange)) {
          valid = false;
          valRange = constraints.valRange;
          if (lpp.isArray(valRange)) {
            for (_l = 0, _len3 = valRange.length; _l < _len3; _l++) {
              rangeItem = valRange[_l];
              if (valid = _val === rangeItem) {
                break;
              }
            }
          } else {
            valid = _val === valRange;
          }
        }
        val.push(valid ? _success(_val) : constraints.defaultVal != null ? constraints.defaultVal : '');
      }
      return val;
    }
  };

  /* 增强native dom功能
  */


  if ((window.find != null) && (HTMLElement.prototype.contains == null)) {
    HTMLElement.prototype.contains = function(B) {
      return this.compareDocumentPosition(B) - 19 > 0;
    };
  }

}).call(this);
